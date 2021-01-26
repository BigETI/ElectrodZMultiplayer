using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a list lobbies failed message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ListLobbiesFailedMessageData : BaseFailedMessageData<ListLobbiesMessageData, EListLobbiesFailedReason>
    {
        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (Reason != EListLobbiesFailedReason.Invalid);

        /// <summary>
        /// Constructs a list lobbies message for deserializers
        /// </summary>
        public ListLobbiesFailedMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a list lobbies failed message
        /// </summary>
        /// <param name="message">Received message</param>
        /// <param name="reason">Reason</param>
        public ListLobbiesFailedMessageData(ListLobbiesMessageData message, EListLobbiesFailedReason reason) : base(Naming.GetMessageTypeNameFromMessageDataType<ListLobbiesFailedMessageData>(), message, reason)
        {
            if (reason == EListLobbiesFailedReason.Invalid)
            {
                throw new ArgumentException("List lobbies failed reason can't be invalid.", nameof(reason));
            }
        }
    }
}
