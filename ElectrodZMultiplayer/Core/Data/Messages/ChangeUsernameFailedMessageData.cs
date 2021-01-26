using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a change username failed message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ChangeUsernameFailedMessageData : BaseFailedMessageData<ChangeUsernameMessageData, EChangeUsernameFailedReason>
    {
        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (Reason != EChangeUsernameFailedReason.Invalid);

        /// <summary>
        /// Constructs a change username failed message for deserializers
        /// </summary>
        public ChangeUsernameFailedMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a change username failed message
        /// </summary>
        /// <param name="message">Received message</param>
        /// <param name="reason">reason</param>
        public ChangeUsernameFailedMessageData(ChangeUsernameMessageData message, EChangeUsernameFailedReason reason) : base(Naming.GetMessageTypeNameFromMessageDataType<ChangeLobbyRulesFailedMessageData>(), message, reason)
        {
            if (reason == EChangeUsernameFailedReason.Invalid)
            {
                throw new ArgumentException("Change username failed reason can't be invalid.", nameof(reason));
            }
        }
    }
}
