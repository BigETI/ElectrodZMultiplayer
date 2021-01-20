using System.Drawing;

/// <summary>
/// ElectrodZ multiplayer server namespace
/// </summary>
namespace ElectrodZMultiplayer.Server
{
    /// <summary>
    /// An interface that describes a server user
    /// </summary>
    public interface IServerUser : IServerEntity, IUser
    {
        /// <summary>
        /// Peer
        /// </summary>
        IPeer Peer { get; }

        /// <summary>
        /// Server
        /// </summary>
        IServerSynchronizer Server { get; }

        /// <summary>
        /// Token
        /// </summary>
        string Token { get; }

        /// <summary>
        /// Updates username
        /// </summary>
        /// <param name="name">Username</param>
        void UpdateUsername(string name);

        /// <summary>
        /// Updates user lobby color
        /// </summary>
        /// <param name="color">Lobby color</param>
        void UpdateUserLobbyColor(Color lobbyColor);

        /// <summary>
        /// Disconnects user
        /// </summary>
        /// <param name="reason">Reason</param>
        void Disconnect(EDisconnectionReason reason);

        /// <summary>
        /// Bans user
        /// </summary>
        /// <param name="reason">Ban reason</param>
        void Ban(string reason);
    }
}
