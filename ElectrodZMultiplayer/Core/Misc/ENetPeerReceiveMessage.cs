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
        /// Message
        /// </summary>
        public byte[] Message { get; }

        /// <summary>
        /// Constructs a ENet peer receive message
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="channelID">Channel ID</param>
        /// <param name="message">Message</param>
        public ENetPeerReceiveMessage(Peer peer, uint channelID, byte[] message)
        {
            Peer = peer;
            ChannelID = channelID;
            Message = message;
        }
    }
}
