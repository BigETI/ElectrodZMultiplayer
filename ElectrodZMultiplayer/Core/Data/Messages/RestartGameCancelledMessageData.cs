using Newtonsoft.Json;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a restart game cancelled message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class RestartGameCancelledMessageData : BaseMessageData
    {
        /// <summary>
        /// Constructs a restart game cancelled message
        /// </summary>
        public RestartGameCancelledMessageData() : base(Naming.GetMessageTypeNameFromMessageDataType<StartGameCancelledMessageData>())
        {
            // ...
        }
    }
}
