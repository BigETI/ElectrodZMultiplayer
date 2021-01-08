using System;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Receive local peer message structure
    /// </summary>
    internal readonly struct LocalPeerReceiveMessage
    {
        /// <summary>
        /// Peer
        /// </summary>
        public ILocalPeer Peer { get; }

        /// <summary>
        /// Message
        /// </summary>
        public byte[] Message { get; }

        /// <summary>
        /// Starting index
        /// </summary>
        public uint Index { get; }

        /// <summary>
        /// Message length in bytes
        /// </summary>
        public uint Length { get; }

        /// <summary>
        /// Constructs a local peer recieve message
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="message">Message</param>
        /// <param name="length">Starting index</param>
        /// <param name="length">Message length in bytes</param>
        public LocalPeerReceiveMessage(ILocalPeer peer, byte[] message, uint index, uint length)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            if (index >= length)
            {
                throw new ArgumentException("Starting index is greater or equal to length.");
            }
            if ((index + length) > message.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "STarting index plus length is bigger than message length.");
            }
            Peer = peer ?? throw new ArgumentNullException(nameof(peer));
            Message = message;
            Index = index;
            Length = length;
        }
    }
}
