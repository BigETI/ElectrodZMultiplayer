/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Used to signal a server tick
    /// </summary>
    /// <param name="lobby">Lobby</param>
    /// <param name="time">Elapsed time</param>
    public delegate void ServerTickedDelegate(ILobby lobby, float time);
}
