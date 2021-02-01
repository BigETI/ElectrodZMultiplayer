using Newtonsoft.Json;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a start game cancelled message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class StartGameCancelledMessageData : BaseMessageData
    {
        /// <summary>
        /// Constructs a start game cancelled message
        /// </summary>
        public StartGameCancelledMessageData() : base(Naming.GetMessageTypeNameFromMessageDataType<StartGameCancelledMessageData>())
        {
            // ...
        }
    }
}
