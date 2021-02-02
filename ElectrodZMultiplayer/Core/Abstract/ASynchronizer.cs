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
        /// This event will be invoked when an authentification has failed.
        /// </summary>
        public abstract event AuthentificationFailedDelegate OnAuthentificationFailed;

        /// <summary>
        /// This event will be invoked when lobbies have been listed.
        /// </summary>
        public abstract event LobbiesListedDelegate OnLobbiesListed;

        /// <summary>
        /// This event will be invoked when listing lobbies has failed.
        /// </summary>
        public abstract event ListLobbiesFailedDelegate OnListLobbiesFailed;

        /// <summary>
        /// This event will be invoked when available game modes have been listed.
        /// </summary>
        public abstract event AvailableGameModesListedDelegate OnAvailableGameModesListed;

        /// <summary>
        /// This event will be invoked when listing available game modes has failed.
        /// </summary>
        public abstract event ListAvailableGameModesFailedDelegate OnListAvailableGameModesFailed;

        /// <summary>
        /// This event will be invoked when an user has joined this lobby.
        /// </summary>
        public abstract event LobbyUserJoinedDelegate OnUserJoined;

        /// <summary>
        /// This event will be invoked when joining a lobby has failed.
        /// </summary>
        public abstract event JoinLobbyFailedDelegate OnJoinLobbyFailed;

        /// <summary>
        /// This event will be invoked when creating a lobby has failed.
        /// </summary>
        public abstract event CreateLobbyFailedDelegate OnCreateLobbyFailed;

        /// <summary>
        /// This event will be invoked when an user left this lobby.
        /// </summary>
        public abstract event LobbyUserLeftDelegate OnUserLeft;

        /// <summary>
        /// This event will be invoked when the username changes.
        /// </summary>
        public abstract event UserUsernameUpdatedDelegate OnUsernameUpdated;

        /// <summary>
        /// This event will be invoked when changing username has failed.
        /// </summary>
        public abstract event ChangeUsernameFailedDelegate OnChangeUsernameFailed;

        /// <summary>
        /// This event will be invoked when the user lobby color changes.
        /// </summary>
        public abstract event UserUserLobbyColorUpdatedDelegate OnUserLobbyColorUpdated;

        /// <summary>
        /// This event will be invoked when changing user lobby color has failed.
        /// </summary>
        public abstract event ChangeUserLobbyColorFailedDelegate OnChangeUserLobbyColorFailed;

        /// <summary>
        /// This event will be invoked when the lobby owner of this lobby has been changed.
        /// </summary>
        public abstract event LobbyLobbyOwnershipChangedDelegate OnLobbyOwnershipChanged;

        /// <summary>
        /// This event will be invoked when the lobby rules of this lobby has been changed.
        /// </summary>
        public abstract event LobbyLobbyRulesChangedDelegate OnLobbyRulesChanged;

        /// <summary>
        /// This event will be invoked when changing lobby rules have failed.
        /// </summary>
        public abstract event ChangeLobbyRulesFailedDelegate OnChangeLobbyRulesFailed;

        /// <summary>
        /// This event will be invoked when kicking a user has failed.
        /// </summary>
        public abstract event KickUserFailedDelegate OnKickUserFailed;

        /// <summary>
        /// This event will be invoked when a game has been started.
        /// </summary>
        public abstract event LobbyGameStartedDelegate OnGameStarted;

        /// <summary>
        /// This event will be invoked when a game start has been requested.
        /// </summary>
        public abstract event LobbyGameStartRequestedDelegate OnGameStartRequested;

        /// <summary>
        /// This event will be invoked when starting a game has failed.
        /// </summary>
        public abstract event StartGameFailedDelegate OnStartGameFailed;

        /// <summary>
        /// This event will be invoked when a game has been restarted.
        /// </summary>
        public abstract event LobbyGameRestartedDelegate OnGameRestarted;

        /// <summary>
        /// This event will be invoked when a game restart has been requested.
        /// </summary>
        public abstract event LobbyGameRestartRequestedDelegate OnGameRestartRequested;

        /// <summary>
        /// This event will be invoked when restarting a game has failed.
        /// </summary>
        public abstract event RestartGameFailedDelegate OnRestartGameFailed;

        /// <summary>
        /// This event will be invoked when a game has been stopped.
        /// </summary>
        public abstract event LobbyGameStoppedDelegate OnGameStopped;

        /// <summary>
        /// This event will be invoked when a game stop has been requested.
        /// </summary>
        public abstract event LobbyGameStopRequestedDelegate OnGameStopRequested;

        /// <summary>
        /// This event will be invoked when stopping a game has failed.
        /// </summary>
        public abstract event StopGameFailedDelegate OnStopGameFailed;

        /// <summary>
        /// This event will be invoked when starting a game has been cancelled.
        /// </summary>
        public abstract event LobbyStartGameCancelledDelegate OnStartGameCancelled;

        /// <summary>
        /// This event will be invoked when restarting a game has been cancelled.
        /// </summary>
        public abstract event LobbyRestartGameCancelledDelegate OnRestartGameCancelled;

        /// <summary>
        /// This event will be invoked when stopping a game has been cancelled.
        /// </summary>
        public abstract event LobbyStopGameCancelledDelegate OnStopGameCancelled;

        /// <summary>
        /// This event will be invoked when cancelling a game start, restart or stop timer has failed.
        /// </summary>
        public abstract event CancelStartRestartStopGameTimerFailedDelegate OnCancelStartRestartStopGameTimerFailed;

        /// <summary>
        /// This event will be invoked when the user finished loading their game.
        /// </summary>
        public abstract event UserGameLoadingFinishedDelegate OnGameLoadingFinished;

        /// <summary>
        /// This event will be invoked when a client tick has been performed.
        /// </summary>
        public abstract event UserClientTickedDelegate OnClientTicked;

        /// <summary>
        /// This event will be invoked when a client tick has failed.
        /// </summary>
        public abstract event ClientTickFailedDelegate OnClientTickFailed;

        /// <summary>
        /// This event will be invoked when a server tick has been performed.
        /// </summary>
        public abstract event UserServerTickedDelegate OnServerTicked;

        /// <summary>
        /// This event will be invoked when a server tick has failed.
        /// </summary>
        public abstract event ServerTickFailedDelegate OnServerTickFailed;

        /// <summary>
        /// Constructs a generalised synchronizer object
        /// </summary>
        public ASynchronizer() =>
            AddMessageParser<ErrorMessageData>
            (
                (_, message, json) =>
                {
                    OnErrorMessageReceived?.Invoke(message.ErrorType, message.Message);
                    Console.Error.WriteLine($"[{ message.ErrorType }] { message.Message }");
                },
                (peer, message, _) =>
                {
                    if (message.ErrorType == EErrorType.Invalid)
                    {
                        SendErrorMessageToPeer<ErrorMessageData>(peer, EErrorType.InvalidErrorType, "An error message with an invalid error type has been sent.");
                    }
                    else if (string.IsNullOrWhiteSpace(message.MessageType))
                    {
                        SendErrorMessageToPeer<ErrorMessageData>(peer, EErrorType.InvalidMessageType, "An error message with an invalid message type has been sent.");
                    }
                    else if (message.Message == null)
                    {
                        SendErrorMessageToPeer<ErrorMessageData>(peer, EErrorType.MessageIsNull, "An error message with message being null has been sent.");
                    }
                    else
                    {
                        SendUnknownErrorMessageToPeer<ErrorMessageData>(peer, "Message validation has failed.");
                    }
                },
                MessageParseFailedEvent<ErrorMessageData>
            );

        /// <summary>
        /// Listens to any message parse failed event
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="expectedMessageType">Expected message type</param>
        /// <param name="message">Message</param>
        /// <param name="json">JSON</param>
        /// <param name="isFatal">Is error fatal</param>
        protected void MessageParseFailedEvent<T>(IPeer peer, string expectedMessageType, string json, bool isFatal) where T : IBaseMessageData => SendErrorMessageToPeer<T>(peer, EErrorType.InvalidMessageParameters, $"Message is invalid. Expected message type: \"{ expectedMessageType }\"{ Environment.NewLine }{ Environment.NewLine }JSON:{ Environment.NewLine }{ json }", isFatal);

        /// <summary>
        /// Listens to any message parse failed event
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="expectedMessageType">Expected message tyoe</param>
        /// <param name="message">Message</param>
        /// <param name="json">JSON</param>
        protected void MessageParseFailedEvent<T>(IPeer peer, string expectedMessageType, string json) where T : IBaseMessageData => MessageParseFailedEvent<T>(peer, expectedMessageType, json, false);

        /// <summary>
        /// Listens to any message parse failed event that is fatal
        /// </summary>
        /// <param name="peer">peer</param>
        /// <param name="expectedMessageType">Expected message type</param>
        /// <param name="message">Message</param>
        /// <param name="json">JSON</param>
        protected void FatalMessageParseFailedEvent<T>(IPeer peer, string expectedMessageType, string json) where T : IBaseMessageData => MessageParseFailedEvent<T>(peer, expectedMessageType, json, true);

        /// <summary>
        /// Listens to any message validation event
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="peer">Peer</param>
        /// <param name="message">Received message</param>
        /// <param name="json">Message JSON</param>
        /// <param name="isFatal">Is validation fail fatal</param>
        protected void MessageValidationFailedEvent<T>(IPeer peer, T message, string json, bool isFatal) where T : IBaseMessageData => SendErrorMessageToPeer<T>(peer, EErrorType.InvalidMessageParameters, $"Message is invalid. Message type: \"{ message.GetType().FullName }\"{ Environment.NewLine }{ Environment.NewLine }JSON:{ Environment.NewLine }{ json }", isFatal);

        /// <summary>
        /// Listens to any message validation event
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="peer">Peer</param>
        /// <param name="message">Received message</param>
        /// <param name="json">Message JSON</param>
        protected void MessageValidationFailedEvent<T>(IPeer peer, T message, string json) where T : IBaseMessageData => MessageValidationFailedEvent(peer, message, json, false);

        /// <summary>
        /// Listens to any message validation event that is fatal
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="peer">Peer</param>
        /// <param name="message">Received message</param>
        /// <param name="json">Message JSON</param>
        protected void FatalMessageValidationFailedEvent<T>(IPeer peer, T message, string json) where T : IBaseMessageData => MessageValidationFailedEvent<T>(peer, message, json, true);

        /// <summary>
        /// Adds an automatic message parser
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="onMessageParsed">On message parsed</param>
        /// <param name="isFatal">Is validation fail or error fatal</param>
        /// <returns>Message parser</returns>
        protected IMessageParser<T> AddAutomaticMessageParser<T>(MessageParsedDelegate<T> onMessageParsed, bool isFatal) where T : IBaseMessageData => AddMessageParser<T>(onMessageParsed, isFatal ? (MessageValidationFailedDelegate<T>)FatalMessageValidationFailedEvent : MessageValidationFailedEvent, isFatal ? (MessageParseFailedDelegate)FatalMessageParseFailedEvent<T> : MessageParseFailedEvent<T>);

        /// <summary>
        /// Adds an automatic message parser
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="onMessageParsed">On message parsed</param>
        /// <returns>Message parser</returns>
        protected IMessageParser<T> AddAutomaticMessageParser<T>(MessageParsedDelegate<T> onMessageParsed) where T : IBaseMessageData => AddAutomaticMessageParser(onMessageParsed, false);

        /// <summary>
        /// Adds an automatic message parser that is fatal on validation fail or error
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="onMessageParsed">On message parsed</param>
        /// <returns>Message parser</returns>
        protected IMessageParser<T> AddAutomaticMessageParserWithFatality<T>(MessageParsedDelegate<T> onMessageParsed) where T : IBaseMessageData => AddAutomaticMessageParser(onMessageParsed, false);

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
        /// Adds a message parser
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="onMessageParsed">On message parsed</param>
        /// <param name="onMessageValidationFailed">On message validation failed</param>
        /// <param name="onMessageParseFailed">On message parse failed</param>
        /// <returns>Message parser</returns>
        public IMessageParser<T> AddMessageParser<T>(MessageParsedDelegate<T> onMessageParsed, MessageValidationFailedDelegate<T> onMessageValidationFailed = null, MessageParseFailedDelegate onMessageParseFailed = null) where T : IBaseMessageData
        {
            IMessageParser<T> ret = new MessageParser<T>(onMessageParsed, onMessageValidationFailed, onMessageParseFailed);
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
        /// Gets message parsers for the specified type
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <returns>Message parsers if successful, otherwise "null"</returns>
        public IEnumerable<IMessageParser<T>> GetMessageParsersForType<T>() where T : IBaseMessageData => TryGetMessageParsersForType<T>(out IEnumerable<IMessageParser<T>> ret) ? ret : null;

        /// <summary>
        /// Tries to get message parsers for the specified type
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="messageParsers">Message parsers</param>
        /// <returns>"true" if message parsers are available, otherwise "false"</returns>
        public bool TryGetMessageParsersForType<T>(out IEnumerable<IMessageParser<T>> messageParsers) where T : IBaseMessageData
        {
            string key = Naming.GetMessageTypeNameFromMessageDataType<T>();
            bool ret = this.messageParsers.ContainsKey(key);
            if (ret)
            {
                List<IMessageParser<T>> message_parsers = new List<IMessageParser<T>>();
                foreach (IBaseMessageParser base_message_parser in this.messageParsers[key])
                {
                    if (base_message_parser is IMessageParser<T> message_parser)
                    {
                        message_parsers.Add(message_parser);
                    }
                }
                messageParsers = message_parsers;
            }
            else
            {
                messageParsers = null;
            }
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
            if ((base_network_message_data != null) && base_network_message_data.IsValid)
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
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="peer">Peer</param>
        /// <param name="errorType">Error type</param>
        /// <param name="errorMessage">Error message</param>
        public void SendErrorMessageToPeer<T>(IPeer peer, EErrorType errorType, string errorMessage) where T : IBaseMessageData => SendErrorMessageToPeer<T>(peer, errorType, errorMessage, false);

        /// <summary>
        /// Sends an error message to peer
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="peer">Peer</param>
        /// <param name="errorType">Error tyoe</param>
        /// <param name="errorMessage">Error message</param>
        /// <param name="isFatal">Is error fatal</param>
        public void SendErrorMessageToPeer<T>(IPeer peer, EErrorType errorType, string errorMessage, bool isFatal) where T : IBaseMessageData
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
            SendMessageToPeer(peer, new ErrorMessageData(errorType, Naming.GetMessageTypeNameFromMessageDataType<T>(), errorMessage));
            if (isFatal)
            {
                peer.Disconnect(EDisconnectionReason.Error);
            }
        }

        /// <summary>
        /// Sends an invalid message parameters error message to peer
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="peer">Peer</param>
        /// <param name="errorMessage">Error message</param>
        public void SendInvalidMessageParametersErrorMessageToPeer<T>(IPeer peer, string errorMessage) where T : IBaseMessageData => SendErrorMessageToPeer<T>(peer, EErrorType.InvalidMessageParameters, errorMessage);

        /// <summary>
        /// Sends an invalid message context error message to peer
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="peer">Peer</param>
        /// <param name="errorMessage">Error message</param>
        public void SendInvalidMessageContextErrorMessageToPeer<T>(IPeer peer, string errorMessage) where T : IBaseMessageData => SendErrorMessageToPeer<T>(peer, EErrorType.InvalidMessageContext, errorMessage);

        /// <summary>
        /// Sends an unknown error message to peer
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="peer">Peer</param>
        /// <param name="errorMessage">Error message</param>
        public void SendUnknownErrorMessageToPeer<T>(IPeer peer, string errorMessage) where T : IBaseMessageData => SendErrorMessageToPeer<T>(peer, EErrorType.Unknown, errorMessage);

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
