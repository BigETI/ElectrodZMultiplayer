/// <summary>
/// ElectrodZ multiplayer server namespace
/// </summary>
namespace ElectrodZMultiplayer.Server
{
    /// <summary>
    /// Server lobby interface
    /// </summary>
    public interface IServerLobby : ILobby
    {
        /// <summary>
        /// Server
        /// </summary>
        IServerSynchronizer Server { get; }

        /// <summary>
        /// Gets invoked when lobby has been closed
        /// </summary>
        event LobbyClosedDelegate OnLobbyClosed;

        /// <summary>
        /// Remove user
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="reason">Reason</param>
        /// <returns>"true" if user was removed, otherwise "false"</returns>
        bool RemoveUser(IUser user, string reason);

        /// <summary>
        /// Close lobby
        /// </summary>
        void Close();
    }
}
