using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// An interface that represents a generalized synchronizer
    /// </summary>
    public interface ISynchronizer : IDisposable
    {
        /// <summary>
        /// Available connectors
        /// </summary>
        IEnumerable<IConnector> Connectors { get; }

        /// <summary>
        /// This event will be invoked when a peer has attempted to connect to any of the available connectors.
        /// </summary>
        event PeerConnectionAttemptedDelegate OnPeerConnectionAttempted;

        /// <summary>
        /// This event will be invoked when a peer has successfully connected to any of the available connectors.
        /// </summary>
        event PeerConnectedDelegate OnPeerConnected;

        /// <summary>
        /// This event will be invoked when a peer has been disconnected from any of the available connectors.
        /// </summary>
        event PeerDisconnectedDelegate OnPeerDisconnected;

        /// <summary>
        /// This event will be invoked when a message has been received from a peer.
        /// </summary>
        event PeerMessageReceivedDelegate OnPeerMessageReceived;

        /// <summary>
        /// This event will be invoked when a non-meaningful message has been received from a peer.
        /// </summary>
        event UnknownMessageReceivedDelegate OnUnknownMessageReceived;

        /// <summary>
        /// This event will be invoked when an error has been received.
        /// </summary>
        event ErrorMessageReceivedDelegate OnErrorMessageReceived;

        /// <summary>
        /// This event will be invoked when an authentification was acknowledged.
        /// </summary>
        event AuthentificationAcknowledgedDelegate OnAuthentificationAcknowledged;

        /// <summary>
        /// This event will be invoked when an authentification has failed.
        /// </summary>
        event AuthentificationFailedDelegate OnAuthentificationFailed;

        /// <summary>
        /// This event will be invoked when lobbies have been listed.
        /// </summary>
        event LobbiesListedDelegate OnLobbiesListed;

        /// <summary>
        /// This event will be invoked when listing lobbies has failed.
        /// </summary>
        event ListLobbiesFailedDelegate OnListLobbiesFailed;

        /// <summary>
        /// This event will be invoked when available game modes have been listed.
        /// </summary>
        event AvailableGameModesListedDelegate OnAvailableGameModesListed;

        /// <summary>
        /// This event will be invoked when listing available game modes has failed.
        /// </summary>
        event ListAvailableGameModesFailedDelegate OnListAvailableGameModesFailed;

        /// <summary>
        /// This event will be invoked when an user has joined this lobby.
        /// </summary>
        event LobbyUserJoinedDelegate OnUserJoined;

        /// <summary>
        /// This event will be invoked when joining a lobby has failed.
        /// </summary>
        event JoinLobbyFailedDelegate OnJoinLobbyFailed;

        /// <summary>
        /// This event will be invoked when creating a lobby has failed.
        /// </summary>
        event CreateLobbyFailedDelegate OnCreateLobbyFailed;

        /// <summary>
        /// This event will be invoked when an user left this lobby.
        /// </summary>
        event LobbyUserLeftDelegate OnUserLeft;

        /// <summary>
        /// This event will be invoked when the username changes.
        /// </summary>
        event UserUsernameUpdatedDelegate OnUsernameUpdated;

        /// <summary>
        /// This event will be invoked when changing username has failed.
        /// </summary>
        event ChangeUsernameFailedDelegate OnChangeUsernameFailed;

        /// <summary>
        /// This event will be invoked when the user lobby color changes.
        /// </summary>
        event UserUserLobbyColorUpdatedDelegate OnUserLobbyColorUpdated;

        /// <summary>
        /// This event will be invoked when changing user lobby color has failed.
        /// </summary>
        event ChangeUserLobbyColorFailedDelegate OnChangeUserLobbyColorFailed;

        /// <summary>
        /// This event will be invoked when the lobby owner of this lobby has been changed.
        /// </summary>
        event LobbyLobbyOwnershipChangedDelegate OnLobbyOwnershipChanged;

        /// <summary>
        /// This event will be invoked when the lobby rules of this lobby has been changed.
        /// </summary>
        event LobbyLobbyRulesChangedDelegate OnLobbyRulesChanged;

        /// <summary>
        /// This event will be invoked when changing lobby rules have failed.
        /// </summary>
        event ChangeLobbyRulesFailedDelegate OnChangeLobbyRulesFailed;

        /// <summary>
        /// This event will be invoked when kicking a user has failed.
        /// </summary>
        event KickUserFailedDelegate OnKickUserFailed;

        /// <summary>
        /// This event will be invoked when a game has been started.
        /// </summary>
        event LobbyGameStartedDelegate OnGameStarted;

        /// <summary>
        /// This event will be invoked when a game start has been requested.
        /// </summary>
        event LobbyGameStartRequestedDelegate OnGameStartRequested;

        /// <summary>
        /// This event will be invoked when starting a game has failed.
        /// </summary>
        event StartGameFailedDelegate OnStartGameFailed;

        /// <summary>
        /// This event will be invoked when a game has been restarted.
        /// </summary>
        event LobbyGameRestartedDelegate OnGameRestarted;

        /// <summary>
        /// This event will be invoked when a game restart has been requested.
        /// </summary>
        event LobbyGameRestartRequestedDelegate OnGameRestartRequested;

        /// <summary>
        /// This event will be invoked when restarting a game has failed.
        /// </summary>
        event RestartGameFailedDelegate OnRestartGameFailed;

        /// <summary>
        /// This event will be invoked when a game has been stopped.
        /// </summary>
        event LobbyGameStoppedDelegate OnGameStopped;

        /// <summary>
        /// This event will be invoked when a game stop has been requested.
        /// </summary>
        event LobbyGameStopRequestedDelegate OnGameStopRequested;

        /// <summary>
        /// This event will be invoked when stopping a game has failed.
        /// </summary>
        event StopGameFailedDelegate OnStopGameFailed;

        /// <summary>
        /// This event will be invoked when starting a game has been cancelled.
        /// </summary>
        event LobbyStartGameCancelledDelegate OnStartGameCancelled;

        /// <summary>
        /// This event will be invoked when restarting a game has been cancelled.
        /// </summary>
        event LobbyRestartGameCancelledDelegate OnRestartGameCancelled;

        /// <summary>
        /// This event will be invoked when stopping a game has been cancelled.
        /// </summary>
        event LobbyStopGameCancelledDelegate OnStopGameCancelled;

        /// <summary>
        /// This event will be invoked when cancelling a game start, restart or stop timer has failed.
        /// </summary>
        event CancelStartRestartStopGameTimerFailedDelegate OnCancelStartRestartStopGameTimerFailed;

        /// <summary>
        /// This event will be invoked when the user finished loading their game.
        /// </summary>
        event UserGameLoadingFinishedDelegate OnGameLoadingFinished;

        /// <summary>
        /// This event will be invoked when a client tick has been performed.
        /// </summary>
        event UserClientTickedDelegate OnClientTicked;

        /// <summary>
        /// This event will be invoked when a client tick has failed.
        /// </summary>
        event ClientTickFailedDelegate OnClientTickFailed;

        /// <summary>
        /// This event will be invoked when a server tick has been performed.
        /// </summary>
        event UserServerTickedDelegate OnServerTicked;

        /// <summary>
        /// This event will be invoked when a server tick has failed.
        /// </summary>
        event ServerTickFailedDelegate OnServerTickFailed;

        /// <summary>
        /// This event will be invoked when an user entity has been created.
        /// </summary>
        event LobbyUserEntityCreatedDelegate OnUserEntityCreated;

        /// <summary>
        /// This event will be invoked when an user entity has been updated.
        /// </summary>
        event LobbyUserEntityUpdatedDelegate OnUserEntityUpdated;

        /// <summary>
        /// This event will be invoked when an user entity has been destroyed.
        /// </summary>
        event LobbyUserEntityDestroyedDelegate OnUserEntityDestroyed;

        /// <summary>
        /// This event will be invoked when an entity has been created.
        /// </summary>
        event LobbyEntityCreatedDelegate OnEntityCreated;

        /// <summary>
        /// This event will be invoked when an entity has been updated.
        /// </summary>
        event LobbyEntityUpdatedDelegate OnEntityUpdated;

        /// <summary>
        /// This event will be invoked when an entity has been destroyed.
        /// </summary>
        event LobbyEntityDestroyedDelegate OnEntityDestroyed;

        /// <summary>
        /// Add connector
        /// </summary>
        /// <param name="connector">Connector</param>
        /// <returns>"true" if connector was successfully added, otherwise "false"</returns>
        bool AddConnector(IConnector connector);

        /// <summary>
        /// Remove connector
        /// </summary>
        /// <param name="connector">Connector</param>
        /// <returns>"true" if connector was successfully removed, otherwise "false"</returns>
        bool RemoveConnector(IConnector connector);

        /// <summary>
        /// Gets a connector with the specified type
        /// </summary>
        /// <typeparam name="T">Connector type</typeparam>
        /// <returns>Connector of specified type if successful, otherwise "null"</returns>
        T GetConnectorOfType<T>() where T : IConnector;

        /// <summary>
        /// Tries to get a connector of the specified type
        /// </summary>
        /// <typeparam name="T">Connector type</typeparam>
        /// <param name="connector">Connector</param>
        /// <returns>"true" if connector of the specified type is available, otherwise "false"</returns>
        bool TryGetConnectorOfType<T>(out T connector) where T : IConnector;

        /// <summary>
        /// Sends a message to peer
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="peer">Peer</param>
        /// <param name="message">Message</param>
        void SendMessageToPeer<T>(IPeer peer, T message) where T : IBaseMessageData;

        /// <summary>
        /// Adds a message parser
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="onMessageParsed">On message parsed</param>
        /// <param name="onMessageValidationFailed">On message validation failed</param>
        /// <param name="onMessageParseFailed">On message parse failed</param>
        /// <returns>Message parser</returns>
        IMessageParser<T> AddMessageParser<T>(MessageParsedDelegate<T> onMessageParsed, MessageValidationFailedDelegate<T> onMessageValidationFailed = null, MessageParseFailedDelegate onMessageParseFailed = null) where T : IBaseMessageData;

        /// <summary>
        /// Gets message parsers for the specified type
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <returns>Message parsers if successful, otherwise "null"</returns>
        IEnumerable<IMessageParser<T>> GetMessageParsersForType<T>() where T : IBaseMessageData;

        /// <summary>
        /// Tries to get message parsers for the specified type
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="messageParsers">Message parsers</param>
        /// <returns>"true" if message parsers are available, otherwise "false"</returns>
        bool TryGetMessageParsersForType<T>(out IEnumerable<IMessageParser<T>> messageParsers) where T : IBaseMessageData;

        /// <summary>
        /// Removes the specified message parser
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="messageParser">Message parser</param>
        /// <returns>"true" if message parser was successfully removed, otherwise "false"</returns>
        bool RemoveMessageParser<T>(IMessageParser<T> messageParser) where T : IBaseMessageData;

        /// <summary>
        /// Parses incoming message
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="json">JSON</param>
        void ParseMessage(IPeer peer, string json);

        /// <summary>
        /// Sends an invalid message parameters error message to peer
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="peer">Peer</param>
        /// <param name="errorMessage">Error message</param>
        void SendInvalidMessageParametersErrorMessageToPeer<T>(IPeer peer, string errorMessage) where T : IBaseMessageData;

        /// <summary>
        /// Sends an invalid message context error message to peer
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="peer">Peer</param>
        /// <param name="errorMessage">Error message</param>
        void SendInvalidMessageContextErrorMessageToPeer<T>(IPeer peer, string errorMessage) where T : IBaseMessageData;

        /// <summary>
        /// Sends an unknown error message to peer
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="peer">Peer</param>
        /// <param name="errorMessage">Error message</param>
        void SendUnknownErrorMessageToPeer<T>(IPeer peer, string errorMessage) where T : IBaseMessageData;

        /// <summary>
        /// Closes connections to all peers
        /// </summary>
        /// <param name="reason">Disconnection reason</param>
        void Close(EDisconnectionReason reason);
    }
}
