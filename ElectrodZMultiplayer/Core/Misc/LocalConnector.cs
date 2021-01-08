using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// A class that describes a local connector
    /// </summary>
    internal class LocalConnector : AConnector, IInternalLocalConnector
    {
        /// <summary>
        /// Peer connection attempt messages
        /// </summary>
        private readonly ConcurrentBag<LocalPeerConnectionAttemptMessage> localPeerConnectionAttemptMessages = new ConcurrentBag<LocalPeerConnectionAttemptMessage>();

        /// <summary>
        /// Peer disconnection messages
        /// </summary>
        private readonly ConcurrentBag<LocalPeerDisconnectionMessage> localPeerDisconnectionMessages = new ConcurrentBag<LocalPeerDisconnectionMessage>();

        /// <summary>
        /// Peer receive messages
        /// </summary>
        private readonly ConcurrentBag<LocalPeerReceiveMessage> localPeerReceiveMessages = new ConcurrentBag<LocalPeerReceiveMessage>();

        /// <summary>
        /// Peer to peer lookup
        /// </summary>
        private readonly Dictionary<string, ILocalPeer> peerPeerLookup = new Dictionary<string, ILocalPeer>();

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
        /// Constructs a local connector
        /// </summary>
        /// <param name="onHandlePeerConnectionAttempt">Handles peer connection attempts</param>
        public LocalConnector(HandlePeerConnectionAttemptDelegate onHandlePeerConnectionAttempt) : base(onHandlePeerConnectionAttempt)
        {
            // ...
        }

        /// <summary>
        /// Processes all events
        /// </summary>
        public override void ProcessEvents()
        {
            while (localPeerConnectionAttemptMessages.TryTake(out LocalPeerConnectionAttemptMessage local_peer_connection_attempt_message))
            {
                IPeer peer = local_peer_connection_attempt_message.Peer;
                OnPeerConnectionAttempted?.Invoke(peer);
                if (IsConnectionAllowed(peer))
                {
                    peers.Add(peer.GUID.ToString(), peer);
                    OnPeerConnected?.Invoke(peer);
                }
                else
                {
                    local_peer_connection_attempt_message.Peer.Disconnect(EDisconnectionReason.Banned);
                }
            }
            while (localPeerDisconnectionMessages.TryTake(out LocalPeerDisconnectionMessage local_peer_disconnection_message))
            {
                IPeer peer = local_peer_disconnection_message.Peer;
                string key = peer.GUID.ToString();
                if (peers.ContainsKey(key))
                {
                    OnPeerDisconnected?.Invoke(peer);
                    peers.Remove(key);
                }
                foreach (KeyValuePair<string, ILocalPeer> peer_peer in peerPeerLookup)
                {
                    if (peer.GUID == peer_peer.Value.GUID)
                    {
                        peerPeerLookup.Remove(peer_peer.Key);
                        break;
                    }
                }
            }
            while (localPeerReceiveMessages.TryTake(out LocalPeerReceiveMessage local_peer_receive_message))
            {
                OnPeerMessageReceived?.Invoke(local_peer_receive_message.Peer, Compression.Decompress(local_peer_receive_message.Message, local_peer_receive_message.Index, local_peer_receive_message.Length));
            }
        }

        /// <summary>
        /// Pushes a new message
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="message">Message</param>
        public void PushMessage(ILocalPeer peer, byte[] message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            PushMessage(peer, message, 0U, (uint)message.Length);
        }

        /// <summary>
        /// Pushes a new message
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="message">Message</param>
        /// <param name="index">Starting index</param>
        /// <param name="length">Message length in bytes</param>
        public void PushMessage(ILocalPeer peer, byte[] message, uint index, uint length)
        {
            if (peer == null)
            {
                throw new ArgumentNullException(nameof(peer));
            }
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
            string key = peer.GUID.ToString();
            if (peerPeerLookup.ContainsKey(key))
            {
                localPeerReceiveMessages.Add(new LocalPeerReceiveMessage(peerPeerLookup[key], message, index, length));
            }
        }

        /// <summary>
        /// Connects to a local instance
        /// </summary>
        /// <param name="targetConnector">Target local connector</param>
        /// <returns>Peer</returns>
        public IPeer ConnectToLocal(ILocalConnector targetConnector)
        {
            if (targetConnector == null)
            {
                throw new ArgumentNullException(nameof(targetConnector));
            }
            if (targetConnector == this)
            {
                throw new ArgumentException("Target connector must be of another instance.", nameof(targetConnector));
            }
            if (!(targetConnector is IInternalLocalConnector target_connector))
            {
                throw new ArgumentException($"Target connector does not inherit from \"{ nameof(IInternalLocalConnector) }\".", nameof(target_connector));
            }
            IInternalLocalPeer ret = new LocalPeer(Guid.NewGuid(), this, target_connector);
            target_connector.RegisterConnectingPeer(ret);
            return ret;
        }

        /// <summary>
        /// Registers a connecting peer
        /// </summary>
        /// <param name="peer">Peer</param>
        public void RegisterConnectingPeer(IInternalLocalPeer peer)
        {
            if (peer == null)
            {
                throw new ArgumentNullException(nameof(peer));
            }
            if (peer.InternalTargetConnector != this)
            {
                throw new ArgumentException("Target local connector of local peer must be the same instance as the callee.", nameof(peer));
            }
            IInternalLocalPeer new_peer = new LocalPeer(Guid.NewGuid(), this, peer.InternalOwningConnector);
            peerPeerLookup.Add(peer.GUID.ToString(), new_peer);
            localPeerConnectionAttemptMessages.Add(new LocalPeerConnectionAttemptMessage(new_peer));
            peer.InternalOwningConnector.AcknowledgeConnectionAttempt(peer, new_peer);
        }

        /// <summary>
        /// Acknowledges connection attempt
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="newPeer">New peer</param>
        public void AcknowledgeConnectionAttempt(IInternalLocalPeer peer, IInternalLocalPeer newPeer)
        {
            if (peer == null)
            {
                throw new ArgumentNullException(nameof(peer));
            }
            if (newPeer == null)
            {
                throw new ArgumentNullException(nameof(newPeer));
            }
            if (peer.InternalOwningConnector != this)
            {
                throw new ArgumentException("Owning local connector of local peer must be the same instance as the callee.", nameof(peer));
            }
            if (newPeer.TargetConnector != this)
            {
                throw new ArgumentException("Target local connector of new local peer must be the same instance as the callee.", nameof(newPeer));
            }
            string key = newPeer.GUID.ToString();
            if (!peerPeerLookup.ContainsKey(key))
            {
                peerPeerLookup.Add(key, peer);
                localPeerConnectionAttemptMessages.Add(new LocalPeerConnectionAttemptMessage(peer));
            }
        }

        /// <summary>
        /// Disconnects a peer
        /// </summary>
        /// <param name="peer">Peer</param>
        public void DisconnectPeer(IInternalLocalPeer peer)
        {
            if (peer == null)
            {
                throw new ArgumentNullException(nameof(peer));
            }
            if (peer.InternalOwningConnector != this)
            {
                throw new ArgumentException("Owning local connector of local peer must be the same instance as the callee.", nameof(peer));
            }
            localPeerDisconnectionMessages.Add(new LocalPeerDisconnectionMessage(peer));
            peer.InternalTargetConnector.NotifyPeerDisconnection(peer);
        }

        /// <summary>
        /// Notifies peer disconnection
        /// </summary>
        /// <param name="peer">Peer</param>
        public void NotifyPeerDisconnection(IInternalLocalPeer peer)
        {
            if (peer == null)
            {
                throw new ArgumentNullException(nameof(peer));
            }
            if (peer.InternalTargetConnector != this)
            {
                throw new ArgumentException("Target local connector of local peer must be the same instance as the callee.", nameof(peer));
            }
            string key = peer.GUID.ToString();
            if (peerPeerLookup.ContainsKey(key))
            {
                localPeerDisconnectionMessages.Add(new LocalPeerDisconnectionMessage(peerPeerLookup[key]));
            }
        }
    }
}
