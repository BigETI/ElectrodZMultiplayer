/// <summary>
/// ElectrodZ multiplayer client namespace
/// </summary>
namespace ElectrodZMultiplayer.Client
{
    /// <summary>
    /// This is being used to signal acknowledging lobby being joined
    /// </summary>
    /// <param name="lobby">Lobby</param>
    public delegate void LobbyJoinAcknowledgedDelegate(ILobby lobby);
}
