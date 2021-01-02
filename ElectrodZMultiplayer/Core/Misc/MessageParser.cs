using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Message parser class
    /// </summary>
    /// <typeparam name="T">Message type</typeparam>
    internal class MessageParser<T> : IMessageParser<T> where T : IBaseMessageData
    {
        /// <summary>
        /// Message type
        /// </summary>
        public string MessageType { get; }

        /// <summary>
        /// On message parsed
        /// </summary>
        public event MessageParsedDelegate<T> OnMessageParsed;

        /// <summary>
        /// On message parse failed
        /// </summary>
        public event MessageParseFailedDelegate<T> OnMessageParseFailed;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="onMessageParsed">On message parsed</param>
        /// <param name="onMessageParseFailed">On message parse failed</param>
        public MessageParser(MessageParsedDelegate<T> onMessageParsed, MessageParseFailedDelegate<T> onMessageParseFailed)
        {
            MessageType = Naming.GetMessageTypeNameFromMessageDataType<T>();
            OnMessageParsed += onMessageParsed ?? throw new ArgumentNullException(nameof(onMessageParsed));
            if (onMessageParseFailed != null)
            {
                OnMessageParseFailed += onMessageParseFailed;
            }
        }

        /// <summary>
        /// Parses incoming message
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="json">JSON</param>
        public void ParseMessage(IPeer peer, string json)
        {
            if (peer == null)
            {
                throw new ArgumentNullException(nameof(peer));
            }
            if (!peer.IsValid)
            {
                throw new ArgumentException("Peer is not valid.", nameof(peer));
            }
            if (json == null)
            {
                throw new ArgumentNullException(nameof(json));
            }
            T message = JsonConvert.DeserializeObject<T>(json);
            if ((message == null) || !message.IsValid || (message.MessageType != MessageType))
            {
                OnMessageParseFailed?.Invoke(peer, MessageType, message, json);
            }
            else
            {
                OnMessageParsed?.Invoke(peer, message, json);
            }
        }
    }
}
