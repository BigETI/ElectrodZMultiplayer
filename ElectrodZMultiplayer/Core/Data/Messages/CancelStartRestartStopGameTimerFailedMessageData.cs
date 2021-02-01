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
    internal class CancelStartRestartStopGameTimerFailedMessageData : BaseFailedMessageData<CancelStartRestartStopGameTimerMessageData, ECancelStartRestartStopGameTimerFailedReason>
    {
        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (Reason != ECancelStartRestartStopGameTimerFailedReason.Invalid);

        /// <summary>
        /// Constructs a message to stop a timer caused by start, restart or stop
        /// </summary>
        /// <param name="message">Received message</param>
        /// <param name="reason">Reason</param>
        public CancelStartRestartStopGameTimerFailedMessageData(CancelStartRestartStopGameTimerMessageData message, ECancelStartRestartStopGameTimerFailedReason reason) : base(Naming.GetMessageTypeNameFromMessageDataType<CancelStartRestartStopGameTimerFailedMessageData>(), message, reason)
        {
            // ...
        }
    }
}
