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
        /// Is user authenticated
        /// </summary>
        public bool IsAuthenticated => user != null;

        /// <summary>
        /// On acknowledge authentication message received
        /// </summary>
        public event AuthenticationAcknowledgedDelegate OnAuthenticationAcknowledged;

        /// <summary>
        /// On lobbies listed
        /// </summary>
        public event LobbiesListedDelegate OnLobbiesListed;

        /// <summary>
        /// On lobby join acknowledged
        /// </summary>
        public event LobbyJoinAcknowledgedDelegate OnLobbyJoinAcknowledged;

        /// <summary>
        /// On user joined
        /// </summary>
        public event UserJoinedDelegate OnUserJoined;

        /// <summary>
        /// On user left
        /// </summary>
        public event UserLeftDelegate OnUserLeft;

        /// <summary>
        /// On lobby rules changed
        /// </summary>
        public event LobbyRulesChangedDelegate OnLobbyRulesChanged;

        /// <summary>
        /// On username changed
        /// </summary>
        public event UsernameChangedDelegate OnUsernameChanged;

        /// <summary>
        /// On user game color changed
        /// </summary>
        public event UserGameColorChangedDelegate OnUserGameColorChanged;

        /// <summary>
        /// On user lobby color changed
        /// </summary>
        public event UserGameColorChangedDelegate OnUserLobbyColorChanged;

        /// <summary>
        /// On game start requested
        /// </summary>
        public event GameStartRequestedDelegate OnGameStartRequested;

        /// <summary>
        /// On game restart requested
        /// </summary>
        public event GameRestartRequestedDelegate OnGameRestartRequested;

        /// <summary>
        /// On game stop requested
        /// </summary>
        public event GameStopRequestedDelegate OnGameStopRequested;

        /// <summary>
        /// On game started
        /// </summary>
        public event GameStartedDelegate OnGameStarted;

        /// <summary>
        /// On game restarted
        /// </summary>
        public event GameRestartedDelegate OnGameRestarted;

        /// <summary>
        /// On game stopped
        /// </summary>
        public event GameStoppedDelegate OnGameStopped;

        /// <summary>
        /// On server ticked
        /// </summary>
        public event ServerTickedDelegate OnServerTicked;

        /// <summary>
        /// On game ended
        /// </summary>
        public event GameEndedDelegate OnGameEnded;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="token">Token</param>
        public ClientSynchronizer(IPeer peer, string token) : base()
        {
            Peer = peer ?? throw new ArgumentNullException(nameof(peer));
            AddMessageParser<AuthenticationAcknowledgedMessageData>((_, message, json) =>
            {
                if (IsAuthenticated)
                {
                    SendErrorMessageToPeer(peer, EErrorType.InvalidMessageContext, "Authentification has been already acknowledged.", true);
                }
                else
                {
                    Token = message.Token;
                    user = new ClientUser(message.GUID);
                    OnAuthenticationAcknowledged?.Invoke(User);
                }
            }, FatalMessageParseFailedEvent);
            AddMessageParser<ListLobbyResultsMessageData>((_, message, json) =>
            {
                ILobbyView[] lobbies = new ILobbyView[message.Lobbies.Count];
                Parallel.For(0, lobbies.Length, (index) => lobbies[index] = (LobbyView)message.Lobbies[index]);
                OnLobbiesListed?.Invoke(lobbies);
            }, MessageParseFailedEvent);
            AddMessageParser<JoinLobbyAcknowledgedMessageData>((_, message, json) =>
                AssertIsUserAuthenticated((clientUser) =>
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
                    clientLobby.SetLobbyCodeInternally(rules.LobbyCode);
                    clientLobby.SetNameInternally(rules.Name);
                    clientLobby.SetMinimalUserCountInternally(rules.MinimalUserCount);
                    clientLobby.SetMaximalUserCountInternally(rules.MaximalUserCount);
                    clientLobby.SetStartingGameAutomaticallyStateInternally(rules.IsStartingGameAutomatically);
                    clientLobby.SetGameModeInternally(rules.GameMode);
                    clientLobby.InternalGameModeRules.Clear();
                    foreach (KeyValuePair<string, object> game_mode_rule in rules.GameModeRules)
                    {
                        clientLobby.InternalGameModeRules.Add(game_mode_rule.Key, game_mode_rule.Value);
                    }
                    OnLobbyRulesChanged(clientLobby);
                }), MessageParseFailedEvent);
            AddMessageParser<UserJoinedMessageData>((_, message, json) =>
                AssertIsUserInLobby((clientUser, clientLobby) =>
                {
                    Dictionary<string, IUser> users = clientLobby.InternalUsers;
                    string key = message.GUID.ToString();
                    if (users.ContainsKey(key))
                    {
                        SendErrorMessageToPeer(peer, EErrorType.InvalidMessageContext, $"User \"{ users[key].Name }\" with GUID \"{ key }\" is already in a lobby.");
                    }
                    else
                    {
                        IUser user = new ClientUser(message.GUID, message.GameColor, message.Name, message.LobbyColor);
                        users.Add(key, user);
                        OnUserJoined?.Invoke(user);
                    }
                }), MessageParseFailedEvent);
            AddMessageParser<UserLeftMessageData>((_, message, json) =>
                AssertTargetLobbyUser(message.GUID, (clientUser, clientLobby, targetUser) =>
                {
                    clientLobby.InternalUsers.Remove(targetUser.GUID.ToString());
                    targetUser.ClientLobby = null;
                    OnUserLeft?.Invoke(user, message.Reason);
                }), MessageParseFailedEvent);
            AddMessageParser<UsernameChangedMessageData>((_, message, json) =>
                AssertTargetLobbyUser(message.GUID, (clientUser, clientLobby, targetUser) =>
                {
                    targetUser.SetNameInternally(message.NewUsername);
                    OnUsernameChanged?.Invoke(targetUser);
                }), MessageParseFailedEvent);
            AddMessageParser<UserGameColorChangedMessageData>((_, message, json) =>
                AssertTargetLobbyUser(message.GUID, (clientUser, clientLobby, targetUser) =>
                {
                    targetUser.SetGameColorInternally(message.NewGameColor);
                    OnUserGameColorChanged?.Invoke(targetUser);
                }), MessageParseFailedEvent);
            AddMessageParser<UserLobbyColorChangedMessageData>((_, message, json) =>
                AssertTargetLobbyUser(message.GUID, (clientUser, clientLobby, targetUser) =>
                {
                    targetUser.SetLobbyColorInternally(message.NewLobbyColor);
                    OnUserLobbyColorChanged?.Invoke(targetUser);
                }), MessageParseFailedEvent);
            AddMessageParser<GameStartRequestedMessageData>((_, message, json) =>
                AssertIsUserInLobby((clientUser, clientLobby) =>
                {
                    OnGameStartRequested?.Invoke(message.Time);
                }), MessageParseFailedEvent);
            AddMessageParser<GameRestartRequestedMessageData>((_, message, json) =>
                AssertIsUserInLobby((clientUser, clientLobby) =>
                {
                    OnGameRestartRequested?.Invoke(message.Time);
                }), MessageParseFailedEvent);
            AddMessageParser<GameStopRequestedMessageData>((_, message, json) =>
                AssertIsUserInLobby((clientUser, clientLobby) =>
                {
                    OnGameStopRequested?.Invoke(message.Time);
                }), MessageParseFailedEvent);
            AddMessageParser<StartGameMessageData>((_, message, json) =>
                AssertIsUserInLobby((clientUser, clientLobby) =>
                {
                    OnGameStarted?.Invoke(message.Time);
                }), MessageParseFailedEvent);
            AddMessageParser<RestartGameMessageData>((_, message, json) =>
                AssertIsUserInLobby((clientUser, clientLobby) =>
                {
                    OnGameRestarted?.Invoke(message.Time);
                }), MessageParseFailedEvent);
            AddMessageParser<StopGameMessageData>((_, message, json) =>
                AssertIsUserInLobby((clientUser, clientLobby) =>
                {
                    OnGameStopped?.Invoke();
                }), MessageParseFailedEvent);
            AddMessageParser<ServerTickMessageData>((_, message, json) =>
                AssertIsUserInLobby((clientUser, clientLobby) =>
                {
                    Dictionary<string, IUser> users = clientLobby.InternalUsers;
                    Dictionary<string, IEntity> entities = clientLobby.InternalEntities;
                    HashSet<string> remove_entity_keys = new HashSet<string>(clientLobby.InternalEntities.Keys);
                    foreach (EntityData entity in message.Entities)
                    {
                        string key = entity.GUID.ToString();
                        if (users.ContainsKey(key))
                        {
                            remove_entity_keys.Remove(key);
                            if (users[key] is IInternalClientUser client_user)
                            {
                                if (entity.EntityType != null)
                                {
                                    client_user.SetEntityTypeInternally(entity.EntityType);
                                }
                                if (entity.Position != null)
                                {
                                    client_user.SetPositionInternally(new Vector3(entity.Position.X, entity.Position.Y, entity.Position.Z));
                                }
                                if (entity.Rotation != null)
                                {
                                    client_user.SetRotationInternally(new Quaternion(entity.Rotation.X, entity.Rotation.Y, entity.Rotation.Z, entity.Rotation.W));
                                }
                                if (entity.Velocity != null)
                                {
                                    client_user.SetVelocityInternally(new Vector3(entity.Velocity.X, entity.Velocity.Y, entity.Velocity.Z));
                                }
                                if (entity.AngularVelocity != null)
                                {
                                    client_user.SetAngularVelocityInternally(new Vector3(entity.AngularVelocity.X, entity.AngularVelocity.Y, entity.AngularVelocity.Z));
                                }
                                if (entity.Actions != null)
                                {
                                    client_user.SetActionsInternally(entity.Actions);
                                }
                            }
                        }
                        else if (entities.ContainsKey(key))
                        {
                            remove_entity_keys.Remove(key);
                        }
                    }
                    foreach (string remove_entity_key in remove_entity_keys)
                    {
                        entities.Remove(remove_entity_key);
                    }
                    remove_entity_keys.Clear();
                    OnServerTicked?.Invoke(clientLobby, message.Time);
                }), MessageParseFailedEvent);
            AddMessageParser<GameEndedMessageData>((_, message, json) =>
            {
                if (User == null)
                {
                    SendErrorMessageToPeer(peer, EErrorType.InvalidMessageContext, "User has not been authenticated yet.", true);
                }
                else
                {
                    IClientLobby client_lobby = user.ClientLobby;
                    if (client_lobby == null)
                    {
                        SendErrorMessageToPeer(peer, EErrorType.InvalidMessageContext, "User is not in any lobby yet.");
                    }
                    else
                    {
                        Dictionary<string, UserWithResults> users = new Dictionary<string, UserWithResults>();
                        foreach (GameEndUserData user in message.Users)
                        {
                            string key = user.GUID.ToString();
                            if (client_lobby.Users.ContainsKey(key))
                            {
                                users.Add(key, new UserWithResults(client_lobby.Users[key], user.Results));
                            }
                            else
                            {
                                SendErrorMessageToPeer(peer, EErrorType.InvalidMessageContext, $"User with GUID \"{ key }\" is not in lobby \"{ client_lobby.Name }\" with lobby code \"{ client_lobby.LobbyCode }\".");
                            }
                        }
                        OnGameEnded?.Invoke(users, message.Results);
                    }
                }
            }, MessageParseFailedEvent);
            OnPeerConnected += (_) => SendAuthenticationMessage(token);
        }

        /// <summary>
        /// Asserts that user is authenticated
        /// </summary>
        /// <param name="onUserIsAuthenticated">On user is authenticated</param>
        private void AssertIsUserAuthenticated(UserIsAuthenticatedDelegate onUserIsAuthenticated)
        {
            if (User == null)
            {
                SendErrorMessage(EErrorType.InvalidMessageContext, "User has not been authenticated yet.", true);
            }
            else if (User is IInternalClientUser client_user)
            {
                onUserIsAuthenticated(client_user);
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
            AssertIsUserAuthenticated((clientUser) =>
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
                Dictionary<string, IUser> users = clientLobby.InternalUsers;
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
        /// Sends a authentication message to peer
        /// </summary>
        public void SendAuthenticationMessage() => SendAuthenticationMessage(null);

        /// <summary>
        /// Sends a authentication message to peer
        /// </summary>
        /// <param name="token">Existing authentication token</param>
        public void SendAuthenticationMessage(string token) => SendMessage(new AuthenticateMessageData(token));

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
        /// Send change game color message
        /// </summary>
        /// <param name="gameColor">Game color</param>
        public void SendChangeGameColorMessage(EGameColor gameColor) => SendMessage(new ChangeUserGameColorMessageData(gameColor));

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
        public void SendStartGameMessage(float time) => SendMessage(new StartGameMessageData(time));

        /// <summary>
        /// Sends a restart game message to peer
        /// </summary>
        /// <param name="time">Time to restart game in seconds</param>
        public void SendRestartGameMessage(float time) => SendMessage(new RestartGameMessageData(time));

        /// <summary>
        /// Sends a stop game message to peer
        /// </summary>
        /// <param name="time">Time to stop game in seconds</param>
        public void SendStopGameMessage(float time) => SendMessage(new StopGameMessageData(time));

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
