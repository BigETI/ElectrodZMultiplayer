﻿using ElectrodZMultiplayer.Data;
using ElectrodZMultiplayer.Data.Messages;
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
        private readonly Dictionary<string, IEntity> entities = new Dictionary<string, IEntity>();

        /// <summary>
        /// Game mode rules
        /// </summary>
        private readonly Dictionary<string, object> gameModeRules = new Dictionary<string, object>();

        /// <summary>
        /// Loaded users
        /// </summary>
        private readonly Dictionary<string, IUser> loadedUsers = new Dictionary<string, IUser>();

        /// <summary>
        /// Client
        /// </summary>
        public IClientSynchronizer Client => client;

        /// <summary>
        /// Lobby owner
        /// </summary>
        public IUser Owner { get; private set; }

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
        /// Game mode
        /// </summary>
        public string GameMode { get; private set; }

        /// <summary>
        /// Is lobby private
        /// </summary>
        public bool IsPrivate { get; private set; }

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
        /// This event will be invoked when the lobby owner of this lobby has been changed.
        /// </summary>
        public event LobbyOwnershipChangedDelegate OnLobbyOwnershipChanged;

        /// <summary>
        /// This event will be invoked when the lobby rules of this lobby has been changed.
        /// </summary>
        public event LobbyRulesChangedDelegate OnLobbyRulesChanged;

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
        /// This event will be invoked when an user entity has been created.
        /// </summary>
        public event UserEntityCreatedDelegate OnUserEntityCreated;

        /// <summary>
        /// This event will be invoked when an user entity has been updated.
        /// </summary>
        public event UserEntityUpdatedDelegate OnUserEntityUpdated;

        /// <summary>
        /// This event will be invoked when an user entity has been destroyed.
        /// </summary>
        public event UserEntityDestroyedDelegate OnUserEntityDestroyed;

        /// <summary>
        /// This event will be invoked when an entity has been created.
        /// </summary>
        public event EntityCreatedDelegate OnEntityCreated;

        /// <summary>
        /// This event will be invoked when an entity has been updated.
        /// </summary>
        public event EntityUpdatedDelegate OnEntityUpdated;

        /// <summary>
        /// This event will be invoked when an entity has been destroyed.
        /// </summary>
        public event EntityDestroyedDelegate OnEntityDestroyed;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client">Client</param>
        /// <param name="lobbyCode">Lobby code</param>
        /// <param name="name">Lobby name</param>
        /// <param name="gameMode">Game mode</param>
        /// <param name="isPrivate">Is lobby private</param>
        /// <param name="minimalUserCount">Minimal user count</param>
        /// <param name="maximalUserCount">Maximal user count</param>
        /// <param name="users">Users</param>
        /// <param name="isStartingGameAutomatically">Is starting game automatically</param>
        /// <param name="gameModeRules">Game mode rules</param>
        public ClientLobby(IInternalClientSynchronizer client, string lobbyCode, string name, string gameMode, bool isPrivate, uint minimalUserCount, uint maximalUserCount, bool isStartingGameAutomatically, IReadOnlyDictionary<string, object> gameModeRules, IUser owner, IReadOnlyDictionary<string, IUser> users)
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
            GameMode = gameMode;
            IsPrivate = isPrivate;
            MinimalUserCount = minimalUserCount;
            MaximalUserCount = maximalUserCount;
            IsStartingGameAutomatically = isStartingGameAutomatically;
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
                RegisterUserEvents(user.Value);
                this.users.Add(user.Key, user.Value);
            }
        }

        /// <summary>
        /// Registers user events
        /// </summary>
        /// <param name="user">User</param>
        private void RegisterUserEvents(IUser user)
        {
            user.OnGameLoadingFinished += () =>
            {
                AddLoadedUser(user);
            };
        }

        /// <summary>
        /// Adds loaded user
        /// </summary>
        /// <param name="user">User</param>
        private void AddLoadedUser(IUser user)
        {
            string key = user.GUID.ToString();
            if (!loadedUsers.ContainsKey(key))
            {
                loadedUsers.Add(key, user);
                OnUserEntityCreated?.Invoke(user);
            }
        }

        /// <summary>
        /// Removes loaded user
        /// </summary>
        /// <param name="key">Key</param>
        private void RemoveLoadedUser(string key)
        {
            if (loadedUsers.ContainsKey(key))
            {
                OnUserEntityDestroyed?.Invoke(loadedUsers[key]);
                loadedUsers.Remove(key);
            }
        }

        /// <summary>
        /// Adds the specified entity
        /// </summary>
        /// <param name="entity">Entity</param>
        private void AddEntity(IEntity entity)
        {
            string key = entity.GUID.ToString();
            if (!entities.ContainsKey(key))
            {
                entities.Add(key, entity);
                OnEntityCreated?.Invoke(entity);
            }
        }

        /// <summary>
        /// Removes the specified entity
        /// </summary>
        /// <param name="key">Key</param>
        private void RemoveEntity(string key)
        {
            if (entities.ContainsKey(key))
            {
                OnEntityDestroyed?.Invoke(entities[key]);
                entities.Remove(key);
            }
        }

        /// <summary>
        /// Applies entity data to entity
        /// </summary>
        /// <param name="entityData">Entity data</param>
        /// <param name="entity">Entity</param>
        private static void ApplyEntityDataToEntity(EntityData entityData, IInternalEntity entity)
        {
            if (!string.IsNullOrWhiteSpace(entityData.EntityType))
            {
                entity.SetEntityTypeInternally(entityData.EntityType);
            }
            if (entityData.IsSpectating != null)
            {
                entity.SetSpectatingStateInternally(entityData.IsSpectating.Value);
            }
            if (entityData.GameColor != null)
            {
                entity.SetGameColorInternally(entityData.GameColor.Value);
            }
            if (entityData.Position != null)
            {
                entity.SetPositionInternally(new Vector3(entityData.Position.X, entityData.Position.Y, entityData.Position.Z));
            }
            if (entityData.Rotation != null)
            {
                entity.SetRotationInternally(new Quaternion(entityData.Rotation.X, entityData.Rotation.Y, entityData.Rotation.Z, entityData.Rotation.W));
            }
            if (entityData.Velocity != null)
            {
                entity.SetVelocityInternally(new Vector3(entityData.Velocity.X, entityData.Velocity.Y, entityData.Velocity.Z));
            }
            if (entityData.AngularVelocity != null)
            {
                entity.SetAngularVelocityInternally(new Vector3(entityData.AngularVelocity.X, entityData.AngularVelocity.Y, entityData.AngularVelocity.Z));
            }
            if (entityData.Actions != null)
            {
                entity.SetActionsInternally(entityData.Actions);
            }
            if (entityData.IsResyncRequested != null)
            {
                entity.SetResyncRequestedStateInternally(entityData.IsResyncRequested.Value);
            }
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
        /// Leave lobby
        /// </summary>
        public void Leave() => client.SendQuitLobbyMessage();

        /// <summary>
        /// Adds a new user to the lobby
        /// </summary>
        /// <param name="user">User</param>
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
                RegisterUserEvents(user);
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
        /// <param name="message">Message</param>
        /// <returns>"true" if user was successfully removed, otherwise "false"</returns>
        public bool RemoveUserInternally(IUser user, EDisconnectionReason reason, string message)
        {
            bool ret = false;
            string key = user.GUID.ToString();
            if (users.ContainsKey(key))
            {
                RemoveLoadedUser(key);
                OnUserLeft?.Invoke(user, reason, message);
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
            List<string> remove_entity_keys = new List<string>(entities.Keys);
            CurrentGameTime = 0.0;
            foreach (IUser user in loadedUsers.Values)
            {
                OnUserEntityDestroyed?.Invoke(user);
            }
            foreach (string entity_key in remove_entity_keys)
            {
                RemoveEntity(entity_key);
            }
            remove_entity_keys.Clear();
            OnGameRestarted?.Invoke();
            foreach (IUser user in loadedUsers.Values)
            {
                OnUserEntityCreated?.Invoke(user);
            }
        }

        /// <summary>
        /// Invokes the game stopped event internally
        /// </summary>
        /// <param name="users">Users</param>
        /// <param name="results">Results</param>
        public void InvokeGameStoppedEventInternally(IReadOnlyDictionary<string, UserWithResults> users, IReadOnlyDictionary<string, object> results)
        {
            List<string> remove_loaded_user_keys = new List<string>(loadedUsers.Keys);
            List<string> remove_entity_keys = new List<string>(entities.Keys);
            CurrentGameTime = 0.0;
            foreach (string loaded_user_key in remove_loaded_user_keys)
            {
                RemoveLoadedUser(loaded_user_key);
            }
            remove_loaded_user_keys.Clear();
            foreach (string entity_key in remove_entity_keys)
            {
                RemoveEntity(entity_key);
            }
            remove_entity_keys.Clear();
            OnGameStopped?.Invoke(users, results);
        }

        /// <summary>
        /// Invokes the start game cancelled event internally
        /// </summary>
        public void InvokeStartGameCancelledEventInternally() => OnStartGameCancelled?.Invoke();

        /// <summary>
        /// Invokes the restart game cancelled event internally
        /// </summary>
        public void InvokeRestartGameCancelledEventInternally() => OnRestartGameCancelled?.Invoke();

        /// <summary>
        /// Invokes the stop game cancelled event internally
        /// </summary>
        public void InvokeStopGameCancelledEventInternally() => OnStopGameCancelled?.Invoke();

        /// <summary>
        /// Changes lobby ownership internally
        /// </summary>
        /// <param name="newOwner">New owner</param>
        public void ChangeLobbyOwnershipInternally(IUser newOwner)
        {
            if (newOwner == null)
            {
                throw new ArgumentNullException(nameof(newOwner));
            }
            if (!newOwner.IsValid)
            {
                throw new ArgumentException("New owner is invalid.", nameof(newOwner));
            }
            string key = newOwner.GUID.ToString();
            if (users.ContainsKey(key))
            {
                Owner = users[key];
                OnLobbyOwnershipChanged?.Invoke();
            }
        }

        /// <summary>
        /// Changes lobby rules internally
        /// </summary>
        /// <param name="newLobbyCode">New lobby code</param>
        /// <param name="newName">New lobby name</param>
        /// <param name="newGameMode">New game mode</param>
        /// <param name="newPrivateState">New lobby private state</param>
        /// <param name="newMinimalUserCount">New minimal user count</param>
        /// <param name="newMaximalUserCount">New maximal user count</param>
        /// <param name="newStartingGameAutomaticallyState">New starting game automatically state</param>
        /// <param name="newGameModeRules">New game mode rules</param>
        public void ChangeLobbyRulesInternally(string newLobbyCode, string newName, string newGameMode, bool newPrivateState, uint newMinimalUserCount, uint newMaximalUserCount, bool newStartingGameAutomaticallyState, IReadOnlyDictionary<string, object> newGameModeRules)
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
            IsPrivate = newPrivateState;
            MinimalUserCount = newMinimalUserCount;
            MaximalUserCount = newMaximalUserCount;
            IsStartingGameAutomatically = newStartingGameAutomaticallyState;
            gameModeRules.Clear();
            foreach (KeyValuePair<string, object> game_mode_rule in newGameModeRules)
            {
                gameModeRules.Add(game_mode_rule.Key, game_mode_rule.Value);
            }
            OnLobbyRulesChanged?.Invoke();
        }

        /// <summary>
        /// Processes server tick internally
        /// </summary>
        /// <param name="time">Time in seconds elapsed since game start</param>
        /// <param name="entityDeltas">Entity deltas</param>
        /// <param name="serverHits">Server hits</param>
        public void ProcessServerTickInternally(double time, IEnumerable<EntityData> entityDeltas, IEnumerable<ServerHitData> serverHits)
        {
            List<IEntityDelta> entity_deltas = new List<IEntityDelta>();
            HashSet<string> remove_entity_keys = new HashSet<string>(entities.Keys);
            List<IHit> hits = null;
            bool is_successful = true;
            remove_entity_keys.UnionWith(loadedUsers.Keys);
            foreach (EntityData entity in entityDeltas)
            {
                string key = entity.GUID.ToString();
                if (loadedUsers.ContainsKey(key))
                {
                    remove_entity_keys.Remove(key);
                    if (loadedUsers[key] is IInternalClientUser internal_user)
                    {
                        ApplyEntityDataToEntity(entity, internal_user);
                        OnUserEntityUpdated?.Invoke(internal_user);
                    }
                }
                else if (entities.ContainsKey(key))
                {
                    remove_entity_keys.Remove(key);
                    if (entities[key] is IInternalEntity internal_entity)
                    {
                        ApplyEntityDataToEntity(entity, internal_entity);
                        OnEntityUpdated?.Invoke(internal_entity);
                    }
                }
                else if (string.IsNullOrWhiteSpace(entity.EntityType))
                {
                    Console.Error.WriteLine($"Entity with GUID \"{ entity.GUID }\" could not be instantiated, because no entity type was received.");
                }
                else
                {
                    AddEntity((Entity)entity);
                }
                entity_deltas.Add((EntityDelta)entity);
            }
            foreach (string remove_entity_key in remove_entity_keys)
            {
                if (loadedUsers.ContainsKey(remove_entity_key))
                {
                    RemoveLoadedUser(remove_entity_key);
                }
                else
                {
                    RemoveEntity(remove_entity_key);
                }
            }
            remove_entity_keys.Clear();
            if (serverHits != null)
            {
                foreach (ServerHitData server_hit in serverHits)
                {
                    string issuer_key = server_hit.IssuerGUID.ToString();
                    string victim_key = server_hit.VictimGUID.ToString();
                    IEntity issuer = null;
                    IEntity victim = null;
                    if (Users.ContainsKey(issuer_key))
                    {
                        issuer = Users[issuer_key];
                    }
                    else if (Entities.ContainsKey(issuer_key))
                    {
                        issuer = Users[issuer_key];
                    }
                    else
                    {
                        is_successful = false;
                        client.SendErrorMessage<ServerTickMessageData>(EErrorType.InvalidMessageParameters, $"Issuer GUID \"{ issuer_key }\" is invalid.");
                        break;
                    }
                    if (Users.ContainsKey(victim_key))
                    {
                        victim = Users[victim_key];
                    }
                    else if (Entities.ContainsKey(victim_key))
                    {
                        victim = Users[victim_key];
                    }
                    else
                    {
                        is_successful = false;
                        client.SendErrorMessage<ServerTickMessageData>(EErrorType.InvalidMessageParameters, $"Victim GUID \"{ victim_key }\" is invalid.");
                        break;
                    }
                    hits = hits ?? new List<IHit>();
                    hits.Add(new Hit(issuer, victim, server_hit.WeaponName, new Vector3(server_hit.HitPosition.X, server_hit.HitPosition.Y, server_hit.HitPosition.Z), new Vector3(server_hit.HitForce.X, server_hit.HitForce.Y, server_hit.HitForce.Z), server_hit.Damage));
                }
            }
            if (is_successful)
            {
                CurrentGameTime = time;
                if (client.User is IInternalClientUser my_client_user)
                {
                    my_client_user.InvokeServerTickedEvent(time, entity_deltas, hits ?? (IEnumerable<IHit>)Array.Empty<IHit>());
                }
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            // ...
        }
    }
}
