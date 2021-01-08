/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// An interface that represents an internal local peer
    /// </summary>
    internal interface IInternalLocalPeer : ILocalPeer
    {
        /// <summary>
        /// Connector that owns this connector
        /// </summary>
        IInternalLocalConnector InternalOwningConnector { get; }

        /// <summary>
        /// Target connector
        /// </summary>
        IInternalLocalConnector InternalTargetConnector { get; }
    }
}
