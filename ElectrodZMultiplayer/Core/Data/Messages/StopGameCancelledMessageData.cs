using Newtonsoft.Json;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a stop game cancelled message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class StopGameCancelledMessageData : BaseMessageData
    {
        /// <summary>
        /// Constructs a stop game cancelled message
        /// </summary>
        public StopGameCancelledMessageData() : base(Naming.GetMessageTypeNameFromMessageDataType<StopGameCancelledMessageData>())
        {
            // ...
        }
    }
}
