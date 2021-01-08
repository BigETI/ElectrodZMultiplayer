/// <summary>
/// ElectrodZ multiplayer server namespace
/// </summary>
namespace ElectrodZMultiplayer.Server
{
    /// <summary>
    /// Used to assert a peer being authenticated
    /// </summary>
    /// <param name="serverUser">Server user</param>
    internal delegate void PeerAuthenticatedDelegate(IInternalServerUser serverUser);
}
