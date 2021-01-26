using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a start game failed message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class StartGameFailedMessageData : BaseFailedMessageData<StartGameMessageData, EStartGameFailedReason>
    {
        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (Reason != EStartGameFailedReason.Invalid);

        /// <summary>
        /// Constructs a start game failed message for deserializers
        /// </summary>
        public StartGameFailedMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a start game failed message
        /// </summary>
        /// <param name="message">Received message</param>
        /// <param name="reason">Reason</param>
        public StartGameFailedMessageData(StartGameMessageData message, EStartGameFailedReason reason) : base(Naming.GetMessageTypeNameFromMessageDataType<StartGameFailedMessageData>(), message, reason)
        {
            if (reason == EStartGameFailedReason.Invalid)
            {
                throw new ArgumentException("Start game failed reason can't be invalid.", nameof(reason));
            }
        }
    }
}
