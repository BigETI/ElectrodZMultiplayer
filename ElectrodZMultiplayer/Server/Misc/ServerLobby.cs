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
        /// Server
        /// </summary>
        public IServerSynchronizer Server { get; }

        /// <summary>
        /// Owner
        /// </summary>
        public IUser Owner { get; }

        /// <summary>
        /// Internal users
        /// </summary>
        public Dictionary<string, IUser> InternalUsers { get; } = new Dictionary<string, IUser>();

        /// <summary>
        /// Users
        /// </summary>
        public IReadOnlyDictionary<string, IUser> Users => InternalUsers;

        /// <summary>
        /// Internal entities
        /// </summary>
        public Dictionary<string, IEntity> InternalEntities { get; } = new Dictionary<string, IEntity>();

        /// <summary>
        /// Entities
        /// </summary>
        public IReadOnlyDictionary<string, IEntity> Entities => InternalEntities;

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
        public string GameMode { get; private set; }

        /// <summary>
        /// Game mode rules
        /// </summary>
        public IReadOnlyDictionary<string, object> GameModeRules => gameModeRules;

        /// <summary>
        /// User count
        /// </summary>
        public uint UserCount => (uint)InternalUsers.Count;

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
            (InternalUsers != null) &&
            Protection.IsValid(gameModeRules.Values) &&
            Protection.IsValid(InternalUsers.Values);

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
        /// <param name="gameMode">Game mode</param>
        /// <param name="gameModeRules">Game mode rules</param>
        /// <param name="server">Server</param>
        public ServerLobby(string lobbyCode, string name, uint minimalUserCount, uint maximalUserCount, bool isStartingGameAutomatically, string gameMode, IReadOnlyDictionary<string, object> gameModeRules, IServerSynchronizer server, IUser owner)
        {
            if (minimalUserCount > maximalUserCount)
            {
                throw new ArgumentException("Minimal user count can't be greater than maximal user count.");
            }
            LobbyCode = lobbyCode ?? throw new ArgumentNullException(nameof(lobbyCode));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            MinimalUserCount = minimalUserCount;
            MaximalUserCount = maximalUserCount;
            IsStartingGameAutomatically = isStartingGameAutomatically;
            GameMode = gameMode ?? Defaults.gameMode;
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
        /// Sets a new gamemode
        /// </summary>
        /// <param name="gameMode"></param>
        public void SetGameModeInternally(string gameMode)
        {
            if (string.IsNullOrWhiteSpace(gameMode))
            {
                throw new ArgumentNullException(nameof(gameMode));
            }
            GameMode = gameMode;
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
        /// Removes user
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="reason">Reason</param>
        /// <returns>"true" if user was removed, otherwise "false"</returns>
        public bool RemoveUser(IUser user, string reason)
        {
            bool ret = false;
            string key = user.GUID.ToString();
            if (InternalUsers.ContainsKey(key))
            {
                SendUserLeftMessage(user, reason);
                ret = InternalUsers.Remove(key);
            }
            return ret;
        }

        /// <summary>
        /// Sends a message to all
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="message">Message</param>
        public void SendMessageToAll<T>(T message) where T : IBaseMessageData
        {
            foreach (IServerUser user in InternalUsers.Values)
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
        /// <param name="user">User</param>
        public void SendUserJoinedMessage(IUser user) => SendMessageToAll(new UserJoinedMessageData(user));

        /// <summary>
        /// Sends an user left message
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="reason">Reason</param>
        public void SendUserLeftMessage(IUser user, string reason) => SendMessageToAll(new UserLeftMessageData(user, reason));

        /// <summary>
        /// Sends an user game color changed message
        /// </summary>
        /// <param name="user">User</param>
        public void SendUserGameColorChangedMessage(IUser user) => SendMessageToAll(new UserGameColorChangedMessageData(user));

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
        public void SendGameStartRequestedMessage(float time) => SendMessageToAll(new GameStartRequestedMessageData(time));

        /// <summary>
        /// Sends a game restart requested message
        /// </summary>
        /// <param name="time">Time to restart in seconds</param>
        public void SendGameRestartRequestedMessage(float time) => SendMessageToAll(new GameRestartRequestedMessageData(time));

        /// <summary>
        /// Sends a game stop requested message
        /// </summary>
        /// <param name="time">Time to stop game in seconds</param>
        public void SendGameStopRequestedMessage(float time) => SendMessageToAll(new GameStopRequestedMessageData(time));

        /// <summary>
        /// Sends a start game message
        /// </summary>
        public void SendStartGameMessage() => SendMessageToAll(new GameStartedMessageData());

        /// <summary>
        /// Sends a restart game message
        /// </summary>
        public void SendRestartGameMessage() => SendMessageToAll(new GameRestartedMessageData());

        /// <summary>
        /// Sends a stop game message
        /// </summary>
        public void SendStopGameMessage() => SendMessageToAll(new GameStoppedMessageData());

        /// <summary>
        /// Sends a server tick message
        /// </summary>
        /// <param name="time">Time</param>
        /// <param name="entities">Entities</param>
        public void SendServerTickMessage(float time, IEnumerable<IEntity> entities) => SendMessageToAll(new ServerTickMessageData(time, entities));

        /// <summary>
        /// Closes lobby
        /// </summary>
        public void Close()
        {
            List<IUser> users = new List<IUser>(InternalUsers.Values);
            foreach (IServerUser user in users)
            {
                RemoveUser(user, "Lobby has been closed.");
            }
            users.Clear();
            OnLobbyClosed?.Invoke(this);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() => Close();
    }
}
