using Newtonsoft.Json;
using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a list lobbies request message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ListLobbiesMessageData : BaseMessageData
    {
        /// <summary>
        /// Exclude full lobbies
        /// </summary>
        [JsonProperty("excludeFull")]
        public bool? ExcludeFull { get; set; }

        /// <summary>
        /// Lobby name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Minimal user count
        /// </summary>
        [JsonProperty("minPlayerCount")]
        public uint? MinimalUserCount { get; set; }

        /// <summary>
        /// Maximal user count
        /// </summary>
        [JsonProperty("maxPlayerCOunt")]
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
            ((MinimalUserCount == null) || (MaximalUserCount == null) || (MinimalUserCount <= MaximalUserCount)) &&
            ((GameMode == null) || !string.IsNullOrWhiteSpace(GameMode)) &&
            ((GameModeRules == null) || !GameModeRules.ContainsValue(null));

        /// <summary>
        /// Constructs a list lobbies request message for deserializers
        /// </summary>
        public ListLobbiesMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a list lobbies request message for deserializers
        /// </summary>
        /// <param name="excludeFull">Exclude full lobbies</param>
        /// <param name="name">Lobby name</param>
        /// <param name="minimalUserCount">Minimal user count</param>
        /// <param name="maximalUserCount">Maximal user count</param>
        /// <param name="isStartingGameAutomatically">Is starting game automatically</param>
        /// <param name="gameMode">Game mode</param>
        /// <param name="gameModeRules">Game mode rules</param>
        public ListLobbiesMessageData(bool? excludeFull, string name, uint? minimalUserCount, uint? maximalUserCount, bool? isStartingGameAutomatically, string gameMode, Dictionary<string, object> gameModeRules) : base(Naming.GetMessageTypeNameFromMessageDataType<ListLobbiesMessageData>())
        {
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
            ExcludeFull = excludeFull;
            Name = name;
            MinimalUserCount = minimalUserCount;
            MaximalUserCount = maximalUserCount;
            IsStartingGameAutomatically = isStartingGameAutomatically;
            GameMode = gameMode;
            GameModeRules = gameModeRules;
        }
    }
}
