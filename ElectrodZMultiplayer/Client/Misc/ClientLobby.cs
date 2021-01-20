using ElectrodZMultiplayer.Data;
using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer client namespace
/// </summary>
namespace ElectrodZMultiplayer.Client
{
    /// <summary>
    /// A class that describes a lobby speicific for a client
    /// </summary>
    internal class ClientLobby : IInternalClientLobby
    {
        /// <summary>
        /// Client
        /// </summary>
        private readonly IInternalClientSynchronizer client;

        /// <summary>
        /// Users
        /// </summary>
        private readonly Dictionary<string, IUser> users = new Dictionary<string, IUser>();

        /// <summary>
        /// Entities
        /// </summary>
        private Dictionary<string, IEntity> entities = new Dictionary<string, IEntity>();

        /// <summary>
        /// Game mode rules
        /// </summary>
        private readonly Dictionary<string, object> gameModeRules = new Dictionary<string, object>();

        /// <summary>
        /// Client
        /// </summary>
        public IClientSynchronizer Client => client;

        /// <summary>
        /// Lobby owner
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
        public string LobbyCode { get; private set; }

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
        /// User count
        /// </summary>
        public uint UserCount => (uint)users.Count;

        /// <summary>
        /// Is starting game automatically
        /// </summary>
        public bool IsStartingGameAutomatically { get; private set; }

        /// <summary>
        /// Game mode
        /// </summary>
        public string GameMode { get; private set; }

        /// <summary>
        /// Game mode rules
        /// </summary>
        public IReadOnlyDictionary<string, object> GameModeRules => gameModeRules;

        /// <summary>
        /// Current game time
        /// </summary>
        public double CurrentGameTime { get; private set; }

        /// <summary>
        /// Is valid
        /// </summary>
        public bool IsValid =>
            (LobbyCode != null) &&
            (Name != null) &&
            (MinimalUserCount <= MaximalUserCount) &&
            (UserCount <= MaximalUserCount) &&
            !string.IsNullOrWhiteSpace(GameMode) &&
            Protection.IsValid(gameModeRules.Values) &&
            Protection.IsValid(users.Values) &&
            Protection.IsValid(entities.Values);

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
        /// This event will be invoked when the game has ended.
        /// </summary>
        public event GameEndedDelegate OnGameEnded;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">Client</param>
        /// <param name="lobbyCode">Lobby code</param>
        /// <param name="name">Lobby name</param>
        /// <param name="minimalUserCount">Minimal user count</param>
        /// <param name="maximalUserCount">Maximal user count</param>
        /// <param name="users">Users</param>
        /// <param name="isStartingGameAutomatically">Is starting game automatically</param>
        /// <param name="gameMode">Game mode</param>
        /// <param name="gameModeRules">Game mode rules</param>
        public ClientLobby(IInternalClientSynchronizer client, string lobbyCode, string name, uint minimalUserCount, uint maximalUserCount, bool isStartingGameAutomatically, string gameMode, IReadOnlyDictionary<string, object> gameModeRules, IUser owner, IReadOnlyDictionary<string, IUser> users)
        {
            if (minimalUserCount > maximalUserCount)
            {
                throw new ArgumentException("Minimal user count can't be greater than maximal user count.");
            }
            if (users == null)
            {
                throw new ArgumentNullException(nameof(users));
            }
            if (users.Count > maximalUserCount)
            {
                throw new ArgumentException("User count can't be greater than maximal user count.");
            }
            if (string.IsNullOrWhiteSpace(gameMode))
            {
                throw new ArgumentException("Game mode can't be unknown.");
            }
            this.client = client ?? throw new ArgumentNullException(nameof(client));
            LobbyCode = lobbyCode ?? throw new ArgumentNullException(nameof(lobbyCode));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            MinimalUserCount = minimalUserCount;
            MaximalUserCount = maximalUserCount;
            IsStartingGameAutomatically = isStartingGameAutomatically;
            GameMode = gameMode;
            foreach (KeyValuePair<string, object> game_mode_rule in gameModeRules)
            {
                if (game_mode_rule.Value == null)
                {
                    throw new ArgumentException($"Value of key \"{ game_mode_rule.Key }\" is null.");
                }
                this.gameModeRules.Add(game_mode_rule.Key, game_mode_rule.Value);
            }
            Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            foreach (KeyValuePair<string, IUser> user in users)
            {
                if (user.Value == null)
                {
                    throw new ArgumentException($"Value of key \"{ user.Key }\" is null.");
                }
                this.users.Add(user.Key, user.Value);
            }
        }

        /// <summary>
        /// Leave lobby
        /// </summary>
        public void Leave() => client.SendQuitLobbyMessage();

        /// <summary>
        /// Adds a new user to the lobby
        /// </summary>
        /// <param name="user"></param>
        /// <returns>"true" if user was successfully added, otherwise "false"</returns>
        public bool AddUserInternally(IUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            bool ret = false;
            string key = user.GUID.ToString();
            if (!users.ContainsKey(key))
            {
                users.Add(key, user);
                OnUserJoined?.Invoke(user);
                ret = true;
            }
            return ret;
        }

        /// <summary>
        /// Removes the specified user
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="reason">Reason</param>
        /// <returns>"true" if user was successfully removed, othewrwise </returns>
        public bool RemoveUserInternally(IUser user, string reason)
        {
            bool ret = false;
            string key = user.GUID.ToString();
            if (users.ContainsKey(key))
            {
                OnUserLeft?.Invoke(user, reason);
                ret = users.Remove(key);
            }
            return ret;
        }

