using ENet;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// ENet connector class
    /// </summary>
    internal class ENetConnector : AConnector, IENetConnector
    {
        /// <summary>
        /// Peer connection attempt messages
        /// </summary>
        private readonly ConcurrentBag<ENetPeerConnectionAttemptMessage> peerConnectionAttemptMessages = new ConcurrentBag<ENetPeerConnectionAttemptMessage>();

        /// <summary>
        /// Peer disconnection messages
        /// </summary>
        private readonly ConcurrentBag<ENetPeerDisconnectionMessage> peerDisconnectionMessages = new ConcurrentBag<ENetPeerDisconnectionMessage>();

        /// <summary>
        /// Peer time out messages
        /// </summary>
        private readonly ConcurrentBag<ENetPeerTimeOutMessage> peerTimeOutMessages = new ConcurrentBag<ENetPeerTimeOutMessage>();

        /// <summary>
        /// Peer receive messages
        /// </summary>
        private readonly ConcurrentBag<ENetPeerReceiveMessage> peerReceiveMessages = new ConcurrentBag<ENetPeerReceiveMessage>();

        /// <summary>
        /// Peer ID to peer lookup
        /// </summary>
        private readonly Dictionary<uint, IPeer> peerIDToPeerLookup = new Dictionary<uint, IPeer>();

        /// <summary>
        /// Connector thread
        /// </summary>
        private readonly Thread connectorThread;

        /// <summary>
        /// Is connector thread running
        /// </summary>
        private bool isConnectorThreadRunning = true;

        /// <summary>
        /// Buffer
        /// </summary>
        private byte[] buffer = new byte[2048];

        /// <summary>
        /// Host
        /// </summary>
        public Host Host { get; }

        /// <summary>
        /// Port
        /// </summary>
        public ushort Port { get; }

        /// <summary>
        /// Timeout time in seconds
        /// </summary>
        public uint TimeoutTime { get; }

        /// <summary>
        /// On peer connection attempted
        /// </summary>
        public override event PeerConnectionAttemptedDelegate OnPeerConnectionAttempted;

        /// <summary>
        /// On peer connected
        /// </summary>
        public override event PeerConnectedDelegate OnPeerConnected;

        /// <summary>
        /// On peer disconnected
        /// </summary>
        public override event PeerDisconnectedDelegate OnPeerDisconnected;

        /// <summary>
        /// On peer message received
        /// </summary>
        public override event PeerMessageReceivedDelegate OnPeerMessageReceived;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Host">Host</param>
        /// <param name="port">Port</param>
        /// <param name="timeoutTime">Timeout time in seconds</param>
        /// <param name="onHandlePeerConnectionAttempt">Handles peer connection attempts</param>
        public ENetConnector(Host host, ushort port, uint timeoutTime, HandlePeerConnectionAttemptDelegate onHandlePeerConnectionAttempt) : base(onHandlePeerConnectionAttempt)
        {
            Host = host ?? throw new ArgumentNullException(nameof(host));
            Port = port;
            TimeoutTime = timeoutTime;
            connectorThread = new Thread(() =>
            {
                while (isConnectorThreadRunning)
                {
                    if (Host.CheckEvents(out Event network_event) <= 0)
                    {
                        if (Host.Service((int)TimeoutTime, out network_event) <= 0)
                        {
                            continue;
                        }
                    }
                    switch (network_event.Type)
                    {
                        case EventType.Connect:
                            peerConnectionAttemptMessages.Add(new ENetPeerConnectionAttemptMessage(network_event.Peer));
                            break;
                        case EventType.Disconnect:
                            peerDisconnectionMessages.Add(new ENetPeerDisconnectionMessage(network_event.Peer));
                            break;
                        case EventType.Receive:
                            peerReceiveMessages.Add(new ENetPeerReceiveMessage(network_event.Peer, network_event.ChannelID, network_event.Packet));
                            break;
                        case EventType.Timeout:
                            peerTimeOutMessages.Add(new ENetPeerTimeOutMessage(network_event.Peer));
                            break;
                    }
                }
            });
            connectorThread.Start();
        }

        /// <summary>
        /// Remove peer
        /// </summary>
        /// <param name="peer">Peer</param>
        private void RemovePeer(Peer peer)
        {
            if (peerIDToPeerLookup.ContainsKey(peer.ID))
            {
                IPeer peer_peer = peerIDToPeerLookup[peer.ID];
                OnPeerDisconnected?.Invoke(peer_peer);
                peerIDToPeerLookup.Remove(peer.ID);
                peers.Remove(peer_peer.GUID.ToString());
            }
        }

        /// <summary>
        /// Connects to a network
        /// </summary>
        /// <param name="address">Network address</param>
        /// <returns>Peer</returns>
        public IPeer ConnectToNetwork(Address address) => new ENetPeer(Guid.NewGuid(), Host.Connect(address), Host);

        /// <summary>
        /// Processes all events appeared since last call
        /// </summary>
        public override void ProcessEvents()
        {
            while (peerConnectionAttemptMessages.TryTake(out ENetPeerConnectionAttemptMessage connection_attempt_message))
            {
                ENetPeer peer = new ENetPeer(Guid.NewGuid(), connection_attempt_message.Peer, Host);
                OnPeerConnectionAttempted?.Invoke(peer);
                if (IsConnectionAllowed(peer))
                {
                    peers.Add(peer.GUID.ToString(), peer);
                    peerIDToPeerLookup.Add(peer.Peer.ID, peer);
                    OnPeerConnected?.Invoke(peer);
                }
                else
                {
                    connection_attempt_message.Peer.Disconnect((uint)EDisconnectionReason.Banned);
                }
            }
            while (peerDisconnectionMessages.TryTake(out ENetPeerDisconnectionMessage disconnection_message))
            {
                RemovePeer(disconnection_message.Peer);
            }
            while (peerTimeOutMessages.TryTake(out ENetPeerTimeOutMessage time_out_message))
            {
                RemovePeer(time_out_message.Peer);
            }
            while (peerReceiveMessages.TryTake(out ENetPeerReceiveMessage receive_message))
            {
                if (peerIDToPeerLookup.ContainsKey(receive_message.Peer.ID))
                {
                    if (buffer.Length < receive_message.Packet.Length)
                    {
                        buffer = new byte[receive_message.Packet.Length / buffer.Length * (((receive_message.Packet.Length % buffer.Length) == 0) ? 1 : 2) * buffer.Length];
                    }
                    Marshal.Copy(receive_message.Packet.Data, buffer, 0, receive_message.Packet.Length);
                    OnPeerMessageReceived?.Invoke(peerIDToPeerLookup[receive_message.Peer.ID], Compression.Decompress(buffer, 0U, (uint)receive_message.Packet.Length));
                }
                receive_message.Packet.Dispose();
            }
        }

        /// <summary>
        /// Closes connection to all peers
        /// </summary>
        /// <param name="reason">Disconnection reason</param>
        public override void Close(EDisconnectionReason reason)
        {
            base.Close(reason);
            isConnectorThreadRunning = false;
            connectorThread.Join();
        }
    }
}
