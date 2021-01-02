using System;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// AN abstract class that descibes any peer
    /// </summary>
    internal abstract class APeer : IPeer
    {
        /// <summary>
        /// Peer GUID
        /// </summary>
        public Guid GUID { get; }

        /// <summary>
        /// Peer secret
        /// </summary>
        public string Secret { get; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public virtual bool IsValid => GUID != Guid.Empty;

        /// <summary>
        /// Construct a generalized peer object
        /// </summary>
        /// <param name="guid">Peer GUID</param>
        /// <param name="secret">Peer secret</param>
        public APeer(Guid guid, string secret)
        {
            if (guid == Guid.Empty)
            {
                throw new ArgumentException("Peer GUID can't be empty.");
            }
            GUID = guid;
            Secret = secret ?? throw new ArgumentNullException(nameof(secret));
        }

        /// <summary>
        /// Sends a message to peer
        /// </summary>
        /// <param name="message">Message</param>
        public abstract void SendMessage(byte[] message);

        /// <summary>
        /// Sends a message to peer
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="length">Message length in bytes</param>
        public abstract void SendMessage(byte[] message, uint length);

        /// <summary>
        /// Closes the connection to peer
        /// </summary>
        /// <param name="reason">Disconnection reason</param>
        public abstract void Disconnect(EDisconnectionReason reason);
    }
}