        /// <summary>
        /// Invokes the game start requested event internally
        /// </summary>
        /// <param name="time">Time to start game in seconds</param>
        public void InvokeGameStartRequestedEventInternally(double time) => OnGameStartRequested?.Invoke(time);

        /// <summary>
        /// Invokes the game restart requested event internally
        /// </summary>
        /// <param name="time">Time to restart the game in seconds</param>
        public void InvokeGameRestartRequestedEventInternally(double time) => OnGameRestartRequested?.Invoke(time);

        /// <summary>
        /// Invokes the game stop requested event internally
        /// </summary>
        /// <param name="time">Time to stop the game in seconds</param>
        public void InvokeGameStopRequestedEventInternally(double time) => OnGameStopRequested?.Invoke(time);

        /// <summary>
        /// Invokes the game started event internally
        /// </summary>
        public void InvokeGameStartedEventInternally()
        {
            CurrentGameTime = 0.0;
            OnGameStarted?.Invoke();
        }

        /// <summary>
        /// Invokes the game restarted event internally
        /// </summary>
        public void InvokeGameRestartedEventInternally()
        {
            CurrentGameTime = 0.0;
            OnGameRestarted?.Invoke();
        }

        /// <summary>
        /// Invokes the game stopped event internally
        /// </summary>
        public void InvokeGameStoppedEventInternally()
        {
            CurrentGameTime = 0.0;
            OnGameStopped?.Invoke();
        }

        /// <summary>
        /// Updates game mode rules internally
        /// </summary>
        /// <param name="newLobbyCode">New lobby code</param>
        /// <param name="newName">New lobby name</param>
        /// <param name="newGameMode">New game mode</param>
        /// <param name="newMinimalUserCount">New minimal user count</param>
        /// <param name="newMaximalUserCount">New maximal user count</param>
        /// <param name="newStartingGameAutomaticallyState">New starting game automatically state</param>
        /// <param name="newGameModeRules">New game mode rules</param>
        public void UpdateGameModeRulesInternally(string newLobbyCode, string newName, string newGameMode, uint newMinimalUserCount, uint newMaximalUserCount, bool newStartingGameAutomaticallyState, IReadOnlyDictionary<string, object> newGameModeRules)
        {
            if (string.IsNullOrWhiteSpace(newGameMode))
            {
                throw new ArgumentException("Game mode can't be unknown.", nameof(newGameMode));
            }
            if (newGameModeRules == null)
            {
                throw new ArgumentNullException(nameof(newGameModeRules));
            }
            LobbyCode = newLobbyCode ?? throw new ArgumentNullException(nameof(newLobbyCode));
            Name = newName ?? throw new ArgumentNullException(nameof(newName));
            GameMode = newGameMode;
            MinimalUserCount = newMinimalUserCount;
            MaximalUserCount = newMaximalUserCount;
            IsStartingGameAutomatically = newStartingGameAutomaticallyState;
            gameModeRules.Clear();
            foreach (KeyValuePair<string, object> game_mode_rule in newGameModeRules)
            {
                gameModeRules.Add(game_mode_rule.Key, game_mode_rule.Value);
            }
            OnLobbyRulesUpdated?.Invoke();
        }

        /// <summary>
        /// Processes server tick internally
        /// </summary>
        /// <param name="time">Time in seconds elapsed since game start</param>
        /// <param name="entityDeltas">Entity deltas</param>
        public void ProcessServerTickInternally(double time, IEnumerable<EntityData> entityDeltas)
        {
            List<IEntityDelta> entity_deltas = new List<IEntityDelta>();
            HashSet<string> remove_entity_keys = new HashSet<string>(entities.Keys);
            foreach (EntityData entity in entityDeltas)
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
                entity_deltas.Add
                (
                    new EntityDelta
                    (
                        entity.GUID,
                        entity.EntityType,
                        entity.GameColor,
                        (entity.Position == null) ? (Vector3?)null : new Vector3(entity.Position.X, entity.Position.Y, entity.Position.Z),
                        (entity.Rotation == null) ? (Quaternion?)null : new Quaternion(entity.Rotation.X, entity.Rotation.Y, entity.Rotation.Z, entity.Rotation.W),
                        (entity.Velocity == null) ? (Vector3?)null : new Vector3(entity.Velocity.X, entity.Velocity.Y, entity.Velocity.Z),
                        (entity.AngularVelocity == null) ? (Vector3?)null : new Vector3(entity.AngularVelocity.X, entity.AngularVelocity.Y, entity.AngularVelocity.Z),
                        entity.Actions
                    )
                );
            }
            foreach (string remove_entity_key in remove_entity_keys)
            {
                entities.Remove(remove_entity_key);
            }
            remove_entity_keys.Clear();
            CurrentGameTime = time;
            if (client.User is IInternalClientUser my_client_user)
            {
                my_client_user.InvokeServerTickedEvent(time, entity_deltas);
            }
        }

        /// <summary>
        /// Invokes the game ended event internally
        /// </summary>
        /// <param name="users">Users</param>
        /// <param name="results">Results</param>
        public void InvokeGameEndedEventInternally(IReadOnlyDictionary<string, UserWithResults> users, IReadOnlyDictionary<string, object> results) => OnGameEnded?.Invoke(users, results);

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            // ...
        }
    }
}
