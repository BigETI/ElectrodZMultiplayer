using ENet;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// ENet peer send message structure
    /// </summary>
    internal readonly struct ENetPeerSendMessage
    {
        /// <summary>
        /// Peer
        /// </summary>
        public Peer Peer { get; }

        /// <summary>
        /// Message
        /// </summary>
        public byte[] Message { get; }

        /// <summary>
        /// Index
        /// </summary>
        public uint Index { get; }

        /// <summary>
        /// Length
        /// </summary>
        public uint Length { get; }

        /// <summary>
        /// Constructs a ENet peer send message
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="message">Message</param>
        /// <param name="index">Index</param>
        /// <param name="length">Length</param>
        public ENetPeerSendMessage(Peer peer, byte[] message, uint index, uint length)
        {
            Peer = peer;
            Message = message;
            Index = index;
            Length = length;
        }
    }
}
