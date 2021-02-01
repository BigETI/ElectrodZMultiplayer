using ElectrodZMultiplayer.Data.Messages;
using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer server namespace
/// </summary>
namespace ElectrodZMultiplayer.Server
{
    /// <summary>
    /// A class that describes a server lobby
    /// </summary>
    internal class ServerLobby : IInternalServerLobby
    {
        /// <summary>
        /// Game mode rules
        /// </summary>
        private readonly Dictionary<string, object> gameModeRules = new Dictionary<string, object>();

        /// <summary>
        /// Users
        /// </summary>
        private readonly Dictionary<string, IUser> users = new Dictionary<string, IUser>();

        /// <summary>
        /// Entities
        /// </summary>
        public readonly Dictionary<string, IEntity> entities = new Dictionary<string, IEntity>();

        /// <summary>
        /// Game users
        /// </summary>
        private readonly Dictionary<string, IGameUser> gameUsers = new Dictionary<string, IGameUser>();

        /// <summary>
        /// Current hits
        /// </summary>
        private readonly List<IHit> currentHits = new List<IHit>();

        /// <summary>
        /// Game mode type
        /// </summary>
        private (IGameResource, Type) gameModeType;

        /// <summary>
        /// Game user factory
        /// </summary>
        private IGameUserFactory gameUserFactory;

        /// <summary>
        /// Game entity factory
        /// </summary>
        private IGameEntityFactory gameEntityFactory;

        /// <summary>
        /// Server
        /// </summary>
        public IServerSynchronizer Server { get; }

        /// <summary>
        /// Remaining game start time in seconds
        /// </summary>
        public double RemainingGameStartTime { get; set; }


        /// <summary>
        /// Remaining game stop time in seconds
        /// </summary>
        public double RemainingGameStopTime { get; set; }

        /// <summary>
        /// Owner
        /// </summary>
        public IUser Owner { get; }

        /// <summary>
        /// Users
        /// </summary>
        public IReadOnlyDictionary<string, IUser> Users => users;

        /// <summary>
        /// Entities
        /// </summary>
        public IReadOnlyDictionary<string, IEntity> Entities => entities;

        /// <summary>
        /// Lobby code
        /// </summary>
        public string LobbyCode { get; }

        /// <summary>
        /// Lobby name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Minimal user count
        /// </summary>
        public uint MinimalUserCount { get; private set; }

        /// <summary>
        /// Maximal user count
        /// </summary>
        public uint MaximalUserCount { get; private set; }

        /// <summary>
        /// Is starting game automatically
        /// </summary>
        public bool IsStartingGameAutomatically { get; private set; }

        /// <summary>
        /// Game mode
        /// </summary>
        public string GameMode => gameModeType.Item2.FullName;

        /// <summary>
        /// Game mode rules
        /// </summary>
        public IReadOnlyDictionary<string, object> GameModeRules => gameModeRules;

        /// <summary>
        /// User count
        /// </summary>
        public uint UserCount => (uint)users.Count;

