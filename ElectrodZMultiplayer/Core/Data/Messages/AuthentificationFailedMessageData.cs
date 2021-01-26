using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes an authentification failed message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class AuthentificationFailedMessageData : BaseFailedMessageData<AuthenticateMessageData, EAuthentificationFailedReason>
    {
        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (Reason != EAuthentificationFailedReason.Invalid);

        /// <summary>
        /// Constructs an authentification failed message for deserializers
        /// </summary>
        public AuthentificationFailedMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs an authentification failed message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="reason">Reason</param>
        public AuthentificationFailedMessageData(AuthenticateMessageData message, EAuthentificationFailedReason reason) : base(Naming.GetMessageTypeNameFromMessageDataType<AuthentificationFailedMessageData>(), message, reason)
        {
            if (reason == EAuthentificationFailedReason.Invalid)
            {
                throw new ArgumentException("Authentification failed reason can't be invalid.", nameof(reason));
            }
        }
    }
}
