using ENet;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// ENet Peer connection attempt message structure
    /// </summary>
    internal readonly struct ENetPeerConnectionAttemptMessage
    {
        /// <summary>
        /// Peer
        /// </summary>
        public Peer Peer { get; }

        /// <summary>
        /// Constructs a ENet peer connection attempt message
        /// </summary>
        /// <param name="peer">Peer</param>
        public ENetPeerConnectionAttemptMessage(Peer peer) => Peer = peer;
    }
}
