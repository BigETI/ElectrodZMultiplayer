using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a client tick failed message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ClientTickFailedMessageData : BaseFailedMessageData<ClientTickMessageData, EClientTickFailedReason>
    {
        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (Reason != EClientTickFailedReason.Invalid);

        /// <summary>
        /// Constructs a client tick failed message for deserializers
        /// </summary>
        public ClientTickFailedMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a client tick failed message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="reason">Reason</param>
        public ClientTickFailedMessageData(ClientTickMessageData message, EClientTickFailedReason reason) : base(Naming.GetMessageTypeNameFromMessageDataType<ClientTickFailedMessageData>(), message, reason)
        {
            if (reason == EClientTickFailedReason.Invalid)
            {
                throw new ArgumentException("Client tick failed reason can't be invalid.", nameof(reason));
            }
        }
    }
}
