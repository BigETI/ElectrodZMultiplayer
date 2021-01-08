using System.Collections.Generic;
using System.Drawing;

/// <summary>
/// ElectrodZ multiplayer client namespace
/// </summary>
namespace ElectrodZMultiplayer.Client
{
    /// <summary>
    /// An interface that represents internally a generalized synchronizer specific to a client
    /// </summary>
    internal interface IInternalClientSynchronizer : IClientSynchronizer
    {
        /// <summary>
        /// Sends a message to peer
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="message">Message</param>
        void SendMessage<T>(T message) where T : IBaseMessageData;

        /// <summary>
        /// Sends a authentication message to peer
        /// </summary>
        void SendAuthenticationMessage();

        /// <summary>
        /// Sends a authentication message to peer
        /// </summary>
        /// <param name="token">Existing authentication token</param>
        void SendAuthenticationMessage(string token);

        /// <summary>
        /// Sends a list lobbies message to peer
        /// </summary>
        /// <param name="excludeFull">Exclude full lobbies</param>
        /// <param name="name">Lobby name</param>
        /// <param name="minimalUserCount">Minimal user count</param>
        /// <param name="maximalUserCount">Maximal user count</param>
        /// <param name="isStartingGameAutomatically">Is starting game automatically</param>
        /// <param name="gameMode">Game mode</param>
        /// <param name="gameModeRules">Game mode rules</param>
        void SendListLobbiesMessage(bool? excludeFull = null, string name = null, uint? minimalUserCount = null, uint? maximalUserCount = null, bool? isStartingGameAutomatically = null, string gameMode = null, IReadOnlyDictionary<string, object> gameModeRules = null);

        /// <summary>
        /// Sends a join lobby message to peer
        /// </summary>
        /// <param name="lobbyCode">Lobby code</param>
        /// <param name="username">Username</param>
        void SendJoinLobbyMessage(string lobbyCode, string username);

        /// <summary>
        /// Sends a create and join lobby message to peer
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="lobbyName">Lobby name</param>
        /// <param name="minimalUserCount">Minimal user count</param>
        /// <param name="maximalUserCount">Maximal user count</param>
        /// <param name="isStartingGameAutomatically">Is starting game automatically</param>
        /// <param name="gameMode">Game mode</param>
        /// <param name="gameModeRules">Game mode rules</param>
        void SendCreateAndJoinLobbyMessage(string username, string lobbyName, uint? minimalUserCount = null, uint? maximalUserCount = null, bool? isStartingGameAutomatically = null, string gameMode = null, IReadOnlyDictionary<string, object> gameModeRules = null);

        /// <summary>
        /// Sends a quit lobbym message to peer
        /// </summary>
        void SendQuitLobbyMessage();

        /// <summary>
        /// Sends a change username message to peer
        /// </summary>
        /// <param name="newUsername">New username</param>
        void SendChangeUsernameMessage(string newUsername);

        /// <summary>
        /// Send change game color message
        /// </summary>
        /// <param name="gameColor">Game color</param>
        void SendChangeGameColorMessage(EGameColor gameColor);

        /// <summary>
        /// Send change lobby color message
        /// </summary>
        /// <param name="lobbyColor">Lobby color</param>
        void SendChangeLobbyColorMessage(Color lobbyColor);

        /// <summary>
        /// Sends a change lobby rules message to peer
        /// </summary>
        /// <param name="name">Lobby name</param>
        /// <param name="minimalUserCount">Minimal user count</param>
        /// <param name="maximalUserCount">Maximal user count</param>
        /// <param name="isStartingGameAutomatically">Is starting game automatically</param>
        /// <param name="gameMode">Game mode</param>
        /// <param name="gameModeRules">Game mode rules</param>
        void SendChangeLobbyRules(string name = null, uint? minimalUserCount = null, uint? maximalUserCount = null, bool? isStartingGameAutomatically = null, string gameMode = null, IReadOnlyDictionary<string, object> gameModeRules = null);

        /// <summary>
        /// Sends a kick user message to peer
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="reason">Reason</param>
        void SendKickUserMessage(IUser user, string reason);

        /// <summary>
        /// Sends a start game message to peer
        /// </summary>
        /// <param name="time">Time to start game in seconds</param>
        void SendStartGameMessage(float time);

        /// <summary>
        /// Sends a restart game message to peer
        /// </summary>
        /// <param name="time">Time to restart game in seconds</param>
        void SendRestartGameMessage(float time);

        /// <summary>
        /// Sends a stop game message to peer
        /// </summary>
        /// <param name="time">Time to stop game in seconds</param>
        void SendStopGameMessage(float time);

        /// <summary>
        /// Send client tick message
        /// </summary>
        /// <param name="color">Game color</param>
        /// <param name="position">Position</param>
        /// <param name="rotation">Rotation</param>
        /// <param name="velocity">Velocity</param>
        void SendClientTickMessage(EGameColor color, Vector3<float>? position = null, Quaternion<float>? rotation = null, Vector3<float>? velocity = null, IEnumerable<EGameAction> actions = null);

        /// <summary>
        /// Sends an error message to peer
        /// </summary>
        /// <param name="errorType">Error type</param>
        /// <param name="errorMessage">Error message</param>
        void SendErrorMessage(EErrorType errorType, string errorMessage);

        /// <summary>
        /// Sends an error message to peer
        /// </summary>
        /// <param name="errorType"></param>
        /// <param name="errorMessage"></param>
        void SendErrorMessage(EErrorType errorType, string errorMessage, bool isFatal);
    }
}
