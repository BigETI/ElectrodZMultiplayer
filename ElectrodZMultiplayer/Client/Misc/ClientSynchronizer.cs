using ElectrodZMultiplayer.Data;
using ElectrodZMultiplayer.Data.Messages;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

/// <summary>
/// ElectrodZ multiplayer client namespace
/// </summary>
namespace ElectrodZMultiplayer.Client
{
    /// <summary>
    /// A class that describes a synchronizer specific to a client
    /// </summary>
    internal class ClientSynchronizer : ASynchronizer, IInternalClientSynchronizer
    {
        /// <summary>
        /// Client user
        /// </summary>
        private IInternalClientUser user;

        /// <summary>
        /// Peer
        /// </summary>
        public IPeer Peer { get; private set; }

        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; private set; }

        /// <summary>
        /// Client user
        /// </summary>
        public IClientUser User => user;

        /// <summary>
        /// Is peer connected
        /// </summary>
        public bool IsConnected => Peer != null;

        /// <summary>
        /// Is user authentificated
        /// </summary>
        public bool IsAuthentificated => user != null;

        /// <summary>
        /// This event will be invoked when an authentification was acknowledged.
        /// </summary>
        public override event AuthentificationAcknowledgedDelegate OnAuthentificationAcknowledged;

        /// <summary>
        /// This event will be invoked when lobbies have been listed.
        /// </summary>
        public override event LobbiesListedDelegate OnLobbiesListed;

        /// <summary>
        /// This event will be invoked when available game modes have been listed.
        /// </summary>
        public override event AvailableGameModesListedDelegate OnAvailableGameModesListed;

