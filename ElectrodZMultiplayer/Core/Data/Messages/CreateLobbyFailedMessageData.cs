using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a create lobby failed message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class CreateLobbyFailedMessageData : BaseFailedMessageData<CreateAndJoinLobbyMessageData, ECreateLobbyFailedReason>
    {
        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (Reason != ECreateLobbyFailedReason.Invalid);

        /// <summary>
        /// Constructs a create lobby failed message for deserializers
        /// </summary>
        public CreateLobbyFailedMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a create lobby failed message
        /// </summary>
        /// <param name="message">Received message</param>
        /// <param name="reason">Reason</param>
        public CreateLobbyFailedMessageData(CreateAndJoinLobbyMessageData message, ECreateLobbyFailedReason reason) : base(Naming.GetMessageTypeNameFromMessageDataType<CreateLobbyFailedMessageData>(), message, reason)
        {
            if (reason == ECreateLobbyFailedReason.Invalid)
            {
                throw new ArgumentException("Create lobby failed reason can't be invalid.", nameof(reason));
            }
        }
    }
}
