using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a kick user failed message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class KickUserFailedMessageData : BaseFailedMessageData<KickUserMessageData, EKickUserFailedReason>
    {
        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (Reason != EKickUserFailedReason.Invalid);

        /// <summary>
        /// Constructs a kick user failed message for deserializers
        /// </summary>
        public KickUserFailedMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a kick user failed failed message
        /// </summary>
        /// <param name="message">Received message</param>
        /// <param name="reason">Reason</param>
        public KickUserFailedMessageData(KickUserMessageData message, EKickUserFailedReason reason) : base(Naming.GetMessageTypeNameFromMessageDataType<KickUserFailedMessageData>(), message, reason)
        {
            if (reason == EKickUserFailedReason.Invalid)
            {
                throw new ArgumentException("Kick user failed reason can't be invalid.", nameof(reason));
            }
        }
    }
}
