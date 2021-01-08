using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// An interface that represents any connector
    /// </summary>
    public interface IConnector : IDisposable
    {
        /// <summary>
        /// Connected peers
        /// </summary>
        IReadOnlyDictionary<string, IPeer> Peers { get; }
        
        /// <summary>
        /// This event will be invoked when a peer attempted to connect to this connector.
        /// </summary>
        event PeerConnectionAttemptedDelegate OnPeerConnectionAttempted;

        /// <summary>
        /// This event will be invoked when a peer has been successfully connected to this connector.
        /// </summary>
        event PeerConnectedDelegate OnPeerConnected;

        /// <summary>
        /// This event will be invoked when a peer has disconnected from this connector.
        /// </summary>
        event PeerDisconnectedDelegate OnPeerDisconnected;

        /// <summary>
        /// This event will be invoked when a peer has sent a message to this connector
        /// </summary>
        event PeerMessageReceivedDelegate OnPeerMessageReceived;

        /// <summary>
        /// Processes all events appeared since last call
        /// </summary>
        void ProcessEvents();

        /// <summary>
        /// Is connection by peer allowed
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <returns>"true" if connection is allowed, otherwise "false"</returns>
        bool IsConnectionAllowed(IPeer peer);

        /// <summary>
        /// Closes connection to all connected peers in this connector
        /// </summary>
        /// <param name="reason">Disconnection reason</param>
        void Close(EDisconnectionReason reason);
    }
}
