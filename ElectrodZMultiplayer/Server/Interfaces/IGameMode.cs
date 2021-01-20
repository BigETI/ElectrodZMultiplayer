using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer server namespace
/// </summary>
namespace ElectrodZMultiplayer.Server
{
    /// <summary>
    /// Game mode interface
    /// </summary>
    public interface IGameMode
    {
        /// <summary>
        /// Users with results
        /// </summary>
        IReadOnlyDictionary<string, UserWithResults> UserResults { get; }

        /// <summary>
        /// Results
        /// </summary>
        IReadOnlyDictionary<string, object> Results { get; }

        /// <summary>
        /// Game mode has been initialized
        /// </summary>
        /// <param name="gameResource">Game resource</param>
        /// <param name="serverLobby">Server lobby</param>
        void OnInitialized(IGameResource gameResource, IServerLobby serverLobby);

        /// <summary>
        /// Game mode has been closed
        /// </summary>
        void OnClosed();

        /// <summary>
        /// User has joined the game
        /// </summary>
        /// <param name="gameUser">Game user</param>
        void OnUserJoined(IGameUser gameUser);

        /// <summary>
        /// User has left the game
        /// </summary>
        /// <param name="gameUser">Game user</param>
        void OnUserLeft(IGameUser gameUser);

        /// <summary>
        /// User has been spawned
        /// </summary>
        /// <param name="gameUser">Game user</param>
        void OnUserSpawned(IGameUser gameUser);

        /// <summary>
        /// User has been killed
        /// </summary>
        /// <param name="vistim">Victim</param>
        /// <param name="issuer">Issuer</param>
        void OnUserKilled(IGameUser victim, IGameEntity issuer);

        /// <summary>
        /// Game entity has been created
        /// </summary>
        /// <param name="gameEntity">Game entity</param>
        void OnGameEntityCreated(IGameEntity gameEntity);

        /// <summary>
        /// Game entity has been destroyed
        /// </summary>
        /// <param name="gameEntity">Game entity</param>
        void OnGameEntityDestroyed(IGameEntity gameEntity);

        /// <summary>
        /// Game has been ticked
        /// </summary>
        /// <param name="deltaTime">Delta time</param>
        void OnGameTicked(TimeSpan deltaTime);
    }
}