        /// <summary>
        /// On lobby join acknowledged
        /// </summary>
        public event LobbyJoinAcknowledgedDelegate OnLobbyJoinAcknowledged;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="token">Token</param>
        public ClientSynchronizer(IPeer peer, string token) : base()
        {
            Peer = peer ?? throw new ArgumentNullException(nameof(peer));
            AddMessageParser<AuthentificationAcknowledgedMessageData>((_, message, json) =>
            {
                if (IsAuthentificated)
                {
                    SendErrorMessageToPeer(peer, EErrorType.InvalidMessageContext, "Authentification has been already acknowledged.", true);
                }
                else
                {
                    Token = message.Token;
                    user = new ClientUser(message.GUID);
                    OnAuthentificationAcknowledged?.Invoke(User);
                }
            }, FatalMessageParseFailedEvent);
            AddMessageParser<ListLobbyResultsMessageData>((_, message, json) =>
            {
                ILobbyView[] lobbies = new ILobbyView[message.Lobbies.Count];
                Parallel.For(0, lobbies.Length, (index) => lobbies[index] = (LobbyView)message.Lobbies[index]);
                OnLobbiesListed?.Invoke(lobbies);
            }, MessageParseFailedEvent);
            AddMessageParser<ListAvailableGameModeResultsMessageData>((_, message, json) => OnAvailableGameModesListed?.Invoke(message.GameModes), MessageParseFailedEvent);
            AddMessageParser<JoinLobbyAcknowledgedMessageData>((_, message, json) =>
                AssertIsUserAuthentificated((clientUser) =>
                {
                    if (clientUser.Lobby == null)
                    {
                        Dictionary<string, IUser> users = new Dictionary<string, IUser>();
                        List<IInternalClientUser> user_list = new List<IInternalClientUser>();
                        foreach (UserData user in message.Users)
                        {
                            IInternalClientUser client_user;
                            if (user.GUID == clientUser.GUID)
                            {
                                client_user = clientUser;
                                this.user.SetNameInternally(user.Name);
                                this.user.SetLobbyColorInternally(user.LobbyColor);
                            }
                            else
                            {
                                client_user = new ClientUser(user.GUID, user.GameColor, user.Name, user.LobbyColor);
                            }
                            users.Add(user.GUID.ToString(), client_user);
                            user_list.Add(client_user);
                        }
                        IInternalClientLobby client_lobby = new ClientLobby(this, message.Rules.LobbyCode, message.Rules.Name, message.Rules.MinimalUserCount, message.Rules.MaximalUserCount, message.Rules.IsStartingGameAutomatically, message.Rules.GameMode, message.Rules.GameModeRules, users[message.OwnerGUID.ToString()], users);
                        user.ClientLobby = client_lobby;
                        foreach (IInternalClientUser client_user in user_list)
                        {
                            client_user.ClientLobby = client_lobby;
                        }
                        user_list.Clear();
                        OnLobbyJoinAcknowledged?.Invoke(client_lobby);
                    }
                    else
                    {
                        SendErrorMessageToPeer(peer, EErrorType.InvalidMessageContext, $"User is already in lobby \"{ clientUser.Lobby.Name }\" with lobby code \"{ clientUser.Lobby.LobbyCode }\".");
                    }
                }), MessageParseFailedEvent);
            AddMessageParser<LobbyRulesChangedMessageData>((_, message, json) =>
                AssertIsUserInLobby((clientUser, clientLobby) =>
                {
                    LobbyRulesData rules = message.Rules;
                    clientLobby.UpdateGameModeRulesInternally
                    (
                        rules.LobbyCode,
                        rules.Name,
                        rules.GameMode,
                        rules.MinimalUserCount,
                        rules.MaximalUserCount,
                        rules.IsStartingGameAutomatically,
                        rules.GameModeRules
                    );
                }), MessageParseFailedEvent);
            AddMessageParser<UserJoinedMessageData>((_, message, json) =>
                AssertIsUserInLobby((clientUser, clientLobby) =>
                {
                    IUser user = new ClientUser(message.GUID, message.GameColor, message.Name, message.LobbyColor);
                    if (!clientLobby.AddUserInternally(user))
                    {
                        SendErrorMessageToPeer(peer, EErrorType.InvalidMessageContext, $"Failed to add user \"{ user.Name }\" with GUID \"{ user.GUID }\" to lobby \"{ clientLobby.Name }\" with lobby code \"{ clientLobby.LobbyCode }\".");
                    }
                }), MessageParseFailedEvent);
            AddMessageParser<UserLeftMessageData>((_, message, json) =>
                AssertTargetLobbyUser(message.GUID, (clientUser, clientLobby, targetUser) =>
                {
                    if (!clientLobby.RemoveUserInternally(targetUser, message.Reason))
                    {
                        SendErrorMessageToPeer(peer, EErrorType.InvalidMessageContext, $"Failed to remove user \"{ targetUser.Name }\" with GUID \"{ targetUser.GUID }\" from lobby \"{ clientLobby.Name }\" with lobby code \"{ clientLobby.LobbyCode }\".");
                    }
                    targetUser.ClientLobby = null;
                }), MessageParseFailedEvent);
            AddMessageParser<UsernameChangedMessageData>((_, message, json) => AssertTargetLobbyUser(message.GUID, (clientUser, clientLobby, targetUser) => targetUser.SetNameInternally(message.NewUsername)), MessageParseFailedEvent);
            AddMessageParser<UserLobbyColorChangedMessageData>((_, message, json) => AssertTargetLobbyUser(message.GUID, (clientUser, clientLobby, targetUser) => targetUser.SetLobbyColorInternally(message.NewLobbyColor)), MessageParseFailedEvent);
            AddMessageParser<GameStartRequestedMessageData>((_, message, json) => AssertIsUserInLobby((clientUser, clientLobby) => clientLobby.InvokeGameStartRequestedEventInternally(message.Time)), MessageParseFailedEvent);
            AddMessageParser<GameRestartRequestedMessageData>((_, message, json) => AssertIsUserInLobby((clientUser, clientLobby) => clientLobby.InvokeGameRestartRequestedEventInternally(message.Time)), MessageParseFailedEvent);
            AddMessageParser<GameStopRequestedMessageData>((_, message, json) => AssertIsUserInLobby((clientUser, clientLobby) => clientLobby.InvokeGameStopRequestedEventInternally(message.Time)), MessageParseFailedEvent);
            AddMessageParser<GameStartedMessageData>((_, message, json) => AssertIsUserInLobby((clientUser, clientLobby) => clientLobby.InvokeGameStartedEventInternally()), MessageParseFailedEvent);
            AddMessageParser<GameRestartedMessageData>((_, message, json) => AssertIsUserInLobby((clientUser, clientLobby) => clientLobby.InvokeGameRestartedEventInternally()), MessageParseFailedEvent);
            AddMessageParser<GameStoppedMessageData>((_, message, json) => AssertIsUserInLobby((clientUser, clientLobby) => clientLobby.InvokeGameStoppedEventInternally()), MessageParseFailedEvent);
            AddMessageParser<ServerTickMessageData>((_, message, json) => AssertIsUserInLobby((clientUser, clientLobby) => clientLobby.ProcessServerTickInternally(message.Time, message.Entities)), MessageParseFailedEvent);
            AddMessageParser<GameEndedMessageData>((_, message, json) =>
                AssertIsUserInLobby((clientUser, clientLobby) =>
                {
                    Dictionary<string, UserWithResults> users = new Dictionary<string, UserWithResults>();
                    foreach (GameEndUserData user in message.Users)
                    {
                        string key = user.GUID.ToString();
                        if (clientLobby.Users.ContainsKey(key))
                        {
                            users.Add(key, new UserWithResults(clientLobby.Users[key], user.Results));
                        }
                        else
                        {
                            SendErrorMessage(EErrorType.InvalidMessageContext, $"User with GUID \"{ key }\" is not in lobby \"{ clientLobby.Name }\" with lobby code \"{ clientLobby.LobbyCode }\".");
                        }
                    }
                    clientLobby.InvokeGameEndedEventInternally(users, message.Results);
                }), MessageParseFailedEvent);
            OnPeerConnected += (_) => SendAuthenticateMessage(token);
        }

