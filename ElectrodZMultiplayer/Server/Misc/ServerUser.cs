using ElectrodZMultiplayer.Data.Messages;
using System;
using System.Collections.Generic;
using System.Drawing;

/// <summary>
/// ElectrodZ multiplayer server namespace
/// </summary>
namespace ElectrodZMultiplayer.Server
{
    /// <summary>
    /// Network server user class
    /// </summary>
    internal class ServerUser : Entity, IInternalServerUser
    {
        /// <summary>
        /// Peer
        /// </summary>
        public IPeer Peer { get; private set; }

        /// <summary>
        /// Server
        /// </summary>
        public IServerSynchronizer Server { get; }

        /// <summary>
        /// Server lobby
        /// </summary>
        public IInternalServerLobby ServerLobby { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; }

        /// <summary>
        /// Lobby
        /// </summary>
        public ILobby Lobby { get; private set; }

        /// <summary>
        /// Username
        /// </summary>
        public string Name { get; private set; } = string.Empty;

        /// <summary>
        /// Lobby color
        /// </summary>
        public Color LobbyColor { get; private set; }

        /// <summary>
        /// Score
        /// </summary>
        public long Score { get; private set; }

        /// <summary>
        /// Constructs an server user
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="server">Server</param>
        /// <param name="token">Token</param>
        public ServerUser(IPeer peer, IServerSynchronizer server, string token) : base((peer == null) ? Guid.Empty : peer.GUID, EGameColor.Default)
        {
            Peer = peer ?? throw new ArgumentNullException(nameof(peer));
            Server = server ?? throw new ArgumentNullException(nameof(server));
            Token = token ?? throw new ArgumentNullException(nameof(token));
        }

        /// <summary>
        /// Updates username
        /// </summary>
        /// <param name="name">Username</param>
        public void UpdateName(string name)
        {
            SetNameInternally(name);
            ServerLobby.SendUsernameChangedMessage(this);
        }

        /// <summary>
        /// Updates lobby color
        /// </summary>
        /// <param name="color">Lobby color</param>
        public void UpdateLobbyColor(Color lobbyColor)
        {
            SetLobbyColorInternally(lobbyColor);
            ServerLobby.SendUserLobbyColorChangedMessage(this);
        }

        /// <summary>
        /// Sets a new peer
        /// </summary>
        /// <param name="peer">Peer</param>
        public void SetPeerInternally(IPeer peer)
        {
            if (peer == null)
            {
                throw new ArgumentNullException(nameof(peer));
            }
            if (!peer.IsValid)
            {
                throw new ArgumentException("Peer is not valid.", nameof(peer));
            }
            Peer = peer;
            SetGUIDInternally(peer.GUID);
        }

        /// <summary>
        /// Sets a new username
        /// </summary>
        /// <param name="name">Username</param>
        public void SetNameInternally(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            string new_name = name.Trim();
            if ((new_name.Length < Defaults.minimalUsernameLength) || (new_name.Length > Defaults.maximalUsernameLength))
            {
                throw new ArgumentException($"Username must be between { Defaults.minimalUsernameLength } and { Defaults.maximalUsernameLength } characters long.", nameof(name));
            }
            Name = new_name;
        }

        /// <summary>
        /// Sets a new lobby color
        /// </summary>
        /// <param name="lobbyColor">Lobby color</param>
        public void SetLobbyColorInternally(Color lobbyColor) => LobbyColor = lobbyColor;

        /// <summary>
        /// Sends a message
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="message">Message</param>
        public void SendMessage<T>(T message) where T : IBaseMessageData => Server.SendMessageToPeer(Peer, message);

        /// <summary>
        /// Sends an authentication acknowledged message
        /// </summary>
        public void SendAuthenticationAcknowledgedMessage() => SendMessage(new AuthenticationAcknowledgedMessageData(GUID, Token));

        /// <summary>
        /// Sends a join lobby acknowledged message
        /// </summary>
        /// <param name="lobby">Lobby</param>
        public void SendJoinLobbyAcknowledgedMessage(IServerLobby lobby) => SendMessage(new JoinLobbyAcknowledgedMessageData(lobby));

        /// <summary>
        /// Sends a list lobby results message
        /// </summary>
        /// <param name="lobbies">Lobbies</param>
        public void SendListLobbyResultsMessage(IEnumerable<ILobbyView> lobbies) => SendMessage(new ListLobbyResultsMessageData(lobbies));

        /// <summary>
        /// Sends an error message
        /// </summary>
        /// <param name="errorType">Error type</param>
        /// <param name="message">Error message</param>
        public void SendErrorMessage(EErrorType errorType, string message) => SendErrorMessage(errorType, message, false);

        /// <summary>
        /// Sends an error message
        /// </summary>
        /// <param name="errorType">Error type</param>
        /// <param name="message">Error message</param>
        /// <param name="isFatal">Is error fatal</param>
        public void SendErrorMessage(EErrorType errorType, string message, bool isFatal)
        {
            SendMessage(new ErrorMessageData(errorType, message));
            if (isFatal)
            {
                Peer.Disconnect(EDisconnectionReason.Error);
            }
        }

        /// <summary>
        /// Disconnects an user
        /// </summary>
        /// <param name="reason">Reason</param>
        public void Disconnect(EDisconnectionReason reason) => Peer.Disconnect(reason);

        /// <summary>
        /// Bans user
        /// </summary>
        /// <param name="reason">Ban reason</param>
        public void Ban(string reason)
        {
            if (reason == null)
            {
                throw new ArgumentNullException(nameof(reason));
            }
            if (Lobby is IServerLobby server_lobby)
            {
                server_lobby.Server.Bans.AddPeer(Peer, reason);
            }
            Disconnect(EDisconnectionReason.Banned);
        }
    }
}
