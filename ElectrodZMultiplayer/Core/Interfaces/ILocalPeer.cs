/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// An interface that describes a local peer
    /// </summary>
    public interface ILocalPeer : IPeer
    {
        /// <summary>
        /// Connector that owns this connector
        /// </summary>
        ILocalConnector OwningConnector { get; }
        
        /// <summary>
        /// Target connector
        /// </summary>
        ILocalConnector TargetConnector { get; }
    }
}
