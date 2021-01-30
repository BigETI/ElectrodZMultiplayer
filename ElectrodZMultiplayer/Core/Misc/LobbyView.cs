using ElectrodZMultiplayer.Data;
using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Lobby view class
    /// </summary>
    internal class LobbyView : ILobbyView
    {
        /// <summary>
        /// Lobby code
        /// </summary>
        public string LobbyCode { get; }

        /// <summary>
        /// Lobby name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Game mode
        /// </summary>
        public string GameMode { get; }

        /// <summary>
        /// Is lobby private
        /// </summary>
        public bool IsPrivate { get; }

        /// <summary>
        /// Minimal user count
        /// </summary>
        public uint MinimalUserCount { get; }

        /// <summary>
        /// Maximal user count
        /// </summary>
        public uint MaximalUserCount { get; }

        /// <summary>
        /// Is starting game automatically
        /// </summary>
        public bool IsStartingGameAutomatically { get; }

        /// <summary>
        /// Game mode rules
        /// </summary>
        public IReadOnlyDictionary<string, object> GameModeRules { get; }

        /// <summary>
        /// User count
        /// </summary>
        public uint UserCount { get; }

        /// <summary>
        /// Is valid
        /// </summary>
        public bool IsValid =>
            (LobbyCode != null) &&
            (Name != null) &&
            (MinimalUserCount <= MaximalUserCount) &&
            (UserCount <= MaximalUserCount) &&
            !string.IsNullOrWhiteSpace(GameMode) &&
            (GameModeRules != null) &&
            Protection.IsValid(GameModeRules.Values);

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lobbyCode">Lobby code</param>
        /// <param name="name">Lobby name</param>
        /// <param name="gameMode">Game mode</param>
        /// <param name="isPrivate">Is lobby private</param>
        /// <param name="minimalUserCount">Minimal user count</param>
        /// <param name="maximalUserCount">Maximal user count</param>
        /// <param name="isStartingGameAutomatically">Is starting game automatically</param>
        /// <param name="gameModeRules">Game mode rules</param>
        /// <param name="userCount">User count</param>
        public LobbyView(string lobbyCode, string name, string gameMode, bool isPrivate, uint minimalUserCount, uint maximalUserCount, bool isStartingGameAutomatically, IReadOnlyDictionary<string, object> gameModeRules, uint userCount)
        {
            if (minimalUserCount > maximalUserCount)
            {
                throw new ArgumentException("Minimal user count can't be greater than maximal user count.");
            }
            if (userCount > maximalUserCount)
            {
                throw new ArgumentException("User count can't be greater than maximal user count.");
            }
            if (string.IsNullOrWhiteSpace(gameMode))
            {
                throw new ArgumentException("Game mode can't be unknown.");
            }
            LobbyCode = lobbyCode ?? throw new ArgumentNullException(nameof(lobbyCode));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            GameMode = gameMode;
            IsPrivate = isPrivate;
            MinimalUserCount = minimalUserCount;
            MaximalUserCount = maximalUserCount;
            IsStartingGameAutomatically = isStartingGameAutomatically;
            Dictionary<string, object> game_mode_rules = new Dictionary<string, object>();
            foreach (KeyValuePair<string, object> game_mode_rule in gameModeRules)
            {
                if (game_mode_rule.Value == null)
                {
                    throw new ArgumentException($"Value of key \"{ game_mode_rule.Key }\" is null.");
                }
                game_mode_rules.Add(game_mode_rule.Key, game_mode_rule.Value);
            }
            GameModeRules = game_mode_rules;
            UserCount = userCount;
        }

        /// <summary>
        /// Explicit cast operator
        /// </summary>
        /// <param name="lobby">Lobby</param>
        public static explicit operator LobbyView(LobbyData lobby)
        {
            if (lobby == null)
            {
                throw new ArgumentNullException(nameof(lobby));
            }
            if (!lobby.IsValid)
            {
                throw new ArgumentException($"\"{ nameof(lobby) }\" is invalid.", nameof(lobby));
            }
            return new LobbyView(lobby.LobbyCode, lobby.Name, lobby.GameMode, lobby.IsPrivate, lobby.MinimalUserCount, lobby.MaximalUserCount, lobby.IsStartingGameAutomatically, lobby.GameModeRules, lobby.UserCount);
        }
    }
}
