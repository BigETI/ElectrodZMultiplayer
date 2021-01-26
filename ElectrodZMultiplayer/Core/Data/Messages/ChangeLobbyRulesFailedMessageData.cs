using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a change lobby rules failed message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ChangeLobbyRulesFailedMessageData : BaseFailedMessageData<ChangeLobbyRulesMessageData, EChangeLobbyRulesFailedReason>
    {
        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (Reason != EChangeLobbyRulesFailedReason.Invalid);

        /// <summary>
        /// Constructs a change lobby rules failed message for deserializers
        /// </summary>
        public ChangeLobbyRulesFailedMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a change lobby rules failed message
        /// </summary>
        /// <param name="message">Received message</param>
        /// <param name="reason">reason</param>
        public ChangeLobbyRulesFailedMessageData(ChangeLobbyRulesMessageData message, EChangeLobbyRulesFailedReason reason) : base(Naming.GetMessageTypeNameFromMessageDataType<ChangeLobbyRulesFailedMessageData>(), message, reason)
        {
            if (reason == EChangeLobbyRulesFailedReason.Invalid)
            {
                throw new ArgumentException("Change lobby rules failed reason can't be invalid.", nameof(reason));
            }
        }
    }
}