        /// <summary>
        /// Current game time in seconds
        /// </summary>
        public double CurrentGameTime { get; private set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public bool IsValid =>
            (Server != null) &&
            (LobbyCode != null) &&
            (Name != null) &&
            (MinimalUserCount <= MaximalUserCount) &&
            (UserCount <= MaximalUserCount) &&
            !string.IsNullOrWhiteSpace(GameMode) &&
            (gameModeRules != null) &&
            (users != null) &&
            Protection.IsValid(gameModeRules.Values) &&
            Protection.IsValid(users.Values);

        /// <summary>
        /// Currently loaded game mode
        /// </summary>
        public IGameMode CurrentlyLoadedGameMode { get; private set; }

        /// <summary>
        /// This event will be invoked when an user has joined this lobby.
        /// </summary>
        public event UserJoinedDelegate OnUserJoined;

        /// <summary>
        /// This event will be invoked when an user left this lobby.
        /// </summary>
        public event UserLeftDelegate OnUserLeft;

        /// <summary>
        /// This event will be invoked when the lobby rules of this lobby has been updated.
        /// </summary>
        public event LobbyRulesUpdatedDelegate OnLobbyRulesUpdated;

        /// <summary>
        /// This event will be invoked when a game start has been requested.
        /// </summary>
        public event GameStartRequestedDelegate OnGameStartRequested;

        /// <summary>
        /// This event will be invoked when a game restart has been requested.
        /// </summary>
        public event GameRestartRequestedDelegate OnGameRestartRequested;

        /// <summary>
        /// This event will be invoked when a game stop has been requested.
        /// </summary>
        public event GameStopRequestedDelegate OnGameStopRequested;

        /// <summary>
        /// This event will be invoked when a game has been started.
        /// </summary>
        public event GameStartedDelegate OnGameStarted;

        /// <summary>
        /// This event will be invoked when a game has been restarted.
        /// </summary>
        public event GameRestartedDelegate OnGameRestarted;

        /// <summary>
        /// This event will be invoked when a game has been stopped.
        /// </summary>
        public event GameStoppedDelegate OnGameStopped;

        /// <summary>
        /// Gets invoked when a game mode has been started
        /// </summary>
        public event GameModeStartedDelegate OnGameModeStarted;

        /// <summary>
        /// Gets invoked when a game mode has been stopped
        /// </summary>
        public event GameModeStoppedDelegate OnGameModeStopped;

        /// <summary>
        /// This event will be invoked when starting a game has been cancelled.
        /// </summary>
        public event StartGameCancelledDelegate OnStartGameCancelled;

        /// <summary>
        /// This event will be invoked when restarting a game has been cancelled.
        /// </summary>
        public event RestartGameCancelledDelegate OnRestartGameCancelled;

        /// <summary>
        /// This event will be invoked when stopping a game has been cancelled.
        /// </summary>
        public event StopGameCancelledDelegate OnStopGameCancelled;

        /// <summary>
        /// Gets invoked when lobby has been closed
        /// </summary>
        public event LobbyClosedDelegate OnLobbyClosed;

        /// <summary>
        /// Constructs a server lobby
        /// </summary>
        /// <param name="lobbyCode">Lobby code</param>
        /// <param name="name">Lobby name</param>
        /// <param name="minimalUserCount">Minimal user count</param>
        /// <param name="maximalUserCount">Maximal user count</param>
        /// <param name="isStartingGameAutomatically">Is starting game automatically</param>
        /// <param name="gameModeType">Game mode type</param>
        /// <param name="gameModeRules">Game mode rules</param>
        /// <param name="server">Server</param>
        public ServerLobby(string lobbyCode, string name, uint minimalUserCount, uint maximalUserCount, bool isStartingGameAutomatically, (IGameResource, Type) gameModeType, IReadOnlyDictionary<string, object> gameModeRules, IServerSynchronizer server, IUser owner)
        {
            if (minimalUserCount > maximalUserCount)
            {
                throw new ArgumentException("Minimal user count can't be greater than maximal user count.");
            }
            if (gameModeType.Item1 == null)
            {
                throw new ArgumentNullException(nameof(gameModeType.Item1));
            }
            if (gameModeType.Item2 == null)
            {
                throw new ArgumentNullException(nameof(gameModeType.Item2));
            }
            LobbyCode = lobbyCode ?? throw new ArgumentNullException(nameof(lobbyCode));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            MinimalUserCount = minimalUserCount;
            MaximalUserCount = maximalUserCount;
            IsStartingGameAutomatically = isStartingGameAutomatically;
            this.gameModeType = gameModeType;
            if (gameModeRules != null)
            {
                foreach (KeyValuePair<string, object> game_mode_rule in gameModeRules)
                {
                    if (game_mode_rule.Value == null)
                    {
                        throw new ArgumentException($"Value of key \"{ game_mode_rule.Key }\" is null.");
                    }
                    this.gameModeRules.Add(game_mode_rule.Key, game_mode_rule.Value);
                }
            }
            Server = server ?? throw new ArgumentNullException(nameof(server));
            Owner = owner ?? throw new ArgumentNullException(nameof(owner));
        }

        /// <summary>
        /// Gets user by GUID
        /// </summary>
        /// <param name="guid">User GUID</param>
        /// <returns>User if available, otherwise "null"</returns>
        public IUser GetUserByGUID(Guid guid) => TryGetUserByGUID(guid, out IUser ret) ? ret : null;

        /// <summary>
        /// Tries to get user by GUID
        /// </summary>
        /// <param name="guid">User GUID</param>
        /// <param name="user">User</param>
        /// <returns>"true" if user is available, otherwise "false"</returns>
        public bool TryGetUserByGUID(Guid guid, out IUser user) => users.TryGetValue(guid.ToString(), out user);

        /// <summary>
        /// Gets entity by GUID
        /// </summary>
        /// <param name="guid">Entity GUID</param>
        /// <returns>Entity if available, otherwise "null"</returns>
        public IEntity GetEntityByGUID(Guid guid) => TryGetEntityByGUID(guid, out IEntity ret) ? ret : null;

        /// <summary>
        /// Tries to get entity by GUID
        /// </summary>
        /// <param name="guid">Entity GUID</param>
        /// <param name="entity">Entity</param>
        /// <returns>"true" if entity exists, otherwise "false"</returns>
        public bool TryGetEntityByGUID(Guid guid, out IEntity entity) => entities.TryGetValue(guid.ToString(), out entity);

        /// <summary>
        /// Gets game mode rule
        /// </summary>
        /// <typeparam name="T">Game mode rule type</typeparam>
        /// <param name="key">Game mode rule key</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>Value if successful, otherwise the specified default value</returns>
        public T GetGameModeRule<T>(string key, T defaultValue = default) => TryGetGameModeRule(key, out T ret) ? ret : defaultValue;

        /// <summary>
        /// Tries to get game mode rule
        /// </summary>
        /// <typeparam name="T">Game mode rule type</typeparam>
        /// <param name="key">Game mode rule key</param>
        /// <param name="value">Value</param>
        /// <returns>Value if successful, otherwise the specified default value</returns>
        public bool TryGetGameModeRule<T>(string key, out T value)
        {
            bool ret = gameModeRules.TryGetValue(key ?? throw new ArgumentNullException(nameof(key)), out object object_value);
            value = ret ? ((object_value is T result) ? result : default) : default;
            return ret;
        }

        /// <summary>
        /// Sets a new lobby name
        /// </summary>
        /// <param name="name">Lobby name</param>
        public void SetNameInternally(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            string new_name = name.Trim();
            if ((new_name.Length < Defaults.minimalLobbyNameLength) || (new_name.Length > Defaults.maximalLobbyNameLength))
            {
                throw new ArgumentException($"Lobby name must be between { Defaults.minimalLobbyNameLength } and { Defaults.maximalLobbyNameLength } characters long.", nameof(name));
            }
            Name = new_name;
        }

        /// <summary>
        /// Sets a new minimal user count to start a game
        /// </summary>
        /// <param name="minimalUserCount"></param>
        public void SetMinimalUserCountInternally(uint minimalUserCount)
        {
            if (minimalUserCount > MaximalUserCount)
            {
                throw new ArgumentException("Minimal user count can't be greater than maximal user count.");
            }
            MinimalUserCount = minimalUserCount;
        }

        /// <summary>
        /// Sets a new maximal amount of users allowed in lobby
        /// </summary>
        /// <param name="maximalUserCount"></param>
        public void SetMaximalUserCountInternally(uint maximalUserCount)
        {
            if (maximalUserCount < MinimalUserCount)
            {
                throw new ArgumentException("Maximal user count can't be less than minimal user count.");
            }
            MaximalUserCount = maximalUserCount;
        }

        /// <summary>
        /// Sets new minimal amount of users to start a game and maximal amount of users allowed in lobby
        /// </summary>
        /// <param name="minimalUserCount">Minimal user count</param>
        /// <param name="maximalUserCount">Maximal user count</param>
        public void SetMinimalAndMaximalUserCountInternally(uint minimalUserCount, uint maximalUserCount)
        {
            if (minimalUserCount > maximalUserCount)
            {
                throw new ArgumentException("Minimal user count can't be greater than maximal user count.");
            }
            MinimalUserCount = minimalUserCount;
            MaximalUserCount = maximalUserCount;
        }

        /// <summary>
        /// Sets a new starting game automatically state
        /// </summary>
        /// <param name="isStartingGameAutomatically">Is starting game automatically</param>
        public void SetStartingGameAutomaticallyStateInternally(bool isStartingGameAutomatically) => IsStartingGameAutomatically = isStartingGameAutomatically;

        /// <summary>
        /// Sets a new game mode type
        /// </summary>
        /// <param name="gameModeType">Game mode type</param>
        public void SetGameModeTypeInternally((IGameResource, Type) gameModeType)
        {
            if (gameModeType.Item1 == null)
            {
                throw new ArgumentNullException(nameof(gameModeType.Item1));
            }
            if (gameModeType.Item2 == null)
            {
                throw new ArgumentNullException(nameof(gameModeType.Item2));
            }
            this.gameModeType = gameModeType;
        }

        /// <summary>
        /// Adds a new game mode rule
        /// </summary>
        /// <param name="key">Game mode rule key</param>
        /// <param name="value">Game mode rule value</param>
        public void AddGameModeRuleInternally(string key, object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            gameModeRules.Add(key, value);
        }

        /// <summary>
        /// Removes the specified game mode rule by key
        /// </summary>
        /// <param name="key">Game mode rule key</param>
        /// <returns>"true" if game mode rule was successfully removed, otherwise "false"</returns>
        public bool RemoveGameModeRuleInternally(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            return gameModeRules.Remove(key);
        }

        /// <summary>
        /// Clears all game mode rules
        /// </summary>
        public void ClearGameModeRulesInternally() => gameModeRules.Clear();

        /// <summary>
        /// Starts a new game mode instance 
        /// </summary>
        public void StartNewGameModeInstance()
        {
            bool was_running = StopGameModeInstance();
            gameUserFactory = gameModeType.Item1.CreateNewGameUserFactory();
            if (gameUserFactory == null)
            {
                throw new InvalidOperationException("Failed to create a new game user factory.");
            }
            gameEntityFactory = gameModeType.Item1.CreateNewGameEntityFactory();
            if (gameEntityFactory == null)
            {
                throw new InvalidOperationException("Failed to create a new game entity factory.");
            }
            CurrentlyLoadedGameMode = (IGameMode)Activator.CreateInstance(gameModeType.Item2);
            CurrentlyLoadedGameMode.OnInitialized(gameModeType.Item1, this);
            OnGameModeStarted?.Invoke(CurrentlyLoadedGameMode);
            foreach (KeyValuePair<string, IUser> user in users)
            {
                if (user.Value is IServerUser server_user)
                {
                    IGameUser game_user = gameUserFactory.CreateNewGameUser(server_user);
                    if (game_user == null)
                    {
                        throw new InvalidOperationException("Failed to create a new game user from game user factory.");
                    }
                    if (game_user.GUID != user.Value.GUID)
                    {
                        throw new InvalidOperationException("Game user GUID does not match user GUID.");
                    }
                    gameUsers.Add(user.Key, game_user);
                    CurrentlyLoadedGameMode.OnUserJoined(game_user);
                }
            }
            RemainingGameStartTime = 0.0;
            if (was_running)
            {
                OnGameRestarted?.Invoke();
                SendMessageToAll(new GameRestartedMessageData());
            }
            else
            {
                OnGameStarted?.Invoke();
                SendMessageToAll(new GameStartedMessageData());
            }
        }

        /// <summary>
        /// Stops the currently running game mode instance
        /// </summary>
        /// <returns>"true" if a running game mode instance has been stopped, otherwise "false"</returns>
        public bool StopGameModeInstance()
        {
            bool ret = false;
            if (CurrentlyLoadedGameMode != null)
            {
                List<KeyValuePair<string, IEntity>> remove_entities = new List<KeyValuePair<string, IEntity>>(entities);
                foreach (KeyValuePair<string, IEntity> remove_entity in remove_entities)
                {
                    if (remove_entity.Value is IGameEntity game_entity)
                    {
                        CurrentlyLoadedGameMode.OnGameEntityDestroyed(game_entity);
                    }
                    entities.Remove(remove_entity.Key);
                }
                remove_entities.Clear();
                entities.Clear();
                List<KeyValuePair<string, IGameUser>> remove_users = new List<KeyValuePair<string, IGameUser>>(gameUsers);
                foreach (KeyValuePair<string, IGameUser> remove_user in remove_users)
                {
                    if (gameUsers.ContainsKey(remove_user.Key))
                    {
                        CurrentlyLoadedGameMode.OnUserLeft(remove_user.Value);
                        gameUsers.Remove(remove_user.Key);
                    }
                }
                remove_users.Clear();
                gameUsers.Clear();
                OnGameModeStopped?.Invoke(CurrentlyLoadedGameMode);
                OnGameStopped?.Invoke(CurrentlyLoadedGameMode.UserResults, CurrentlyLoadedGameMode.Results);
                CurrentlyLoadedGameMode.OnClosed();
                gameUserFactory = null;
                gameEntityFactory = null;
                CurrentlyLoadedGameMode = null;
                ret = true;
            }
            return ret;
        }

        /// <summary>
        /// Adds the specified user
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>"true" if the specified user has been successfully added, otherwise "false"</returns>
        public bool AddUser(IInternalServerUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (!user.IsValid)
            {
                throw new ArgumentException("User is not valid.", nameof(user));
            }
            bool ret = false;
            string key = user.GUID.ToString();
            if (!users.ContainsKey(key))
            {
                users.Add(key, user);
                if ((CurrentlyLoadedGameMode != null) && (gameUserFactory != null))
                {
                    IGameUser game_user = gameUserFactory.CreateNewGameUser(user);
                    if (game_user == null)
                    {
                        throw new InvalidOperationException("Failed to create a new game user from game user factory.");
                    }
                    if (game_user.GUID != user.GUID)
                    {
                        throw new InvalidOperationException("Game user GUID does not match user GUID.");
                    }
                    gameUsers.Add(key, game_user);
                    CurrentlyLoadedGameMode.OnUserJoined(game_user);
                }
                OnUserJoined?.Invoke(user);
                SendUserJoinedMessage(user);
                ret = true;
            }
            return ret;
        }

        /// <summary>
        /// Removes the specified user
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="reason">Reason</param>
        /// <param name="message">Message</param>
        /// <returns>"true" if the specified user has been successfully removed, otherwise "false"</returns>
        public bool RemoveUser(IUser user, EDisconnectionReason reason, string message)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (!user.IsValid)
            {
                throw new ArgumentException("User is not valid.", nameof(user));
            }
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            bool ret = false;
            string key = user.GUID.ToString();
            if (gameUsers.ContainsKey(key))
            {
                CurrentlyLoadedGameMode?.OnUserLeft(gameUsers[key]);
                gameUsers.Remove(key);
            }
            if (users.ContainsKey(key))
            {
                IUser real_user = users[key];
                if (real_user is IInternalServerUser server_user)
                {
                    server_user.ServerLobby = null;
                }
                OnUserLeft?.Invoke(real_user, reason, message);
                SendUserLeftMessage(real_user, reason, message);
                ret = users.Remove(key);
            }
            return ret;
        }

