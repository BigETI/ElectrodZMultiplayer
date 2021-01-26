using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a restart game failed message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class RestartGameFailedMessageData : BaseFailedMessageData<RestartGameMessageData, ERestartGameFailedReason>
    {
        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (Reason != ERestartGameFailedReason.Invalid);

        /// <summary>
        /// Constructs a restart game failed message for deserializers
        /// </summary>
        public RestartGameFailedMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a restart game failed message
        /// </summary>
        /// <param name="message">Received message</param>
        /// <param name="reason">Reason</param>
        public RestartGameFailedMessageData(RestartGameMessageData message, ERestartGameFailedReason reason) : base(Naming.GetMessageTypeNameFromMessageDataType<RestartGameFailedMessageData>(), message, reason)
        {
            if (reason == ERestartGameFailedReason.Invalid)
            {
                throw new ArgumentException("Restart game failed reason can't be invalid.", nameof(reason));
            }
        }
    }
}
