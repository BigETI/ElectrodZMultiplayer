using System;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Local peer disconnection message structure
    /// </summary>
    internal readonly struct LocalPeerDisconnectionMessage
    {
        /// <summary>
        /// Peer
        /// </summary>
        public ILocalPeer Peer { get; }

        /// <summary>
        /// Constructs a local peer disconnection message
        /// </summary>
        /// <param name="peer">Peer</param>
        public LocalPeerDisconnectionMessage(ILocalPeer peer) => Peer = peer ?? throw new ArgumentNullException(nameof(peer));
    }
}
