using ElectrodZMultiplayer.Data.Messages;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer server namespace
/// </summary>
namespace ElectrodZMultiplayer.Server
{
    /// <summary>
    /// A class that describes a server synchronizer
    /// </summary>
    internal class ServerSynchronizer : ASynchronizer, IServerSynchronizer
    {
        /// <summary>
        /// Token characters
        /// </summary>
        private static readonly IReadOnlyList<char> tokenCharacters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz".ToCharArray();

        /// <summary>
        /// Human friendly lobby code characters
        /// </summary>
        private static readonly IReadOnlyList<char> humanFriendlyLobbyCodeCharacters = "123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        /// <summary>
        /// Users
        /// </summary>
        private readonly Dictionary<string, IUser> users = new Dictionary<string, IUser>();

        /// <summary>
        /// Token to user lookup dictionary
        /// </summary>
        private readonly Dictionary<string, IInternalServerUser> tokenToUserLookup = new Dictionary<string, IInternalServerUser>();

        /// <summary>
        /// Lobbies
        /// </summary>
        private readonly Dictionary<string, ILobby> lobbies = new Dictionary<string, ILobby>();

        /// <summary>
        /// Bans
        /// </summary>
        public IBans Bans { get; } = new Bans();

        /// <summary>
        /// Users
        /// </summary>
        public IReadOnlyDictionary<string, IUser> Users => users;

        /// <summary>
        /// Lobbies
        /// </summary>
        public IReadOnlyDictionary<string, ILobby> Lobbies => lobbies;