        /// <summary>
        /// Updates the lobby rules
        /// </summary>
        /// <param name="newName">New lobby name</param>
        /// <param name="newGameMode">New game mode</param>
        /// <param name="newMinimalUserCount">New minimal user count</param>
        /// <param name="newMaximalUserCount">New maximal user count</param>
        /// <param name="newStartingGameAutomaticallyState">New starting game automatically state</param>
        /// <param name="newGameModeRules">New game mode ruels</param>
        public void UpdateLobbyRules(string newName = null, (IGameResource, Type)? newGameMode = null, uint? newMinimalUserCount = null, uint? newMaximalUserCount = null, bool? newStartingGameAutomaticallyState = null, IReadOnlyDictionary<string, object> newGameModeRules = null)
        {
            if (newName != null)
            {
                SetNameInternally(newName);
            }
            if (newMinimalUserCount != null)
            {
                SetMinimalUserCountInternally(newMinimalUserCount.Value);
            }
            if (newMaximalUserCount != null)
            {
                SetMaximalUserCountInternally(newMaximalUserCount.Value);
            }
            if (newStartingGameAutomaticallyState != null)
            {
                SetStartingGameAutomaticallyStateInternally(newStartingGameAutomaticallyState.Value);
            }
            if (newGameMode != null)
            {
                SetGameModeTypeInternally(newGameMode.Value);
            }
            if (newGameModeRules != null)
            {
                ClearGameModeRulesInternally();
                foreach (KeyValuePair<string, object> new_game_mode_rule in newGameModeRules)
                {
                    AddGameModeRuleInternally(new_game_mode_rule.Key, new_game_mode_rule.Value);
                }
            }
            OnLobbyRulesUpdated?.Invoke();
            SendLobbyRulesChangedMessage();
        }