        /// <summary>
        /// Asserts that user is authentificated
        /// </summary>
        /// <param name="onUserIsAuthentificated">On user is authentificated</param>
        private void AssertIsUserAuthentificated(UserIsAuthentificatedDelegate onUserIsAuthentificated)
        {
            if (User == null)
            {
                SendErrorMessage(EErrorType.InvalidMessageContext, "User has not been authentificated yet.", true);
            }
            else if (User is IInternalClientUser client_user)
            {
                onUserIsAuthentificated(client_user);
            }
            else
            {
                SendErrorMessage(EErrorType.InternalError, $"User \"{ User.Name }\" with GUID \"{ User.GUID }\" does not inherit from \"{ nameof(IInternalClientUser) }\".");
            }
        }

        /// <summary>
        /// Asserts that user is in lobby
        /// </summary>
        /// <param name="onUserIsInLobby">On user is in lobby</param>
        private void AssertIsUserInLobby(UserIsInLobbyDelegate onUserIsInLobby) =>
            AssertIsUserAuthentificated((clientUser) =>
            {
                if (clientUser.ClientLobby == null)
                {
                    SendErrorMessage(EErrorType.InvalidMessageContext, "User is not in any lobby yet.");
                }
                else
                {
                    onUserIsInLobby(clientUser, clientUser.ClientLobby);
                }
            });

        /// <summary>
        /// Asserts that a user is being targeted in a lobby
        /// </summary>
        /// <param name="targetUserGUID">Target user GUID</param>
        /// <param name="onUserIsInLobby">On user is in lobby</param>
        private void AssertTargetLobbyUser(Guid targetUserGUID, TargetLobbyUserDelegate onTargetLobbyUser) =>
            AssertIsUserInLobby((clientUser, clientLobby) =>
            {
                IReadOnlyDictionary<string, IUser> users = clientLobby.Users;
                string key = targetUserGUID.ToString();
                if (users.ContainsKey(key))
                {
                    if (users[key] is IInternalClientUser client_user)
                    {
                        onTargetLobbyUser(clientUser, clientLobby, client_user);
                    }
                    else
                    {
                        SendErrorMessage(EErrorType.InternalError, $"User \"{ users[key].Name }\" with GUID \"{ key }\" does not inherit from \"{ nameof(IInternalClientUser) }\".");
                    }
                }
                else
                {
                    SendErrorMessage(EErrorType.InvalidMessageContext, $"User with GUID \"{ key }\" is not in lobby \"{ User.Lobby.Name }\" with lobby code \"{ User.Lobby.LobbyCode }\".");
                }
            });

        /// <summary>
        /// Sends a request to create and join a lobby
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="lobbyName">Lobby name</param>
        /// <param name="gameMode">Game mode</param>
        /// <param name="minimalUserCount">Minimal user count</param>
        /// <param name="maximalUserCount">Maximal user count</param>
        /// <param name="isStartingGameAutomatically">Is starting game automatically</param>
        /// <param name="gameModeRules">Game mode rules</param>
        public void CreateAndJoinLobby(string username, string lobbyName, string gameMode, uint? minimalUserCount = null, uint? maximalUserCount = null, bool? isStartingGameAutomatically = null, IReadOnlyDictionary<string, object> gameModeRules = null)
        {
            if (User.Lobby == null)
            {
                SendCreateAndJoinLobbyMessage(username, lobbyName, gameMode, minimalUserCount, maximalUserCount, isStartingGameAutomatically, gameModeRules);
            }
        }