        /// <summary>
        /// Constructs a server synchronizer
        /// </summary>
        public ServerSynchronizer() : base()
        {
            AddMessageParser<AuthenticateMessageData>((peer, message, json) =>
            {
                string key = peer.GUID.ToString();
                if (users.ContainsKey(key))
                {
                    SendErrorMessageToPeer(peer, EErrorType.InvalidMessageContext, "User is already authenticated");
                }
                else if ((message.Token == null) || !tokenToUserLookup.ContainsKey(message.Token))
                {
                    string token;
                    do
                    {
                        token = Randomizer.GetRandomString(32U, tokenCharacters);
                    }
                    while (tokenToUserLookup.ContainsKey(token));
                    IInternalServerUser server_user = new ServerUser(peer, this, token);
                    users.Add(key, server_user);
                    tokenToUserLookup.Add(token, server_user);
                    server_user.SendAuthenticationAcknowledgedMessage();
                }
                else
                {
                    IInternalServerUser server_user = tokenToUserLookup[message.Token];
                    if (users.ContainsKey(server_user.GUID.ToString()))
                    {
                        SendErrorMessageToPeer(peer, EErrorType.InvalidMessageContext, "Token is already being used for a connected user.", true);
                    }
                    else
                    {
                        server_user.SetPeerInternally(peer);
                        users.Add(key, server_user);
                        server_user.SendAuthenticationAcknowledgedMessage();
                        // TODO: Establish state back after returning from the dead.
                    }
                }
            }, FatalMessageParseFailedEvent);
            AddMessageParser<ListLobbiesMessageData>((peer, message, json) =>
                AssertAuthenticatedPeer(peer, (serverUser) =>
                {
                    List<ILobbyView> lobby_list = new List<ILobbyView>();
                    foreach (ILobby lobby in lobbies.Values)
                    {
                        if
                        (
                            ((message.Name == null) || lobby.Name.Contains(message.Name.Trim())) &&
                            ((message.MinimalUserCount == null) || (lobby.UserCount >= message.MinimalUserCount)) &&
                            ((message.MaximalUserCount == null) || (lobby.UserCount <= message.MaximalUserCount)) &&
                            ((message.ExcludeFull == null) || (lobby.UserCount < lobby.MaximalUserCount)) &&
                            ((message.IsStartingGameAutomatically == null) || (lobby.IsStartingGameAutomatically == message.IsStartingGameAutomatically)) &&
                            ((message.GameMode == null) || (lobby.GameMode.Contains(message.GameMode))) &&
                            ((message.GameModeRules == null) || AreGameModeRulesContained(lobby.GameModeRules, message.GameModeRules))
                        )
                        {
                            lobby_list.Add(lobby);
                        }
                    }
                    serverUser.SendListLobbyResultsMessage(lobby_list);
                    lobby_list.Clear();
                }), MessageParseFailedEvent);
            AddMessageParser<JoinLobbyMessageData>((peer, message, json) =>
                AssertPeerIsNotInLobby(peer, (serverUser) =>
                {
                    string lobby_code = message.LobbyCode.ToUpper();
                    if (lobbies.ContainsKey(lobby_code))
                    {
                        if (lobbies[lobby_code] is IInternalServerLobby server_lobby)
                        {
                            if (server_lobby.UserCount < server_lobby.MaximalUserCount)
                            {
                                serverUser.SetNameInternally(message.Username);
                                server_lobby.InternalUsers.Add(serverUser.GUID.ToString(), serverUser);
                                serverUser.SendJoinLobbyAcknowledgedMessage(server_lobby);
                            }
                            else
                            {
                                serverUser.SendErrorMessage(EErrorType.Full, $"Lobby \"{ server_lobby.Name }\" with lobby code \"{ server_lobby.LobbyCode }\" is full.");
                            }
                        }
                        else
                        {
                            serverUser.SendErrorMessage(EErrorType.InternalError, $"Lobby with lobby code \"{ lobby_code }\" does not inherit from \"{ nameof(IInternalServerLobby) }\".");
                        }
                    }
                    else
                    {
                        serverUser.SendErrorMessage(EErrorType.NotFound, $"Lobby with lobby code \"{ lobby_code }\" does not exist.");
                    }
                }), MessageParseFailedEvent);
            AddMessageParser<CreateAndJoinLobbyMessageData>((peer, message, json) =>
                AssertPeerIsNotInLobby(peer, (serverUser) =>
                {
                    string lobby_code;
                    do
                    {
                        lobby_code = Randomizer.GetRandomString(6U, humanFriendlyLobbyCodeCharacters);
                    }
                    while (lobbies.ContainsKey(lobby_code));
                    IInternalServerLobby server_lobby = new ServerLobby(lobby_code, message.LobbyName, message.MinimalUserCount ?? Defaults.minimalUserCount, message.MaximalUserCount ?? Defaults.maximalUserCount, message.IsStartingGameAutomatically ?? Defaults.isStartingGameAutomatically, message.GameMode, message.GameModeRules, this, serverUser);
                    lobbies.Add(lobby_code, server_lobby);
                    serverUser.ServerLobby = server_lobby;
                    serverUser.SetNameInternally(message.Username);
                    server_lobby.InternalUsers.Add(serverUser.GUID.ToString(), serverUser);
                    serverUser.SendJoinLobbyAcknowledgedMessage(server_lobby);
                }), MessageParseFailedEvent);
            AddMessageParser<QuitLobbyMessageData>((peer, message, json) =>
                AssertPeerIsInLobby(peer, (serverUser, serverLobby) =>
                {
                    serverLobby.SendUserLeftMessage(serverUser, "User has left the lobby.");
                    serverLobby.InternalUsers.Remove(serverUser.GUID.ToString());
                    serverUser.ServerLobby = null;
                }), MessageParseFailedEvent);
            AddMessageParser<ChangeUsernameMessageData>((peer, message, json) =>
                AssertPeerIsInLobby(peer, (serverUser, serverLobby) =>
                {
                    serverUser.SetNameInternally(message.NewUsername.Trim());
                    serverLobby.SendUsernameChangedMessage(serverUser);
                }), MessageParseFailedEvent);
            AddMessageParser<ChangeUserGameColorMessageData>((peer, message, json) =>
                AssertPeerIsInLobby(peer, (serverUser, serverLobby) =>
                {
                    serverUser.SetGameColorInternally(message.NewUserGameColor);
                    serverLobby.SendUserGameColorChangedMessage(serverUser);
                }), MessageParseFailedEvent);
            AddMessageParser<ChangeUserLobbyColorMessageData>((peer, message, json) =>
                AssertPeerIsInLobby(peer, (serverUser, serverLobby) =>
                {
                    serverUser.SetLobbyColorInternally(message.NewUserLobbyColor);
                    serverLobby.SendUserLobbyColorChangedMessage(serverUser);
                }), MessageParseFailedEvent);
            AddMessageParser<ChangeLobbyRulesMessageData>((peer, message, json) =>
                AssertPeerIsLobbyOwner(peer, (serverUser, serverLobby) =>
                {
                    if (message.Name != null)
                    {
                        serverLobby.SetNameInternally(message.Name);
                    }
                    if (message.MinimalUserCount != null)
                    {
                        serverLobby.SetMinimalUserCountInternally(message.MinimalUserCount.Value);
                    }
                    if (message.MaximalUserCount != null)
                    {
                        serverLobby.SetMaximalUserCountInternally(message.MaximalUserCount.Value);
                    }
                    if (message.IsStartingGameAutomatically != null)
                    {
                        serverLobby.SetStartingGameAutomaticallyStateInternally(message.IsStartingGameAutomatically.Value);
                    }
                    if (message.GameMode != null)
                    {
                        serverLobby.SetGameModeInternally(message.GameMode);
                    }
                    if (message.GameModeRules != null)
                    {
                        serverLobby.ClearGameModeRulesInternally();
                        foreach (KeyValuePair<string, object> game_mode_rule in message.GameModeRules)
                        {
                            serverLobby.AddGameModeRuleInternally(game_mode_rule.Key, game_mode_rule.Value);
                        }
                    }
                    serverLobby.SendLobbyRulesChangedMessage();
                }), MessageParseFailedEvent);
            AddMessageParser<KickUserMessageData>((peer, message, json) =>
                AssertPeerIsLobbyOwner(peer, (serverUser, serverLobby) =>
                {
                    string key = message.UserGUID.ToString();
                    if (serverLobby.InternalUsers.ContainsKey(key))
                    {
                        if (serverLobby.InternalUsers[key] is IInternalServerUser server_user)
                        {
                            serverLobby.SendUserLeftMessage(server_user, message.Reason);
                            serverLobby.InternalUsers.Remove(key);
                            server_user.ServerLobby = null;
                        }
                        else
                        {
                            serverUser.SendErrorMessage(EErrorType.InternalError, $"User \"{ serverLobby.InternalUsers[key].Name }\" with GUID \"{ key }\" does not inherit from \"{ nameof(IInternalServerUser) }\".");
                        }
                    }
                    else
                    {
                        serverUser.SendErrorMessage(EErrorType.InvalidMessageParameters, $"User with GUID \"{ key }\" does not exist.");
                    }
                }), MessageParseFailedEvent);
            AddMessageParser<StartGameMessageData>((peer, message, json) =>
                AssertPeerIsLobbyOwner(peer, (serverUser, serverLobby) =>
                {
                    if (message.Time <= float.Epsilon)
                    {
                        serverLobby.SendStartGameMessage();
                    }
                    else
                    {
                        serverLobby.SendGameStartRequestedMessage(message.Time);
                    }
                }), MessageParseFailedEvent);
            AddMessageParser<RestartGameMessageData>((peer, message, json) =>
                AssertPeerIsLobbyOwner(peer, (serverUser, serverLobby) =>
                {
                    if (message.Time <= float.Epsilon)
                    {
                        serverLobby.SendRestartGameMessage();
                    }
                    else
                    {
                        serverLobby.SendGameRestartRequestedMessage(message.Time);
                    }
                }), MessageParseFailedEvent);
            AddMessageParser<StopGameMessageData>((peer, message, json) =>
                AssertPeerIsLobbyOwner(peer, (serverUser, serverLobby) =>
                {
                    if (message.Time <= float.Epsilon)
                    {
                        serverLobby.SendStopGameMessage();
                    }
                    else
                    {
                        serverLobby.SendGameStopRequestedMessage(message.Time);
                    }
                }), MessageParseFailedEvent);
            AddMessageParser<ClientTickMessageData>((peer, message, json) =>
                AssertPeerIsInLobby(peer, (serverUser, serverLobby) =>
                {
                    if (message.Position != null)
                    {
                        serverUser.SetPositionInternally(new Vector3<float>(message.Position.X, message.Position.Y, message.Position.Z));
                    }
                    if (message.Rotation != null)
                    {
                        serverUser.SetRotationInternally(new Quaternion<float>(message.Rotation.W, message.Rotation.X, message.Rotation.Y, message.Rotation.Z));
                    }
                    if (message.Velocity != null)
                    {
                        serverUser.SetVelocityInternally(new Vector3<float>(message.Velocity.X, message.Velocity.Y, message.Velocity.Z));
                    }
                    if (message.Color != null)
                    {
                        serverUser.SetGameColorInternally(message.Color.Value);
                    }
                    if (message.Actions != null)
                    {
                        serverUser.ClearGameActionsInternally();
                        foreach (EGameAction action in message.Actions)
                        {
                            serverUser.AddGameActionInternally(action);
                        }
                    }
                    // TODO: Process entity updates
                }), MessageParseFailedEvent);
        }

