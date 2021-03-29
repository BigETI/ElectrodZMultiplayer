using ElectrodZMultiplayer.Data;
using ElectrodZMultiplayer.Data.Messages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

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
        /// Game resources
        /// </summary>
        private readonly Dictionary<string, IGameResource> gameResources = new Dictionary<string, IGameResource>();

        /// <summary>
        /// Available game mode types
        /// </summary>
        private readonly Dictionary<string, (IGameResource, Type)> availableGameModeTypes = new Dictionary<string, (IGameResource, Type)>();

        /// <summary>
        /// Entity deltas
        /// </summary>
        private readonly List<IEntityDelta> entityDeltas = new List<IEntityDelta>();

        /// <summary>
        /// Hits
        /// </summary>
        private readonly List<IHit> hits = new List<IHit>();

        /// <summary>
        /// Tick stopwatch
        /// </summary>
        private readonly Stopwatch tickStopwatch = new Stopwatch();

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
        /// Game resources
        /// </summary>
        public IReadOnlyDictionary<string, IGameResource> GameResources => gameResources;

        /// <summary>
        /// Available game mode types
        /// </summary>
        public IReadOnlyDictionary<string, (IGameResource, Type)> AvailableGameModeTypes => availableGameModeTypes;

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
        /// Constructs a server synchronizer
        /// </summary>
        public ServerSynchronizer() : base()
        {
            AddMessageParser<AuthenticateMessageData>
            (
                (peer, message, _) =>
                {
                    string key = peer.GUID.ToString();
                    if (users.ContainsKey(key) && users[key] is IInternalServerUser server_user)
                    {
                        SendAuthentificationFailedMessage(peer, message, EAuthentificationFailedReason.AlreadyAuthenticated);
                    }
                    else if ((message.Token == null) || !tokenToUserLookup.ContainsKey(message.Token))
                    {
                        string token;
                        do
                        {
                            token = Randomizer.GetRandomString(32U, tokenCharacters);
                        }
                        while (tokenToUserLookup.ContainsKey(token));
                        server_user = new ServerUser(peer, this, token);
                        server_user.OnUsernameUpdated += () => OnUsernameUpdated?.Invoke(server_user);
                        server_user.OnUserLobbyColorUpdated += () => OnUserLobbyColorUpdated?.Invoke(server_user);
                        server_user.OnGameLoadingFinished += () => OnGameLoadingFinished?.Invoke(server_user);
                        server_user.OnClientTicked += (entityDeltas, hits) => OnClientTicked?.Invoke(server_user, entityDeltas, hits);
                        server_user.OnServerTicked += (time, entityDeltas, hits) => OnServerTicked?.Invoke(server_user, time, entityDeltas, hits);
                        users.Add(key, server_user);
                        tokenToUserLookup.Add(token, server_user);
                        OnAuthentificationAcknowledged?.Invoke(server_user);
                        server_user.SendAuthentificationAcknowledgedMessage();
                    }
                    else
                    {
                        server_user = tokenToUserLookup[message.Token];
                        if (users.ContainsKey(server_user.GUID.ToString()))
                        {
                            SendAuthentificationFailedMessage(peer, message, EAuthentificationFailedReason.TokenIsAlreadyInUse, true);
                        }
                        else
                        {
                            server_user.SetPeerInternally(peer);
                            users.Add(key, server_user);
                            OnAuthentificationAcknowledged?.Invoke(server_user);
                            server_user.SendAuthentificationAcknowledgedMessage();
                            // TODO: Establish state back after returning from the dead.
                        }
                    }
                },
                (peer, message, _) =>
                {
                    if (message.Version == null)
                    {
                        SendAuthentificationFailedMessage(peer, message, EAuthentificationFailedReason.VersionIsNull, true);
                    }
                    else if (message.Version != Defaults.apiVersion)
                    {
                        SendAuthentificationFailedMessage(peer, message, EAuthentificationFailedReason.NotSupportedVersion, true);
                    }
                    else
                    {
                        SendAuthentificationFailedMessage(peer, message, EAuthentificationFailedReason.Unknown, true);
                    }
                },
                FatalMessageParseFailedEvent<AuthenticateMessageData>
            );
            AddMessageParser<ListLobbiesMessageData>
            (
                (peer, message, _) => AssertPeerIsAuthentificated<ListLobbiesMessageData>
                (
                    peer,
                    (serverUser) =>
                    {
                        List<ILobbyView> lobby_list = new List<ILobbyView>();
                        foreach (ILobby lobby in lobbies.Values)
                        {
                            if
                            (
                                !lobby.IsPrivate &&
                                ((message.Name == null) || lobby.Name.Contains(message.Name.Trim())) &&
                                ((message.MinimalUserCount == null) || (lobby.UserCount >= message.MinimalUserCount)) &&
                                ((message.MaximalUserCount == null) || (lobby.UserCount <= message.MaximalUserCount)) &&
                                ((message.ExcludeFull == null) || (lobby.UserCount < lobby.MaximalUserCount)) &&
                                ((message.IsStartingGameAutomatically == null) || (lobby.IsStartingGameAutomatically == message.IsStartingGameAutomatically)) &&
                                ((message.GameMode == null) || lobby.GameMode.Contains(message.GameMode)) &&
                                ((message.GameModeRules == null) || AreGameModeRulesContained(lobby.GameModeRules, message.GameModeRules))
                            )
                            {
                                lobby_list.Add(lobby);
                            }
                        }
                        OnLobbiesListed?.Invoke(lobby_list);
                        serverUser.SendListLobbyResultsMessage(lobby_list);
                        lobby_list.Clear();
                    }
                ),
                (peer, message, _) =>
                {
                    OnListLobbiesFailed?.Invoke(peer, message, EListLobbiesFailedReason.Unknown);
                    SendMessageToPeer(peer, new ListLobbiesFailedMessageData(message, EListLobbiesFailedReason.Unknown));
                },
                MessageParseFailedEvent<ListLobbiesMessageData>
            );
            AddMessageParser<ListAvailableGameModesMessageData>
            (
                (peer, message, _) => AssertPeerIsAuthentificated<ListAvailableGameModesMessageData>
                (
                    peer,
                    (serverUser) =>
                    {
                        HashSet<string> available_game_modes = null;
                        string name = message.Name ?? string.Empty;
                        foreach (string available_game_mode in availableGameModeTypes.Keys)
                        {
                            if (available_game_mode.Contains(name))
                            {
                                available_game_modes = available_game_modes ?? new HashSet<string>();
                                available_game_modes.Add(available_game_mode);
                            }
                        }
                        OnAvailableGameModesListed?.Invoke(available_game_modes);
                        serverUser.SendListAvailableGameModeResultsMessage(available_game_modes);
                        available_game_modes.Clear();
                    }
                ),
                (peer, message, _) =>
                {
                    OnListAvailableGameModesFailed?.Invoke(peer, message, EListAvailableGameModesFailedReason.Unknown);
                    SendMessageToPeer(peer, new ListAvailableGameModesFailedMessageData(message, EListAvailableGameModesFailedReason.Unknown));
                },
                MessageParseFailedEvent<ListAvailableGameModesMessageData>
            );
            AddMessageParser<JoinLobbyMessageData>
            (
                (peer, message, json) => AssertPeerIsNotInLobby<JoinLobbyMessageData>
                (
                    peer,
                    (serverUser) =>
                    {
                        string lobby_code = message.LobbyCode.ToUpper();
                        if (lobbies.ContainsKey(lobby_code))
                        {
                            if (lobbies[lobby_code] is IInternalServerLobby server_lobby)
                            {
                                if (server_lobby.UserCount < server_lobby.MaximalUserCount)
                                {
                                    serverUser.ServerLobby = server_lobby;
                                    serverUser.SetNameInternally(message.Username);
                                    if (server_lobby.AddUser(serverUser))
                                    {
                                        serverUser.SendJoinLobbyAcknowledgedMessage(server_lobby);
                                    }
                                    else
                                    {
                                        serverUser.ServerLobby = null;
                                        SendJoinLobbyFailedMessage(peer, message, EJoinLobbyFailedReason.Unknown);
                                    }
                                }
                                else
                                {
                                    SendJoinLobbyFailedMessage(peer, message, EJoinLobbyFailedReason.Full);
                                }
                            }
                            else
                            {
                                SendJoinLobbyFailedMessage(peer, message, EJoinLobbyFailedReason.Unknown);
                            }
                        }
                        else
                        {
                            SendJoinLobbyFailedMessage(peer, message, EJoinLobbyFailedReason.NotFound);
                        }
                    }
                ),
                (peer, message, _) =>
                {
                    if (message.LobbyCode == null)
                    {
                        SendJoinLobbyFailedMessage(peer, message, EJoinLobbyFailedReason.LobbyCodeIsNull);
                    }
                    else if (message.Username == null)
                    {
                        SendJoinLobbyFailedMessage(peer, message, EJoinLobbyFailedReason.UsernameIsNull);
                    }
                    else
                    {
                        string trimmed_username = message.Username.Trim();
                        if ((message.Username.Trim().Length < Defaults.minimalUsernameLength) || (message.Username.Trim().Length > Defaults.maximalUsernameLength))
                        {
                            SendJoinLobbyFailedMessage(peer, message, EJoinLobbyFailedReason.InvalidUsernameLength);
                        }
                        else
                        {
                            SendJoinLobbyFailedMessage(peer, message, EJoinLobbyFailedReason.Unknown);
                        }
                    }
                },
                MessageParseFailedEvent<JoinLobbyMessageData>
            );
            AddMessageParser<CreateAndJoinLobbyMessageData>
            (
                (peer, message, json) => AssertPeerIsNotInLobby<CreateAndJoinLobbyMessageData>
                (
                    peer, (serverUser) =>
                    {
                        if (availableGameModeTypes.ContainsKey(message.GameMode))
                        {
                            string lobby_code;
                            do
                            {
                                lobby_code = Randomizer.GetRandomString(6U, humanFriendlyLobbyCodeCharacters);
                            }
                            while (lobbies.ContainsKey(lobby_code));
                            IInternalServerLobby server_lobby = new ServerLobby(lobby_code, message.LobbyName, availableGameModeTypes[message.GameMode], message.IsPrivate ?? Defaults.isLobbyPrivate, message.MinimalUserCount ?? Defaults.minimalUserCount, message.MaximalUserCount ?? Defaults.maximalUserCount, message.IsStartingGameAutomatically ?? Defaults.isStartingGameAutomatically, message.GameModeRules, this, serverUser);
                            server_lobby.OnUserJoined += (user) => OnUserJoined?.Invoke(server_lobby, user);
                            server_lobby.OnUserLeft += (user, reason, leaveMessage) => OnUserLeft?.Invoke(server_lobby, user, reason, leaveMessage);
                            server_lobby.OnLobbyOwnershipChanged += () => OnLobbyOwnershipChanged?.Invoke(server_lobby);
                            server_lobby.OnLobbyRulesChanged += () => OnLobbyRulesChanged?.Invoke(server_lobby);
                            server_lobby.OnGameStarted += () => OnGameStarted?.Invoke(server_lobby);
                            server_lobby.OnGameStartRequested += (time) => OnGameStartRequested?.Invoke(server_lobby, time);
                            server_lobby.OnGameRestarted += () => OnGameRestarted?.Invoke(server_lobby);
                            server_lobby.OnGameRestartRequested += (time) => OnGameRestartRequested?.Invoke(server_lobby, time);
                            server_lobby.OnGameStopped += (users, results) => OnGameStopped?.Invoke(server_lobby, users, results);
                            server_lobby.OnGameStopRequested += (time) => OnGameStopRequested?.Invoke(server_lobby, time);
                            server_lobby.OnStartGameCancelled += () => OnStartGameCancelled?.Invoke(server_lobby);
                            server_lobby.OnRestartGameCancelled += () => OnRestartGameCancelled?.Invoke(server_lobby);
                            server_lobby.OnStopGameCancelled += () => OnStopGameCancelled?.Invoke(server_lobby);
                            server_lobby.OnUserEntityCreated += (user) => OnUserEntityCreated?.Invoke(server_lobby, user);
                            server_lobby.OnUserEntityUpdated += (user) => OnUserEntityUpdated?.Invoke(server_lobby, user);
                            server_lobby.OnUserEntityDestroyed += (user) => OnUserEntityDestroyed?.Invoke(server_lobby, user);
                            server_lobby.OnEntityCreated += (entity) => OnEntityCreated?.Invoke(server_lobby, entity);
                            server_lobby.OnEntityUpdated += (entity) => OnEntityUpdated?.Invoke(server_lobby, entity);
                            server_lobby.OnEntityDestroyed += (entity) => OnEntityDestroyed?.Invoke(server_lobby, entity);
                            lobbies.Add(lobby_code, server_lobby);
                            serverUser.ServerLobby = server_lobby;
                            serverUser.SetNameInternally(message.Username);
                            server_lobby.AddUser(serverUser);
                            serverUser.SendJoinLobbyAcknowledgedMessage(server_lobby);
                        }
                        else
                        {
                            SendCreateLobbyFailedMessage(peer, message, ECreateLobbyFailedReason.GameModeIsNotAvailable);
                        }
                    }
                ),
                (peer, message, _) =>
                {
                    if (message.Username == null)
                    {
                        SendCreateLobbyFailedMessage(peer, message, ECreateLobbyFailedReason.UsernameIsNull);
                    }
                    else if ((message.Username.Length < Defaults.minimalUsernameLength) || (message.Username.Length > Defaults.maximalUsernameLength))
                    {
                        SendCreateLobbyFailedMessage(peer, message, ECreateLobbyFailedReason.InvalidUsernameLength);
                    }
                    else if (message.LobbyName == null)
                    {
                        SendCreateLobbyFailedMessage(peer, message, ECreateLobbyFailedReason.LobbyNameIsNull);
                    }
                    else if ((message.LobbyName.Length < Defaults.minimalLobbyNameLength) || (message.LobbyName.Length > Defaults.maximalLobbyNameLength))
                    {
                        SendCreateLobbyFailedMessage(peer, message, ECreateLobbyFailedReason.InvalidLobbyNameLength);
                    }
                    else if (string.IsNullOrWhiteSpace(message.GameMode))
                    {
                        SendCreateLobbyFailedMessage(peer, message, ECreateLobbyFailedReason.InvalidGameMode);
                    }
                    else if ((message.MinimalUserCount != null) && (message.MaximalUserCount != null) && (message.MinimalUserCount > message.MaximalUserCount))
                    {
                        SendCreateLobbyFailedMessage(peer, message, ECreateLobbyFailedReason.MinimalUserCountIsBiggerThanMaximalUserCount);
                    }
                    else if ((message.GameModeRules != null) && Protection.IsContained(message.GameModeRules.Values, (value) => value == null))
                    {
                        SendCreateLobbyFailedMessage(peer, message, ECreateLobbyFailedReason.GameModeRulesContainNull);
                    }
                    else
                    {
                        SendCreateLobbyFailedMessage(peer, message, ECreateLobbyFailedReason.Unknown);
                    }
                },
                MessageParseFailedEvent<CreateAndJoinLobbyMessageData>
            );
            AddMessageParser<QuitLobbyMessageData>
            (
                (peer, message, json) => AssertPeerIsInLobby<QuitLobbyMessageData>
                (
                    peer,
                    (serverUser, serverLobby) =>
                    {
                        if (serverLobby.RemoveUser(serverUser, EDisconnectionReason.Quit, "User has left the lobby.") && (serverLobby.UserCount == 0U))
                        {
                            RemoveServerLobby(serverLobby);
                        }
                    }
                ),
                (peer, message, _) => SendMessageToPeer(peer, new QuitLobbyFailedMessageData(message, EQuitLobbyFailedReason.Unknown)), MessageParseFailedEvent<QuitLobbyMessageData>
            );
            AddMessageParser<ChangeUsernameMessageData>
            (
                (peer, message, json) => AssertPeerIsInLobby<ChangeUsernameMessageData>(peer, (serverUser, serverLobby) => serverUser.UpdateUsername(message.NewUsername.Trim())),
                (peer, message, _) =>
                {
                    if (message.NewUsername == null)
                    {
                        SendChangeUsernameFailedMessage(peer, message, EChangeUsernameFailedReason.UsernameIsNull);
                    }
                    else if ((message.NewUsername.Length < Defaults.minimalUsernameLength) || (message.NewUsername.Length > Defaults.maximalUsernameLength))
                    {
                        SendChangeUsernameFailedMessage(peer, message, EChangeUsernameFailedReason.InvalidUsernameLength);
                    }
                    else
                    {
                        SendChangeUsernameFailedMessage(peer, message, EChangeUsernameFailedReason.Unknown);
                    }
                },
                MessageParseFailedEvent<ChangeUsernameMessageData>
            );
            AddMessageParser<ChangeUserLobbyColorMessageData>
            (
                (peer, message, json) => AssertPeerIsInLobby<ChangeUserLobbyColorMessageData>
                (
                    peer,
                    (serverUser, serverLobby) => serverUser.UpdateUserLobbyColor(message.NewUserLobbyColor)
                ),
                (peer, message, _) =>
                {
                    OnChangeUserLobbyColorFailed?.Invoke(peer, message, EChangeUserLobbyColorFailedReason.Unknown);
                    SendMessageToPeer(peer, new ChangeUserLobbyColorFailedMessageData(message, EChangeUserLobbyColorFailedReason.Unknown));
                },
                MessageParseFailedEvent<ChangeUserLobbyColorMessageData>
            );
            AddMessageParser<ChangeLobbyRulesMessageData>
            (
                (peer, message, json) => AssertPeerIsLobbyOwner<ChangeLobbyRulesMessageData>
                (
                    peer,
                    (serverUser, serverLobby) =>
                    {
                        if ((message.GameMode == null) || availableGameModeTypes.ContainsKey(message.GameMode))
                        {
                            serverLobby.UpdateLobbyRules
                            (
                                message.Name,
                                (message.GameMode == null) ? ((IGameResource, Type)?)null : availableGameModeTypes[message.GameMode],
                                message.MinimalUserCount,
                                message.MaximalUserCount,
                                message.IsStartingGameAutomatically,
                                message.GameModeRules
                            );
                        }
                        else
                        {
                            SendChangeLobbyRulesFailedMessage(peer, message, EChangeLobbyRulesFailedReason.GameModeIsNotAvailable);
                        }
                    }
                ),
                (peer, message, _) =>
                {
                    if ((message.Name != null) && ((message.Name.Length < Defaults.minimalLobbyNameLength) || (message.Name.Length > Defaults.maximalLobbyNameLength)))
                    {
                        SendChangeLobbyRulesFailedMessage(peer, message, EChangeLobbyRulesFailedReason.InvalidLobbyNameLength);
                    }
                    else if ((message.GameMode != null) && string.IsNullOrWhiteSpace(message.GameMode))
                    {
                        SendChangeLobbyRulesFailedMessage(peer, message, EChangeLobbyRulesFailedReason.InvalidGameMode);
                    }
                    else if ((message.MinimalUserCount != null) && (message.MaximalUserCount != null) && (message.MinimalUserCount > message.MaximalUserCount))
                    {
                        SendChangeLobbyRulesFailedMessage(peer, message, EChangeLobbyRulesFailedReason.MinimalUserCountIsBiggerThanMaximalUserCount);
                    }
                    else if ((message.GameModeRules != null) && message.GameModeRules.ContainsValue(null))
                    {
                        SendChangeLobbyRulesFailedMessage(peer, message, EChangeLobbyRulesFailedReason.GameModeRulesContainNull);
                    }
                    else
                    {
                        SendChangeLobbyRulesFailedMessage(peer, message, EChangeLobbyRulesFailedReason.Unknown);
                    }
                },
                MessageParseFailedEvent<ChangeLobbyRulesMessageData>
            );
            AddMessageParser<KickUserMessageData>
            (
                (peer, message, json) => AssertPeerIsLobbyOwner<KickUserMessageData>
                (
                    peer,
                    (serverUser, serverLobby) =>
                    {
                        string key = message.UserGUID.ToString();
                        if (users.ContainsKey(key))
                        {
                            IUser user = users[key];
                            if (user.Lobby == null)
                            {
                                SendKickUserFailedMessage(peer, message, EKickUserFailedReason.InvalidUserGUID);
                            }
                            else if (serverLobby.LobbyCode == user.Lobby.LobbyCode)
                            {
                                if (serverLobby.RemoveUser(user, EDisconnectionReason.Kicked, message.Reason))
                                {
                                    if (serverLobby.UserCount == 0U)
                                    {
                                        RemoveServerLobby(serverLobby);
                                    }
                                }
                                else
                                {
                                    SendKickUserFailedMessage(peer, message, EKickUserFailedReason.FailedExecution);
                                }
                            }
                            else
                            {
                                SendKickUserFailedMessage(peer, message, EKickUserFailedReason.InvalidUserGUID);
                            }
                        }
                    }
                ),
                (peer, message, _) =>
                {
                    if (message.UserGUID == Guid.Empty)
                    {
                        SendKickUserFailedMessage(peer, message, EKickUserFailedReason.InvalidUserGUID);
                    }
                    else
                    {
                        SendKickUserFailedMessage(peer, message, EKickUserFailedReason.Unknown);
                    }
                },
                MessageParseFailedEvent<KickUserMessageData>
            );
            AddMessageParser<StartGameMessageData>
            (
                (peer, message, json) => AssertPeerIsLobbyOwner<StartGameMessageData>
                (
                    peer,
                    (serverUser, serverLobby) =>
                    {
                        if (serverLobby.CurrentlyLoadedGameMode == null)
                        {
                            if (message.Time <= double.Epsilon)
                            {
                                serverLobby.StartNewGameModeInstance();
                            }
                            else
                            {
                                serverLobby.RemainingGameStartTime = message.Time;
                                serverLobby.SendGameStartRequestedMessage(message.Time);
                            }
                        }
                        else
                        {
                            SendStartGameFailedMessage(peer, message, EStartGameFailedReason.GameModeIsAlreadyRunning);
                        }
                    }
                ),
                (peer, message, _) =>
                {
                    if (message.Time < 0.0)
                    {
                        SendStartGameFailedMessage(peer, message, EStartGameFailedReason.NegativeTime);
                    }
                    else
                    {
                        SendStartGameFailedMessage(peer, message, EStartGameFailedReason.Unknown);
                    }
                },
                MessageParseFailedEvent<StartGameMessageData>
            );
            AddMessageParser<RestartGameMessageData>
            (
                (peer, message, json) => AssertPeerIsLobbyOwner<RestartGameMessageData>
                (
                    peer,
                    (serverUser, serverLobby) =>
                    {
                        if (serverLobby.CurrentlyLoadedGameMode == null)
                        {
                            SendRestartGameFailedMessage(peer, message, ERestartGameFailedReason.GameModeIsNotRunning);
                        }
                        else
                        {
                            if (message.Time <= double.Epsilon)
                            {
                                serverLobby.StartNewGameModeInstance();
                            }
                            else
                            {
                                serverLobby.RemainingGameStartTime = message.Time;
                                serverLobby.SendGameRestartRequestedMessage(message.Time);
                            }
                        }
                    }
                ),
                (peer, message, _) =>
                {
                    if (message.Time < 0.0)
                    {
                        SendRestartGameFailedMessage(peer, message, ERestartGameFailedReason.NegativeTime);
                    }
                    else
                    {
                        SendRestartGameFailedMessage(peer, message, ERestartGameFailedReason.Unknown);
                    }
                },
                MessageParseFailedEvent<RestartGameMessageData>
            );
            AddMessageParser<StopGameMessageData>
            (
                (peer, message, json) => AssertPeerIsLobbyOwner<StopGameMessageData>
                (
                    peer,
                    (serverUser, serverLobby) =>
                    {
                        if (serverLobby.CurrentlyLoadedGameMode == null)
                        {
                            SendStopGameFailedMessage(peer, message, EStopGameFailedReason.GameModeIsNotRunning);
                        }
                        else if (message.Time <= double.Epsilon)
                        {
                            serverLobby.StopGameModeInstance();
                        }
                        else
                        {
                            serverLobby.RemainingGameStopTime = message.Time;
                            serverLobby.SendGameStopRequestedMessage(message.Time);
                        }
                    }
                ),
                (peer, message, _) =>
                {
                    if (message.Time < 0.0)
                    {
                        SendStopGameFailedMessage(peer, message, EStopGameFailedReason.NegativeTime);
                    }
                    else
                    {
                        SendStopGameFailedMessage(peer, message, EStopGameFailedReason.Unknown);
                    }
                },
                MessageParseFailedEvent<StopGameMessageData>
            );
            AddAutomaticMessageParser<CancelStartRestartStopGameTimerMessageData>
            (
                (peer, message, json) => AssertPeerIsLobbyOwner<CancelStartRestartStopGameTimerMessageData>
                (
                    peer,
                    (serverUser, serverLobby) =>
                    {
                        if (serverLobby.CurrentlyLoadedGameMode == null)
                        {
                            if (serverLobby.RemainingGameStartTime > 0.0f)
                            {
                                serverLobby.CancelStartGameTime();
                            }
                            else
                            {
                                SendCancelStartRestartStopGameTimerFailedMessage(peer, message, ECancelStartRestartStopGameTimerFailedReason.GameStartTimerIsNotRunning);
                            }
                        }
                        else
                        {
                            if (serverLobby.RemainingGameStartTime > 0.0f)
                            {
                                serverLobby.CancelStartGameTime();
                            }
                            else if (serverLobby.RemainingGameStopTime > 0.0f)
                            {
                                serverLobby.CancelStopGameTime();
                            }
                            else
                            {
                                SendCancelStartRestartStopGameTimerFailedMessage(peer, message, ECancelStartRestartStopGameTimerFailedReason.GameRestartStopTimersAreNotRunning);
                            }
                        }
                    }
                )
            );
            AddMessageParser<ClientGameLoadingProcessFinishedMessageData>
            (
                (peer, message, _) => AssertPeerIsInRunningGame<ClientGameLoadingProcessFinishedMessageData>
                (
                    peer,
                    (serverUser, serverLobby, gameMode) =>
                    {
                        if (serverLobby.AddGameUserInternally(serverUser))
                        {
                            serverUser.InvokeClientGameLoadingProcessFinishedEvent();
                        }
                        else
                        {
                            SendMessageToPeer(peer, new ClientGameLoadingProcessFinishedFailedMessageData(message, EClientGameLoadingProcessFinishedFailedReason.Unknown));
                        }
                    }
                ),
                (peer, message, _) => SendMessageToPeer(peer, new ClientGameLoadingProcessFinishedFailedMessageData(message, EClientGameLoadingProcessFinishedFailedReason.Unknown)),
                MessageParseFailedEvent<ClientGameLoadingProcessFinishedMessageData>
            );
            AddMessageParser<ClientTickMessageData>
            (
                (peer, message, _) =>
                AssertPeerIsInRunningGame<ClientTickMessageData>
                (
                    peer,
                    (serverUser, serverLobby, gameMode) =>
                    {
                        bool is_successful = true;
                        entityDeltas.Clear();
                        hits.Clear();
                        if (message.Entities != null)
                        {
                            foreach (EntityData entity in message.Entities)
                            {
                                if ((entity != null) && entity.IsValid)
                                {
                                    string key = entity.GUID.ToString();
                                    if (serverLobby.Entities.ContainsKey(key) && serverLobby.Entities[key] is IGameEntity game_entity)
                                    {
                                        bool is_simulating = true;
                                        float magnitude_squared = (game_entity.Position - serverUser.Position).MagnitudeSquared;
                                        foreach (IGameUser user in serverLobby.GameUsers.Values)
                                        {
                                            if ((user.GUID != serverUser.GUID) && ((game_entity.Position - user.Position).MagnitudeSquared < magnitude_squared))
                                            {
                                                is_simulating = false;
                                                break;
                                            }
                                        }
                                        if (is_simulating)
                                        {
                                            SimulateEntity(game_entity, (EntityDelta)entity);
                                        }
                                    }
                                    else if ((serverUser.GUID == entity.GUID) && serverLobby.GameUsers.ContainsKey(key))
                                    {
                                        SimulateEntity(serverLobby.GameUsers[key], (EntityDelta)entity);
                                    }
                                }
                            }
                        }
                        if (message.Hits != null)
                        {
                            foreach (ClientHitData client_hit in message.Hits)
                            {
                                string key = client_hit.VictimGUID.ToString();
                                IEntity victim;
                                if (serverLobby.Users.ContainsKey(key))
                                {
                                    victim = serverLobby.Users[key];
                                }
                                else if (serverLobby.Entities.ContainsKey(key))
                                {
                                    victim = serverLobby.Entities[key];
                                }
                                else
                                {
                                    is_successful = false;
                                    SendInvalidMessageParametersErrorMessageToPeer<ClientTickMessageData>(peer, $"Invalid victim GUID \"{ key }\".");
                                    break;
                                }
                                IHit hit = new Hit(serverUser, victim, client_hit.WeaponName, new Vector3(client_hit.HitPosition.X, client_hit.HitPosition.Y, client_hit.HitPosition.Z), new Vector3(client_hit.HitForce.X, client_hit.HitForce.Y, client_hit.HitForce.Z), client_hit.Damage);
                                hits.Add(hit);
                            }
                        }
                        if (is_successful)
                        {
                            foreach (IHit hit in hits)
                            {
                                serverLobby.PerformHit(hit);
                            }
                            serverUser.InvokeClientTickedEvent(entityDeltas, hits);
                        }
                    }
                ),
                (peer, message, _) =>
                {
                    if ((message.Entities != null) && !Protection.IsValid(message.Entities))
                    {
                        SendClientTickFailedMessage(peer, message, EClientTickFailedReason.InvalidEntities);
                    }
                    else
                    {
                        SendClientTickFailedMessage(peer, message, EClientTickFailedReason.Unknown);
                    }
                },
                MessageParseFailedEvent<ClientTickMessageData>
            );
            AddAutomaticMessageParser<ServerTickFailedMessageData>((peer, message, json) => OnServerTickFailed?.Invoke(peer, message.Message, message.Reason));
            OnPeerDisconnected += (peer) =>
            {
                string key = peer.GUID.ToString();
                if (users.ContainsKey(key))
                {
                    IUser user = users[key];
                    if (user.Lobby is IServerLobby server_lobby)
                    {
                        if (server_lobby.RemoveUser(user, EDisconnectionReason.Disconnected, "User has been disconnected.") && (server_lobby.UserCount == 0U))
                        {
                            RemoveServerLobby(server_lobby);
                        }
                    }
                    users.Remove(key);
                }
            };
            tickStopwatch.Start();
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
        /// Asserts peer is authentificated
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="peer">Peer</param>
        /// <param name="onPeerIsAuthentificated">On peer is authentificated</param>
        private void AssertPeerIsAuthentificated<T>(IPeer peer, PeerIsAuthentificatedDelegate onPeerIsAuthentificated) where T : IBaseMessageData
        {
            string key = peer.GUID.ToString();
            if (users.ContainsKey(key))
            {
                if (users[key] is IInternalServerUser server_user)
                {
                    onPeerIsAuthentificated(server_user);
                }
                else
                {
                    SendUnknownErrorMessageToPeer<T>(peer, $"User with GUID \"{ key }\" does not inherit from \"{ nameof(IInternalServerUser) }\".");
                }
            }
            else
            {
                SendErrorMessageToPeer<T>(peer, EErrorType.InvalidMessageContext, "User is not authentificated yet.", true);
            }
        }

        /// <summary>
        /// Asserts peer is not in lobby
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="peer">Peer</param>
        /// <param name="onPeerIsInLobby">On peer is not in lobby</param>
        private void AssertPeerIsNotInLobby<T>(IPeer peer, PeerIsNotInLobbyDelegate onPeerIsNotInLobby) where T : IBaseMessageData =>
            AssertPeerIsAuthentificated<T>(peer, (serverUser) =>
            {
                if (serverUser.ServerLobby == null)
                {
                    onPeerIsNotInLobby(serverUser);
                }
                else
                {
                    SendInvalidMessageContextErrorMessageToPeer<T>(peer, $"User \"{ serverUser.Name }\" with GUID \"{ serverUser.GUID }\" is already in lobby \"{ serverUser.ServerLobby.Name }\" with lobby code \"{ serverUser.ServerLobby.LobbyCode }\".");
                }
            });

        /// <summary>
        /// Asserts peer is in lobby
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="peer">Peer</param>
        /// <param name="onPeerIsInLobby">On peer is in lobby</param>
        private void AssertPeerIsInLobby<T>(IPeer peer, PeerIsInLobbyDelegate onPeerIsInLobby) where T : IBaseMessageData =>
            AssertPeerIsAuthentificated<T>(peer, (serverUser) =>
            {
                if (serverUser.ServerLobby == null)
                {
                    SendInvalidMessageContextErrorMessageToPeer<T>(peer, $"User \"{ serverUser.Name }\" with GUID \"{ serverUser.GUID }\" is not in a lobby.");
                }
                else
                {
                    onPeerIsInLobby(serverUser, serverUser.ServerLobby);
                }
            });

        /// <summary>
        /// Asserts peer is lobby owner
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="peer">Peer</param>
        /// <param name="onPeerIsLobbyOwner">On peer is lobby owner</param>
        private void AssertPeerIsLobbyOwner<T>(IPeer peer, PeerIsLobbyOwnerDelegate onPeerIsLobbyOwner) where T : IBaseMessageData =>
            AssertPeerIsInLobby<T>(peer, (serverUser, serverLobby) =>
            {
                if (serverUser.GUID == serverLobby.Owner.GUID)
                {
                    onPeerIsLobbyOwner(serverUser, serverLobby);
                }
                else
                {
                    SendInvalidMessageContextErrorMessageToPeer<T>(peer, $"User \"{ serverUser.Name }\" with GUID \"{ serverUser.GUID }\" is not the lobby owner of \"{ serverLobby.Name }\" with lobby code \"{ serverLobby.LobbyCode }\".");
                }
            });

        /// <summary>
        /// Asserts peer is in a running game
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="peer">Peer</param>
        /// <param name="onPeerIsInRunningGame">On peer is in running game</param>
        private void AssertPeerIsInRunningGame<T>(IPeer peer, PeerIsInRunningGameDelegate onPeerIsInRunningGame) where T : IBaseMessageData =>
            AssertPeerIsInLobby<T>(peer, (serverUser, serverLobby) =>
            {
                if (serverLobby.CurrentlyLoadedGameMode == null)
                {
                    SendInvalidMessageContextErrorMessageToPeer<T>(peer, $"User \"{ serverUser.Name }\" with GUID \"{ serverUser.GUID }\" lobby \"{ serverLobby.Name }\" with lobby code \"{ serverLobby.LobbyCode }\" is not in a running game.");
                }
                else
                {
                    onPeerIsInRunningGame(serverUser, serverLobby, serverLobby.CurrentlyLoadedGameMode);
                }
            });

        /// <summary>
        /// Simulates game entities
        /// </summary>
        /// <param name="gameEntity">Game entity</param>
        /// <param name="entityDelta">Entity delta</param>
        private void SimulateEntity(IGameEntity gameEntity, EntityDelta entityDelta)
        {
            if (entityDelta.IsSpectating != null)
            {
                gameEntity.SetSpectatingState(entityDelta.IsSpectating.Value, true);
            }
            if (entityDelta.GameColor != null)
            {
                gameEntity.SetGameColor(entityDelta.GameColor.Value, true);
            }
            if (entityDelta.Position != null)
            {
                gameEntity.SetPosition(entityDelta.Position.Value, true);
            }
            if (entityDelta.Rotation != null)
            {
                gameEntity.SetRotation(entityDelta.Rotation.Value, true);
            }
            if (entityDelta.Velocity != null)
            {
                gameEntity.SetVelocity(entityDelta.Velocity.Value, true);
            }
            if (entityDelta.AngularVelocity != null)
            {
                gameEntity.SetAngularVelocity(entityDelta.AngularVelocity.Value, true);
            }
            if (entityDelta.Actions != null)
            {
                gameEntity.SetActions(entityDelta.Actions, true);
            }
            entityDeltas.Add(entityDelta);
        }

        /// <summary>
        /// Sends an authentification failed message
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="message">Received message</param>
        /// <param name="reason">Reason</param>
        /// <param name="isFatal">Is authentification fail fatal</param>
        private void SendAuthentificationFailedMessage(IPeer peer, AuthenticateMessageData message, EAuthentificationFailedReason reason, bool isFatal)
        {
            OnAuthentificationFailed?.Invoke(peer, message, EAuthentificationFailedReason.AlreadyAuthenticated);
            SendMessageToPeer(peer, new AuthentificationFailedMessageData(message, reason));
            if (isFatal)
            {
                peer.Disconnect(EDisconnectionReason.Error);
            }
        }

        /// <summary>
        /// Sends an authentification failed message
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="message">Received message</param>
        /// <param name="reason">Reason</param>
        private void SendAuthentificationFailedMessage(IPeer peer, AuthenticateMessageData message, EAuthentificationFailedReason reason) => SendAuthentificationFailedMessage(peer, message, reason, false);

        /// <summary>
        /// Sends a join lobby failed message
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="message">Received message</param>
        /// <param name="reason">Reason</param>
        private void SendJoinLobbyFailedMessage(IPeer peer, JoinLobbyMessageData message, EJoinLobbyFailedReason reason)
        {
            OnJoinLobbyFailed?.Invoke(peer, message, reason);
            SendMessageToPeer(peer, new JoinLobbyFailedMessageData(message, reason));
        }

        /// <summary>
        /// Sends a create lobby failed message
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="message">Received message</param>
        /// <param name="reason">Reason</param>
        private void SendCreateLobbyFailedMessage(IPeer peer, CreateAndJoinLobbyMessageData message, ECreateLobbyFailedReason reason)
        {
            OnCreateLobbyFailed?.Invoke(peer, message, reason);
            SendMessageToPeer(peer, new CreateLobbyFailedMessageData(message, reason));
        }

        /// <summary>
        /// Sends a change username failed message
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="message">Received message</param>
        /// <param name="reason">Reason</param>
        private void SendChangeUsernameFailedMessage(IPeer peer, ChangeUsernameMessageData message, EChangeUsernameFailedReason reason)
        {
            OnChangeUsernameFailed?.Invoke(peer, message, reason);
            SendMessageToPeer(peer, new ChangeUsernameFailedMessageData(message, reason));
        }

        /// <summary>
        /// Sends a change lobby rules failed message
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="message">Received message</param>
        /// <param name="reason">Reason</param>
        private void SendChangeLobbyRulesFailedMessage(IPeer peer, ChangeLobbyRulesMessageData message, EChangeLobbyRulesFailedReason reason)
        {
            OnChangeLobbyRulesFailed?.Invoke(peer, message, reason);
            SendMessageToPeer(peer, new ChangeLobbyRulesFailedMessageData(message, reason));
        }

        /// <summary>
        /// Sends a kick user failed message
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="message">Message</param>
        /// <param name="reason">Reason</param>
        private void SendKickUserFailedMessage(IPeer peer, KickUserMessageData message, EKickUserFailedReason reason)
        {
            OnKickUserFailed?.Invoke(peer, message, reason);
            SendMessageToPeer(peer, new KickUserFailedMessageData(message, reason));
        }

        /// <summary>
        /// Sends a start game failed message
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="message">Received message</param>
        /// <param name="reason">Reason</param>
        private void SendStartGameFailedMessage(IPeer peer, StartGameMessageData message, EStartGameFailedReason reason)
        {
            OnStartGameFailed?.Invoke(peer, message, reason);
            SendMessageToPeer(peer, new StartGameFailedMessageData(message, reason));
        }

        /// <summary>
        /// Sends a restart game failed message
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="message">Received message</param>
        /// <param name="reason">Reason</param>
        private void SendRestartGameFailedMessage(IPeer peer, RestartGameMessageData message, ERestartGameFailedReason reason)
        {
            OnRestartGameFailed?.Invoke(peer, message, reason);
            SendMessageToPeer(peer, new RestartGameFailedMessageData(message, reason));
        }

        /// <summary>
        /// Sends a stop game failed message
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="message">Received message</param>
        /// <param name="reason">Reason</param>
        private void SendStopGameFailedMessage(IPeer peer, StopGameMessageData message, EStopGameFailedReason reason)
        {
            OnStopGameFailed?.Invoke(peer, message, reason);
            SendMessageToPeer(peer, new StopGameFailedMessageData(message, reason));
        }

        /// <summary>
        /// Sends a cancel start, restart and stop game timer failed message
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="message">Received message</param>
        /// <param name="reason">Reason</param>
        private void SendCancelStartRestartStopGameTimerFailedMessage(IPeer peer, CancelStartRestartStopGameTimerMessageData message, ECancelStartRestartStopGameTimerFailedReason reason)
        {
            OnCancelStartRestartStopGameTimerFailed?.Invoke(peer, message, reason);
            SendMessageToPeer(peer, new CancelStartRestartStopGameTimerFailedMessageData(message, reason));
        }

        /// <summary>
        /// Sends a client tick failed message
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="message">Received message</param>
        /// <param name="reason">Reason</param>
        private void SendClientTickFailedMessage(IPeer peer, ClientTickMessageData message, EClientTickFailedReason reason)
        {
            OnClientTickFailed?.Invoke(peer, message, reason);
            SendMessageToPeer(peer, new ClientTickFailedMessageData(message, reason));
        }

        /// <summary>
        /// Gets user by GUID
        /// </summary>
        /// <param name="guid">User GUID</param>
        /// <returns>User if user is available, otherwise "null"</returns>
        public IUser GetUserByGUID(Guid guid) => TryGetUserByGUID(guid, out IUser ret) ? ret : null;

        /// <summary>
        /// Tries to get user by GUID
        /// </summary>
        /// <param name="guid">User GUID</param>
        /// <param name="user">User</param>
        /// <returns>"true" if user is available, otherwise "false"</returns>
        public bool TryGetUserByGUID(Guid guid, out IUser user) => users.TryGetValue(guid.ToString(), out user);

        /// <summary>
        /// Gets lobby by lobby code
        /// </summary>
        /// <param name="lobbyCode">Lobby code</param>
        /// <returns>Lobby if lobby is available, otherwise "null"</returns>
        public ILobby GetLobbyByLobbyCode(string lobbyCode) => TryGetLobbyByLobbyCode(lobbyCode, out ILobby ret) ? ret : null;

        /// <summary>
        /// Tries to get lobby by lobby code
        /// </summary>
        /// <param name="lobbyCode">Lobby code</param>
        /// <param name="lobby">Lobby</param>
        /// <returns>"true" if lobby is available, otherwise "false"</returns>
        public bool TryGetLobbyByLobbyCode(string lobbyCode, out ILobby lobby) => lobbies.TryGetValue(lobbyCode ?? throw new ArgumentNullException(nameof(lobbyCode)), out lobby);

        /// <summary>
        /// Adds a new game resource
        /// </summary>
        /// <typeparam name="T">Game resource type</typeparam>
        /// <returns>"true" if the specified game resource has been added, otherwise "false"</returns>
        public bool AddGameResource<T>() where T : IGameResource => AddGameResource(typeof(T));

        /// <summary>
        /// Adds a new game resource
        /// </summary>
        /// <param name="gameResourceType">Game resource type</param>
        /// <returns>"true" if the specified game resource has been added, otherwise "false"</returns>
        public bool AddGameResource(Type gameResourceType)
        {
            if (gameResourceType == null)
            {
                throw new ArgumentNullException(nameof(gameResourceType));
            }
            string key = gameResourceType.FullName;
            if (!typeof(IGameResource).IsAssignableFrom(gameResourceType))
            {
                throw new InvalidOperationException($"Game resource type \"{ key }\" does not inherit from \"{ typeof(IGameResource).FullName }\".");
            }
            if (!gameResourceType.IsClass)
            {
                throw new InvalidOperationException($"Game resource type \"{ key }\" is not a class.");
            }
            if (gameResourceType.IsAbstract)
            {
                throw new InvalidOperationException($"Game resource type \"{ key }\" is abstract.");
            }
            bool has_no_default_constructor = false;
            foreach (ConstructorInfo constructor in gameResourceType.GetConstructors(BindingFlags.Public))
            {
                has_no_default_constructor = true;
                if (constructor.GetParameters().Length <= 0)
                {
                    has_no_default_constructor = false;
                    break;
                }
            }
            if (has_no_default_constructor)
            {
                throw new InvalidOperationException($"Game resource type \"{ key }\" has no default constructor.");
            }
            bool ret = false;
            if (!gameResources.ContainsKey(key))
            {
                IGameResource game_resource = (IGameResource)Activator.CreateInstance(gameResourceType);
                if (game_resource.AvailableGameModeTypes == null)
                {
                    throw new NullReferenceException($"Available game mode types is null.");
                }
                if (!Protection.IsValid(game_resource.AvailableGameModeTypes.Values))
                {
                    throw new InvalidOperationException($"Available game mode types are not valid.");
                }
                foreach (KeyValuePair<string, Type> game_mode_type in game_resource.AvailableGameModeTypes)
                {
                    if (availableGameModeTypes.ContainsKey(game_mode_type.Key))
                    {
                        Console.Error.WriteLine($"Skipping duplicate available game mode \"{ game_mode_type.Key }\".");
                    }
                    else
                    {
                        availableGameModeTypes.Add(game_mode_type.Key, (game_resource, game_mode_type.Value));
                    }
                }
                gameResources.Add(key, game_resource);
                game_resource.OnInitialized(this);
                ret = true;
            }
            return ret;
        }

        /// <summary>
        /// Gets a game resources of type
        /// </summary>
        /// <typeparam name="T">Game resource type</typeparam>
        /// <returns>Game resource if available, otherwise "null"</returns>
        public IGameResource GetGameResourceOfType<T>() where T : IGameResource => TryGetGameResourceOfType(out T ret) ? ret : default;

        /// <summary>
        /// Tries to get a game resource by type
        /// </summary>
        /// <typeparam name="T">Game resource type</typeparam>
        /// <param name="gameResource">Game resource</param>
        /// <returns>"true" if game resource is available, otherwise "false"</returns>
        public bool TryGetGameResourceOfType<T>(out T gameResource) where T : IGameResource
        {
            bool ret = gameResources.TryGetValue(typeof(T).FullName, out IGameResource game_resource);
            gameResource = ret ? (T)game_resource : default;
            return ret;
        }

        /// <summary>
        /// Is game mode available
        /// </summary>
        /// <param name="gameMode">Game mode</param>
        /// <returns>"true" if game mode is available, otherwise "false"</returns>
        public bool IsGameModeAvailable(string gameMode) => availableGameModeTypes.ContainsKey(gameMode ?? throw new ArgumentNullException(nameof(gameMode)));

        /// <summary>
        /// Removes the specified game resource
        /// </summary>
        /// <typeparam name="T">Game resource type</typeparam>
        /// <returns>"true" if the specified game resource has been removed, otherwise "false"</returns>
        public bool RemoveGameResource<T>() where T : IGameResource => RemoveGameResource(typeof(T));

        /// <summary>
        /// Removes the specified game resource
        /// </summary>
        /// <param name="gameResourceType">Game resource type</param>
        /// <returns>"true" if the specified game resource has been removed, otherwise "false"</returns>
        public bool RemoveGameResource(Type gameResourceType)
        {
            if (gameResourceType == null)
            {
                throw new ArgumentNullException(nameof(gameResourceType));
            }
            bool ret = false;
            string key = gameResourceType.FullName;
            if (gameResources.ContainsKey(key))
            {
                IGameResource game_resource = gameResources[key];
                HashSet<string> remove_available_game_mode_types = new HashSet<string>();
                game_resource.OnClosed();
                foreach (KeyValuePair<string, (IGameResource, Type)> available_game_mode_type in availableGameModeTypes)
                {
                    if (available_game_mode_type.Value.Item1 == game_resource)
                    {
                        remove_available_game_mode_types.Add(available_game_mode_type.Key);
                    }
                }
                foreach (string remove_game_mode_type in remove_available_game_mode_types)
                {
                    availableGameModeTypes.Remove(remove_game_mode_type);
                }
                remove_available_game_mode_types.Clear();
                ret = gameResources.Remove(key);
            }
            return ret;
        }

        /// <summary>
        /// Removes server lobby
        /// </summary>
        /// <param name="serverLobby">Server lobby</param>
        /// <returns>"true" if server lobby was successfully removed, otherwise "false"</returns>
        public bool RemoveServerLobby(IServerLobby serverLobby)
        {
            if (serverLobby == null)
            {
                throw new ArgumentNullException(nameof(serverLobby));
            }
            if (!serverLobby.IsValid)
            {
                throw new ArgumentException("Server lobby is not valid.", nameof(serverLobby));
            }
            return lobbies.Remove(serverLobby.LobbyCode);
        }

        /// <summary>
        /// Processes all events appeared since last call
        /// </summary>
        public void ProcessEvents()
        {
            foreach (IConnector connector in Connectors)
            {
                connector.ProcessEvents();
            }
            tickStopwatch.Stop();
            TimeSpan delta_time = tickStopwatch.Elapsed;
            foreach (ILobby lobby in lobbies.Values)
            {
                if (lobby is IInternalServerLobby server_lobby)
                {
                    server_lobby.Tick(delta_time);
                }
            }
            tickStopwatch.Restart();
        }

        /// <summary>
        /// Closes connections to all peers
        /// </summary>
        /// <param name="reason">Disconnection reason</param>
        public override void Close(EDisconnectionReason reason)
        {
            base.Close(reason);
            foreach (ILobby lobby in lobbies.Values)
            {
                if (lobby is IServerLobby server_lobby)
                {
                    server_lobby.Close(reason);
                }
            }
            lobbies.Clear();
            availableGameModeTypes.Clear();
            foreach (IGameResource game_resource in gameResources.Values)
            {
                game_resource.OnClosed();
            }
            gameResources.Clear();
        }
    }
}
