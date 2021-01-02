using ENet;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Peer time out message structure
    /// </summary>
    internal readonly struct PeerTimeOutMessage
    {
        /// <summary>
        /// Peer
        /// </summary>
        public Peer Peer { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="peer">Peer</param>
        public PeerTimeOutMessage(Peer peer) => Peer = peer;
    }
}
