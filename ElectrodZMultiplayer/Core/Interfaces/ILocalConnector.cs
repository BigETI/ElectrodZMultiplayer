/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// An interface that describes a local connector
    /// </summary>
    public interface ILocalConnector : IConnector
    {
        /// <summary>
        /// Pushes a new message
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="message">Message</param>
        void PushMessage(ILocalPeer peer, byte[] message);

        /// <summary>
        /// Pushes a new message
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="message">Message</param>
        /// <param name="length">Message length in bytes</param>
        void PushMessage(ILocalPeer peer, byte[] message, uint length);

        /// <summary>
        /// Connects to a local instance
        /// </summary>
        /// <param name="targetConnector">Target local connector</param>
        /// <returns>Peer</returns>
        IPeer ConnectToLocal(ILocalConnector targetConnector);
    }
}
