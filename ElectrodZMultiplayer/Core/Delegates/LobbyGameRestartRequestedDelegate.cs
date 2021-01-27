/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Used to signal a request to restart a game
    /// </summary>
    /// <param name="lobby">Lobby</param>
    /// <param name="time">Time to restart game in seconds</param>
    public delegate void LobbyGameRestartRequestedDelegate(ILobby lobby, double time);
}
