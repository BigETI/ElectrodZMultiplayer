using System.Drawing;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// An interface that describes a user
    /// </summary>
    public interface IUser : IEntity
    {
        /// <summary>
        /// Lobby
        /// </summary>
        ILobby Lobby { get; }

        /// <summary>
        /// Name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Lobby color
        /// </summary>
        Color LobbyColor { get; }
    }
}
