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
    internal class ENetConnector : AConnector, IInternalENetConnector
    {
        /// <summary>
        /// Peer connection attempt messages
        /// </summary>
        private readonly ConcurrentQueue<ENetPeerConnectionAttemptMessage> peerConnectionAttemptMessages = new ConcurrentQueue<ENetPeerConnectionAttemptMessage>();

        /// <summary>
        /// Peer disconnection messages
        /// </summary>
        private readonly ConcurrentQueue<ENetPeerDisconnectionMessage> peerDisconnectionMessages = new ConcurrentQueue<ENetPeerDisconnectionMessage>();

        /// <summary>
        /// Peer time out messages
        /// </summary>
        private readonly ConcurrentQueue<ENetPeerTimeOutMessage> peerTimeOutMessages = new ConcurrentQueue<ENetPeerTimeOutMessage>();

        /// <summary>
        /// Peer receive messages
        /// </summary>
        private readonly ConcurrentQueue<ENetPeerReceiveMessage> peerReceiveMessages = new ConcurrentQueue<ENetPeerReceiveMessage>();

        /// <summary>
        /// Peer send messages
        /// </summary>
        private readonly ConcurrentQueue<ENetPeerSendMessage> peerSendMessages = new ConcurrentQueue<ENetPeerSendMessage>();

        /// <summary>
        /// Dispose packets
        /// </summary>
        private readonly List<Packet> disposePackets = new List<Packet>();

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
        private volatile bool isConnectorThreadRunning = true;

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
                HashSet<uint> available_peer_ids = new HashSet<uint>();
                while (isConnectorThreadRunning)
                {
                    bool has_network_event = true;
                    if (Host.CheckEvents(out Event network_event) <= 0)
                    {
                        if (Host.Service((int)TimeoutTime, out network_event) <= 0)
                        {
                            has_network_event = false;
                        }
                    }
                    if (has_network_event)
                    {
                        switch (network_event.Type)
                        {
                            case EventType.Connect:
                                available_peer_ids.Add(network_event.Peer.ID);
                                peerConnectionAttemptMessages.Enqueue(new ENetPeerConnectionAttemptMessage(network_event.Peer));
                                break;
                            case EventType.Disconnect:
                                available_peer_ids.Remove(network_event.Peer.ID);
                                peerDisconnectionMessages.Enqueue(new ENetPeerDisconnectionMessage(network_event.Peer));
                                break;
                            case EventType.Receive:
                                Packet packet = network_event.Packet;
                                if (buffer.Length < packet.Length)
                                {
                                    buffer = new byte[packet.Length / buffer.Length * (((packet.Length % buffer.Length) == 0) ? 1 : 2) * buffer.Length];
                                }
                                Marshal.Copy(packet.Data, buffer, 0, packet.Length);
                                peerReceiveMessages.Enqueue(new ENetPeerReceiveMessage(network_event.Peer, network_event.ChannelID, Compression.Decompress(buffer, 0U, (uint)packet.Length)));
                                break;
                            case EventType.Timeout:
                                available_peer_ids.Remove(network_event.Peer.ID);
                                peerTimeOutMessages.Enqueue(new ENetPeerTimeOutMessage(network_event.Peer));
                                break;
                        }
                    }
                    while (peerSendMessages.TryDequeue(out ENetPeerSendMessage send_message))
                    {
                        if (available_peer_ids.Contains(send_message.Peer.ID))
                        {
                            Packet packet = default;
                            packet.Create(send_message.Message, (int)send_message.Index, (int)send_message.Length, PacketFlags.Reliable);
                            send_message.Peer.Send(0, ref packet);
                            disposePackets.Add(packet);
                        }
                    }
                    if (disposePackets.Count > 0)
                    {
                        Host.Flush();
                        foreach (Packet packet in disposePackets)
                        {
                            packet.Dispose();
                        }
                        disposePackets.Clear();
                    }
                }
                available_peer_ids.Clear();
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
        /// Sends a message to peer internally
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="message">Message</param>
        /// <param name="index">Index</param>
        /// <param name="length">Length</param>
        public void SendMessageToPeerInternally(Peer peer, byte[] message, uint index, uint length)
        {
            if (peerIDToPeerLookup.ContainsKey(peer.ID))
            {
                peerSendMessages.Enqueue(new ENetPeerSendMessage(peer, message, index, length));
            }
        }

        /// <summary>
        /// Connects to a network
        /// </summary>
        /// <param name="address">Network address</param>
        /// <returns>Peer</returns>
        public IPeer ConnectToNetwork(Address address) => new ENetPeer(Guid.NewGuid(), Host.Connect(address), this);

        /// <summary>
        /// Processes all events appeared since last call
        /// </summary>
        public override void ProcessEvents()
        {
            while (peerConnectionAttemptMessages.TryDequeue(out ENetPeerConnectionAttemptMessage connection_attempt_message))
            {
                ENetPeer peer = new ENetPeer(Guid.NewGuid(), connection_attempt_message.Peer, this);
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
            while (peerDisconnectionMessages.TryDequeue(out ENetPeerDisconnectionMessage disconnection_message))
            {
                RemovePeer(disconnection_message.Peer);
            }
            while (peerTimeOutMessages.TryDequeue(out ENetPeerTimeOutMessage time_out_message))
            {
                RemovePeer(time_out_message.Peer);
            }
            while (peerReceiveMessages.TryDequeue(out ENetPeerReceiveMessage receive_message))
            {
                if (peerIDToPeerLookup.ContainsKey(receive_message.Peer.ID))
                {
                    OnPeerMessageReceived?.Invoke(peerIDToPeerLookup[receive_message.Peer.ID], receive_message.Message);
                }
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
