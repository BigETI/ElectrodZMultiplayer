using Newtonsoft.Json;
using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes changes performed in lobby rules
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class LobbyRulesChangedMessageData : BaseMessageData
    {
        /// <summary>
        /// Rules
        /// </summary>
        [JsonProperty("rules")]
        public LobbyRulesData Rules { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            Protection.IsValid(Rules);

        /// <summary>
        /// Constructs a message informing changes in lobby rules for deserializers
        /// </summary>
        public LobbyRulesChangedMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a message informing changes in lobby rules
        /// </summary>
        /// <param name="lobby">Lobby</param>
        public LobbyRulesChangedMessageData(ILobby lobby) : base(Naming.GetMessageTypeNameFromMessageDataType<LobbyRulesChangedMessageData>())
        {
            Dictionary<string, object> game_mode_rules = new Dictionary<string, object>();
            foreach (KeyValuePair<string, object> game_mode_rule in lobby.GameModeRules)
            {
                if (game_mode_rule.Value == null)
                {
                    throw new ArgumentException($"Value of game mode rule key \"{ game_mode_rule.Key }\" is null.", nameof(lobby));
                }
                game_mode_rules.Add(game_mode_rule.Key, game_mode_rule.Value);
            }
            Rules = new LobbyRulesData(lobby.LobbyCode, lobby.Name, lobby.IsPrivate, lobby.MinimalUserCount, lobby.MaximalUserCount, lobby.IsStartingGameAutomatically, lobby.GameMode, game_mode_rules);
        }
    }
}
