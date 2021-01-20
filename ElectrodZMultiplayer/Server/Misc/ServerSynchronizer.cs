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
        /// Tick stopwatch
        /// </summary>
        private Stopwatch tickStopwatch = new Stopwatch();

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
        /// This event will be invoked when lobbies have been listed.
        /// </summary>
        public override event LobbiesListedDelegate OnLobbiesListed;

        /// <summary>
        /// This event will be invoked when available game modes have been listed.
        /// </summary>
        public override event AvailableGameModesListedDelegate OnAvailableGameModesListed;

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
                    SendErrorMessageToPeer(peer, EErrorType.InvalidMessageContext, "User is already authentificated");
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
                    OnAuthentificationAcknowledged?.Invoke(server_user);
                    server_user.SendAuthentificationAcknowledgedMessage();
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
                        OnAuthentificationAcknowledged?.Invoke(server_user);
                        server_user.SendAuthentificationAcknowledgedMessage();
                        // TODO: Establish state back after returning from the dead.
                    }
                }
            }, FatalMessageParseFailedEvent);
            AddMessageParser<ListLobbiesMessageData>((peer, message, json) =>
                AssertPeerIsAuthentificated(peer, (serverUser) =>
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
                    OnLobbiesListed?.Invoke(lobby_list);
                    serverUser.SendListLobbyResultsMessage(lobby_list);
                    lobby_list.Clear();
                }), MessageParseFailedEvent);
            AddMessageParser<ListAvailableGameModesMessageData>((peer, message, json) =>
                AssertPeerIsAuthentificated(peer, (serverUser) =>
                {
                    HashSet<string> available_game_modes = new HashSet<string>();
                    string name = message.Name ?? string.Empty;
                    foreach (string available_game_mode in availableGameModeTypes.Keys)
                    {
                        if (available_game_mode.Contains(name))
                        {
                            available_game_modes.Add(available_game_mode);
                        }
                    }
                    OnAvailableGameModesListed?.Invoke(available_game_modes);
                    serverUser.SendListAvailableGameModeResultsMessage(available_game_modes);
                    available_game_modes.Clear();
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
                                server_lobby.AddUser(serverUser);
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
                    if (availableGameModeTypes.ContainsKey(message.GameMode))
                    {
                        string lobby_code;
                        do
                        {
                            lobby_code = Randomizer.GetRandomString(6U, humanFriendlyLobbyCodeCharacters);
                        }
                        while (lobbies.ContainsKey(lobby_code));
                        IInternalServerLobby server_lobby = new ServerLobby(lobby_code, message.LobbyName, message.MinimalUserCount ?? Defaults.minimalUserCount, message.MaximalUserCount ?? Defaults.maximalUserCount, message.IsStartingGameAutomatically ?? Defaults.isStartingGameAutomatically, availableGameModeTypes[message.GameMode], message.GameModeRules, this, serverUser);
                        lobbies.Add(lobby_code, server_lobby);
                        serverUser.ServerLobby = server_lobby;
                        serverUser.SetNameInternally(message.Username);
                        server_lobby.AddUser(serverUser);
                        serverUser.SendJoinLobbyAcknowledgedMessage(server_lobby);
                    }
                    else
                    {
                        serverUser.SendErrorMessage(EErrorType.InvalidMessageParameters, $"Game mode \"{ message.GameMode }\" is not available.");
                    }
                }), MessageParseFailedEvent);
            AddMessageParser<QuitLobbyMessageData>((peer, message, json) =>
                AssertPeerIsInLobby(peer, (serverUser, serverLobby) =>
                {
                    serverLobby.RemoveUser(serverUser, "User has left the lobby.");
                    serverUser.ServerLobby = null;
                }), MessageParseFailedEvent);
            AddMessageParser<ChangeUsernameMessageData>((peer, message, json) => AssertPeerIsInLobby(peer, (serverUser, serverLobby) => serverUser.UpdateUsername(message.NewUsername.Trim())), MessageParseFailedEvent);
            AddMessageParser<ChangeUserLobbyColorMessageData>((peer, message, json) => AssertPeerIsInLobby(peer, (serverUser, serverLobby) => serverUser.UpdateUserLobbyColor(message.NewUserLobbyColor)), MessageParseFailedEvent);
            AddMessageParser<ChangeLobbyRulesMessageData>((peer, message, json) =>
                AssertPeerIsLobbyOwner(peer, (serverUser, serverLobby) =>
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
                        SendErrorMessageToPeer(peer, EErrorType.InvalidMessageParameters, $"Game mode \"{ message.GameMode }\" is not available.");
                    }
                }), MessageParseFailedEvent);
            AddMessageParser<KickUserMessageData>((peer, message, json) =>
                AssertPeerIsLobbyOwner(peer, (serverUser, serverLobby) =>
                {
                    string key = message.UserGUID.ToString();
                    if (users.ContainsKey(key))
                    {
                        IUser user = users[key];
                        if (user.Lobby == null)
                        {
                            serverUser.SendErrorMessage(EErrorType.InternalError, $"User with GUID \"{ user.GUID }\" does not exist.");
                        }
                        else if (serverLobby.LobbyCode == user.Lobby.LobbyCode)
                        {
                            if (!serverLobby.RemoveUser(user, message.Reason))
                            {
                                serverUser.SendErrorMessage(EErrorType.InternalError, $"Failed to remove user \"{ user.Name }\" with GUID \"{ user.GUID }\" from lobby \"{ serverLobby.Name }\" with lobby code \"{ serverLobby.LobbyCode }\".");
                            }
                        }
                        else
                        {
                            serverUser.SendErrorMessage(EErrorType.InternalError, $"User with GUID \"{ user.GUID }\" does not exist.");
                        }
                    }
                }), MessageParseFailedEvent);
            AddMessageParser<StartGameMessageData>((peer, message, json) =>
                AssertPeerIsLobbyOwner(peer, (serverUser, serverLobby) =>
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
                        serverUser.SendErrorMessage(EErrorType.InvalidMessageContext, "Game mode is already running.");
                    }
                }), MessageParseFailedEvent);
            AddMessageParser<RestartGameMessageData>((peer, message, json) =>
                AssertPeerIsLobbyOwner(peer, (serverUser, serverLobby) =>
                {
                    if (serverLobby.CurrentlyLoadedGameMode == null)
                    {
                        serverUser.SendErrorMessage(EErrorType.InvalidMessageContext, "Game mode is not runnning yet.");
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
                }), MessageParseFailedEvent);
            AddMessageParser<StopGameMessageData>((peer, message, json) =>
                AssertPeerIsLobbyOwner(peer, (serverUser, serverLobby) =>
                {
                    if (serverLobby.CurrentlyLoadedGameMode == null)
                    {
                        serverUser.SendErrorMessage(EErrorType.InvalidMessageContext, $"User \"{ serverUser.Name }\" with GUID \"{ serverUser.GUID }\" lobby \"{ serverLobby.Name }\" with lobby code \"{ serverLobby.LobbyCode }\" is not in a running game.");
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
                }), MessageParseFailedEvent);
            AddMessageParser<ClientTickMessageData>((peer, message, json) =>
                AssertPeerIsInRunningGame(peer, (serverUser, serverLobby, gameMode) =>
                {
                    entityDeltas.Clear();
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
                                    foreach (IUser user in serverLobby.Users.Values)
                                    {
                                        if ((user.GUID != serverUser.GUID) && ((game_entity.Position - user.Position).MagnitudeSquared < magnitude_squared))
                                        {
                                            is_simulating = false;
                                            break;
                                        }
                                    }
                                    if (is_simulating)
                                    {
                                        EntityDelta entity_delta = (EntityDelta)entity;
                                        if (entity_delta.GameColor != null)
                                        {
                                            game_entity.SetGameColor(entity_delta.GameColor.Value);
                                        }
                                        if (entity_delta.Position != null)
                                        {
                                            game_entity.SetPosition(entity_delta.Position.Value);
                                        }
                                        if (entity_delta.Rotation != null)
                                        {
                                            game_entity.SetRotation(entity_delta.Rotation.Value);
                                        }
                                        if (entity_delta.Velocity != null)
                                        {
                                            game_entity.SetVelocity(entity_delta.Velocity.Value);
                                        }
                                        if (entity_delta.AngularVelocity != null)
                                        {
                                            game_entity.SetAngularVelocity(entity_delta.AngularVelocity.Value);
                                        }
                                        if (entity_delta.Actions != null)
                                        {
                                            game_entity.SetActions(entity_delta.Actions);
                                        }
                                        entityDeltas.Add(entity_delta);
                                    }
                                }
                            }
                        }
                    }
                    serverUser.InvokeClientTickedEvent(entityDeltas);
                }), MessageParseFailedEvent);
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
        /// <param name="peer">Peer</param>
        /// <param name="onPeerIsAuthentificated">On peer is authentificated</param>
        private void AssertPeerIsAuthentificated(IPeer peer, PeerIsAuthentificatedDelegate onPeerIsAuthentificated)
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
                    SendErrorMessageToPeer(peer, EErrorType.InvalidMessageContext, $"User with GUID \"{ key }\" does not inherit from \"{ nameof(IInternalServerUser) }\".");
                }
            }
            else
            {
                SendErrorMessageToPeer(peer, EErrorType.InvalidMessageContext, "User is not authentificated yet.", true);
            }
        }

        /// <summary>
        /// Asserts peer is not in lobby
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="onPeerIsInLobby">On peer is not in lobby</param>
        private void AssertPeerIsNotInLobby(IPeer peer, PeerIsNotInLobbyDelegate onPeerIsNotInLobby) =>
            AssertPeerIsAuthentificated(peer, (serverUser) =>
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
            AssertPeerIsAuthentificated(peer, (serverUser) =>
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
        /// Asserts peer is in a running game
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="onPeerIsInRunningGame">On peer is in running game</param>
        private void AssertPeerIsInRunningGame(IPeer peer, PeerIsInRunningGameDelegate onPeerIsInRunningGame) =>
            AssertPeerIsInLobby(peer, (serverUser, serverLobby) =>
            {
                if (serverLobby.CurrentlyLoadedGameMode == null)
                {
                    serverUser.SendErrorMessage(EErrorType.InvalidMessageContext, $"User \"{ serverUser.Name }\" with GUID \"{ serverUser.GUID }\" lobby \"{ serverLobby.Name }\" with lobby code \"{ serverLobby.LobbyCode }\" is not in a running game.");
                }
                else
                {
                    onPeerIsInRunningGame(serverUser, serverLobby, serverLobby.CurrentlyLoadedGameMode);
                }
            });

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
                    server_lobby.Close();
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
