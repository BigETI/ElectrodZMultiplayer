/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Used to signal when an user entity has been created in an entity streamer
    /// </summary>
    /// <param name="lobby">Lobby</param>
    /// <param name="user">User</param>
    public delegate void LobbyUserEntityCreatedDelegate(ILobby lobby, IUser user);
}
