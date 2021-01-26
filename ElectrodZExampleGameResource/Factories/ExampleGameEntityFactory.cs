using ElectrodZMultiplayer.Server;

/// <summary>
/// ElectrodZ example game resource factories namespace
/// </summary>
namespace ElectrodZExampleGameResource.Factories
{
    /// <summary>
    /// A class that describes an example game entity factory
    /// </summary>
    internal class ExampleGameEntityFactory : IGameEntityFactory
    {
        /// <summary>
        /// Creates a new game entity
        /// </summary>
        /// <param name="serverEntity">Game entity type</param>
        /// <returns>Game entity</returns>
        public IGameEntity CreateNewGameEntity(IServerEntity serverEntity) => new ExampleGameEntity(serverEntity);
    }
}
