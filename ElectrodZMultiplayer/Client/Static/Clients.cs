using ENet;
using System;

/// <summary>
/// ElectrodZ multiplayer client namespace
/// </summary>
namespace ElectrodZMultiplayer.Client
{
    /// <summary>
    /// A class to provide an API for creating clients
    /// </summary>
    public static class Clients
    {
        /// <summary>
        /// Connects to a local instance
        /// </summary>
        /// <param name="targetConnector">Target connector</param>
        /// <param name="token">Token</param>
        /// <returns>Client synchronizer if successful, otherwise "null"</returns>
        public static IClientSynchronizer ConnectToLocalInstance(ILocalConnector targetConnector, string token)
        {
            if (targetConnector == null)
            {
                throw new ArgumentNullException(nameof(targetConnector));
            }
            ILocalConnector connector = new LocalConnector((_) => true);
            IClientSynchronizer ret = new ClientSynchronizer(connector.ConnectToLocal(targetConnector), token);
            ret.AddConnector(connector);
            return ret;
        }

        /// <summary>
        /// Connects to a network instance
        /// </summary>
        /// <param name="ipAddress">IP address</param>
        /// <param name="port">Port</param>
        /// <param name="token">Token</param>
        /// <param name="timeoutTime">Timeout time in seconds</param>
        /// <returns>Client synchronizer if successful, otherwise "null"</returns>
        public static IClientSynchronizer ConnectToNetwork(string ipAddress, ushort port, string token, uint timeoutTime)
        {
            if (ipAddress == null)
            {
                throw new ArgumentNullException(nameof(ipAddress));
            }
            if (port <= 0)
            {
                throw new ArgumentException(nameof(port));
            }
            IClientSynchronizer ret = null;
            if (NetworkLibrary.Initialize())
            {
                Host client_host = null;
                try
                {
                    client_host = new Host();
                    Address address = new Address();
                    address.SetHost(ipAddress);
                    address.Port = port;
                    client_host.Create(1, 0);
                    ENetConnector connector = new ENetConnector(client_host, port, timeoutTime, (_) => true);
                    ret = new ClientSynchronizer(connector.ConnectToNetwork(address), token);
                    ret.AddConnector(connector);
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                    if (client_host != null)
                    {
                        client_host.Dispose();
                    }
                    NetworkLibrary.Deinitialize();
                }
            }
            return ret;
        }
    }
}
