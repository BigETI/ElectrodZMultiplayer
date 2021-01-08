using ENet;
using System;

/// <summary>
/// ElectrodZ multiplayer server namespace
/// </summary>
namespace ElectrodZMultiplayer.Server
{
    /// <summary>
    /// Servers namespace
    /// </summary>
    public static class Servers
    {
        /// <summary>
        /// Create server
        /// </summary>
        /// <returns>Server if successful, otherwise "null</returns>
        public static IServerSynchronizer Create() => Create(Defaults.networkPort, Defaults.timeoutTime);

        /// <summary>
        /// Create server
        /// </summary>
        /// <param name="networkPort">Server network port</param>
        /// <param name="timeoutTime">Timeout time in seconds</param>
        /// <returns>Server if successful, otherwise "null"</returns>
        public static IServerSynchronizer Create(ushort networkPort, uint timeoutTime)
        {
            if (networkPort < 1)
            {
                throw new ArgumentException("Network port can't be smaller than 1.");
            }
            IServerSynchronizer ret = null;
            if (NetworkLibrary.Initialize())
            {
                Host server_host = null;
                IConnector[] connectors = new IConnector[2];
                try
                {
                    server_host = new Host();
                    Address address = new Address
                    {
                        Port = networkPort
                    };
                    server_host.Create(address, (int)Library.maxPeers, 0);
                    ret = new ServerSynchronizer();
                    HandlePeerConnectionAttemptDelegate on_handle_peer_connection_attempt = (peer) => !ret.Bans.IsPeerBanned(peer, out _);
                    connectors[0] = new LocalConnector(on_handle_peer_connection_attempt);
                    connectors[1] = new ENetConnector(server_host, networkPort, timeoutTime, on_handle_peer_connection_attempt);
                    foreach (IConnector connector in connectors)
                    {
                        ret.AddConnector(connector);
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                    server_host?.Dispose();
                    foreach (IConnector connector in connectors)
                    {
                        connector?.Dispose();
                    }
                    ret?.Dispose();
                    NetworkLibrary.Deinitialize();
                }
            }
            return ret;
        }
    }
}
