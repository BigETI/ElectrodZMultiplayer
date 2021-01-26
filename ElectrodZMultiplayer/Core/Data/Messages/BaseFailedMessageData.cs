using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a base failed message
    /// </summary>
    /// <typeparam name="TMessage">Message type</typeparam>
    /// <typeparam name="TReason">Reason type</typeparam>
    [JsonObject(MemberSerialization.OptIn)]
    public class BaseFailedMessageData<TMessage, TReason> : BaseMessageData, IBaseFailedMessageData<TMessage, TReason> where TMessage : IBaseMessageData where TReason : struct
    {
        /// <summary>
        /// Received message
        /// </summary>
        [JsonProperty("message")]
        public TMessage Message { get; set; }

        /// <summary>
        /// Reason
        /// </summary>
        [JsonProperty("reason")]
        public TReason Reason { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (Message != null);

        /// <summary>
        /// Constructs a base failed message for deserializers
        /// </summary>
        public BaseFailedMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a base failed message
        /// </summary>
        /// <param name="messageType">Message type</param>
        /// <param name="message">Received message</param>
        public BaseFailedMessageData(string messageType, TMessage message, TReason reason) : base(messageType)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            Message = message;
            Reason = reason;
        }
    }
}