        /// <summary>
        /// Joins a lobby with the specified lobby code
        /// </summary>
        /// <param name="lobbyCode">Lobby code</param>
        /// <param name="username">Username</param>
        public void JoinLobby(string lobbyCode, string username)
        {
            if (User.Lobby == null)
            {
                SendJoinLobbyMessage(lobbyCode, username);
            }
        }

        /// <summary>
        /// Process events
        /// </summary>
        public void ProcessEvents()
        {
            foreach (IConnector connector in Connectors)
            {
                connector.ProcessEvents();
            }
        }

        /// <summary>
        /// Sends a message to peer
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="message">Message</param>
        public void SendMessage<T>(T message) where T : IBaseMessageData => SendMessageToPeer(Peer, message);

        /// <summary>
        /// Sends an authenticate message to peer
        /// </summary>
        public void SendAuthenticateMessage() => SendAuthenticateMessage(null);

        /// <summary>
        /// Sends an authenticate message to peer
        /// </summary>
        /// <param name="token">Existing authentification token</param>
        public void SendAuthenticateMessage(string token) => SendMessage(new AuthenticateMessageData(token));

        /// <summary>
        /// Sends a list available game modes message
        /// </summary>
        /// <param name="name">Game mode name filter</param>
        public void SendListAvailableGameModesMessage(string name = null) => SendMessage(new ListAvailableGameModesMessageData(name));

        /// <summary>
        /// Sends a list lobbies message to peer
        /// </summary>
        /// <param name="excludeFull">Exclude full lobbies</param>
        /// <param name="name">Lobby name</param>
        /// <param name="minimalUserCount">Minimal user count</param>
        /// <param name="maximalUserCount">Maximal user count</param>
        /// <param name="isStartingGameAutomatically">Is starting game automatically</param>
        /// <param name="gameMode">Game mode</param>
        /// <param name="gameModeRules">Game mode rules</param>
        public void SendListLobbiesMessage(bool? excludeFull = null, string name = null, uint? minimalUserCount = null, uint? maximalUserCount = null, bool? isStartingGameAutomatically = null, string gameMode = null, IReadOnlyDictionary<string, object> gameModeRules = null)
        {
            Dictionary<string, object> game_mode_rules = null;
            if (gameModeRules != null)
            {
                game_mode_rules = new Dictionary<string, object>();
                foreach (KeyValuePair<string, object> game_mode_rule in gameModeRules)
                {
                    game_mode_rules.Add(game_mode_rule.Key, game_mode_rule.Value);
                }
            }
            SendMessage(new ListLobbiesMessageData(excludeFull, name, minimalUserCount, maximalUserCount, isStartingGameAutomatically, gameMode, game_mode_rules));
        }

        /// <summary>
        /// Sends a join lobby message to peer
        /// </summary>
        /// <param name="lobbyCode">Lobby code</param>
        /// <param name="username">Username</param>
        public void SendJoinLobbyMessage(string lobbyCode, string username) => SendMessage(new JoinLobbyMessageData(lobbyCode, username));

