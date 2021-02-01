/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// This is being used to signal when restarting a game has been cancelled.
    /// </summary>
    /// <param name="lobby">Lobby</param>
    public delegate void LobbyRestartGameCancelledDelegate(ILobby lobby);
}
