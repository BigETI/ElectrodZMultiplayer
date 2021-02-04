using System;

/// <summary>
/// ElectrodZ multiplayer server namespace
/// </summary>
namespace ElectrodZMultiplayer.Server
{
    /// <summary>
    /// An interface that represents an internal server lobby
    /// </summary>
    internal interface IInternalServerLobby : IServerLobby
    {
        /// <summary>
        /// Remaining game start time in seconds
        /// </summary>
        double RemainingGameStartTime { get; set; }

        /// <summary>
        /// Remaining game stop time in seconds
        /// </summary>
        double RemainingGameStopTime { get; set; }

        /// <summary>
        /// Adds the specified user
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>"true" if the specified user has been successfully added, otherwise "false"</returns>
        bool AddUser(IInternalServerUser user);

        /// <summary>
        /// Performs a game tick
        /// </summary>
        /// <param name="deltaTime">Delta time</param>
        void Tick(TimeSpan deltaTime);

        /// <summary>
        /// Sets a new lobby name
        /// </summary>
        /// <param name="name">Lobby name</param>
        void SetNameInternally(string name);

        /// <summary>
        /// Sets a new minimal user count to start a game
        /// </summary>
        /// <param name="minimalUserCount"></param>
        void SetMinimalUserCountInternally(uint minimalUserCount);

        /// <summary>
        /// Sets a new maximal amount of users allowed in lobby
        /// </summary>
        /// <param name="maximalUserCount"></param>
        void SetMaximalUserCountInternally(uint maximalUserCount);

        /// <summary>
        /// Sets new minimal amount of users to start a game and maximal amount of users allowed in lobby
        /// </summary>
        /// <param name="minimalUserCount">Minimal user count</param>
        /// <param name="maximalUserCount">Maximal user count</param>
        void SetMinimalAndMaximalUserCountInternally(uint minimalUserCount, uint maximalUserCount);

        /// <summary>
        /// Sets a new starting game automatically state
        /// </summary>
        /// <param name="isStartingGameAutomatically">Is starting game automatically</param>
        void SetStartingGameAutomaticallyStateInternally(bool isStartingGameAutomatically);

        /// <summary>
        /// Sets a new game mode type
        /// </summary>
        /// <param name="gameModeType">Game mode type</param>
        void SetGameModeTypeInternally((IGameResource, Type) gameModeType);

        /// <summary>
        /// Adds a new game mode rule
        /// </summary>
        /// <param name="key">Game mode rule key</param>
        /// <param name="value">Game mode rule value</param>
        void AddGameModeRuleInternally(string key, object value);

        /// <summary>
        /// Removes the specified game mode rule by key
        /// </summary>
        /// <param name="key">Game mode rule key</param>
        /// <returns>"true" if game mode rule was successfully removed, otherwise "false"</returns>
        bool RemoveGameModeRuleInternally(string key);

        /// <summary>
        /// Clears all game mode rules
        /// </summary>
        void ClearGameModeRulesInternally();

        /// <summary>
        /// Adds a game user internally
        /// </summary>
        /// <param name="serverUser">Server user</param>
        /// <returns>"true" if game user was added successfully, otherwise "false"</returns>
        bool AddGameUserInternally(IServerUser serverUser);

        /// <summary>
        /// Sends a message to all
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="message">Message</param>
        void SendMessageToAll<T>(T message) where T : IBaseMessageData;

        /// <summary>
        /// Sends a lobby rules changed message
        /// </summary>
        void SendLobbyRulesChangedMessage();

        /// <summary>
        /// Sends an user joined message
        /// </summary>
        /// <param name="user"></param>
        void SendUserJoinedMessage(IUser user);

        /// <summary>
        /// Sends an user left message
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="reason">Reason</param>
        /// <param name="message">Message</param>
        void SendUserLeftMessage(IUser user, EDisconnectionReason reason, string message);

        /// <summary>
        /// Sends an username changed message
        /// </summary>
        /// <param name="user">User</param>
        void SendUsernameChangedMessage(IUser user);

        /// <summary>
        /// Sends an user lobby color changed message
        /// </summary>
        /// <param name="user">User</param>
        void SendUserLobbyColorChangedMessage(IUser user);

        /// <summary>
        /// Sends a game start requested message
        /// </summary>
        /// <param name="time">Time to start in seconds</param>
        void SendGameStartRequestedMessage(double time);

        /// <summary>
        /// Sends a restart game requested message
        /// </summary>
        /// <param name="time">Time to restart in seconds</param>
        void SendGameRestartRequestedMessage(double time);

        /// <summary>
        /// Sends a game stop requested message
        /// </summary>
        /// <param name="time">Time to stop game in seconds</param>
        void SendGameStopRequestedMessage(double time);
    }
}
