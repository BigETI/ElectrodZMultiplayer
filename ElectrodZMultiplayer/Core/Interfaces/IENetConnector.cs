using ENet;
using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// An interface that describes a connector for ENet
    /// </summary>
    public interface IENetConnector : IConnector
    {
        /// <summary>
        /// Host
        /// </summary>
        Host Host { get; }

        /// <summary>
        /// Port
        /// </summary>
        ushort Port { get; }

        /// <summary>
        /// Timeout time in seconds
        /// </summary>
        uint TimeoutTime { get; }

        /// <summary>
        /// Connects to a network
        /// </summary>
        /// <param name="address">Network address</param>
        /// <returns>Peer</returns>
        IPeer ConnectToNetwork(Address address);
    }
}
