using ElectrodZMultiplayer.Data;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer client namespace
/// </summary>
namespace ElectrodZMultiplayer.Client
{
    /// <summary>
    /// An interface that represents an internal lobby specific to a client
    /// </summary>
    internal interface IInternalClientLobby : IClientLobby
    {
        /// <summary>
        /// Updates game mode rules internally
        /// </summary>
        /// <param name="newLobbyCode">New lobby code</param>
        /// <param name="newName">New lobby name</param>
        /// <param name="newGameMode">New game mode</param>
        /// <param name="newMinimalUserCount">New minimal user count</param>
        /// <param name="newMaximalUserCount">New maximal user count</param>
        /// <param name="newStartingGameAutomaticallyState">New starting game automatically state</param>
        /// <param name="newGameModeRules">New game mode rules</param>
        void UpdateGameModeRulesInternally(string newLobbyCode, string newName, string newGameMode, uint newMinimalUserCount, uint newMaximalUserCount, bool newStartingGameAutomaticallyState, IReadOnlyDictionary<string, object> newGameModeRules);

        /// <summary>
        /// Adds a new user to the lobby
        /// </summary>
        /// <param name="user"></param>
        /// <returns>"true" if user was successfully added, otherwise "false"</returns>
        bool AddUserInternally(IUser user);

        /// <summary>
        /// Removes the specified user
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="reason">Reason</param>
        /// <returns>"true" if user was successfully removed, othewrwise </returns>
        bool RemoveUserInternally(IUser user, string reason);

        /// <summary>
        /// Invokes the game start requested event internally
        /// </summary>
        /// <param name="time">Time to start game in seconds</param>
        void InvokeGameStartRequestedEventInternally(double time);

        /// <summary>
        /// Invokes the game restart requested event internally
        /// </summary>
        /// <param name="time">Time to restart the game in seconds</param>
        void InvokeGameRestartRequestedEventInternally(double time);

        /// <summary>
        /// Invokes the game stop requested event internally
        /// </summary>
        /// <param name="time">Time to stop the game in seconds</param>
        void InvokeGameStopRequestedEventInternally(double time);

        /// <summary>
        /// Invokes the game started event internally
        /// </summary>
        void InvokeGameStartedEventInternally();

        /// <summary>
        /// Invokes the game restarted event internally
        /// </summary>
        void InvokeGameRestartedEventInternally();

        /// <summary>
        /// Invokes the game stopped event internally
        /// </summary>
        void InvokeGameStoppedEventInternally();

        /// <summary>
        /// Processes server tick internally
        /// </summary>
        /// <param name="time">Time in seconds elapsed since game start</param>
        /// <param name="entityDeltas">Entity deltas</param>
        void ProcessServerTickInternally(double time, IEnumerable<EntityData> entityDeltas);

        /// <summary>
        /// Invokes the game ended event internally
        /// </summary>
        /// <param name="users">Users</param>
        /// <param name="results">Results</param>
        void InvokeGameEndedEventInternally(IReadOnlyDictionary<string, UserWithResults> users, IReadOnlyDictionary<string, object> results);
    }
}
