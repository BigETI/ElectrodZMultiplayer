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
        /// Is user authenticated
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// On acknowledge authentication message received
        /// </summary>
        event AuthenticationAcknowledgedDelegate OnAuthenticationAcknowledged;

        /// <summary>
        /// On lobbies listed
        /// </summary>
        event LobbiesListedDelegate OnLobbiesListed;

        /// <summary>
        /// On lobby join acknowledged
        /// </summary>
        event LobbyJoinAcknowledgedDelegate OnLobbyJoinAcknowledged;

        /// <summary>
        /// On user joined
        /// </summary>
        event UserJoinedDelegate OnUserJoined;

        /// <summary>
        /// On user left
        /// </summary>
        event UserLeftDelegate OnUserLeft;

        /// <summary>
        /// On lobby rules changed
        /// </summary>
        event LobbyRulesChangedDelegate OnLobbyRulesChanged;

        /// <summary>
        /// On username changed
        /// </summary>
        event UsernameChangedDelegate OnUsernameChanged;

        /// <summary>
        /// On user game color changed
        /// </summary>
        event UserGameColorChangedDelegate OnUserGameColorChanged;

        /// <summary>
        /// On user lobby color changed
        /// </summary>
        event UserGameColorChangedDelegate OnUserLobbyColorChanged;

        /// <summary>
        /// On game start requested
        /// </summary>
        event GameStartRequestedDelegate OnGameStartRequested;

        /// <summary>
        /// On game restart requested
        /// </summary>
        event GameRestartRequestedDelegate OnGameRestartRequested;

        /// <summary>
        /// On game stop requested
        /// </summary>
        event GameStopRequestedDelegate OnGameStopRequested;

        /// <summary>
        /// On game started
        /// </summary>
        event GameStartedDelegate OnGameStarted;

        /// <summary>
        /// On game restarted
        /// </summary>
        event GameRestartedDelegate OnGameRestarted;

        /// <summary>
        /// On game stopped
        /// </summary>
        event GameStoppedDelegate OnGameStopped;

        /// <summary>
        /// On server ticked
        /// </summary>
        event ServerTickedDelegate OnServerTicked;

        /// <summary>
        /// On game ended
        /// </summary>
        event GameEndedDelegate OnGameEnded;

        /// <summary>
        /// Process events
        /// </summary>
        void ProcessEvents();

        /// <summary>
        /// Sends a request to create and join a lobby
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="lobbyName">Lobby name</param>
        /// <param name="gameMode">Game mode</param>
        /// <param name="minimalUserCount">Minimal user count</param>
        /// <param name="maximalUserCount">Maximal user count</param>
        /// <param name="isStartingGameAutomatically">Is starting game automatically</param>
        /// <param name="gameModeRules">Game mode rules</param>
        void CreateAndJoinLobby(string username, string lobbyName, string gameMode, uint? minimalUserCount = null, uint? maximalUserCount = null, bool? isStartingGameAutomatically = null, IReadOnlyDictionary<string, object> gameModeRules = null);

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
        /// <param name="gameMode">Game mode</param>
        /// <param name="minimalUserCount">Minimal user count</param>
        /// <param name="maximalUserCount">Maximal user count</param>
        /// <param name="isStartingGameAutomatically">Is starting game automatically</param>
        /// <param name="gameModeRules">Game mode rules</param>
        void SendCreateAndJoinLobbyMessage(string username, string lobbyName, string gameMode, uint? minimalUserCount = null, uint? maximalUserCount = null, bool? isStartingGameAutomatically = null, IReadOnlyDictionary<string, object> gameModeRules = null);

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
        /// Sends a client tick message
        /// </summary>
        /// <param name="entities">Entities to update</param>
        void SendClientTickMessage(IEnumerable<IEntityDelta> entities = null);

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
