using Newtonsoft.Json;
using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer data namespace
/// </summary>
namespace ElectrodZMultiplayer.Data
{
    /// <summary>
    /// A class that describes lobby data
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class LobbyData : IValidable
    {
        /// <summary>
        /// Lobby code
        /// </summary>
        [JsonProperty("lobbyCode")]
        public string LobbyCode { get; set; }

        /// <summary>
        /// Lobby name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Minimal user count in lobby
        /// </summary>
        [JsonProperty("minUserCount")]
        public uint MinimalUserCount { get; set; }

        /// <summary>
        /// Maximal user count in lobby
        /// </summary>
        [JsonProperty("maxUserCount")]
        public uint MaximalUserCount { get; set; }

        /// <summary>
        /// Is starting game automatically in lobby
        /// </summary>
        [JsonProperty("isStartingGameAutomatically")]
        public bool IsStartingGameAutomatically { get; set; }

        /// <summary>
        /// Game mode
        /// </summary>
        [JsonProperty("gameMode")]
        public string GameMode { get; set; }

        /// <summary>
        /// Game mode rules
        /// </summary>
        [JsonProperty("gameModeRules")]
        public Dictionary<string, object> GameModeRules { get; set; }

        /// <summary>
        /// User count in lobby
        /// </summary>
        [JsonProperty("userCount")]
        public uint UserCount { get; set; }

        /// <summary>
        /// Is valid
        /// </summary>
        public bool IsValid =>
            (LobbyCode != null) &&
            (Name != null) &&
            (MinimalUserCount <= MaximalUserCount) &&
            !string.IsNullOrWhiteSpace(GameMode) &&
            (UserCount <= MaximalUserCount) &&
            !Protection.ContainsNullOrInvalid(GameModeRules);

        /// <summary>
        /// Constructs lobby data for deserializers
        /// </summary>
        public LobbyData()
        {
            // ...
        }

        /// <summary>
        /// Constructs lobby data
        /// </summary>
        /// <param name="lobbyCode">Lobby code</param>
        /// <param name="name">Lobby name</param>
        /// <param name="minimalUserCount">Minimal user count in lobby</param>
        /// <param name="maximalUserCount">Maximal user count in lobby</param>
        /// <param name="isStartingGameAutomatically">Is starting game automatically in lobby</param>
        /// <param name="gameMode">Game mode</param>
        /// <param name="gameModeRules">Game mode rules</param>
        /// <param name="userCount">User count in lobby</param>
        public LobbyData(string lobbyCode, string name, uint minimalUserCount, uint maximalUserCount, bool isStartingGameAutomatically, string gameMode, IReadOnlyDictionary<string, object> gameModeRules, uint userCount)
        {
            if (Protection.ContainsNullOrInvalid(gameModeRules))
            {
                throw new ArgumentException($"\"{ nameof(gameModeRules) }\" contains null.", nameof(gameModeRules));
            }
            if (string.IsNullOrWhiteSpace(gameMode))
            {
                throw new ArgumentException($"Game mode is unknown.", nameof(gameMode));
            }
            LobbyCode = lobbyCode ?? throw new ArgumentNullException(nameof(lobbyCode));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            MinimalUserCount = minimalUserCount;
            MaximalUserCount = maximalUserCount;
            IsStartingGameAutomatically = isStartingGameAutomatically;
            GameMode = gameMode;
            GameModeRules = new Dictionary<string, object>();
            foreach (KeyValuePair<string, object> game_mode_rule in gameModeRules)
            {
                if (game_mode_rule.Value == null)
                {
                    throw new ArgumentException($"Value of game mode rule key \"{ game_mode_rule.Key }\" is null.", nameof(gameModeRules));
                }
                GameModeRules.Add(game_mode_rule.Key, game_mode_rule.Value);
            }
            UserCount = userCount;
        }
    }
}
