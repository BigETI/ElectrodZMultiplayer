using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a change user lobby color failed message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ChangeUserLobbyColorFailedMessageData : BaseFailedMessageData<ChangeUserLobbyColorMessageData, EChangeUserLobbyColorFailedReason>
    {
        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (Reason != EChangeUserLobbyColorFailedReason.Invalid);

        /// <summary>
        /// Constructs a change user lobby color failed message for deserializers
        /// </summary>
        public ChangeUserLobbyColorFailedMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a change user lobby color failed message
        /// </summary>
        /// <param name="message">Received message</param>
        /// <param name="reason">reason</param>
        public ChangeUserLobbyColorFailedMessageData(ChangeUserLobbyColorMessageData message, EChangeUserLobbyColorFailedReason reason) : base(Naming.GetMessageTypeNameFromMessageDataType<ChangeUserLobbyColorFailedMessageData>(), message, reason)
        {
            if (reason == EChangeUserLobbyColorFailedReason.Invalid)
            {
                throw new ArgumentException("Change user lobby color failed reason can't be invalid.", nameof(reason));
            }
        }
    }
}
