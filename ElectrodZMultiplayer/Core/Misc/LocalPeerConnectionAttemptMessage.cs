using System;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Local peer connection attempt message structure
    /// </summary>
    internal readonly struct LocalPeerConnectionAttemptMessage
    {
        /// <summary>
        /// Peer
        /// </summary>
        public ILocalPeer Peer { get; }

        /// <summary>
        /// Constructs a local peer connection attempt message
        /// </summary>
        /// <param name="peer">Peer</param>
        public LocalPeerConnectionAttemptMessage(ILocalPeer peer) => Peer = peer ?? throw new ArgumentNullException(nameof(peer));
    }
}
