using ElectrodZMultiplayer.JSONConverters;
using Newtonsoft.Json;
using System.Drawing;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a user lobby color change as a message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ChangeUserLobbyColorMessageData : BaseMessageData
    {
        /// <summary>
        /// New user lobby color
        /// </summary>
        [JsonProperty("newUserLobbyColor")]
        [JsonConverter(typeof(ColorJSONConverter))]
        public Color NewUserLobbyColor { get; set; }

        /// <summary>
        /// Constructs a user lobby color change message for deserializers
        /// </summary>
        public ChangeUserLobbyColorMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a user lobby color change message
        /// </summary>
        /// <param name="newUserLobbyColor">New user lobby color</param>
        public ChangeUserLobbyColorMessageData(Color newUserLobbyColor) : base(Naming.GetMessageTypeNameFromMessageDataType<ChangeUserLobbyColorMessageData>()) => NewUserLobbyColor = newUserLobbyColor;
    }
}
