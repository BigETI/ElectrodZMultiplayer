using System;
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
        /// Game resources
        /// </summary>
        IReadOnlyDictionary<string, IGameResource> GameResources { get; }

        /// <summary>
        /// Available game mode types
        /// </summary>
        IReadOnlyDictionary<string, (IGameResource, Type)> AvailableGameModeTypes { get; }

        /// <summary>
        /// Adds a new game resource
        /// </summary>
        /// <typeparam name="T">Game resource type</typeparam>
        /// <returns>"true" if the specified game resource has been added, otherwise "false"</returns>
        bool AddGameResource<T>() where T : IGameResource;

        /// <summary>
        /// Adds a new game resource
        /// </summary>
        /// <param name="gameResourceType">Game resource type</param>
        /// <returns>"true" if the specified game resource has been added, otherwise "false"</returns>
        bool AddGameResource(Type gameResourceType);

        /// <summary>
        /// Removes the specified game resource
        /// </summary>
        /// <typeparam name="T">Game resource type</typeparam>
        /// <returns>"true" if the specified game resource has been removed, otherwise "false"</returns>
        bool RemoveGameResource<T>() where T : IGameResource;

        /// <summary>
        /// Removes the specified game resource
        /// </summary>
        /// <param name="gameResourceType">Game resource type</param>
        /// <returns>"true" if the specified game resource has been removed, otherwise "false"</returns>
        bool RemoveGameResource(Type gameResourceType);

        /// <summary>
        /// Processes all events appeared since last call
        /// </summary>
        void ProcessEvents();
    }
}
