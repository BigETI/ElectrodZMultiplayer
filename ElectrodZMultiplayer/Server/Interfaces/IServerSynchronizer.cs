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
        /// Gets user by GUID
        /// </summary>
        /// <param name="guid">User GUID</param>
        /// <returns>User if user is available, otherwise "null"</returns>
        IUser GetUserByGUID(Guid guid);

        /// <summary>
        /// Tries to get user by GUID
        /// </summary>
        /// <param name="guid">User GUID</param>
        /// <param name="user">User</param>
        /// <returns>"true" if user is available, otherwise "false"</returns>
        bool TryGetUserByGUID(Guid guid, out IUser user);

        /// <summary>
        /// Gets lobby by lobby code
        /// </summary>
        /// <param name="lobbyCode">Lobby code</param>
        /// <returns>Lobby if lobby is available, otherwise "null"</returns>
        ILobby GetLobbyByLobbyCode(string lobbyCode);

        /// <summary>
        /// Tries to get lobby by lobby code
        /// </summary>
        /// <param name="lobbyCode">Lobby code</param>
        /// <param name="lobby">Lobby</param>
        /// <returns>"true" if lobby is available, otherwise "false"</returns>
        bool TryGetLobbyByLobbyCode(string lobbyCode, out ILobby lobby);

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
        /// Gets a game resources of type
        /// </summary>
        /// <typeparam name="T">Game resource type</typeparam>
        /// <returns>Game resource if available, otherwise "null"</returns>
        IGameResource GetGameResourceOfType<T>() where T : IGameResource;

        /// <summary>
        /// Tries to get a game resource by type
        /// </summary>
        /// <typeparam name="T">Game resource type</typeparam>
        /// <param name="gameResource">Game resource</param>
        /// <returns>"true" if game resource is available, otherwise "false"</returns>
        bool TryGetGameResourceOfType<T>(out T gameResource) where T : IGameResource;

        /// <summary>
        /// Is game mode available
        /// </summary>
        /// <param name="gameMode">Game mode</param>
        /// <returns>"true" if game mode is available, otherwise "false"</returns>
        bool IsGameModeAvailable(string gameMode);

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
        /// Removes server lobby
        /// </summary>
        /// <param name="serverLobby">Server lobby</param>
        /// <returns>"true" if server lobby was successfully removed, otherwise "false"</returns>
        bool RemoveServerLobby(IServerLobby serverLobby);

        /// <summary>
        /// Processes all events appeared since last call
        /// </summary>
        void ProcessEvents();
    }
}
