using ENet;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// An interface that represents a peer for ENet
    /// </summary>
    public interface IENetPeer : IPeer
    {
        /// <summary>
        /// Peer
        /// </summary>
        Peer Peer { get; }

        /// <summary>
        /// Connector
        /// </summary>
        IENetConnector Connector { get; }
    }
}
