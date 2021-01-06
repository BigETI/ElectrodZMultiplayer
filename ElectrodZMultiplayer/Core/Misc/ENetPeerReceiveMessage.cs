using ENet;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// ENet peer receive message structure
    /// </summary>
    internal readonly struct ENetPeerReceiveMessage
    {
        /// <summary>
        /// Peer
        /// </summary>
        public Peer Peer { get; }

        /// <summary>
        /// Channel ID
        /// </summary>
        public uint ChannelID { get; }

        /// <summary>
        /// Packet
        /// </summary>
        public Packet Packet { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="channelID">Channel ID</param>
        /// <param name="packet">PAcket</param>
        public ENetPeerReceiveMessage(Peer peer, uint channelID, Packet packet)
        {
            Peer = peer;
            ChannelID = channelID;
            Packet = packet;
        }
    }
}
