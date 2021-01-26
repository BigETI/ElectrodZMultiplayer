using ENet;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// ENet peer time out message structure
    /// </summary>
    internal readonly struct ENetPeerTimeOutMessage
    {
        /// <summary>
        /// Peer
        /// </summary>
        public Peer Peer { get; }

        /// <summary>
        /// Constructs a ENet peer time out message
        /// </summary>
        /// <param name="peer">Peer</param>
        public ENetPeerTimeOutMessage(Peer peer) => Peer = peer;
    }
}
