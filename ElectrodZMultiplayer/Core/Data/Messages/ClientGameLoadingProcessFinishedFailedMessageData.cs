using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a client game loading process finished failed message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ClientGameLoadingProcessFinishedFailedMessageData : BaseFailedMessageData<ClientGameLoadingProcessFinishedMessageData, EClientGameLoadingProcessFinishedFailedReason>
    {
        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (Reason != EClientGameLoadingProcessFinishedFailedReason.Invalid);

        /// <summary>
        /// Constructs a client game loading process finished failed message
        /// </summary>
        /// <param name="message">Received message</param>
        /// <param name="reason">Reason</param>
        public ClientGameLoadingProcessFinishedFailedMessageData(ClientGameLoadingProcessFinishedMessageData message, EClientGameLoadingProcessFinishedFailedReason reason) : base(Naming.GetMessageTypeNameFromMessageDataType<ClientGameLoadingProcessFinishedFailedMessageData>(), message, reason)
        {
            if (reason == EClientGameLoadingProcessFinishedFailedReason.Invalid)
            {
                throw new ArgumentException("Client game loading process finished failed reason can't be invalid.", nameof(reason));
            }
        }
    }
}
