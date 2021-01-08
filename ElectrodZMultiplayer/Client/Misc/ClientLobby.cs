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
        /// Client
        /// </summary>
        public IClientSynchronizer Client => client;

        /// <summary>
        /// Internal game mode rules
        /// </summary>
        public Dictionary<string, object> InternalGameModeRules { get; } = new Dictionary<string, object>();

        /// <summary>
        /// Lobby owner
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
        public uint UserCount => (uint)InternalUsers.Count;

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
        public IReadOnlyDictionary<string, object> GameModeRules => InternalGameModeRules;

        /// <summary>
        /// Is valid
        /// </summary>
        public bool IsValid =>
            (LobbyCode != null) &&
            (Name != null) &&
            (MinimalUserCount <= MaximalUserCount) &&
            (UserCount <= MaximalUserCount) &&
            !string.IsNullOrWhiteSpace(GameMode) &&
            !Protection.ContainsNullOrInvalid(GameModeRules) &&
            !Protection.ContainsNullOrInvalid(Users) &&
            !Protection.ContainsNullOrInvalid(Entities);

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="connection">Connection</param>
        /// <param name="lobbyCode">Lobby code</param>
        /// <param name="name">Lobby name</param>
        /// <param name="minimalUserCount">Minimal user count</param>
        /// <param name="maximalUserCount">Maximal user count</param>
        /// <param name="users">Users</param>
        /// <param name="isStartingGameAutomatically">Is starting game automatically</param>
        /// <param name="gameMode">Game mode</param>
        /// <param name="gameModeRules">Game mode rules</param>
        public ClientLobby(IInternalClientSynchronizer connection, string lobbyCode, string name, uint minimalUserCount, uint maximalUserCount, bool isStartingGameAutomatically, string gameMode, IReadOnlyDictionary<string, object> gameModeRules, IUser owner, IReadOnlyDictionary<string, IUser> users)
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
            this.client = connection ?? throw new ArgumentNullException(nameof(connection));
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
                InternalGameModeRules.Add(game_mode_rule.Key, game_mode_rule.Value);
            }
            Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            foreach (KeyValuePair<string, IUser> user in users)
            {
                if (user.Value == null)
                {
                    throw new ArgumentException($"Value of key \"{ user.Key }\" is null.");
                }
                InternalUsers.Add(user.Key, user.Value);
            }
        }

        /// <summary>
        /// Leave lobby
        /// </summary>
        public void Leave() => client.SendQuitLobbyMessage();

        /// <summary>
        /// Sets a new game mode internally
        /// </summary>
        /// <param name="gameMode">Game mode</param>
        public void SetGameModeInternally(string gameMode)
        {
            if (string.IsNullOrWhiteSpace(gameMode))
            {
                throw new ArgumentException("Game mode can't be unknown.");
            }
            GameMode = gameMode;
        }

        /// <summary>
        /// Sets the lobby code
        /// </summary>
        /// <param name="lobbyCode">Lobby code</param>
        public void SetLobbyCodeInternally(string lobbyCode) => LobbyCode = lobbyCode ?? throw new ArgumentNullException(nameof(lobbyCode));

        /// <summary>
        /// Sets the name of this lobby
        /// </summary>
        /// <param name="name">Lobby name</param>
        public void SetNameInternally(string name) => Name = name ?? throw new ArgumentNullException(nameof(name));

        /// <summary>
        /// Sets the minimal amount of users to allow starting a game
        /// </summary>
        /// <param name="minimalUserCount">Minimal user count</param>
        public void SetMinimalUserCountInternally(uint minimalUserCount) => MinimalUserCount = minimalUserCount;

        /// <summary>
        /// Sets the maximal amount of users allowed
        /// </summary>
        /// <param name="maximalUserCount">Maximal user count</param>
        public void SetMaximalUserCountInternally(uint maximalUserCount) => MaximalUserCount = maximalUserCount;

        /// <summary>
        /// Sets the starting game automatically state
        /// </summary>
        /// <param name="isStartingGameAutomatically">Is starting game automatically</param>
        public void SetStartingGameAutomaticallyStateInternally(bool isStartingGameAutomatically) => IsStartingGameAutomatically = isStartingGameAutomatically;

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            // ...
        }
    }
}
