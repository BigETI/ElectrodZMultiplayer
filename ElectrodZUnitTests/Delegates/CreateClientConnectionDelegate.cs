using ElectrodZMultiplayer.Client;
using ElectrodZMultiplayer.Server;

/// <summary>
/// ElectodZ unit tests namespace
/// </summary>
namespace ElectrodZUnitTests
{
    /// <summary>
    /// Used to create a client connection
    /// </summary>
    /// <param name="server">Server</param>
    /// <returns>Client</returns>
    public delegate IClientSynchronizer CreateClientConnectionDelegate(IServerSynchronizer server);
}