        /// <summary>
        /// Are game mode rules contained in game mode rule subset
        /// </summary>
        /// <param name="gameModeRules">Game mode rules</param>
        /// <param name="gameModeRuleSubset">Game mode rul set</param>
        /// <returns>"true" if game rule subset is contained in game mode rules, otherwise "false"</returns>
        private static bool AreGameModeRulesContained(IReadOnlyDictionary<string, object> gameModeRules, IReadOnlyDictionary<string, object> gameModeRuleSubset)
        {
            bool ret = true;
            foreach (KeyValuePair<string, object> game_mode_rule in gameModeRuleSubset)
            {
                if (!gameModeRules.ContainsKey(game_mode_rule.Key) || !Equals(gameModeRules[game_mode_rule.Key], game_mode_rule.Value))
                {
                    ret = false;
                    break;
                }
            }
            return ret;
        }

        /// <summary>
        /// Asserts authenticated peer
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="onPeerAuthenticated">On peer authenticated</param>
        private void AssertAuthenticatedPeer(IPeer peer, PeerAuthenticatedDelegate onPeerAuthenticated)
        {
            string key = peer.GUID.ToString();
            if (users.ContainsKey(key))
            {
                if (users[key] is IInternalServerUser server_user)
                {
                    onPeerAuthenticated(server_user);
                }
                else
                {
                    SendErrorMessageToPeer(peer, EErrorType.InvalidMessageContext, $"User with GUID \"{ key }\" does not inherit from \"{ nameof(IInternalServerUser) }\".");
                }
            }
            else
            {
                SendErrorMessageToPeer(peer, EErrorType.InvalidMessageContext, "User is not authenticated yet.", true);
            }
        }

