using ElectrodZExampleGameResource.Factories;
using ElectrodZMultiplayer.Server;

/// <summary>
/// ElectrodZ example game resource resources namespace
/// </summary>
namespace ElectrodZExampleGameResource.Resources
{
    /// <summary>
    /// A class that describes an example game resource
    /// </summary>
    public class ExampleGameResource : AGameResource
    {
        /// <summary>
        /// Creates a new game user factory
        /// </summary>
        /// <returns>Game user factory</returns>
        public override IGameUserFactory CreateNewGameUserFactory() => new ExampleGameUserFactory();

        /// <summary>
        /// Creates a new game entity factory
        /// </summary>
        /// <returns>Game entity factory</returns>
        public override IGameEntityFactory CreateNewGameEntityFactory() => new ExampleGameEntityFactory();
    }
}
