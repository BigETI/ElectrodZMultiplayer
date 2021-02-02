using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// An interface that represents a lobby
    /// </summary>
    public interface ILobby : ILobbyView, IDisposable
    {
        /// <summary>
        /// Lobby owner
        /// </summary>
        IUser Owner { get; }

        /// <summary>
        /// Users
        /// </summary>
        IReadOnlyDictionary<string, IUser> Users { get; }

        /// <summary>
        /// Entities
        /// </summary>
        IReadOnlyDictionary<string, IEntity> Entities { get; }

        /// <summary>
        /// Current game time in seconds
        /// </summary>
        double CurrentGameTime { get; }

        /// <summary>
        /// This event will be invoked when an user has joined this lobby.
        /// </summary>
        event UserJoinedDelegate OnUserJoined;

        /// <summary>
        /// This event will be invoked when an user left this lobby.
        /// </summary>
        event UserLeftDelegate OnUserLeft;

        /// <summary>
        /// This event will be invoked when the lobby owner of this lobby has been changed.
        /// </summary>
        event LobbyOwnershipChangedDelegate OnLobbyOwnershipChanged;

        /// <summary>
        /// This event will be invoked when the lobby rules of this lobby has been changed.
        /// </summary>
        event LobbyRulesChangedDelegate OnLobbyRulesChanged;

        /// <summary>
        /// This event will be invoked when a game start has been requested.
        /// </summary>
        event GameStartRequestedDelegate OnGameStartRequested;

        /// <summary>
        /// This event will be invoked when a game restart has been requested.
        /// </summary>
        event GameRestartRequestedDelegate OnGameRestartRequested;

        /// <summary>
        /// This event will be invoked when a game stop has been requested.
        /// </summary>
        event GameStopRequestedDelegate OnGameStopRequested;

        /// <summary>
        /// This event will be invoked when a game has been started.
        /// </summary>
        event GameStartedDelegate OnGameStarted;

        /// <summary>
        /// This event will be invoked when a game has been restarted.
        /// </summary>
        event GameRestartedDelegate OnGameRestarted;

        /// <summary>
        /// This event will be invoked when a game has been stopped.
        /// </summary>
        event GameStoppedDelegate OnGameStopped;

        /// <summary>
        /// This event will be invoked when starting a game has been cancelled.
        /// </summary>
        event StartGameCancelledDelegate OnStartGameCancelled;

        /// <summary>
        /// This event will be invoked when restarting a game has been cancelled.
        /// </summary>
        event RestartGameCancelledDelegate OnRestartGameCancelled;

        /// <summary>
        /// This event will be invoked when stopping a game has been cancelled.
        /// </summary>
        event StopGameCancelledDelegate OnStopGameCancelled;

        /// <summary>
        /// Gets user by GUID
        /// </summary>
        /// <param name="guid">User GUID</param>
        /// <returns>User if available, otherwise "null"</returns>
        IUser GetUserByGUID(Guid guid);

        /// <summary>
        /// Tries to get user by GUID
        /// </summary>
        /// <param name="guid">User GUID</param>
        /// <param name="user">User</param>
        /// <returns>"true" if user is available, otherwise "false"</returns>
        bool TryGetUserByGUID(Guid guid, out IUser user);

        /// <summary>
        /// Gets entity by GUID
        /// </summary>
        /// <param name="guid">Entity GUID</param>
        /// <returns>Entity if available, otherwise "null"</returns>
        IEntity GetEntityByGUID(Guid guid);

        /// <summary>
        /// Tries to get entity by GUID
        /// </summary>
        /// <param name="guid">Entity GUID</param>
        /// <param name="entity">Entity</param>
        /// <returns>"true" if entity exists, otherwise "false"</returns>
        bool TryGetEntityByGUID(Guid guid, out IEntity entity);

        /// <summary>
        /// Gets game mode rule
        /// </summary>
        /// <typeparam name="T">Game mode rule type</typeparam>
        /// <param name="key">Game mode rule key</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>Value if successful, otherwise the specified default value</returns>
        T GetGameModeRule<T>(string key, T defaultValue = default);

        /// <summary>
        /// Tries to get game mode rule
        /// </summary>
        /// <typeparam name="T">Game mode rule type</typeparam>
        /// <param name="key">Game mode rule key</param>
        /// <param name="value">Value</param>
        /// <returns>Value if successful, otherwise the specified default value</returns>
        bool TryGetGameModeRule<T>(string key, out T value);
    }
}
