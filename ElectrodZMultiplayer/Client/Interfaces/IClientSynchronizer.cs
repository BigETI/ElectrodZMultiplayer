using System.Collections.Generic;
using System.Drawing;

/// <summary>
/// ElectrodZ multiplayer client interface
/// </summary>
namespace ElectrodZMultiplayer.Client
{
    /// <summary>
    /// An interface that represents a generalized connector specific to a client
    /// </summary>
    public interface IClientSynchronizer : ISynchronizer
    {
        /// <summary>
        /// Peer
        /// </summary>
        IPeer Peer { get; }

        /// <summary>
        /// Token
        /// </summary>
        string Token { get; }

        /// <summary>
        /// Client user
        /// </summary>
        IClientUser User { get; }

        /// <summary>
        /// Is peer connected
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Is user authentificated
        /// </summary>
        bool IsAuthentificated { get; }

        /// <summary>
        /// On lobby join acknowledged
        /// </summary>
        event LobbyJoinAcknowledgedDelegate OnLobbyJoinAcknowledged;

        /// <summary>
        /// Process events
        /// </summary>
        void ProcessEvents();

        /// <summary>
        /// Sends a request to create and join a lobby
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="lobbyName">Lobby name</param>
        /// <param name="isPrivate">Is lobby private</param>
        /// <param name="gameMode">Game mode</param>
        /// <param name="minimalUserCount">Minimal user count</param>
        /// <param name="maximalUserCount">Maximal user count</param>
        /// <param name="isStartingGameAutomatically">Is starting game automatically</param>
        /// <param name="gameModeRules">Game mode rules</param>
        void CreateAndJoinLobby(string username, string lobbyName, bool isPrivate, string gameMode, uint? minimalUserCount = null, uint? maximalUserCount = null, bool? isStartingGameAutomatically = null, IReadOnlyDictionary<string, object> gameModeRules = null);

        /// <summary>
        /// Joins a lobby with the specified lobby code
        /// </summary>
        /// <param name="lobbyCode">Lobby code</param>
        /// <param name="username">Username</param>
        void JoinLobby(string lobbyCode, string username);

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
        /// <param name="isPrivate">Is lobby private</param>
        /// <param name="gameMode">Game mode</param>
        /// <param name="minimalUserCount">Minimal user count</param>
        /// <param name="maximalUserCount">Maximal user count</param>
        /// <param name="isStartingGameAutomatically">Is starting game automatically</param>
        /// <param name="gameModeRules">Game mode rules</param>
        void SendCreateAndJoinLobbyMessage(string username, string lobbyName, bool isPrivate, string gameMode, uint? minimalUserCount = null, uint? maximalUserCount = null, bool? isStartingGameAutomatically = null, IReadOnlyDictionary<string, object> gameModeRules = null);

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
        /// Sends a list available game modes message
        /// </summary>
        /// <param name="name">Game mode name filter</param>
        void SendListAvailableGameModesMessage(string name);

        /// <summary>
        /// Send change lobby color message
        /// </summary>
        /// <param name="lobbyColor">Lobby color</param>
        void SendChangeLobbyColorMessage(Color lobbyColor);

        /// <summary>
        /// Sends a change lobby rules message to peer
        /// </summary>
        /// <param name="name">Lobby name (optional)</param>
        /// <param name="gameMode">Game mode (optional)</param>
        /// <param name="minimalUserCount">Minimal user count (optional)</param>
        /// <param name="maximalUserCount">Maximal user count (optional)</param>
        /// <param name="isStartingGameAutomatically">Is starting game automatically (optional)</param>
        /// <param name="gameModeRules">Game mode rules (optional)</param>
        void SendChangeLobbyRules(string name = null, string gameMode = null, uint? minimalUserCount = null, uint? maximalUserCount = null, bool? isStartingGameAutomatically = null, IReadOnlyDictionary<string, object> gameModeRules = null);

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
        void SendStartGameMessage(double time);

        /// <summary>
        /// Sends a restart game message to peer
        /// </summary>
        /// <param name="time">Time to restart game in seconds</param>
        void SendRestartGameMessage(double time);

        /// <summary>
        /// Sends a stop game message to peer
        /// </summary>
        /// <param name="time">Time to stop game in seconds</param>
        void SendStopGameMessage(double time);

        /// <summary>
        /// Sends a client tick message
        /// </summary>
        /// <param name="entities">Entities to update</param>
        void SendClientTickMessage(IEnumerable<IEntityDelta> entities = null);

        /// <summary>
        /// Sends an error message to peer
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="errorType">Error type</param>
        /// <param name="errorMessage">Error message</param>
        void SendErrorMessage<T>(EErrorType errorType, string errorMessage) where T : IBaseMessageData;

        /// <summary>
        /// Sends an error message to peer
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="errorType"></param>
        /// <param name="errorMessage"></param>
        void SendErrorMessage<T>(EErrorType errorType, string errorMessage, bool isFatal) where T : IBaseMessageData;
    }
}