        /// <summary>
        /// Cancels start game time
        /// </summary>
        public void CancelStartGameTime()
        {
            if (RemainingGameStartTime > 0.0f)
            {
                RemainingGameStartTime = 0.0f;
                if (CurrentlyLoadedGameMode == null)
                {
                    OnStartGameCancelled?.Invoke();
                    SendMessageToAll(new StartGameCancelledMessageData());
                }
                else
                {
                    OnRestartGameCancelled?.Invoke();
                    SendMessageToAll(new RestartGameCancelledMessageData());
                }
            }
        }

        /// <summary>
        /// Cancels stop game time
        /// </summary>
        public void CancelStopGameTime()
        {
            if ((CurrentlyLoadedGameMode != null) && (RemainingGameStartTime > 0.0f))
            {
                RemainingGameStartTime = 0.0f;
                OnStopGameCancelled?.Invoke();
                SendMessageToAll(new StopGameCancelledMessageData());
            }
        }

        /// <summary>
        /// Creates a new game entity
        /// </summary>
        /// <param name="entityType">Game entity type</param>
        /// <param name="gameColor">Game entity game color (optional)</param>
        /// <param name="position">Game entity position (optional)</param>
        /// <param name="rotation">Game entity rotation (optional)</param>
        /// <param name="velocity">Game entity velocity (optional)</param>
        /// <param name="angularVelocity">Game entity angular valocity (optional)</param>
        /// <param name="actions">Game entity game actions (optional)</param>
        /// <returns>Game entity</returns>
        public IGameEntity CreateNewGameEntity(string entityType, EGameColor? gameColor = null, Vector3? position = null, Quaternion? rotation = null, Vector3? velocity = null, Vector3? angularVelocity = null, IEnumerable<EGameAction> actions = null)
        {
            if (CurrentlyLoadedGameMode == null)
            {
                throw new InvalidOperationException("Game mode is not running yet.");
            }
            if (gameEntityFactory == null)
            {
                throw new NullReferenceException("Game entity factory is null.");
            }
            IServerEntity server_entity = new ServerEntity(Guid.NewGuid(), entityType, (gameColor == null) ? EGameColor.Default : gameColor.Value, (position == null) ? Vector3.Zero : position.Value, (rotation == null) ? Quaternion.Identity : rotation.Value, (velocity == null) ? Vector3.Zero : velocity.Value, (angularVelocity == null) ? Vector3.Zero : angularVelocity.Value, actions);
            IGameEntity ret = gameEntityFactory.CreateNewGameEntity(server_entity);
            if (ret == null)
            {
                throw new InvalidOperationException("Failed to create a new game entity.");
            }
            if (!ret.IsValid)
            {
                throw new InvalidOperationException("Game entity is not valid.");
            }
            string key = ret.GUID.ToString();
            if (entities.ContainsKey(key))
            {
                throw new InvalidOperationException($"Game entity of GUID \"{ key }\" already exists.");
            }
            entities.Add(key, ret);
            CurrentlyLoadedGameMode.OnGameEntityCreated(ret);
            return ret;
        }

