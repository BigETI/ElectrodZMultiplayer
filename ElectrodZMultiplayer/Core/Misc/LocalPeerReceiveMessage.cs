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
        /// Message length in bytes
        /// </summary>
        public uint Length { get; }

        /// <summary>
        /// Constructs a local peer recieve message
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="message">Message</param>
        /// <param name="length">Message length in bytes</param>
        public LocalPeerReceiveMessage(ILocalPeer peer, byte[] message, uint length)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            if (length > message.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Message length is bigger than message byte array.");
            }
            Peer = peer ?? throw new ArgumentNullException(nameof(peer));
            Message = message;
            Length = length;
        }
    }
}
