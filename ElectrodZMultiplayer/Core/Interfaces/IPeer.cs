using System;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// An interface that describes a peer
    /// </summary>
    public interface IPeer : IValidable
    {
        /// <summary>
        /// Peer GUID
        /// </summary>
        Guid GUID { get; }

        /// <summary>
        /// Peer secret
        /// </summary>
        string Secret { get; }

        /// <summary>
        /// Send message
        /// </summary>
        /// <param name="message">Message</param>
        void SendMessage(byte[] message);

        /// <summary>
        /// Send message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="length">Message</param>
        void SendMessage(byte[] message, uint length);

        /// <summary>
        /// Closes the connection to peer
        /// </summary>
        /// <param name="reason">Disconnection reason</param>
        void Disconnect(EDisconnectionReason reason);
    }
}