        /// <summary>
        /// Sends a create and join lobby message to peer
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="lobbyName">Lobby name</param>
        /// <param name="gameMode">Game mode</param>
        /// <param name="minimalUserCount">Minimal user count</param>
        /// <param name="maximalUserCount">Maximal user count</param>
        /// <param name="isStartingGameAutomatically">Is starting game automatically</param>
        /// <param name="gameModeRules">Game mode rules</param>
        public void SendCreateAndJoinLobbyMessage(string username, string lobbyName, string gameMode, uint? minimalUserCount = null, uint? maximalUserCount = null, bool? isStartingGameAutomatically = null, IReadOnlyDictionary<string, object> gameModeRules = null)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException(nameof(username));
            }
            if (string.IsNullOrWhiteSpace(lobbyName))
            {
                throw new ArgumentNullException(nameof(lobbyName));
            }
            if (string.IsNullOrWhiteSpace(gameMode))
            {
                throw new ArgumentNullException(nameof(gameMode));
            }
            Dictionary<string, object> game_mode_rules = null;
            if (gameModeRules != null)
            {
                game_mode_rules = new Dictionary<string, object>();
                foreach (KeyValuePair<string, object> game_mode_rule in gameModeRules)
                {
                    game_mode_rules.Add(game_mode_rule.Key, game_mode_rule.Value);
                }
            }
            SendMessage(new CreateAndJoinLobbyMessageData(username, lobbyName, gameMode, minimalUserCount, maximalUserCount, isStartingGameAutomatically, game_mode_rules));
        }

        /// <summary>
        /// Sends a quit lobbym message to peer
        /// </summary>
        public void SendQuitLobbyMessage() => SendMessage(new QuitLobbyMessageData());

        /// <summary>
        /// Sends a change username message to peer
        /// </summary>
        /// <param name="newUsername">New username</param>
        public void SendChangeUsernameMessage(string newUsername) => SendMessage(new ChangeUsernameMessageData(newUsername));

        /// <summary>
        /// Send change lobby color message
        /// </summary>
        /// <param name="lobbyColor">Lobby color</param>
        public void SendChangeLobbyColorMessage(Color lobbyColor) => SendMessage(new ChangeUserLobbyColorMessageData(lobbyColor));

        /// <summary>
        /// Sends a change lobby rules message to peer
        /// </summary>
        /// <param name="name">Lobby name</param>
        /// <param name="minimalUserCount">Minimal user count</param>
        /// <param name="maximalUserCount">Maximal user count</param>
        /// <param name="isStartingGameAutomatically">Is starting game automatically</param>
        /// <param name="gameMode">Game mode</param>
        /// <param name="gameModeRules">Game mode rules</param>
        public void SendChangeLobbyRules(string name = null, uint? minimalUserCount = null, uint? maximalUserCount = null, bool? isStartingGameAutomatically = null, string gameMode = null, IReadOnlyDictionary<string, object> gameModeRules = null)
        {
            Dictionary<string, object> game_mode_rules = null;
            if (gameModeRules != null)
            {
                game_mode_rules = new Dictionary<string, object>();
                foreach (KeyValuePair<string, object> game_mode_rule in gameModeRules)
                {
                    game_mode_rules.Add(game_mode_rule.Key, game_mode_rule.Value);
                }
            }
            SendMessage(new ChangeLobbyRulesMessageData(name, minimalUserCount, maximalUserCount, isStartingGameAutomatically, gameMode, game_mode_rules));
        }

        /// <summary>
        /// Sends a kick user message to peer
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="reason">Reason</param>
        public void SendKickUserMessage(IUser user, string reason)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            SendMessage(new KickUserMessageData(user.GUID, reason));
        }

        /// <summary>
        /// Sends a start game message to peer
        /// </summary>
        /// <param name="time">Time to start game in seconds</param>
        public void SendStartGameMessage(double time) => SendMessage(new StartGameMessageData(time));

        /// <summary>
        /// Sends a restart game message to peer
        /// </summary>
        /// <param name="time">Time to restart game in seconds</param>
        public void SendRestartGameMessage(double time) => SendMessage(new RestartGameMessageData(time));

        /// <summary>
        /// Sends a stop game message to peer
        /// </summary>
        /// <param name="time">Time to stop game in seconds</param>
        public void SendStopGameMessage(double time) => SendMessage(new StopGameMessageData(time));

        /// <summary>
        /// Sends a client tick message
        /// </summary>
        /// <param name="entities">Entities to update</param>
        public void SendClientTickMessage(IEnumerable<IEntityDelta> entities = null) => SendMessage(new ClientTickMessageData(entities));

        /// <summary>
        /// Sends an error message
        /// </summary>
        /// <param name="errorType">Error type</param>
        /// <param name="errorMessage">Error message</param>
        public void SendErrorMessage(EErrorType errorType, string errorMessage) => SendErrorMessageToPeer(Peer, errorType, errorMessage);

        /// <summary>
        /// Sends an error message
        /// </summary>
        /// <param name="errorType">Error tyoe</param>
        /// <param name="errorMessage">Error message</param>
        /// <param name="isFatal">Is error fatal</param>
        public void SendErrorMessage(EErrorType errorType, string errorMessage, bool isFatal) => SendErrorMessageToPeer(Peer, errorType, errorMessage, isFatal);
    }
}
