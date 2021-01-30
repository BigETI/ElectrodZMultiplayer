/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Used to signal an user leaving the lobby
    /// </summary>
    /// <param name="lobby">Lobby</param>
    /// <param name="user">Leaving user</param>
    /// <param name="reason">Reason to leave</param>
    /// <param name="message">Leave message</param>
    public delegate void LobbyUserLeftDelegate(ILobby lobby, IUser user, EDisconnectionReason reason, string message);
}
