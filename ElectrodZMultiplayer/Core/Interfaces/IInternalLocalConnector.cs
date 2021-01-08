/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// An interface that represents an internal local connector
    /// </summary>
    internal interface IInternalLocalConnector : ILocalConnector
    {
        /// <summary>
        /// Registers a connecting peer
        /// </summary>
        /// <param name="peer">Peer</param>
        void RegisterConnectingPeer(IInternalLocalPeer peer);

        /// <summary>
        /// Acknowledges connection attempt
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="newPeer">New peer</param>
        void AcknowledgeConnectionAttempt(IInternalLocalPeer peer, IInternalLocalPeer newPeer);

        /// <summary>
        /// Disconnects a peer
        /// </summary>
        /// <param name="peer">Peer</param>
        void DisconnectPeer(IInternalLocalPeer peer);

        /// <summary>
        /// Notifies peer disconnection
        /// </summary>
        /// <param name="peer">Peer</param>
        void NotifyPeerDisconnection(IInternalLocalPeer peer);
    }
}
