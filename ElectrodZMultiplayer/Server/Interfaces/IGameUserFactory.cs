/// <summary>
/// ElectrodZ multiplayer server namespace
/// </summary>
namespace ElectrodZMultiplayer.Server
{
    /// <summary>
    /// An interface that represents a game user factory
    /// </summary>
    public interface IGameUserFactory
    {
        /// <summary>
        /// Creates a new game user
        /// </summary>
        /// <param name="serverUser">Server user</param>
        /// <returns>Game user</returns>
        IGameUser CreateNewGameUser(IServerUser serverUser);
    }
}
