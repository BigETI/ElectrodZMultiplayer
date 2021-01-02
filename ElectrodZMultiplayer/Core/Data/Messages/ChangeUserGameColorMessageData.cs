using ElectrodZMultiplayer.JSONConverters;
using Newtonsoft.Json;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a user game color change as a message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ChangeUserGameColorMessageData : BaseMessageData
    {
        /// <summary>
        /// New user game color
        /// </summary>
        [JsonProperty("newUserGameColor")]
        [JsonConverter(typeof(GameColorJSONConverter))]
        public EGameColor NewUserGameColor { get; set; }

        /// <summary>
        /// Constructs a user game color change message for deserializers
        /// </summary>
        public ChangeUserGameColorMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a user game color change message
        /// </summary>
        /// <param name="newUserGameColor">New user game color</param>
        public ChangeUserGameColorMessageData(EGameColor newUserGameColor) : base(Naming.GetMessageTypeNameFromMessageDataType<ChangeUserGameColorMessageData>()) => NewUserGameColor = newUserGameColor;
    }
}
