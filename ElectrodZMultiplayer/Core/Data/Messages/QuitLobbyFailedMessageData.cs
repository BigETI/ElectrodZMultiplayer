using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a quit lobby failed message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class QuitLobbyFailedMessageData : BaseFailedMessageData<QuitLobbyMessageData, EQuitLobbyFailedReason>
    {
        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (Reason != EQuitLobbyFailedReason.Invalid);

        /// <summary>
        /// Constructs a quit lobby failed message for deserializers
        /// </summary>
        public QuitLobbyFailedMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a quit lobby failed message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="reason">Reason</param>
        public QuitLobbyFailedMessageData(QuitLobbyMessageData message, EQuitLobbyFailedReason reason) : base(Naming.GetMessageTypeNameFromMessageDataType<JoinLobbyFailedMessageData>(), message, reason)
        {
            if (reason == EQuitLobbyFailedReason.Invalid)
            {
                throw new ArgumentException("Quit lobby failed reason can't be invalid.", nameof(reason));
            }
        }
    }
}
