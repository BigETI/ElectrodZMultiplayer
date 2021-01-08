using ElectrodZMultiplayer.JSONConverters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// Change lobby rules message data class
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ChangeLobbyRulesMessageData : BaseMessageData
    {
        /// <summary>
        /// Lobby name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Minimal user count
        /// </summary>
        [JsonProperty("minUserCount")]
        public uint? MinimalUserCount { get; set; }

        /// <summary>
        /// Maximal user count
        /// </summary>
        [JsonProperty("maxUserCount")]
        public uint? MaximalUserCount { get; set; }

        /// <summary>
        /// Is starting game automatically
        /// </summary>
        [JsonProperty("isStartingGameAutomatically")]
        public bool? IsStartingGameAutomatically { get; set; }

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
        /// Is valid
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            ((Name == null) || ((Name.Trim().Length >= Defaults.minimalLobbyNameLength) && (Name.Trim().Length <= Defaults.maximalLobbyNameLength))) &&
            ((MinimalUserCount == null) || (MaximalUserCount == null) || (MinimalUserCount <= MaximalUserCount)) &&
            ((GameMode == null) || !string.IsNullOrWhiteSpace(GameMode)) &&
            ((GameModeRules == null) || !GameModeRules.ContainsValue(null));

        /// <summary>
        /// Default constructor
        /// </summary>
        public ChangeLobbyRulesMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Lobby name</param>
        /// <param name="minimalUserCount">Minimal user count</param>
        /// <param name="maximalUserCount">Maximal user count</param>
        /// <param name="isStartingGameAutomatically">Is satarting game automatically</param>
        /// <param name="gameMode">Game mode</param>
        /// <param name="gameModeRules">Game mode rules</param>
        public ChangeLobbyRulesMessageData(string name, uint? minimalUserCount, uint? maximalUserCount, bool? isStartingGameAutomatically, string gameMode, Dictionary<string, object> gameModeRules) : base(Naming.GetMessageTypeNameFromMessageDataType<ChangeLobbyRulesMessageData>())
        {
            string new_name = null;
            if (name != null)
            {
                new_name = name.Trim();
                if ((new_name.Length < Defaults.minimalLobbyNameLength) || (new_name.Length > Defaults.maximalLobbyNameLength))
                {
                    throw new ArgumentException($"Lobby name must be between { Defaults.minimalLobbyNameLength } and { Defaults.maximalLobbyNameLength } characters long.", nameof(name));
                }
            }
            if ((minimalUserCount != null) && (maximalUserCount != null) && (minimalUserCount > maximalUserCount))
            {
                throw new ArgumentException("Minimal user count can't be greater than maximal user count.", nameof(minimalUserCount));
            }
            if ((gameMode != null) && string.IsNullOrWhiteSpace(gameMode))
            {
                throw new ArgumentException("Game mode can't be unknown.", nameof(gameMode));
            }
            if ((gameModeRules != null) && gameModeRules.ContainsValue(null))
            {
                throw new ArgumentException("Game mode rules contains null.", nameof(gameModeRules));
            }
            Name = new_name;
            MinimalUserCount = minimalUserCount;
            MaximalUserCount = maximalUserCount;
            IsStartingGameAutomatically = isStartingGameAutomatically;
            GameMode = gameMode;
            GameModeRules = gameModeRules;
        }
    }
}
