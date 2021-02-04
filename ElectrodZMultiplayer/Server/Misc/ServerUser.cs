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
        /// Last entity deltas
        /// </summary>
        private IList<IEntityDelta> lastEntityDeltas = new List<IEntityDelta>();

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
        /// Entity streamer
        /// </summary>
        public IEntityStreamer EntityStreamer { get; } = new EntityStreamer();

        /// <summary>
        /// Lobby
        /// </summary>
        public ILobby Lobby => ServerLobby;

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
        /// This event will be invoked when the username changes.
        /// </summary>
        public event UsernameUpdatedDelegate OnUsernameUpdated;

        /// <summary>
        /// This event will be invoked when the user lobby color changes.
        /// </summary>
        public event UserLobbyColorUpdatedDelegate OnUserLobbyColorUpdated;

        /// <summary>
        /// This event will be invoked when the user finished loading their game.
        /// </summary>
        public event GameLoadingFinishedDelegate OnGameLoadingFinished;

        /// <summary>
        /// This event will be invoked when a client tick has been performed.
        /// </summary>
        public event ClientTickedDelegate OnClientTicked;

        /// <summary>
        /// This event will be invoked when a server tick has been performed.
        /// </summary>
        public event ServerTickedDelegate OnServerTicked;

        /// <summary>
        /// Constructs an server user
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="server">Server</param>
        /// <param name="token">Token</param>
        public ServerUser(IPeer peer, IServerSynchronizer server, string token) : base((peer == null) ? Guid.Empty : peer.GUID, Defaults.playerEntityType)
        {
            Peer = peer ?? throw new ArgumentNullException(nameof(peer));
            Server = server ?? throw new ArgumentNullException(nameof(server));
            Token = token ?? throw new ArgumentNullException(nameof(token));
        }

        /// <summary>
        /// Updates username
        /// </summary>
        /// <param name="name">Username</param>
        public void UpdateUsername(string name)
        {
            SetNameInternally(name);
            OnUsernameUpdated?.Invoke();
            ServerLobby.SendUsernameChangedMessage(this);
        }

        /// <summary>
        /// Updates user lobby color
        /// </summary>
        /// <param name="color">Lobby color</param>
        public void UpdateUserLobbyColor(Color lobbyColor)
        {
            SetLobbyColorInternally(lobbyColor);
            OnUserLobbyColorUpdated?.Invoke();
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
        /// Invokes the client game loading process finished event
        /// </summary>
        public void InvokeClientGameLoadingProcessFinishedEvent() => OnGameLoadingFinished?.Invoke();

        /// <summary>
        /// Invoked the client ticked event
        /// </summary>
        /// <param name="entityDeltas">Entity deltas</param>
        /// <param name="hits">Hits</param>
        public void InvokeClientTickedEvent(IEnumerable<IEntityDelta> entityDeltas, IEnumerable<IHit> hits) => OnClientTicked?.Invoke(entityDeltas, hits);

        /// <summary>
        /// Sends a message
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="message">Message</param>
        public void SendMessage<T>(T message) where T : IBaseMessageData => Server.SendMessageToPeer(Peer, message);

        /// <summary>
        /// Sends an authentification acknowledged message
        /// </summary>
        public void SendAuthentificationAcknowledgedMessage() => SendMessage(new AuthentificationAcknowledgedMessageData(GUID, Token));

        /// <summary>
        /// Sends a list lobby results message
        /// </summary>
        /// <param name="lobbies">Lobbies</param>
        public void SendListLobbyResultsMessage(IEnumerable<ILobbyView> lobbies) => SendMessage(new ListLobbyResultsMessageData(lobbies));

        /// <summary>
        /// Sends a list available game mode results message
        /// </summary>
        /// <param name="gameModes">Available game modes</param>
        public void SendListAvailableGameModeResultsMessage(IEnumerable<string> gameModes) => SendMessage(new ListAvailableGameModeResultsMessageData(gameModes));

        /// <summary>
        /// Sends a join lobby acknowledged message
        /// </summary>
        /// <param name="lobby">Lobby</param>
        public void SendJoinLobbyAcknowledgedMessage(IServerLobby lobby) => SendMessage(new JoinLobbyAcknowledgedMessageData(lobby));

        /// <summary>
        /// Sends a server tick message
        /// </summary>
        /// <param name="time">Time elapsed in seconds since game started</param>
        /// <param name="hits">Hits</param>
        public void SendServerTickMessage(double time, IEnumerable<IHit> hits)
        {
            if (!Protection.IsValid(hits))
            {
                throw new ArgumentException("Hits contain invalid hits.", nameof(hits));
            }
            OnServerTicked?.Invoke(time, EntityStreamer.GetEntityDeltas(Lobby.Users.Values, Lobby.Entities.Values, ref lastEntityDeltas), hits);
            SendMessage(new ServerTickMessageData(time, lastEntityDeltas, hits));
        }

        /// <summary>
        /// Sends an error message
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="errorType">Error type</param>
        /// <param name="message">Error message</param>
        public void SendErrorMessage<T>(EErrorType errorType, string message) where T : IBaseMessageData => SendErrorMessage<T>(errorType, message, false);

        /// <summary>
        /// Sends an error message
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="errorType">Error type</param>
        /// <param name="message">Error message</param>
        /// <param name="isFatal">Is error fatal</param>
        public void SendErrorMessage<T>(EErrorType errorType, string message, bool isFatal) where T : IBaseMessageData
        {
            SendMessage(new ErrorMessageData(errorType, Naming.GetMessageTypeNameFromMessageDataType<T>(), message));
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

        /// <summary>
        /// Sets the new game color
        /// </summary>
        /// <param name="newGameColor">New game color</param>
        public void SetGameColor(EGameColor newGameColor) => SetGameColorInternally(newGameColor);

        /// <summary>
        /// Sets the new position
        /// </summary>
        /// <param name="newPosition"></param>
        public void SetPosition(Vector3 newPosition) => SetPositionInternally(newPosition);

        /// <summary>
        /// Sets the new rotation
        /// </summary>
        /// <param name="newRotation"></param>
        public void SetRotation(Quaternion newRotation) => SetRotationInternally(newRotation);

        /// <summary>
        /// Sets the new velocity
        /// </summary>
        /// <param name="newVelocity"></param>
        public void SetVelocity(Vector3 newVelocity) => SetVelocityInternally(newVelocity);

        /// <summary>
        /// Sets the angular velocity
        /// </summary>
        /// <param name="newAngularVelocity"></param>
        public void SetAngularVelocity(Vector3 newAngularVelocity) => SetAngularVelocityInternally(newAngularVelocity);

        /// <summary>
        /// Sets the new game actions
        /// </summary>
        /// <param name="newActions">New game actions</param>
        /// <returns>Number of game actions set</returns>
        public uint SetActions(IEnumerable<string> newActions) => SetActionsInternally(newActions);
    }
}
