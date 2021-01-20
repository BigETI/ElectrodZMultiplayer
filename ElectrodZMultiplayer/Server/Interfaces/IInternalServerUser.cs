using System.Collections.Generic;
using System.Drawing;

/// <summary>
/// ElectrodZ multiplayer server namespace
/// </summary>
namespace ElectrodZMultiplayer.Server
{
    /// <summary>
    /// An interface that represents an internal server user
    /// </summary>
    internal interface IInternalServerUser : IInternalEntity, IServerUser
    {
        /// <summary>
        /// Server lobby
        /// </summary>
        IInternalServerLobby ServerLobby { get; set; }

        /// <summary>
        /// Sets a new peer
        /// </summary>
        /// <param name="peer">Peer</param>
        void SetPeerInternally(IPeer peer);

        /// <summary>
        /// Sets a new username
        /// </summary>
        /// <param name="name">Username</param>
        void SetNameInternally(string name);

        /// <summary>
        /// Sets a new lobby color
        /// </summary>
        /// <param name="lobbyColor">Lobby color</param>
        void SetLobbyColorInternally(Color lobbyColor);

        /// <summary>
        /// Invoked the client ticked event
        /// </summary>
        /// <param name="entityDeltas">Entity deltas</param>
        void InvokeClientTickedEvent(IEnumerable<IEntityDelta> entityDeltas);

        /// <summary>
        /// Sends a message
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="message">Message</param>
        void SendMessage<T>(T message) where T : IBaseMessageData;

        /// <summary>
        /// Sends an authentication acknowledged message
        /// </summary>
        void SendAuthenticationAcknowledgedMessage();

        /// <summary>
        /// Sends a list lobby results message
        /// </summary>
        /// <param name="lobbies">Lobbies</param>
        void SendListLobbyResultsMessage(IEnumerable<ILobbyView> lobbies);

        /// <summary>
        /// Sends a list available game mode results message
        /// </summary>
        /// <param name="gameModes">Available game modes</param>
        void SendListAvailableGameModeResultsMessage(IEnumerable<string> gameModes);

        /// <summary>
        /// Sends a join lobby acknowledged message
        /// </summary>
        /// <param name="lobby">Lobby</param>
        void SendJoinLobbyAcknowledgedMessage(IServerLobby lobby);

        /// <summary>
        /// Sends a server tick message
        /// </summary>
        /// <param name="time">Time elapsed in seconds since game started</param>
        void SendServerTickMessage(double time);

        /// <summary>
        /// Sends an error message
        /// </summary>
        /// <param name="errorType">Error type</param>
        /// <param name="message">Error message</param>
        void SendErrorMessage(EErrorType errorType, string message);

        /// <summary>
        /// Sends an error message
        /// </summary>
        /// <param name="errorType">Error type</param>
        /// <param name="message">Error message</param>
        /// <param name="isFatal">Is error fatal</param>
        void SendErrorMessage(EErrorType errorType, string message, bool isFatal);
    }
}
