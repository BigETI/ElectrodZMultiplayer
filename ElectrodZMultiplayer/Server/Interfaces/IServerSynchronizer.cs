using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer server namespace
/// </summary>
namespace ElectrodZMultiplayer.Server
{
    /// <summary>
    /// An interface that represents a server synchronizer
    /// </summary>
    public interface IServerSynchronizer : ISynchronizer
    {
        /// <summary>
        /// Bans
        /// </summary>
        IBans Bans { get; }

        /// <summary>
        /// Users
        /// </summary>
        IReadOnlyDictionary<string, IUser> Users { get; }

        /// <summary>
        /// Lobbies
        /// </summary>
        IReadOnlyDictionary<string, ILobby> Lobbies { get; }

        /// <summary>
        /// Processes all events appeared since last call
        /// </summary>
        void ProcessEvents();
    }
}
