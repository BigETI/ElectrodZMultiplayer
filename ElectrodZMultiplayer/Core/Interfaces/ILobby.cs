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
        /// This event will be invoked when the lobby rules of this lobby has been updated.
        /// </summary>
        event LobbyRulesUpdatedDelegate OnLobbyRulesUpdated;

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
        /// This event will be invoked when the game has ended.
        /// </summary>
        event GameEndedDelegate OnGameEnded;
    }
}
