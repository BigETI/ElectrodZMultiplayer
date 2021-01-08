/// <summary>
/// ElectrodZ multiplayer server namespace
/// </summary>
namespace ElectrodZMultiplayer.Server
{
    /// <summary>
    /// Used to assert a peer is in lobby
    /// </summary>
    /// <param name="serverUser">Server user</param>
    /// <param name="serverLobby">Server lobby</param>
    internal delegate void PeerIsInLobbyDelegate(IInternalServerUser serverUser, IInternalServerLobby serverLobby);
}