        /// <summary>
        /// Removes the specified entity
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>"true" if the specified entity has been successfully removed, otherwise "false"</returns>
        public bool RemoveEntity(IEntity entity)
        {
            if (CurrentlyLoadedGameMode == null)
            {
                throw new InvalidOperationException("Game mode is not running yet.");
            }
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (!entity.IsValid)
            {
                throw new ArgumentException("Game entity is invalid.", nameof(entity));
            }
            bool ret = false;
            string key = entity.GUID.ToString();
            if (entities.ContainsKey(key))
            {
                if (entities[key] is IGameEntity game_entity)
                {
                    CurrentlyLoadedGameMode.OnGameEntityDestroyed(game_entity);
                }
                ret = entities.Remove(key);
            }
            return ret;
        }

        /// <summary>
        /// Performs a hit
        /// </summary>
        /// <param name="hit">Hit</param>
        public void PerformHit(IHit hit)
        {
            if (hit == null)
            {
                throw new ArgumentNullException(nameof(hit));
            }
            if (!hit.IsValid)
            {
                throw new ArgumentException("Hit is invalid.", nameof(hit));
            }
            if (CurrentlyLoadedGameMode == null)
            {
                throw new InvalidOperationException("Game mode is not running yet.");
            }
            string issuer_key = hit.Issuer.GUID.ToString();
            string victim_key = hit.Victim.GUID.ToString();
            IGameEntity issuer;
            IGameEntity victim;
            if (gameUsers.ContainsKey(issuer_key))
            {
                issuer = gameUsers[issuer_key];
            }
            else if (entities.ContainsKey(issuer_key) && entities[issuer_key] is IGameEntity game_entity)
            {
                issuer = game_entity;
            }
            else
            {
                throw new ArgumentException($"Issuer GUID \"{ issuer_key }\" is invalid.", nameof(hit));
            }
            if (gameUsers.ContainsKey(victim_key))
            {
                victim = gameUsers[issuer_key];
            }
            else if (entities.ContainsKey(victim_key) && entities[victim_key] is IGameEntity game_entity)
            {
                victim = game_entity;
            }
            else
            {
                throw new ArgumentException($"Victim GUID \"{ victim_key }\" is invalid.", nameof(hit));
            }
            if (CurrentlyLoadedGameMode.OnGameEntityHit(issuer, victim, hit.WeaponName, hit.HitPosition, hit.HitForce, hit.Damage))
            {
                currentHits.Add(hit);
            }
        }

