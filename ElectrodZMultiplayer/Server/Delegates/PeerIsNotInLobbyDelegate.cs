/// <summary>
/// ElectrodZ multiplayer server namespace
/// </summary>
namespace ElectrodZMultiplayer.Server
{
    /// <summary>
    /// Used to assert a peer is not in lobby
    /// </summary>
    /// <param name="serverUser">Server user</param>
    internal delegate void PeerIsNotInLobbyDelegate(IInternalServerUser serverUser);
}
