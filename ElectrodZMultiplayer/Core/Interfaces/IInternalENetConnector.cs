using ENet;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// An interface that represents an internal ENet connector
    /// </summary>
    public interface IInternalENetConnector : IENetConnector
    {
        /// <summary>
        /// Sends a message to peer internally
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="message">Message</param>
        /// <param name="index">Index</param>
        /// <param name="length">Length</param>
        void SendMessageToPeerInternally(Peer peer, byte[] message, uint index, uint length);
    }
}
