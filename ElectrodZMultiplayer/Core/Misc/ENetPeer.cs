using ENet;
using System;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// ENet peer class
    /// </summary>
    internal class ENetPeer : APeer, IENetPeer
    {
        /// <summary>
        /// Connector
        /// </summary>
        private readonly IInternalENetConnector connector;

        /// <summary>
        /// Peer
        /// </summary>
        public Peer Peer { get; }

        /// <summary>
        /// Connector
        /// </summary>
        public IENetConnector Connector => connector;

        /// <summary>
        /// Is valid
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            Peer.IsSet &&
            (connector != null);

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="guid">Peer GUID</param>
        /// <param name="peer">Peer</param>
        /// <param name="connector">Connector</param>
        public ENetPeer(Guid guid, Peer peer, IInternalENetConnector connector) : base(guid, peer.IP)
        {
            if (!peer.IsSet)
            {
                throw new ArgumentException("Peer is not set.", nameof(peer));
            }
            Peer = peer;
            this.connector = connector ?? throw new ArgumentNullException(nameof(connector));
        }

        /// <summary>
        /// Send message
        /// </summary>
        /// <param name="message">Message</param>
        public override void SendMessage(byte[] message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            SendMessage(message, 0U, (uint)message.Length);
        }

        /// <summary>
        /// Send message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="length">Message</param>
        public override void SendMessage(byte[] message, uint index, uint length)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            if (index >= message.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Starting index is out of range.");
            }
            if ((index + length) > message.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Starting index plus message length is bigger than message byte array.");
            }
            if (length > 0U)
            {
                connector.SendMessageToPeerInternally(Peer, message, index, length);
            }
        }

        /// <summary>
        /// Disconnects this peer
        /// </summary>
        /// <param name="reason">Disconnection reason</param>
        public override void Disconnect(EDisconnectionReason reason)
        {
            try
            {
                Peer.Disconnect((uint)reason);
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public override void Dispose() => Disconnect(EDisconnectionReason.Disposed);
    }
}
