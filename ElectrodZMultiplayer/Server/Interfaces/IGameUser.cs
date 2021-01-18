/// <summary>
/// ElectrodZ multiplayer server namespace
/// </summary>
namespace ElectrodZMultiplayer.Server
{
    /// <summary>
    /// An interface that represents a game user
    /// </summary>
    public interface IGameUser : IServerUser
    {
        /// <summary>
        /// Server user
        /// </summary>
        IServerUser ServerUser { get; }
    }
}
