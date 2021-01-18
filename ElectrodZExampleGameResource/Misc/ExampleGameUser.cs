using ElectrodZMultiplayer.Server;

/// <summary>
/// ElectrodZ example game resource namespace
/// </summary>
namespace ElectrodZExampleGameResource
{
    /// <summary>
    /// A class that describes an example game user
    /// </summary>
    internal class ExampleGameUser : AGameUser
    {
        /// <summary>
        /// Constructs an example game user
        /// </summary>
        /// <param name="serverUser"></param>
        public ExampleGameUser(IServerUser serverUser) : base(serverUser)
        {
            // ...
        }
    }
}
