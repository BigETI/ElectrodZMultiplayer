/// <summary>
/// ElectrodZ multiplayer server namespace
/// </summary>
namespace ElectrodZMultiplayer.Server
{
    /// <summary>
    /// An interface that represents a game entity
    /// </summary>
    public interface IGameEntity : IServerEntity
    {
        /// <summary>
        /// Server entity
        /// </summary>
        IServerEntity ServerEntity { get; }
    }
}
