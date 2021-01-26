using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer server namespace
/// </summary>
namespace ElectrodZMultiplayer.Server
{
    /// <summary>
    /// An interface that represents a game resource
    /// </summary>
    public interface IGameResource
    {
        /// <summary>
        /// Server
        /// </summary>
        IServerSynchronizer Server { get; }

        /// <summary>
        /// Creates a new game user factory
        /// </summary>
        /// <returns>Game user factory</returns>
        IGameUserFactory CreateNewGameUserFactory();

        /// <summary>
        /// Creates a new game entity factory
        /// </summary>
        /// <returns>Game entity factory</returns>
        IGameEntityFactory CreateNewGameEntityFactory();

        /// <summary>
        /// Available game mode types
        /// </summary>
        IReadOnlyDictionary<string, Type> AvailableGameModeTypes { get; }

        /// <summary>
        /// Game resource has been initialized
        /// </summary>
        /// <param name="server">Server</param>
        void OnInitialized(IServerSynchronizer server);

        /// <summary>
        /// Game resource has been closed
        /// </summary>
        void OnClosed();
    }
}
