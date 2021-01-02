using ENet;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Peer connection attempt message structure
    /// </summary>
    internal struct PeerConnectionAttemptMessage
    {
        /// <summary>
        /// Peer
        /// </summary>
        public Peer Peer { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="peer">Peer</param>
        public PeerConnectionAttemptMessage(Peer peer) => Peer = peer;
    }
}
