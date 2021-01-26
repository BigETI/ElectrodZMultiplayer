using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a list available game modes failed message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ListAvailableGameModesFailedMessageData : BaseFailedMessageData<ListAvailableGameModesMessageData, EListAvailableGameModesFailedReason>
    {
        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (Reason != EListAvailableGameModesFailedReason.Invalid);

        /// <summary>
        /// Constructs a list available game modes message for deserializers
        /// </summary>
        public ListAvailableGameModesFailedMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a list available game modes failed message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="reason">Reason</param>
        public ListAvailableGameModesFailedMessageData(ListAvailableGameModesMessageData message, EListAvailableGameModesFailedReason reason) : base(Naming.GetMessageTypeNameFromMessageDataType<ListAvailableGameModesFailedMessageData>(), message, reason)
        {
            if (reason == EListAvailableGameModesFailedReason.Invalid)
            {
                throw new ArgumentException("List available game modes failed reason can't be invalid.", nameof(reason));
            }
        }
    }
}
