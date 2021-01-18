using Newtonsoft.Json;
using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer data namespace
/// </summary>
namespace ElectrodZMultiplayer.Data
{
    /// <summary>
    /// A class that describes lobby rules data
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class LobbyRulesData : IValidable
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
        /// Is object in a valid state
        /// </summary>
        public bool IsValid =>
            (LobbyCode != null) &&
            (Name != null) &&
            (MinimalUserCount <= MaximalUserCount) &&
            !string.IsNullOrWhiteSpace(GameMode) &&
            (GameModeRules != null) &&
            Protection.IsValid(GameModeRules);

        /// <summary>
        /// Constructs lobby rules data for deserializers
        /// </summary>
        public LobbyRulesData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs lobby rules data
        /// </summary>
        /// <param name="lobbyCode">Lobby code</param>
        /// <param name="name">Lobby name</param>
        /// <param name="minimalUserCount">Minimal user count in lobby</param>
        /// <param name="maximalUserCount">Maximal user count in lobby</param>
        /// <param name="isStartingGameAutomatically">Is starting game automatically in lobby</param>
        /// <param name="gameMode">Game mode</param>
        /// <param name="gameModeRules">Game mode rules</param>
        public LobbyRulesData(string lobbyCode, string name, uint minimalUserCount, uint maximalUserCount, bool isStartingGameAutomatically, string gameMode, Dictionary<string, object> gameModeRules)
        {
            if (gameModeRules == null)
            {
                throw new ArgumentNullException(nameof(gameModeRules));
            }
            if (!Protection.IsValid(gameModeRules.Values))
            {
                throw new ArgumentException($"Game mode rules are not value.", nameof(gameModeRules));
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
            GameModeRules = gameModeRules;
        }
    }
}