        /// <summary>
        /// Sends a message to all
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="message">Message</param>
        public void SendMessageToAll<T>(T message) where T : IBaseMessageData
        {
            foreach (IServerUser user in users.Values)
            {
                if (user is IServerUser server_user)
                {
                    Server.SendMessageToPeer(server_user.Peer, message);
                }
            }
        }

        /// <summary>
        /// Sends an user joined message
        /// </summary>
        /// <param name="user">Server user</param>
        public void SendUserJoinedMessage(IUser user)
        {
            UserJoinedMessageData user_joined_message = new UserJoinedMessageData(user);
            foreach (IUser current_user in users.Values)
            {
                if (current_user is IInternalServerUser server_user && (server_user.GUID != user.GUID))
                {
                    server_user.SendMessage(user_joined_message);
                }
            }
        }

        /// <summary>
        /// Sends an user left message
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="reason">Reason</param>
        /// <param name="message">Message</param>
        public void SendUserLeftMessage(IUser user, EDisconnectionReason reason, string message) => SendMessageToAll(new UserLeftMessageData(user, reason, message));

        /// <summary>
        /// Sends an user color changed message
        /// </summary>
        /// <param name="user">User</param>
        public void SendUserLobbyColorChangedMessage(IUser user) => SendMessageToAll(new UserLobbyColorChangedMessageData(user));

