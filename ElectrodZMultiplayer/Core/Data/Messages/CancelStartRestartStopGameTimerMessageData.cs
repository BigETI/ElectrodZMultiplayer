using Newtonsoft.Json;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a message to stop a timer caused by start, restart or stop
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class CancelStartRestartStopGameTimerMessageData : BaseMessageData
    {
        /// <summary>
        /// Constructs a message to stop a timer caused by start, restart or stop
        /// </summary>
        public CancelStartRestartStopGameTimerMessageData() : base(Naming.GetMessageTypeNameFromMessageDataType<CancelStartRestartStopGameTimerMessageData>())
        {
            // ...
        }
    }
}
