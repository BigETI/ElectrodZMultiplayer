/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Used to signal when an entity has been destroyed in an entity streamer
    /// </summary>
    /// <param name="lobby">Lobby</param>
    /// <param name="entity">Entity</param>
    public delegate void LobbyEntityDestroyedDelegate(ILobby lobby, IEntity entity);
}