        /// <summary>
        /// Sends an username changed message
        /// </summary>
        /// <param name="user">User</param>
        public void SendUsernameChangedMessage(IUser user) => SendMessageToAll(new UsernameChangedMessageData(user));

        /// <summary>
        /// Sends a lobby rules changed message
        /// </summary>
        public void SendLobbyRulesChangedMessage() => SendMessageToAll(new LobbyRulesChangedMessageData(this));

        /// <summary>
        /// Sends a game start requested message
        /// </summary>
        /// <param name="time">Time to start game in seconds</param>
        public void SendGameStartRequestedMessage(double time)
        {
            if (time < double.Epsilon)
            {
                throw new ArgumentException("Time can't be zero or negative.");
            }
            CancelStartGameTime();
            RemainingGameStartTime = time;
            OnGameStartRequested?.Invoke(time);
            SendMessageToAll(new GameStartRequestedMessageData(time));
        }

        /// <summary>
        /// Sends a game restart requested message
        /// </summary>
        /// <param name="time">Time to restart in seconds</param>
        public void SendGameRestartRequestedMessage(double time)
        {
            if (time < double.Epsilon)
            {
                throw new ArgumentException("Time can't be zero or negative.");
            }
            CancelStartGameTime();
            CancelStopGameTime();
            RemainingGameStartTime = time;
            OnGameRestartRequested?.Invoke(time);
            SendMessageToAll(new GameRestartRequestedMessageData(time));
        }

