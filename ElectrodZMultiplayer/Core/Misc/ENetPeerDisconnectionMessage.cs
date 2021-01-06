using ENet;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// ENet peer disconnection message structure
    /// </summary>
    internal readonly struct ENetPeerDisconnectionMessage
    {
        /// <summary>
        /// Peer
        /// </summary>
        public Peer Peer { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="peer">Peer</param>
        public ENetPeerDisconnectionMessage(Peer peer) => Peer = peer;
    }
}
