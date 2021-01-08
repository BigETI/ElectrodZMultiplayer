using System;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// An interface that represents a generalized peer
    /// </summary>
    public interface IPeer : IValidable, IDisposable
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
        /// Sends a message to peer
        /// </summary>
        /// <param name="message">Message</param>
        void SendMessage(byte[] message);

        /// <summary>
        /// Sends a message to peer
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="index">Starting index</param>
        /// <param name="length">Message</param>
        void SendMessage(byte[] message, uint index, uint length);

        /// <summary>
        /// Closes the connection to peer
        /// </summary>
        /// <param name="reason">Disconnection reason</param>
        void Disconnect(EDisconnectionReason reason);
    }
}
