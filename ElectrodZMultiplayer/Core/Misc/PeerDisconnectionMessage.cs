using ENet;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Peer disconnection message structure
    /// </summary>
    internal readonly struct PeerDisconnectionMessage
    {
        /// <summary>
        /// Peer
        /// </summary>
        public Peer Peer { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="peer">Peer</param>
        public PeerDisconnectionMessage(Peer peer) => Peer = peer;
    }
}
