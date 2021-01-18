using ElectrodZMultiplayer.Server;

/// <summary>
/// ElectrodZ example game resource factories namespace
/// </summary>
namespace ElectrodZExampleGameResource.Factories
{
    /// <summary>
    /// A class that describes an example game user factory
    /// </summary>
    internal class ExampleGameUserFactory : IGameUserFactory
    {
        /// <summary>
        /// Creates a new game user
        /// </summary>
        /// <param name="serverUser">Server user</param>
        /// <returns>Game user</returns>
        public IGameUser CreateNewGameUser(IServerUser serverUser) => new ExampleGameUser(serverUser);
    }
}
