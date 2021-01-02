using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// An abstract class for any connector
    /// </summary>
    internal abstract class AConnector : IConnector
    {
        /// <summary>
        /// Connected peers
        /// </summary>
        protected readonly Dictionary<string, IPeer> peers = new Dictionary<string, IPeer>();

        /// <summary>
        /// Connected peers
        /// </summary>
        public IReadOnlyDictionary<string, IPeer> Peers => peers;

        /// <summary>
        /// This event will be invoked when a peer attempted to connect to this connector.
        /// </summary>
        public abstract event PeerConnectionAttemptedDelegate OnPeerConnectionAttempted;

        /// <summary>
        /// This event will be invoked when a peer has been successfully connected to this connector.
        /// </summary>
        public abstract event PeerConnectedDelegate OnPeerConnected;

        /// <summary>
        /// This event will be invoked when a peer has disconnected from this connector.
        /// </summary>
        public abstract event PeerDisconnectedDelegate OnPeerDisconnected;

        /// <summary>
        /// This event will be invoked when a peer has timed out.
        /// </summary>
        public abstract event PeerTimedOutDelegate OnPeerTimedOut;

        /// <summary>
        /// This event will be invoked when a peer has sent a message to this connector
        /// </summary>
        public abstract event PeerMessageReceivedDelegate OnPeerMessageReceived;

        /// <summary>
        /// Processes all events appeared since last call
        /// </summary>
        public abstract void ProcessEvents();

        /// <summary>
        /// Closes connection to all connected peers in this connector
        /// </summary>
        /// <param name="reason">Disconnection reason</param>
        public virtual void Close(EDisconnectionReason reason)
        {
            foreach (IPeer peer in peers.Values)
            {
                try
                {
                    peer.Disconnect(reason);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                }
            }
            peers.Clear();
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() => Close(EDisconnectionReason.Disposed);
    }
}
