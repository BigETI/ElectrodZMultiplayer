using System.Drawing;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// An interface that represents a generalized user
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

        /// <summary>
        /// This event will be invoked when the username changes.
        /// </summary>
        event UsernameUpdatedDelegate OnUsernameUpdated;

        /// <summary>
        /// This event will be invoked when the user lobby color changes.
        /// </summary>
        event UserLobbyColorUpdatedDelegate OnUserLobbyColorUpdated;

        /// <summary>
        /// This event will be invoked when a client tick has been performed.
        /// </summary>
        event ClientTickedDelegate OnClientTicked;

        /// <summary>
        /// This event will be invoked when a server tick has been performed.
        /// </summary>
        event ServerTickedDelegate OnServerTicked;
    }
}
