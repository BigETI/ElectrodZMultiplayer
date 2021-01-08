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
        /// Internal users
        /// </summary>
        Dictionary<string, IUser> InternalUsers { get; }

        /// <summary>
        /// Entities
        /// </summary>
        Dictionary<string, IEntity> InternalEntities { get; }

        /// <summary>
        /// Internal game mode rules
        /// </summary>
        Dictionary<string, object> InternalGameModeRules { get; }

        /// <summary>
        /// Sets a new lobby code internally
        /// </summary>
        /// <param name="lobbyCode">Lobby code</param>
        void SetLobbyCodeInternally(string lobbyCode);

        /// <summary>
        /// Sets a new lobby name internally
        /// </summary>
        /// <param name="name">Lobby name</param>
        void SetNameInternally(string name);

        /// <summary>
        /// Sets a new minimal user count internally
        /// </summary>
        /// <param name="minimalUserCount">Minimal user count</param>
        void SetMinimalUserCountInternally(uint minimalUserCount);

        /// <summary>
        /// Sets a new maximal user count internally
        /// </summary>
        /// <param name="maximalUserCount">Maximal user count</param>
        void SetMaximalUserCountInternally(uint maximalUserCount);

        /// <summary>
        /// Sets a new starting game automatically state internally
        /// </summary>
        /// <param name="isStartingGameAutomatically">Is starting game automatically</param>
        void SetStartingGameAutomaticallyStateInternally(bool isStartingGameAutomatically);

        /// <summary>
        /// Sets a new game mode internally
        /// </summary>
        /// <param name="gameMode">Game mode</param>
        void SetGameModeInternally(string gameMode);
    }
}
