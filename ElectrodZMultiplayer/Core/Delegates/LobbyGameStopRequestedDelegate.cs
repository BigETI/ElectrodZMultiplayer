/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Used to signal a game stop request
    /// </summary>
    /// <param name="lobby">Lobby</param>
    /// <param name="time">Time to stop game in seconds</param>
    public delegate void LobbyGameStopRequestedDelegate(ILobby lobby, double time);
}
