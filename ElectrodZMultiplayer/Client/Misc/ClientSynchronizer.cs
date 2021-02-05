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
        /// This event will be invoked when an authentification has failed.
        /// </summary>
        public override event AuthentificationFailedDelegate OnAuthentificationFailed;

        /// <summary>
        /// This event will be invoked when lobbies have been listed.
        /// </summary>
        public override event LobbiesListedDelegate OnLobbiesListed;

        /// <summary>
        /// This event will be invoked when listing lobbies has failed.
        /// </summary>
        public override event ListLobbiesFailedDelegate OnListLobbiesFailed;

        /// <summary>
        /// This event will be invoked when available game modes have been listed.
        /// </summary>
        public override event AvailableGameModesListedDelegate OnAvailableGameModesListed;

        /// <summary>
        /// This event will be invoked when listing available game modes has failed.
        /// </summary>
        public override event ListAvailableGameModesFailedDelegate OnListAvailableGameModesFailed;

        /// <summary>
        /// This event will be invoked when an user has joined this lobby.
        /// </summary>
        public override event LobbyUserJoinedDelegate OnUserJoined;

        /// <summary>
        /// On lobby join acknowledged
        /// </summary>
        public event LobbyJoinAcknowledgedDelegate OnLobbyJoinAcknowledged;

        /// <summary>
        /// This event will be invoked when joining a lobby has failed.
        /// </summary>
        public override event JoinLobbyFailedDelegate OnJoinLobbyFailed;

        /// <summary>
        /// This event will be invoked when creating a lobby has failed.
        /// </summary>
        public override event CreateLobbyFailedDelegate OnCreateLobbyFailed;

        /// <summary>
        /// This event will be invoked when an user left this lobby.
        /// </summary>
        public override event LobbyUserLeftDelegate OnUserLeft;

        /// <summary>
        /// This event will be invoked when the username changes.
        /// </summary>
        public override event UserUsernameUpdatedDelegate OnUsernameUpdated;

        /// <summary>
        /// This event will be invoked when changing username has failed.
        /// </summary>
        public override event ChangeUsernameFailedDelegate OnChangeUsernameFailed;

        /// <summary>
        /// This event will be invoked when the user lobby color changes.
        /// </summary>
        public override event UserUserLobbyColorUpdatedDelegate OnUserLobbyColorUpdated;

        /// <summary>
        /// This event will be invoked when changing user lobby color has failed.
        /// </summary>
        public override event ChangeUserLobbyColorFailedDelegate OnChangeUserLobbyColorFailed;

        /// <summary>
        /// This event will be invoked when the lobby owner of this lobby has been changed.
        /// </summary>
        public override event LobbyLobbyOwnershipChangedDelegate OnLobbyOwnershipChanged;

        /// <summary>
        /// This event will be invoked when the lobby rules of this lobby has been changed.
        /// </summary>
        public override event LobbyLobbyRulesChangedDelegate OnLobbyRulesChanged;

        /// <summary>
        /// This event will be invoked when changing lobby rules have failed.
        /// </summary>
        public override event ChangeLobbyRulesFailedDelegate OnChangeLobbyRulesFailed;

        /// <summary>
        /// This event will be invoked when kicking a user has failed.
        /// </summary>
        public override event KickUserFailedDelegate OnKickUserFailed;

        /// <summary>
        /// This event will be invoked when a game has been started.
        /// </summary>
        public override event LobbyGameStartedDelegate OnGameStarted;

        /// <summary>
        /// This event will be invoked when a game start has been requested.
        /// </summary>
        public override event LobbyGameStartRequestedDelegate OnGameStartRequested;

        /// <summary>
        /// This event will be invoked when starting a game has failed.
        /// </summary>
        public override event StartGameFailedDelegate OnStartGameFailed;

        /// <summary>
        /// This event will be invoked when a game has been restarted.
        /// </summary>
        public override event LobbyGameRestartedDelegate OnGameRestarted;

        /// <summary>
        /// This event will be invoked when a game restart has been requested.
        /// </summary>
        public override event LobbyGameRestartRequestedDelegate OnGameRestartRequested;

        /// <summary>
        /// This event will be invoked when restarting a game has failed.
        /// </summary>
        public override event RestartGameFailedDelegate OnRestartGameFailed;

        /// <summary>
        /// This event will be invoked when a game has been stopped.
        /// </summary>
        public override event LobbyGameStoppedDelegate OnGameStopped;

        /// <summary>
        /// This event will be invoked when a game stop has been requested.
        /// </summary>
        public override event LobbyGameStopRequestedDelegate OnGameStopRequested;

        /// <summary>
        /// This event will be invoked when stopping a game has failed.
        /// </summary>
        public override event StopGameFailedDelegate OnStopGameFailed;

        /// <summary>
        /// This event will be invoked when starting a game has been cancelled.
        /// </summary>
        public override event LobbyStartGameCancelledDelegate OnStartGameCancelled;

        /// <summary>
        /// This event will be invoked when restarting a game has been cancelled.
        /// </summary>
        public override event LobbyRestartGameCancelledDelegate OnRestartGameCancelled;

        /// <summary>
        /// This event will be invoked when stopping a game has been cancelled.
        /// </summary>
        public override event LobbyStopGameCancelledDelegate OnStopGameCancelled;

        /// <summary>
        /// This event will be invoked when cancelling a game start, restart or stop timer has failed.
        /// </summary>
        public override event CancelStartRestartStopGameTimerFailedDelegate OnCancelStartRestartStopGameTimerFailed;

        /// <summary>
        /// This event will be invoked when the user finished loading their game.
        /// </summary>
        public override event UserGameLoadingFinishedDelegate OnGameLoadingFinished;

        /// <summary>
        /// This event will be invoked when a client tick has been performed.
        /// </summary>
        public override event UserClientTickedDelegate OnClientTicked;

        /// <summary>
        /// This event will be invoked when a client tick has failed.
        /// </summary>
        public override event ClientTickFailedDelegate OnClientTickFailed;

        /// <summary>
        /// This event will be invoked when a server tick has been performed.
        /// </summary>
        public override event UserServerTickedDelegate OnServerTicked;

        /// <summary>
        /// This event will be invoked when a server tick has failed.
        /// </summary>
        public override event ServerTickFailedDelegate OnServerTickFailed;

        /// <summary>
        /// This event will be invoked when an user entity has been created.
        /// </summary>
        public override event LobbyUserEntityCreatedDelegate OnUserEntityCreated;

        /// <summary>
        /// This event will be invoked when an user entity has been updated.
        /// </summary>
        public override event LobbyUserEntityUpdatedDelegate OnUserEntityUpdated;

        /// <summary>
        /// This event will be invoked when an user entity has been destroyed.
        /// </summary>
        public override event LobbyUserEntityDestroyedDelegate OnUserEntityDestroyed;

        /// <summary>
        /// This event will be invoked when an entity has been created.
        /// </summary>
        public override event LobbyEntityCreatedDelegate OnEntityCreated;

        /// <summary>
        /// This event will be invoked when an entity has been updated.
        /// </summary>
        public override event LobbyEntityUpdatedDelegate OnEntityUpdated;

        /// <summary>
        /// This event will be invoked when an entity has been destroyed.
        /// </summary>
        public override event LobbyEntityDestroyedDelegate OnEntityDestroyed;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="token">Token</param>
        public ClientSynchronizer(IPeer peer, string token) : base()
        {
            Peer = peer ?? throw new ArgumentNullException(nameof(peer));
            AddAutomaticMessageParserWithFatality<AuthentificationAcknowledgedMessageData>
            (
                (_, message, __) =>
                {
                    if (IsAuthentificated)
                    {
                        SendErrorMessageToPeer<AuthentificationAcknowledgedMessageData>(peer, EErrorType.InvalidMessageContext, "Authentification has been already acknowledged.", true);
                    }
                    else
                    {
                        Token = message.Token;
                        user = new ClientUser(message.GUID);
                        RegisterUserEvents(user);
                        OnAuthentificationAcknowledged?.Invoke(User);
                    }
                }
            );
            AddAutomaticMessageParser<AuthentificationFailedMessageData>((currentPeer, message, _) => OnAuthentificationFailed?.Invoke(currentPeer, message.Message, message.Reason));
            AddAutomaticMessageParser<ListLobbyResultsMessageData>
            (
                (_, message, __) =>
                {
                    ILobbyView[] lobbies = new ILobbyView[message.Lobbies.Count];
                    Parallel.For(0, lobbies.Length, (index) => lobbies[index] = (LobbyView)message.Lobbies[index]);
                    OnLobbiesListed?.Invoke(lobbies);
                }
            );
            AddAutomaticMessageParser<ListLobbiesFailedMessageData>((currentPeer, message, _) => OnListLobbiesFailed?.Invoke(currentPeer, message.Message, message.Reason));
            AddAutomaticMessageParser<ListAvailableGameModeResultsMessageData>((_, message, __) => OnAvailableGameModesListed?.Invoke(message.GameModes));
            AddAutomaticMessageParser<ListAvailableGameModesFailedMessageData>((currentPeer, message, _) => OnListAvailableGameModesFailed?.Invoke(currentPeer, message.Message, message.Reason));
            AddAutomaticMessageParser<JoinLobbyAcknowledgedMessageData>
            (
                (_, message, __) => AssertIsUserAuthentificated<JoinLobbyAcknowledgedMessageData>
                (
                    (clientUser) =>
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
                                    RegisterUserEvents(client_user);
                                }
                                users.Add(user.GUID.ToString(), client_user);
                                user_list.Add(client_user);
                            }
                            IInternalClientLobby client_lobby = new ClientLobby(this, message.Rules.LobbyCode, message.Rules.Name, message.Rules.GameMode, message.Rules.IsPrivate, message.Rules.MinimalUserCount, message.Rules.MaximalUserCount, message.Rules.IsStartingGameAutomatically, message.Rules.GameModeRules, users[message.OwnerGUID.ToString()], users);
                            client_lobby.OnUserJoined += (user) => OnUserJoined?.Invoke(client_lobby, user);
                            client_lobby.OnUserLeft += (user, reason, leaveMessage) => OnUserLeft?.Invoke(client_lobby, user, reason, leaveMessage);
                            client_lobby.OnLobbyOwnershipChanged += () => OnLobbyOwnershipChanged?.Invoke(client_lobby);
                            client_lobby.OnLobbyRulesChanged += () => OnLobbyRulesChanged?.Invoke(client_lobby);
                            client_lobby.OnGameStarted += () => OnGameStarted?.Invoke(client_lobby);
                            client_lobby.OnGameStartRequested += (time) => OnGameStartRequested?.Invoke(client_lobby, time);
                            client_lobby.OnGameRestarted += () => OnGameRestarted?.Invoke(client_lobby);
                            client_lobby.OnGameRestartRequested += (time) => OnGameRestartRequested?.Invoke(client_lobby, time);
                            client_lobby.OnGameStopped += (gameStopUsers, results) => OnGameStopped?.Invoke(client_lobby, gameStopUsers, results);
                            client_lobby.OnGameStopRequested += (time) => OnGameStopRequested?.Invoke(client_lobby, time);
                            client_lobby.OnStartGameCancelled += () => OnStartGameCancelled?.Invoke(client_lobby);
                            client_lobby.OnRestartGameCancelled += () => OnRestartGameCancelled?.Invoke(client_lobby);
                            client_lobby.OnStopGameCancelled += () => OnStopGameCancelled?.Invoke(client_lobby);
                            client_lobby.OnUserEntityCreated += (user) => OnUserEntityCreated?.Invoke(client_lobby, user);
                            client_lobby.OnUserEntityUpdated += (user) => OnUserEntityUpdated?.Invoke(client_lobby, user);
                            client_lobby.OnUserEntityDestroyed += (user) => OnUserEntityDestroyed?.Invoke(client_lobby, user);
                            client_lobby.OnEntityCreated += (entity) => OnEntityCreated?.Invoke(client_lobby, entity);
                            client_lobby.OnEntityUpdated += (entity) => OnEntityUpdated?.Invoke(client_lobby, entity);
                            client_lobby.OnEntityDestroyed += (entity) => OnEntityDestroyed?.Invoke(client_lobby, entity);
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
                            SendInvalidMessageContextErrorMessageToPeer<JoinLobbyAcknowledgedMessageData>(peer, $"User is already in lobby \"{ clientUser.Lobby.Name }\" with lobby code \"{ clientUser.Lobby.LobbyCode }\".");
                        }
                    }
                )
            );
            AddAutomaticMessageParser<JoinLobbyFailedMessageData>((currentPeer, message, _) => OnJoinLobbyFailed?.Invoke(currentPeer, message.Message, message.Reason));
            AddAutomaticMessageParser<CreateLobbyFailedMessageData>((currentPeer, message, _) => OnCreateLobbyFailed?.Invoke(currentPeer, message.Message, message.Reason));
            AddAutomaticMessageParser<LobbyRulesChangedMessageData>
            (
                (_, message, __) => AssertIsUserInLobby<LobbyRulesChangedMessageData>
                (
                    (clientUser, clientLobby) =>
                    {
                        LobbyRulesData rules = message.Rules;
                        clientLobby.ChangeLobbyRulesInternally
                        (
                            rules.LobbyCode,
                            rules.Name,
                            rules.GameMode,
                            rules.IsPrivate,
                            rules.MinimalUserCount,
                            rules.MaximalUserCount,
                            rules.IsStartingGameAutomatically,
                            rules.GameModeRules
                        );
                    }
                )
            );
            AddAutomaticMessageParser<UserJoinedMessageData>
            (
                (_, message, __) => AssertIsUserInLobby<UserJoinedMessageData>
                (
                    (clientUser, clientLobby) =>
                    {
                        IUser user = new ClientUser(message.GUID, message.GameColor, message.Name, message.LobbyColor);
                        RegisterUserEvents(user);
                        if (!clientLobby.AddUserInternally(user))
                        {
                            SendInvalidMessageContextErrorMessageToPeer<UserJoinedMessageData>(peer, $"Failed to add user \"{ user.Name }\" with GUID \"{ user.GUID }\" to lobby \"{ clientLobby.Name }\" with lobby code \"{ clientLobby.LobbyCode }\".");
                        }
                    }
                )
            );
            AddAutomaticMessageParser<UserLeftMessageData>
            (
                (_, message, __) => AssertTargetLobbyUser<UserLeftMessageData>
                (
                    message.GUID,
                    (clientUser, clientLobby, targetUser) =>
                    {
                        if (!clientLobby.RemoveUserInternally(targetUser, message.Reason, message.Message))
                        {
                            SendInvalidMessageContextErrorMessageToPeer<UserLeftMessageData>(peer, $"Failed to remove user \"{ targetUser.Name }\" with GUID \"{ targetUser.GUID }\" from lobby \"{ clientLobby.Name }\" with lobby code \"{ clientLobby.LobbyCode }\".");
                        }
                        targetUser.ClientLobby = null;
                    }
                )
            );
            AddAutomaticMessageParser<LobbyOwnershipChangedMessageData>((_, message, __) => AssertTargetLobbyUser<LobbyOwnershipChangedMessageData>(message.NewOwnerGUID, (clientUser, clientLobby, targetUser) => clientLobby.ChangeLobbyOwnershipInternally(targetUser)));
            AddAutomaticMessageParser<UsernameChangedMessageData>((_, message, __) => AssertTargetLobbyUser<UsernameChangedMessageData>(message.GUID, (clientUser, clientLobby, targetUser) => targetUser.SetNameInternally(message.NewUsername)));
            AddAutomaticMessageParser<ChangeUsernameFailedMessageData>((currentPeer, message, _) => OnChangeUsernameFailed?.Invoke(currentPeer, message.Message, message.Reason));
            AddAutomaticMessageParser<UserLobbyColorChangedMessageData>((_, message, __) => AssertTargetLobbyUser<UserLobbyColorChangedMessageData>(message.GUID, (clientUser, clientLobby, targetUser) => targetUser.SetLobbyColorInternally(message.NewLobbyColor)));
            AddAutomaticMessageParser<ChangeUserLobbyColorFailedMessageData>((currentPeer, message, _) => OnChangeUserLobbyColorFailed?.Invoke(currentPeer, message.Message, message.Reason));
            AddAutomaticMessageParser<ChangeLobbyRulesFailedMessageData>((currentPeer, message, _) => OnChangeLobbyRulesFailed?.Invoke(currentPeer, message.Message, message.Reason));
            AddAutomaticMessageParser<KickUserFailedMessageData>((currentPeer, message, _) => OnKickUserFailed?.Invoke(currentPeer, message.Message, message.Reason));
            AddAutomaticMessageParser<GameStartRequestedMessageData>((_, message, __) => AssertIsUserInLobby<GameStartRequestedMessageData>((clientUser, clientLobby) => clientLobby.InvokeGameStartRequestedEventInternally(message.Time)));
            AddAutomaticMessageParser<GameStartedMessageData>((_, message, __) => AssertIsUserInLobby<GameStartedMessageData>((clientUser, clientLobby) => clientLobby.InvokeGameStartedEventInternally()));
            AddAutomaticMessageParser<StartGameFailedMessageData>((currentPeer, message, _) => OnStartGameFailed?.Invoke(currentPeer, message.Message, message.Reason));
            AddAutomaticMessageParser<GameRestartRequestedMessageData>((_, message, __) => AssertIsUserInLobby<GameRestartRequestedMessageData>((clientUser, clientLobby) => clientLobby.InvokeGameRestartRequestedEventInternally(message.Time)));
            AddAutomaticMessageParser<GameRestartedMessageData>((_, message, js__on) => AssertIsUserInLobby<GameRestartedMessageData>((clientUser, clientLobby) => clientLobby.InvokeGameRestartedEventInternally()));
            AddAutomaticMessageParser<RestartGameFailedMessageData>((currentPeer, message, _) => OnRestartGameFailed?.Invoke(currentPeer, message.Message, message.Reason));
            AddAutomaticMessageParser<GameStopRequestedMessageData>((_, message, __) => AssertIsUserInLobby<GameStopRequestedMessageData>((clientUser, clientLobby) => clientLobby.InvokeGameStopRequestedEventInternally(message.Time)));
            AddAutomaticMessageParser<GameStoppedMessageData>
            (
                (_, message, __) => AssertIsUserInLobby<GameStoppedMessageData>
                (
                    (clientUser, clientLobby) =>
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
                                SendErrorMessage<GameStoppedMessageData>(EErrorType.InvalidMessageContext, $"User with GUID \"{ key }\" is not in lobby \"{ clientLobby.Name }\" with lobby code \"{ clientLobby.LobbyCode }\".");
                            }
                        }
                        clientLobby.InvokeGameStoppedEventInternally(users, message.Results);
                    }
                )
            );
            AddAutomaticMessageParser<StopGameFailedMessageData>((currentPeer, message, _) => OnStopGameFailed?.Invoke(currentPeer, message.Message, message.Reason));
            AddAutomaticMessageParser<StartGameCancelledMessageData>((currentPeer, message, _) => AssertIsUserInLobby<GameStoppedMessageData>((__, clientLobby) => clientLobby.InvokeStartGameCancelledEventInternally()));
            AddAutomaticMessageParser<RestartGameCancelledMessageData>((currentPeer, message, _) => AssertIsUserInLobby<GameStoppedMessageData>((__, clientLobby) => clientLobby.InvokeRestartGameCancelledEventInternally()));
            AddAutomaticMessageParser<StopGameCancelledMessageData>((currentPeer, message, _) => AssertIsUserInLobby<GameStoppedMessageData>((__, clientLobby) => clientLobby.InvokeStopGameCancelledEventInternally()));
            AddAutomaticMessageParser<CancelStartRestartStopGameTimerFailedMessageData>((currentPeer, message, _) => OnCancelStartRestartStopGameTimerFailed?.Invoke(currentPeer, message.Message, message.Reason));
            AddAutomaticMessageParser<ServerGameLoadingProcessFinishedMessageData>((currentPeer, message, _) => AssertTargetLobbyUser<ServerGameLoadingProcessFinishedMessageData>(message.UserGUID, (__, ___, targetUser) => targetUser.InvokeServerGameLoadingProcessFinishedEvent()));
            AddMessageParser<ServerTickMessageData>
            (
                (_, message, __) => AssertIsUserInLobby<ServerTickMessageData>((clientUser, clientLobby) => clientLobby.ProcessServerTickInternally(message.Time, message.Entities, message.Hits)),
                (_, message, __) => SendServerTickFailedMessage(message, EServerTickFailedReason.Unknown),
                MessageParseFailedEvent<ServerTickMessageData>
            );
            AddAutomaticMessageParser<ClientTickFailedMessageData>((currentPeer, message, _) => OnClientTickFailed?.Invoke(currentPeer, message.Message, message.Reason));
            OnPeerConnected += (_) => SendAuthenticateMessage(token);
        }

        /// <summary>
        /// Asserts that user is authentificated
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="onUserIsAuthentificated">On user is authentificated</param>
        private void AssertIsUserAuthentificated<T>(UserIsAuthentificatedDelegate onUserIsAuthentificated) where T : IBaseMessageData
        {
            if (User == null)
            {
                SendErrorMessage<T>(EErrorType.InvalidMessageContext, "User has not been authentificated yet.", true);
            }
            else if (User is IInternalClientUser client_user)
            {
                onUserIsAuthentificated(client_user);
            }
            else
            {
                SendErrorMessage<T>(EErrorType.Unknown, $"User \"{ User.Name }\" with GUID \"{ User.GUID }\" does not inherit from \"{ nameof(IInternalClientUser) }\".");
            }
        }

        /// <summary>
        /// Asserts that user is in lobby
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="onUserIsInLobby">On user is in lobby</param>
        private void AssertIsUserInLobby<T>(UserIsInLobbyDelegate onUserIsInLobby) where T : IBaseMessageData =>
            AssertIsUserAuthentificated<T>
            (
                (clientUser) =>
                {
                    if (clientUser.ClientLobby == null)
                    {
                        SendErrorMessage<T>(EErrorType.InvalidMessageContext, "User is not in any lobby yet.");
                    }
                    else
                    {
                        onUserIsInLobby(clientUser, clientUser.ClientLobby);
                    }
                }
            );

        /// <summary>
        /// Asserts that a user is being targeted in a lobby
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="targetUserGUID">Target user GUID</param>
        /// <param name="onUserIsInLobby">On user is in lobby</param>
        private void AssertTargetLobbyUser<T>(Guid targetUserGUID, TargetLobbyUserDelegate onTargetLobbyUser) where T : IBaseMessageData =>
            AssertIsUserInLobby<T>
            (
                (clientUser, clientLobby) =>
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
                            SendErrorMessage<T>(EErrorType.Unknown, $"User \"{ users[key].Name }\" with GUID \"{ key }\" does not inherit from \"{ nameof(IInternalClientUser) }\".");
                        }
                    }
                    else
                    {
                        SendErrorMessage<T>(EErrorType.InvalidMessageContext, $"User with GUID \"{ key }\" is not in lobby \"{ User.Lobby.Name }\" with lobby code \"{ User.Lobby.LobbyCode }\".");
                    }
                }
            );

        /// <summary>
        /// Registers user events
        /// </summary>
        /// <param name="user">User</param>
        private void RegisterUserEvents(IUser user)
        {
            user.OnUsernameUpdated += () => OnUsernameUpdated?.Invoke(user);
            user.OnUserLobbyColorUpdated += () => OnUserLobbyColorUpdated?.Invoke(user);
            user.OnGameLoadingFinished += () => OnGameLoadingFinished?.Invoke(user);
            user.OnClientTicked += (entityDeltas, hits) => OnClientTicked?.Invoke(user, entityDeltas, hits);
            user.OnServerTicked += (time, entityDeltas, hits) => OnServerTicked?.Invoke(user, time, entityDeltas, hits);
        }

        /// <summary>
        /// Sends a server tick failed message
        /// </summary>
        /// <param name="message">Received message</param>
        /// <param name="reason">Reason</param>
        private void SendServerTickFailedMessage(ServerTickMessageData message, EServerTickFailedReason reason)
        {
            OnServerTickFailed?.Invoke(Peer, message, reason);
            SendMessage(new ServerTickFailedMessageData(message, reason));
        }

        /// <summary>
        /// Sends a request to create and join a lobby
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="lobbyName">Lobby name</param>
        /// <param name="gameMode">Game mode</param>
        /// <param name="isPrivate">Is lobby private</param>
        /// <param name="minimalUserCount">Minimal user count</param>
        /// <param name="maximalUserCount">Maximal user count</param>
        /// <param name="isStartingGameAutomatically">Is starting game automatically</param>
        /// <param name="gameModeRules">Game mode rules</param>
        public void CreateAndJoinLobby(string username, string lobbyName, string gameMode, bool? isPrivate = null, uint? minimalUserCount = null, uint? maximalUserCount = null, bool? isStartingGameAutomatically = null, IReadOnlyDictionary<string, object> gameModeRules = null)
        {
            if (User.Lobby == null)
            {
                SendCreateAndJoinLobbyMessage(username, lobbyName, gameMode, isPrivate, minimalUserCount, maximalUserCount, isStartingGameAutomatically, gameModeRules);
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
        /// <param name="gameMode">Game mode</param>
        /// <param name="minimalUserCount">Minimal user count</param>
        /// <param name="maximalUserCount">Maximal user count</param>
        /// <param name="isStartingGameAutomatically">Is starting game automatically</param>
        /// <param name="gameModeRules">Game mode rules</param>
        public void SendListLobbiesMessage(bool? excludeFull = null, string name = null, string gameMode = null, uint? minimalUserCount = null, uint? maximalUserCount = null, bool? isStartingGameAutomatically = null, IReadOnlyDictionary<string, object> gameModeRules = null)
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
            SendMessage(new ListLobbiesMessageData(excludeFull, name, gameMode, minimalUserCount, maximalUserCount, isStartingGameAutomatically, game_mode_rules));
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
        /// <param name="isPrivate">Is lobby private</param>
        /// <param name="minimalUserCount">Minimal user count</param>
        /// <param name="maximalUserCount">Maximal user count</param>
        /// <param name="isStartingGameAutomatically">Is starting game automatically</param>
        /// <param name="gameModeRules">Game mode rules</param>
        public void SendCreateAndJoinLobbyMessage(string username, string lobbyName, string gameMode, bool? isPrivate = null, uint? minimalUserCount = null, uint? maximalUserCount = null, bool? isStartingGameAutomatically = null, IReadOnlyDictionary<string, object> gameModeRules = null)
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
            SendMessage(new CreateAndJoinLobbyMessageData(username, lobbyName, gameMode, isPrivate, minimalUserCount, maximalUserCount, isStartingGameAutomatically, game_mode_rules));
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
        /// <param name="name">Lobby name (optional)</param>
        /// <param name="gameMode">Game mode (optional)</param>
        /// <param name="isPrivate">Is lobby private (optional)</param>
        /// <param name="minimalUserCount">Minimal user count (optional)</param>
        /// <param name="maximalUserCount">Maximal user count (optional)</param>
        /// <param name="isStartingGameAutomatically">Is starting game automatically (optional)</param>
        /// <param name="gameModeRules">Game mode rules (optional)</param>
        public void SendChangeLobbyRules(string name = null, string gameMode = null, bool? isPrivate = null, uint? minimalUserCount = null, uint? maximalUserCount = null, bool? isStartingGameAutomatically = null, IReadOnlyDictionary<string, object> gameModeRules = null)
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
            SendMessage(new ChangeLobbyRulesMessageData(name, gameMode, isPrivate, minimalUserCount, maximalUserCount, isStartingGameAutomatically, game_mode_rules));
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
        /// Sends a start, restart and stop game timer message to peer
        /// </summary>
        public void SendCancelStartRestartStopGameTimerMessage() => SendMessage(new CancelStartRestartStopGameTimerMessageData());

        /// <summary>
        /// Sends a client game loading finished message
        /// </summary>
        public void SendClientGameLoadingProcessFinishedMessage() => SendMessage(new ClientGameLoadingProcessFinishedMessageData());

        /// <summary>
        /// Sends a client tick message
        /// </summary>
        /// <param name="entities">Entities to update</param>
        /// <param name="hits">Hits</param>
        public void SendClientTickMessage(IEnumerable<IEntityDelta> entities = null, IEnumerable<IHit> hits = null)
        {
            List<IHit> actual_hits = null;
            if (hits != null)
            {
                foreach (IHit hit in hits)
                {
                    if ((hit.Issuer != null) && (hit.Issuer.GUID != User.GUID))
                    {
                        throw new ArgumentException("Hits can't contain issuers other than your user.");
                    }
                    actual_hits = actual_hits ?? new List<IHit>();
                    actual_hits.Add((hit.Issuer == null) ? hit : new Hit(hit.Victim, hit.WeaponName, hit.HitPosition, hit.HitForce, hit.Damage));
                }
            }
            SendMessage(new ClientTickMessageData(entities, actual_hits));
        }

        /// <summary>
        /// Sends an error message
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="errorType">Error type</param>
        /// <param name="errorMessage">Error message</param>
        public void SendErrorMessage<T>(EErrorType errorType, string errorMessage) where T : IBaseMessageData => SendErrorMessageToPeer<T>(Peer, errorType, errorMessage);

        /// <summary>
        /// Sends an error message
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="errorType">Error tyoe</param>
        /// <param name="errorMessage">Error message</param>
        /// <param name="isFatal">Is error fatal</param>
        public void SendErrorMessage<T>(EErrorType errorType, string errorMessage, bool isFatal) where T : IBaseMessageData => SendErrorMessageToPeer<T>(Peer, errorType, errorMessage, isFatal);
    }
}
