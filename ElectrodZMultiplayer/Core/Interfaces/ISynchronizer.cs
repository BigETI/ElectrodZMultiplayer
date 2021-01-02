﻿using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// An interface that describes a generalized synchronizer
    /// </summary>
    public interface ISynchronizer : IDisposable
    {
        /// <summary>
        /// Available connectors
        /// </summary>
        IEnumerable<IConnector> Connectors { get; }

        /// <summary>
        /// This event will be invoked when a peer has attempted to connect to any of the available connectors.
        /// </summary>
        event PeerConnectionAttemptedDelegate OnPeerConnectionAttempted;

        /// <summary>
        /// This event will be invoked when a peer has successfully connected to any of the available connectors.
        /// </summary>
        event PeerConnectedDelegate OnPeerConnected;

        /// <summary>
        /// This event will be invoked when a peer has been disconnected from any of the available connectors.
        /// </summary>
        event PeerDisconnectedDelegate OnPeerDisconnected;

        /// <summary>
        /// This event will be invoked when a peer has been timed out from any of the available connectors.
        /// </summary>
        event PeerTimedOutDelegate OnPeerTimedOut;

        /// <summary>
        /// This event will be invoked when a message has been received from a peer.
        /// </summary>
        event PeerMessageReceivedDelegate OnPeerMessageReceived;

        /// <summary>
        /// This event will be invoked when a non-meaningful message has been received from a peer.
        /// </summary>
        event UnknownMessageReceivedDelegate OnUnknownMessageReceived;

        /// <summary>
        /// Add connector
        /// </summary>
        /// <param name="connector">Connector</param>
        /// <returns>"true" if connector was successfully added, otherwise "false"</returns>
        bool AddConnector(IConnector connector);

        /// <summary>
        /// Remove connector
        /// </summary>
        /// <param name="connector">Connector</param>
        /// <returns>"true" if connector was successfully removed, otherwise "false"</returns>
        bool RemoveConnector(IConnector connector);

        /// <summary>
        /// Send message to peer
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="peer">Peer</param>
        /// <param name="message">Message</param>
        void SendMessageToPeer<T>(IPeer peer, T message) where T : IBaseMessageData;

        /// <summary>
        /// Add message parser
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="onMessageParsed">On message parsed</param>
        /// <param name="onMessageParseFailed">On message parse failed</param>
        /// <returns>Message parser</returns>
        IMessageParser<T> AddMessageParser<T>(MessageParsedDelegate<T> onMessageParsed, MessageParseFailedDelegate<T> onMessageParseFailed = null) where T : IBaseMessageData;

        /// <summary>
        /// Removes the specified message parser
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="messageParser">Message parser</param>
        /// <returns>"true" if message parser was successfully removed, otherwise "false"</returns>
        bool RemoveMessageParser<T>(IMessageParser<T> messageParser) where T : IBaseMessageData;

        /// <summary>
        /// Parses incoming message
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="json">JSON</param>
        void ParseMessage(IPeer peer, string json);

        /// <summary>
        /// Closes connections to all peers
        /// </summary>
        /// <param name="reason">Disconnection reason</param>
        void Close(EDisconnectionReason reason);
    }
}
