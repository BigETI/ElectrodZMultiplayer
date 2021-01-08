﻿using ENet;
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
        /// Peer
        /// </summary>
        public Peer Peer { get; }

        /// <summary>
        /// Host
        /// </summary>
        public Host Host { get; }

        /// <summary>
        /// Is valid
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            Peer.IsSet &&
            (Host != null) &&
            Host.IsSet;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="guid">Peer GUID</param>
        /// <param name="peer">Peer</param>
        /// <param name="host">Host</param>
        public ENetPeer(Guid guid, Peer peer, Host host) : base(guid, peer.IP)
        {
            if (!peer.IsSet)
            {
                throw new ArgumentException("Peer is not set.", nameof(peer));
            }
            if (host == null)
            {
                throw new ArgumentNullException(nameof(host));
            }
            if (!host.IsSet)
            {
                throw new ArgumentException("Host is not set.", nameof(host));
            }
            Peer = peer;
            Host = host;
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
                Packet packet = default;
                packet.Create(message, (int)index, (int)length, PacketFlags.Reliable);
                Peer.Send(0, ref packet);
                Host.Flush();
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
