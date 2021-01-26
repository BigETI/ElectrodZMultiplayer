using ElectrodZMultiplayer;
using ElectrodZMultiplayer.Server;
using System;
using System.Collections.Generic;

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
        /// Users with results
        /// </summary>
        public IReadOnlyDictionary<string, UserWithResults> UserResults { get; } = new Dictionary<string, UserWithResults>();

        /// <summary>
        /// Results
        /// </summary>
        public IReadOnlyDictionary<string, object> Results { get; } = new Dictionary<string, object>();

        /// <summary>
        /// Game mode has been initialized
        /// </summary>
        /// <param name="gameResource">Game resource</param>
        /// <param name="serverLobby">Server lobby</param>
        public void OnInitialized(IGameResource gameResource, IServerLobby serverLobby) => Console.WriteLine("Example game mode has been initialized!");

        /// <summary>
        /// Game mode has been closed
        /// </summary>
        public void OnClosed() => Console.WriteLine("Example game mode has been closed!");

        /// <summary>
        /// User has joined the game
        /// </summary>
        /// <param name="gameUser">Game user</param>
        public void OnUserJoined(IGameUser gameUser) => Console.WriteLine($"User \"{ gameUser.Name }\" with GUID \"{ gameUser.GUID }\" has joined the game.");

        /// <summary>
        /// User has left the game
        /// </summary>
        /// <param name="gameUser">Game user</param>
        public void OnUserLeft(IGameUser gameUser) => Console.WriteLine($"User \"{ gameUser.Name }\" with GUID \"{ gameUser.GUID }\" has left the game.");

        /// <summary>
        /// User has been spawned
        /// </summary>
        /// <param name="gameUser">Game user</param>
        public void OnUserSpawned(IGameUser gameUser) => Console.WriteLine($"User \"{ gameUser.Name }\" with GUID \"{ gameUser.GUID }\" has been spawned.");

        /// <summary>
        /// User has been killed
        /// </summary>
        /// <param name="vistim">Victim</param>
        /// <param name="issuer">Issuer</param>
        public void OnUserKilled(IGameUser victim, IGameEntity issuer)
        {
            if (issuer == null)
            {
                Console.WriteLine($"Victim \"{ victim.Name }\" with GUID \"{ victim.GUID }\" has died.");
            }
            else
            {
                Console.WriteLine($"Victim \"{ victim.Name }\" with GUID \"{ victim.GUID }\" has been killed by entity with GUID \"{ issuer.GUID }\".");
            }
        }

        /// <summary>
        /// Game entity has been created
        /// </summary>
        /// <param name="gameEntity">Game entity</param>
        public void OnGameEntityCreated(IGameEntity gameEntity) => Console.WriteLine($"Game entity with GUID \"{ gameEntity.GUID }\" has been created.");

        /// <summary>
        /// Game entity has been destroyed
        /// </summary>
        /// <param name="gameEntity">Game entity</param>
        public void OnGameEntityDestroyed(IGameEntity gameEntity) => Console.WriteLine($"Game entity with GUID \"{ gameEntity.GUID }\" has been destroyed.");

        /// <summary>
        /// Game has been ticked
        /// </summary>
        /// <param name="deltaTime">Delta time</param>
        public void OnGameTicked(TimeSpan deltaTime)
        {
            //Console.WriteLine($"Game has ticked with \"{ deltaTime.TotalMilliseconds }\" delta time in ms.");
        }
    }
}
