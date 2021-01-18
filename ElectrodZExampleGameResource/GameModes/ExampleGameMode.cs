using ElectrodZMultiplayer.Server;
using System;

/// <summary>
/// ElectrodZ example game resource game modes namespace
/// </summary>
namespace ElectrodZExampleGameResource.GameModes
{
    /// <summary>
    /// A class that describes an example game mode
    /// </summary>
    public class ExampleGameMode : IGameMode
    {
        /// <summary>
        /// Game mode has been initialized
        /// </summary>
        /// <param name="gameResource">Game resource</param>
        /// <param name="serverLobby">Server lobby</param>
        public void OnInitialized(IGameResource gameResource, IServerLobby serverLobby)
        {
            // TODO
        }

        /// <summary>
        /// Game mode has been closed
        /// </summary>
        public void OnClosed()
        {
            // TODO
        }

        /// <summary>
        /// User has joined the game
        /// </summary>
        /// <param name="gameUser">Game user</param>
        public void OnUserJoined(IGameUser gameUser)
        {
            // TODO
        }

        /// <summary>
        /// User has left the game
        /// </summary>
        /// <param name="gameUser">Game user</param>
        public void OnUserLeft(IGameUser gameUser)
        {
            // TODO
        }

        /// <summary>
        /// User has been spawned
        /// </summary>
        /// <param name="gameUser">Game user</param>
        public void OnUserSpawned(IGameUser gameUser)
        {
            // TODO
        }

        /// <summary>
        /// User has been killed
        /// </summary>
        /// <param name="vistim">Victim</param>
        /// <param name="issuer">Issuer</param>
        public void OnUserKilled(IGameUser victim, IGameEntity issuer)
        {
            // TODO
        }

        /// <summary>
        /// Game entity has been created
        /// </summary>
        /// <param name="gameEntity">Game entity</param>
        public void OnGameEntityCreated(IGameEntity gameEntity)
        {
            // TODO
        }

        /// <summary>
        /// Game entity has been destroyed
        /// </summary>
        /// <param name="gameEntity">Game entity</param>
        public void OnGameEntityDestroyed(IGameEntity gameEntity)
        {
            // TODO
        }

        /// <summary>
        /// Game has been ticked
        /// </summary>
        /// <param name="deltaTime">Delta time</param>
        public void OnGameTicked(TimeSpan deltaTime)
        {
            // TODO
        }
    }
}
