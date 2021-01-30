/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Used to signal a game start request
    /// </summary>
    /// <param name="lobby">Lobby</param>
    /// <param name="time">Time to start game in seconds</param>
    public delegate void LobbyGameStartRequestedDelegate(ILobby lobby, double time);
}
