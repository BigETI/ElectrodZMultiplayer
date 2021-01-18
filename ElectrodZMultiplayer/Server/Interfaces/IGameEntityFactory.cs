/// <summary>
/// ElectrodZ multiplayer server namespace
/// </summary>
namespace ElectrodZMultiplayer.Server
{
    /// <summary>
    /// An interface that represents a game entity factory
    /// </summary>
    public interface IGameEntityFactory
    {
        /// <summary>
        /// Creates a new game entity
        /// </summary>
        /// <param name="serverEntity">Server entity</param>
        /// <returns>Game entity</returns>
        IGameEntity CreateNewGameEntity(IServerEntity serverEntity);
    }
}
