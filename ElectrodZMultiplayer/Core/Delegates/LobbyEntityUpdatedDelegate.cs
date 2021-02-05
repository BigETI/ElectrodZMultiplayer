/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Used to signal when an entity has been updated in an entity streamer
    /// </summary>
    /// <param name="lobby">Lobby</param>
    /// <param name="entity">Entity</param>
    public delegate void LobbyEntityUpdatedDelegate(ILobby lobby, IEntity entity);
}
