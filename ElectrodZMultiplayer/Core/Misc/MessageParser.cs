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
        /// On message validation failed
        /// </summary>
        public event MessageValidationFailedDelegate<T> OnMessageValidationFailed;

        /// <summary>
        /// On message parse failed
        /// </summary>
        public event MessageParseFailedDelegate OnMessageParseFailed;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="onMessageParsed">On message parsed</param>
        /// <param name="onMessageValidationFailed">On message validation failed</param>
        /// <param name="onMessageParseFailed">On message parse failed</param>
        public MessageParser(MessageParsedDelegate<T> onMessageParsed, MessageValidationFailedDelegate<T> onMessageValidationFailed, MessageParseFailedDelegate onMessageParseFailed)
        {
            MessageType = Naming.GetMessageTypeNameFromMessageDataType<T>();
            OnMessageParsed += onMessageParsed ?? throw new ArgumentNullException(nameof(onMessageParsed));
            if (onMessageValidationFailed != null)
            {
                OnMessageValidationFailed += onMessageValidationFailed;
            }
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
            if ((message == null) || (message.MessageType != MessageType))
            {
                OnMessageParseFailed?.Invoke(peer, MessageType, json);
            }
            if (message.IsValid)
            {
                OnMessageParsed?.Invoke(peer, message, json);
            }
            else
            {
                OnMessageValidationFailed?.Invoke(peer, message, json);
            }
        }
    }
}
