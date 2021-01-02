using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes the base of any sent or received data
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class BaseMessageData : IBaseMessageData
    {
        /// <summary>
        /// Message type
        /// </summary>
        [JsonProperty("type")]
        public string MessageType { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public virtual bool IsValid => MessageType != null;

        /// <summary>
        /// Constructs a base message data
        /// </summary>
        public BaseMessageData()
        {
            // ...
        }

        /// <summary>
        /// Constructs a base message data with the specified message type
        /// </summary>
        /// <param name="messageType">Message type</param>
        public BaseMessageData(string messageType) => MessageType = messageType ?? throw new ArgumentException(nameof(messageType));
    }
}