        /// <summary>
        /// Asserts peer is not in lobby
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="onPeerIsInLobby">On peer is not in lobby</param>
        private void AssertPeerIsNotInLobby(IPeer peer, PeerIsNotInLobbyDelegate onPeerIsNotInLobby) =>
            AssertAuthenticatedPeer(peer, (serverUser) =>
            {
                if (serverUser.ServerLobby == null)
                {
                    onPeerIsNotInLobby(serverUser);
                }
                else
                {
                    serverUser.SendErrorMessage(EErrorType.InvalidMessageContext, $"User \"{ serverUser.Name }\" with GUID \"{ serverUser.GUID }\" is already in lobby \"{ serverUser.ServerLobby.Name }\" with lobby code \"{ serverUser.ServerLobby.LobbyCode }\".");
                }
            });

        /// <summary>
        /// Asserts peer is in lobby
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="onPeerIsInLobby">On peer is in lobby</param>
        private void AssertPeerIsInLobby(IPeer peer, PeerIsInLobbyDelegate onPeerIsInLobby) =>
            AssertAuthenticatedPeer(peer, (serverUser) =>
            {
                if (serverUser.ServerLobby == null)
                {
                    serverUser.SendErrorMessage(EErrorType.InvalidMessageContext, $"User \"{ serverUser.Name }\" with GUID \"{ serverUser.GUID }\" is not in a lobby.");
                }
                else
                {
                    onPeerIsInLobby(serverUser, serverUser.ServerLobby);
                }
            });

        /// <summary>
        /// Asserts peer is lobby owner
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="onPeerIsLobbyOwner">On peer is lobby owner</param>
        private void AssertPeerIsLobbyOwner(IPeer peer, PeerIsLobbyOwnerDelegate onPeerIsLobbyOwner) =>
            AssertPeerIsInLobby(peer, (serverUser, serverLobby) =>
            {
                if (serverUser.GUID == serverLobby.Owner.GUID)
                {
                    onPeerIsLobbyOwner(serverUser, serverLobby);
                }
                else
                {
                    serverUser.SendErrorMessage(EErrorType.InvalidMessageContext, $"User \"{ serverUser.Name }\" with GUID \"{ serverUser.GUID }\" is not the lobby owner of \"{ serverLobby.Name }\" with lobby code \"{ serverLobby.LobbyCode }\".");
                }
            });

        /// <summary>
        /// Processes all events appeared since last call
        /// </summary>
        public void ProcessEvents()
        {
            foreach (IConnector connector in Connectors)
            {
                connector.ProcessEvents();
            }
        }

        /// <summary>
        /// Closes connections to all peers
        /// </summary>
        /// <param name="reason">Disconnection reason</param>
        public override void Close(EDisconnectionReason reason)
        {
            base.Close(reason);
            lobbies.Clear();
        }
    }
}
