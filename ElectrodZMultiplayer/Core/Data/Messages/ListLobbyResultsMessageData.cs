using Newtonsoft.Json;
using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a result set for listing lobbies as a message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ListLobbyResultsMessageData : BaseMessageData
    {
        /// <summary>
        /// Lobbies
        /// </summary>
        [JsonProperty("lobbies")]
        public List<LobbyData> Lobbies { get; set; }

        /// <summary>
        /// Is valid
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            Protection.IsValid(Lobbies);

        /// <summary>
        /// Constructs a message that contains a result set for listing lobbies for deserializers
        /// </summary>
        public ListLobbyResultsMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a message that contains a result set for listing lobbies
        /// </summary>
        /// <param name="lobbies">Lobbies</param>
        public ListLobbyResultsMessageData(IEnumerable<ILobbyView> lobbies) : base(Naming.GetMessageTypeNameFromMessageDataType<ListLobbyResultsMessageData>())
        {
            if (!Protection.IsValid(lobbies))
            {
                throw new ArgumentException($"Lobbies are not valid.", nameof(lobbies));
            }
            Lobbies = new List<LobbyData>();
            foreach (ILobbyView lobby in lobbies)
            {
                Lobbies.Add(new LobbyData(lobby.LobbyCode, lobby.Name, lobby.MinimalUserCount, lobby.MaximalUserCount, lobby.IsStartingGameAutomatically, lobby.GameMode, lobby.GameModeRules, lobby.UserCount));
            }
        }
    }
}
