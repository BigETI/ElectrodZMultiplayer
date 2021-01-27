using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a server tick failed message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ServerTickFailedMessageData : BaseFailedMessageData<ServerTickMessageData, EServerTickFailedReason>
    {
        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (Reason != EServerTickFailedReason.Invalid);

        /// <summary>
        /// Constructs a server tick failed message for deserializers
        /// </summary>
        public ServerTickFailedMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a server tick failed message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="reason">Reason</param>
        public ServerTickFailedMessageData(ServerTickMessageData message, EServerTickFailedReason reason) : base(Naming.GetMessageTypeNameFromMessageDataType<ServerTickFailedMessageData>(), message, reason)
        {
            if (reason == EServerTickFailedReason.Invalid)
            {
                throw new ArgumentException("Server tick failed reason can't be invalid.", nameof(reason));
            }
        }
    }
}
