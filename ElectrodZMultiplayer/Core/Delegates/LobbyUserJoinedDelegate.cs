/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Used to signal an user joining a lobby
    /// </summary>
    /// <param name="lobby">Lobby</param>
    /// <param name="user">Joining user</param>
    public delegate void LobbyUserJoinedDelegate(ILobby lobby, IUser user);
}
