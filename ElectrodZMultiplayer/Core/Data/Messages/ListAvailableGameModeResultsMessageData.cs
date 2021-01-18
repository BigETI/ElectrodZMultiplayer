using Newtonsoft.Json;
using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes an authentication message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ListAvailableGameModeResultsMessageData : BaseMessageData
    {
        /// <summary>
        /// Available game modes
        /// </summary>
        [JsonProperty("gameModes")]
        public List<string> GameModes { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            Protection.IsValid(GameModes);

        /// <summary>
        /// Constructs a list available game mode results message for serializers
        /// </summary>
        public ListAvailableGameModeResultsMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a list available game mode results message
        /// </summary>
        /// <param name="gameModes"></param>
        public ListAvailableGameModeResultsMessageData(IEnumerable<string> gameModes) : base(Naming.GetMessageTypeNameFromMessageDataType<ListAvailableGameModeResultsMessageData>())
        {
            if (!Protection.IsValid(gameModes))
            {
                throw new ArgumentException("Game modes are not valid.", nameof(gameModes));
            }
            GameModes = new List<string>(gameModes);
        }
    }
}
