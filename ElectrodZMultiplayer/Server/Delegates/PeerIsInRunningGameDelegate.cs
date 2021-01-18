/// <summary>
/// ElectrodZ multiplayer server namspace
/// </summary>
namespace ElectrodZMultiplayer.Server
{
    /// <summary>
    /// Used to assert a peer is lobby owner
    /// </summary>
    /// <param name="serverUser">Server user</param>
    /// <param name="serverLobby">Server lobby</param>
    /// <param name="gameMode">Game mode</param>
    internal delegate void PeerIsInRunningGameDelegate(IInternalServerUser serverUser, IInternalServerLobby serverLobby, IGameMode gameMode);
}
