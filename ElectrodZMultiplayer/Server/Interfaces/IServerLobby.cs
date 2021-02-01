using System;
using System.Collections.Generic;

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
        /// Currently loaded game mode
        /// </summary>
        IGameMode CurrentlyLoadedGameMode { get; }

        /// <summary>
        /// Gets invoked when a game mode has been started
        /// </summary>
        event GameModeStartedDelegate OnGameModeStarted;

        /// <summary>
        /// Gets invoked when a game mode has been stopped
        /// </summary>
        event GameModeStoppedDelegate OnGameModeStopped;

        /// <summary>
        /// Gets invoked when lobby has been closed
        /// </summary>
        event LobbyClosedDelegate OnLobbyClosed;

        /// <summary>
        /// Removes the specified user
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="reason">Reason</param>
        /// <param name="message">Message</param>
        /// <returns>"true" if the specified user has been successfully removed, otherwise "false"</returns>
        bool RemoveUser(IUser user, EDisconnectionReason reason, string message);

        /// <summary>
        /// Updates the lobby rules
        /// </summary>
        /// <param name="newName">New lobby name</param>
        /// <param name="newGameMode">New game mode</param>
        /// <param name="newMinimalUserCount">New minimal user count</param>
        /// <param name="newMaximalUserCount">New maximal user count</param>
        /// <param name="newStartingGameAutomaticallyState">New starting game automatically state</param>
        /// <param name="newGameModeRules">New game mode ruels</param>
        void UpdateLobbyRules(string newName = null, (IGameResource, Type)? newGameMode = null, uint? newMinimalUserCount = null, uint? newMaximalUserCount = null, bool? newStartingGameAutomaticallyState = null, IReadOnlyDictionary<string, object> newGameModeRules = null);

        /// <summary>
        /// Cancels start game time
        /// </summary>
        void CancelStartGameTime();

        /// <summary>
        /// Cancels stop game time
        /// </summary>
        void CancelStopGameTime();

        /// <summary>
        /// Creates a new game entity
        /// </summary>
        /// <param name="entityType">Game entity type</param>
        /// <param name="gameColor">Game entity game color (optional)</param>
        /// <param name="position">Game entity position (optional)</param>
        /// <param name="rotation">Game entity rotation (optional)</param>
        /// <param name="velocity">Game entity velocity (optional)</param>
        /// <param name="angularVelocity">Game entity angular valocity (optional)</param>
        /// <param name="actions">Game entity game actions (optional)</param>
        /// <returns>Game entity</returns>
        IGameEntity CreateNewGameEntity(string entityType, EGameColor? gameColor = null, Vector3? position = null, Quaternion? rotation = null, Vector3? velocity = null, Vector3? angularVelocity = null, IEnumerable<EGameAction> actions = null);

        /// <summary>
        /// Removes the specified entity
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>"true" if the specified entity has been successfully removed, otherwise "false"</returns>
        bool RemoveEntity(IEntity entity);

        /// <summary>
        /// Performs a hit
        /// </summary>
        /// <param name="hit">Hit</param>
        void PerformHit(IHit hit);

        /// <summary>
        /// Close lobby
        /// </summary>
        void Close();

        /// <summary>
        /// Closes lobby
        /// </summary>
        /// <param name="reason">Reason</param>
        void Close(EDisconnectionReason reason);
    }
}
