using System;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Local peer class
    /// </summary>
    internal class LocalPeer : APeer, IInternalLocalPeer
    {
        /// <summary>
        /// Internal owning connector
        /// </summary>
        public IInternalLocalConnector InternalOwningConnector { get; }

        /// <summary>
        /// Internal target connector
        /// </summary>
        public IInternalLocalConnector InternalTargetConnector { get; }

        /// <summary>
        /// Owning connector
        /// </summary>
        public ILocalConnector OwningConnector => InternalOwningConnector;

        /// <summary>
        /// Target connector
        /// </summary>
        public ILocalConnector TargetConnector => InternalTargetConnector;

        /// <summary>
        /// Constructs a local peer
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="owningConnector">Owning connector</param>
        /// <param name="targetConnector">Target connector</param>
        public LocalPeer(Guid guid, IInternalLocalConnector owningConnector, IInternalLocalConnector targetConnector) : base(guid, "local")
        {
            if (targetConnector == this)
            {
                throw new ArgumentException("Target connector must be of another instance.", nameof(targetConnector));
            }
            InternalOwningConnector = owningConnector ?? throw new ArgumentNullException(nameof(owningConnector));
            InternalTargetConnector = targetConnector ?? throw new ArgumentNullException(nameof(targetConnector));
        }

        /// <summary>
        /// Sends a message
        /// </summary>
        /// <param name="message">Message</param>
        public override void SendMessage(byte[] message) => InternalTargetConnector.PushMessage(this, message);

        /// <summary>
        /// Sends a message
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="length">Message length in bytes</param>
        public override void SendMessage(byte[] message, uint length) => InternalTargetConnector.PushMessage(this, message, length);

        /// <summary>
        /// Disconnects peer
        /// </summary>
        /// <param name="reason">Reason</param>
        public override void Disconnect(EDisconnectionReason reason) => InternalOwningConnector.DisconnectPeer(this);
    }
}
