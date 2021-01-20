using ElectrodZMultiplayer.Data.Messages;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// An abstract class that describes any synchronizer
    /// </summary>
    public abstract class ASynchronizer : ISynchronizer
    {
        /// <summary>
        /// Available connectors
        /// </summary>
        private readonly List<IConnector> connectors = new List<IConnector>();

        /// <summary>
        /// Available message parsers
        /// </summary>
        private readonly Dictionary<string, List<IBaseMessageParser>> messageParsers = new Dictionary<string, List<IBaseMessageParser>>();

        /// <summary>
        /// Available connectors
        /// </summary>
        public IEnumerable<IConnector> Connectors => connectors;

        /// <summary>
        /// This event will be invoked when a peer has attempted to connect to any of the available connectors.
        /// </summary>
        public event PeerConnectionAttemptedDelegate OnPeerConnectionAttempted;

        /// <summary>
        /// This event will be invoked when a peer has successfully connected to any of the available connectors.
        /// </summary>
        public event PeerConnectedDelegate OnPeerConnected;

        /// <summary>
        /// This event will be invoked when a peer has been disconnected from any of the available connectors.
        /// </summary>
        public event PeerDisconnectedDelegate OnPeerDisconnected;

        /// <summary>
        /// This event will be invoked when a message has been received from a peer.
        /// </summary>
        public event PeerMessageReceivedDelegate OnPeerMessageReceived;

        /// <summary>
        /// This event will be invoked when a non-meaningful message has been received from a peer.
        /// </summary>
        public event UnknownMessageReceivedDelegate OnUnknownMessageReceived;

        /// <summary>
        /// This event will be invoked when an error has been received.
        /// </summary>
        public event ErrorMessageReceivedDelegate OnErrorMessageReceived;

        /// <summary>
        /// This event will be invoked when an authentification was acknowledged.
        /// </summary>
        public abstract event AuthentificationAcknowledgedDelegate OnAuthentificationAcknowledged;

        /// <summary>
        /// This event will be invoked when lobbies have been listed.
        /// </summary>
        public abstract event LobbiesListedDelegate OnLobbiesListed;

        /// <summary>
        /// This event will be invoked when available game modes have been listed.
        /// </summary>
        public abstract event AvailableGameModesListedDelegate OnAvailableGameModesListed;

        /// <summary>
        /// Constructs a generalised synchronizer object
        /// </summary>
        public ASynchronizer() => AddMessageParser<ErrorMessageData>((_, message, json) =>
        {
            OnErrorMessageReceived?.Invoke(message.ErrorType, message.Message);
            Console.Error.WriteLine($"[{ message.ErrorType }] { message.Message }");
        }, MessageParseFailedEvent);

        /// <summary>
        /// Listens to any message parse failed event
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="expectedMessageType">Expected message type</param>
        /// <param name="message">Message</param>
        /// <param name="json">JSON</param>
        /// <param name="isFatal">Is error fatal</param>
        protected void MessageParseFailedEvent<T>(IPeer peer, string expectedMessageType, T message, string json, bool isFatal) where T : IBaseMessageData
        {
            if (message == null)
            {
                SendErrorMessageToPeer(peer, EErrorType.MalformedMessage, $"Received message is null of expected message type \"{ expectedMessageType }\".{ Environment.NewLine }{ Environment.NewLine }JSON:{ Environment.NewLine }{ json }", isFatal);
            }
            else
            {
                SendErrorMessageToPeer(peer, EErrorType.InvalidMessageParameters, $"\"Message is invalid. Expected message type: \"{ expectedMessageType }\"; Current message type: { message.MessageType }{ Environment.NewLine }{ Environment.NewLine }JSON:{ Environment.NewLine }{ json }", isFatal);
            }
        }

        /// <summary>
        /// Listens to any message parse failed event
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="expectedMessageType">Expected message tyoe</param>
        /// <param name="message">Message</param>
        /// <param name="json">JSON</param>
        protected void MessageParseFailedEvent<T>(IPeer peer, string expectedMessageType, T message, string json) where T : IBaseMessageData => MessageParseFailedEvent(peer, expectedMessageType, message, json, false);

        /// <summary>
        /// Listens to any message parse failed event
        /// </summary>
        /// <param name="peer">peer</param>
        /// <param name="expectedMessageType">Expected message type</param>
        /// <param name="message">Message</param>
        /// <param name="json">JSON</param>
        protected void FatalMessageParseFailedEvent<T>(IPeer peer, string expectedMessageType, T message, string json) where T : IBaseMessageData => MessageParseFailedEvent(peer, expectedMessageType, message, json, true);

        /// <summary>
        /// Add connector
        /// </summary>
        /// <param name="connector">Connector</param>
        /// <returns>"true" if connector was successfully added, otherwise "false"</returns>
        public bool AddConnector(IConnector connector)
        {
            if (connector == null)
            {
                throw new ArgumentNullException(nameof(connector));
            }
            bool ret = !connectors.Contains(connector);
            if (ret)
            {
                connectors.Add(connector);
                connector.OnPeerConnectionAttempted += (peer) => OnPeerConnectionAttempted?.Invoke(peer);
                connector.OnPeerConnected += (peer) => OnPeerConnected?.Invoke(peer);
                connector.OnPeerDisconnected += (peer) => OnPeerDisconnected?.Invoke(peer);
                connector.OnPeerMessageReceived += (peer, message) =>
                {
                    OnPeerMessageReceived?.Invoke(peer, message);
                    ParseMessage(peer, Encoding.UTF8.GetString(message));
                };
            }
            return ret;
        }

        /// <summary>
        /// Remove connector
        /// </summary>
        /// <param name="connector">Connector</param>
        /// <returns>"true" if connector was successfully removed, otherwise "false"</returns>
        public bool RemoveConnector(IConnector connector)
        {
            if (connector == null)
            {
                throw new ArgumentNullException(nameof(connector));
            }
            return connectors.Remove(connector);
        }

        /// <summary>
        /// Gets a connector with the specified type
        /// </summary>
        /// <typeparam name="T">Connector type</typeparam>
        /// <returns>Connector of specified type if successful, otherwise "null"</returns>
        public T GetConnectorOfType<T>() where T : IConnector
        {
            TryGetConnectorOfType(out T ret);
            return ret;
        }

        /// <summary>
        /// Tries to get a connector of the specified type
        /// </summary>
        /// <typeparam name="T">Connector type</typeparam>
        /// <param name="connector">Connector</param>
        /// <returns>"true" if connector of the specified type is available, otherwise "false"</returns>
        public bool TryGetConnectorOfType<T>(out T connector) where T : IConnector
        {
            bool ret = false;
            connector = default;
            foreach (IConnector available_connector in connectors)
            {
                if (available_connector is T result)
                {
                    connector = result;
                    ret = true;
                    break;
                }
            }
            return ret;
        }

        /// <summary>
        /// Sends a message to peer
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="peer">Peer</param>
        /// <param name="message">Message</param>
        public void SendMessageToPeer<T>(IPeer peer, T message) where T : IBaseMessageData
        {
            if (peer == null)
            {
                throw new ArgumentNullException(nameof(peer));
            }
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            string json = JsonConvert.SerializeObject(message);
            if (!string.IsNullOrWhiteSpace(json))
            {
                peer.SendMessage(Compression.Compress(Encoding.UTF8.GetBytes(json)));
            }
        }

        /// <summary>
        /// Add message parser
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="onMessageParsed">On message parsed</param>
        /// <param name="onMessageParseFailed">On message parse failed</param>
        /// <returns>Message parser</returns>
        public IMessageParser<T> AddMessageParser<T>(MessageParsedDelegate<T> onMessageParsed, MessageParseFailedDelegate<T> onMessageParseFailed = null) where T : IBaseMessageData
        {
            IMessageParser<T> ret = new MessageParser<T>(onMessageParsed, onMessageParseFailed);
            List<IBaseMessageParser> message_parsers;
            if (messageParsers.ContainsKey(ret.MessageType))
            {
                message_parsers = messageParsers[ret.MessageType];
            }
            else
            {
                message_parsers = new List<IBaseMessageParser>();
                messageParsers.Add(ret.MessageType, message_parsers);
            }
            message_parsers.Add(ret);
            return ret;
        }

        /// <summary>
        /// Removes the specified message parser
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="messageParser">Message parser</param>
        /// <returns>"true" if message parser was successfully removed, otherwise "false"</returns>
        public bool RemoveMessageParser<T>(IMessageParser<T> messageParser) where T : IBaseMessageData
        {
            if (messageParser == null)
            {
                throw new ArgumentNullException(nameof(messageParser));
            }
            bool ret = false;
            if (messageParsers.ContainsKey(messageParser.MessageType))
            {
                List<IBaseMessageParser> message_parsers = messageParsers[messageParser.MessageType];
                ret = message_parsers.Remove(messageParser);
                if (ret && (message_parsers.Count <= 0))
                {
                    messageParsers.Remove(messageParser.MessageType);
                }
            }
            return ret;
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
            BaseMessageData base_network_message_data = JsonConvert.DeserializeObject<BaseMessageData>(json);
            if (base_network_message_data != null)
            {
                if (messageParsers.ContainsKey(base_network_message_data.MessageType))
                {
                    foreach (IBaseMessageParser message_parser in messageParsers[base_network_message_data.MessageType])
                    {
                        message_parser.ParseMessage(peer, json);
                    }
                }
                else
                {
                    OnUnknownMessageReceived?.Invoke(base_network_message_data, json);
                }
            }
        }

        /// <summary>
        /// Sends an error message to peer
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="errorType">Error type</param>
        /// <param name="errorMessage">Error message</param>
        public void SendErrorMessageToPeer(IPeer peer, EErrorType errorType, string errorMessage) => SendErrorMessageToPeer(peer, errorType, errorMessage, false);

        /// <summary>
        /// Sends an error message to peer
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="errorType">Error tyoe</param>
        /// <param name="errorMessage">Error message</param>
        /// <param name="isFatal">Is error fatal</param>
        public void SendErrorMessageToPeer(IPeer peer, EErrorType errorType, string errorMessage, bool isFatal)
        {
            if (peer == null)
            {
                throw new ArgumentNullException(nameof(peer));
            }
            if (!peer.IsValid)
            {
                throw new ArgumentException("Peer is not valid.", nameof(peer));
            }
            if (errorMessage == null)
            {
                throw new ArgumentNullException(nameof(errorMessage));
            }
            Console.Error.WriteLine($"[{ errorType }] { errorMessage }");
            SendMessageToPeer(peer, new ErrorMessageData(errorType, errorMessage));
            if (isFatal)
            {
                peer.Disconnect(EDisconnectionReason.Error);
            }
        }

        /// <summary>
        /// Closes connections to all peers
        /// </summary>
        /// <param name="reason">Disconnection reason</param>
        public virtual void Close(EDisconnectionReason reason)
        {
            foreach (IConnector connector in connectors)
            {
                connector.Close(reason);
            }
            connectors.Clear();
        }

        /// <summary>
        /// Dispose object
        /// </summary>
        public void Dispose() => Close(EDisconnectionReason.Disposed);
    }
}
