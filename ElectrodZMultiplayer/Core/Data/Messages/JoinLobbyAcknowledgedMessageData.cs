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
    /// A class that describes an acknowledgment about joining a lobby as a message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class JoinLobbyAcknowledgedMessageData : BaseMessageData
    {
        /// <summary>
        /// Current lobby rules
        /// </summary>
        [JsonProperty("rules")]
        public LobbyRulesData Rules { get; set; }

        /// <summary>
        /// Lobby owner GUID
        /// </summary>
        [JsonProperty("ownerGUID")]
        [JsonConverter(typeof(GUIDJSONConverter))]
        public Guid OwnerGUID;

        /// <summary>
        /// Users in lobby
        /// </summary>
        [JsonProperty("users")]
        public List<UserData> Users { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            Protection.IsValid(Rules) &&
            (OwnerGUID != Guid.Empty) &&
            !Protection.ContainsNullOrInvalid(Users) &&
            Protection.Contains(Users, (element) => element.GUID == OwnerGUID);

        /// <summary>
        /// Constructs an acknowledgment to join a lobby as a message for deserializers
        /// </summary>
        public JoinLobbyAcknowledgedMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs an acknowledgment to join a lobby as a message for deserializers
        /// </summary>
        /// <param name="lobby">Lobby being acknowledged</param>
        public JoinLobbyAcknowledgedMessageData(ILobby lobby) : base(Naming.GetMessageTypeNameFromMessageDataType<JoinLobbyAcknowledgedMessageData>())
        {
            if (lobby == null)
            {
                throw new ArgumentNullException(nameof(lobby));
            }
            if (!lobby.IsValid)
            {
                throw new ArgumentException("Lobby is not valid.", nameof(lobby));
            }
            Dictionary<string, object> game_mode_rules = new Dictionary<string, object>();
            foreach (KeyValuePair<string, object> game_mode_rule in lobby.GameModeRules)
            {
                if (game_mode_rule.Value == null)
                {
                    throw new ArgumentException($"Value of game mode rule key \"{ game_mode_rule.Key }\" is null", nameof(lobby));
                }
                game_mode_rules.Add(game_mode_rule.Key, game_mode_rule.Value);
            }
            Rules = new LobbyRulesData(lobby.LobbyCode, lobby.Name, lobby.MinimalUserCount, lobby.MaximalUserCount, lobby.IsStartingGameAutomatically, lobby.GameMode, game_mode_rules);
            OwnerGUID = lobby.Owner.GUID;
            Users = new List<UserData>();
            foreach (IUser user in lobby.Users.Values)
            {
                if (user == null)
                {
                    throw new ArgumentException($"Lobby contains null users.", nameof(lobby));
                }
                Users.Add(new UserData(user.GUID, user.GameColor, user.Name, user.LobbyColor));
            }
        }
    }
}