        /// <summary>
        /// Sends a game stop requested message
        /// </summary>
        /// <param name="time">Time to stop game in seconds</param>
        public void SendGameStopRequestedMessage(double time)
        {
            if (time < double.Epsilon)
            {
                throw new ArgumentException("Time can't be zero or negative.");
            }
            CancelStartGameTime();
            CancelStopGameTime();
            RemainingGameStopTime = time;
            OnGameStopRequested?.Invoke(time);
            SendMessageToAll(new GameStopRequestedMessageData(time));
        }

        /// <summary>
        /// Closes lobby
        /// </summary>
        public void Close() => Close(EDisconnectionReason.LobbyClosed);

        /// <summary>
        /// Closes lobby
        /// </summary>
        /// <param name="reason">Reason</param>
        public void Close(EDisconnectionReason reason)
        {
            StopGameModeInstance();
            List<IUser> users = new List<IUser>(this.users.Values);
            foreach (IServerUser user in users)
            {
                RemoveUser(user, reason, "Lobby has been closed.");
            }
            users.Clear();
            OnLobbyClosed?.Invoke(this);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() => Close(EDisconnectionReason.Disposed);

        /// <summary>
        /// Performs a game tick
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Tick(TimeSpan deltaTime)
        {
            if (CurrentlyLoadedGameMode == null)
            {
                CurrentGameTime = 0.0;
                RemainingGameStopTime = 0.0;
                if (RemainingGameStartTime > double.Epsilon)
                {
                    RemainingGameStartTime -= deltaTime.TotalSeconds;
                    if (RemainingGameStartTime <= double.Epsilon)
                    {
                        RemainingGameStartTime = 0.0;
                        StartNewGameModeInstance();
                    }
                }
            }
            else
            {
                CurrentGameTime += deltaTime.TotalSeconds;
                RemainingGameStartTime = 0.0;
                if (RemainingGameStopTime > double.Epsilon)
                {
                    RemainingGameStopTime -= deltaTime.TotalSeconds;
                    if (RemainingGameStopTime <= double.Epsilon)
                    {
                        RemainingGameStopTime = 0.0;
                        StopGameModeInstance();
                    }
                }
                CurrentlyLoadedGameMode?.OnGameTicked(deltaTime);
                IHit[] current_hits = currentHits.ToArray();
                currentHits.Clear();
                foreach (IUser user in users.Values)
                {
                    if (user is IInternalServerUser server_user)
                    {
                        server_user.SendServerTickMessage(CurrentGameTime, current_hits);
                    }
                }
            }
        }
    }
}
