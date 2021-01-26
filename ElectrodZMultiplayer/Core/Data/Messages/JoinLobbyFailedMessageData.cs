using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a join lobby failed message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class JoinLobbyFailedMessageData : BaseFailedMessageData<JoinLobbyMessageData, EJoinLobbyFailedReason>
    {
        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (Reason != EJoinLobbyFailedReason.Invalid);

        /// <summary>
        /// Constructs a join lobby failed message for deserializers
        /// </summary>
        public JoinLobbyFailedMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a join lobby failed message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="reason">Reason</param>
        public JoinLobbyFailedMessageData(JoinLobbyMessageData message, EJoinLobbyFailedReason reason) : base(Naming.GetMessageTypeNameFromMessageDataType<JoinLobbyFailedMessageData>(), message, reason)
        {
            if (reason == EJoinLobbyFailedReason.Invalid)
            {
                throw new ArgumentException("Join lobby failed reason can't be invalid.", nameof(reason));
            }
        }
    }
}
