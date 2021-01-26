using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a stop game failed message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class StopGameFailedMessageData : BaseFailedMessageData<StopGameMessageData, EStopGameFailedReason>
    {
        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (Reason != EStopGameFailedReason.Invalid);

        /// <summary>
        /// Constructs a stop game failed message for deserializers
        /// </summary>
        public StopGameFailedMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a stop game failed message
        /// </summary>
        /// <param name="message">Received message</param>
        /// <param name="reason">Reason</param>
        public StopGameFailedMessageData(StopGameMessageData message, EStopGameFailedReason reason) : base(Naming.GetMessageTypeNameFromMessageDataType<StopGameFailedMessageData>(), message, reason)
        {
            if (reason == EStopGameFailedReason.Invalid)
            {
                throw new ArgumentException("Stop game failed reason can't be invalid.", nameof(reason));
            }
        }
    }
}
