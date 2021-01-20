using System;
using System.Collections.Generic;
using System.Drawing;

/// <summary>
/// ElectrodZ multiplayer server namespace
/// </summary>
namespace ElectrodZMultiplayer.Server
{
    /// <summary>
    /// A class that describes a game user
    /// </summary>
    public abstract class AGameUser : IGameUser
    {
        /// <summary>
        /// Server user
        /// </summary>
        public IServerUser ServerUser { get; }

        /// <summary>
        /// Server entity
        /// </summary>
        public IServerEntity ServerEntity => ServerUser;

        /// <summary>
        /// User GUID
        /// </summary>
        public virtual Guid GUID => ServerUser.GUID;

        /// <summary>
        /// Peer
        /// </summary>
        public virtual IPeer Peer => ServerUser.Peer;

        /// <summary>
        /// Server
        /// </summary>
        public virtual IServerSynchronizer Server => ServerUser.Server;

        /// <summary>
        /// User token
        /// </summary>
        public virtual string Token => ServerUser.Token;

        /// <summary>
        /// Lobby
        /// </summary>
        public virtual ILobby Lobby => ServerUser.Lobby;

        /// <summary>
        /// Username
        /// </summary>
        public virtual string Name => ServerUser.Name;

        /// <summary>
        /// User lobby color
        /// </summary>
        public virtual Color LobbyColor => ServerUser.LobbyColor;

        /// <summary>
        /// Entity type
        /// </summary>
        public virtual string EntityType => ServerUser.EntityType;

        /// <summary>
        /// User game color
        /// </summary>
        public virtual EGameColor GameColor => ServerUser.GameColor;

        /// <summary>
        /// User position
        /// </summary>
        public virtual Vector3 Position => ServerUser.Position;

        /// <summary>
        /// User rotation
        /// </summary>
        public virtual Quaternion Rotation => ServerUser.Rotation;

        /// <summary>
        /// User velocity
        /// </summary>
        public virtual Vector3 Velocity => ServerUser.Velocity;

        /// <summary>
        /// User angular valocity
        /// </summary>
        public virtual Vector3 AngularVelocity => ServerUser.AngularVelocity;

        /// <summary>
        /// User actions
        /// </summary>
        public virtual IEnumerable<EGameAction> Actions => ServerUser.Actions;

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public virtual bool IsValid =>
            (ServerUser != null) &&
            ServerUser.IsValid;

        /// <summary>
        /// This event will be invoked when the username changes.
        /// </summary>
        public event UsernameUpdatedDelegate OnUsernameUpdated;

        /// <summary>
        /// This event will be invoked when the user lobby color changes.
        /// </summary>
        public event UserLobbyColorUpdatedDelegate OnUserLobbyColorUpdated;

        /// <summary>
        /// This event will be invoked when a client tick has been performed.
        /// </summary>
        public event ClientTickedDelegate OnClientTicked;

        /// <summary>
        /// This event will be invoked when a server tick has been performed.
        /// </summary>
        public event ServerTickedDelegate OnServerTicked;

        /// <summary>
        /// Constructs a game user
        /// </summary>
        /// <param name="serverUser">Server user</param>
        public AGameUser(IServerUser serverUser)
        {
            if (serverUser == null)
            {
                throw new ArgumentNullException(nameof(serverUser));
            }
            if (!serverUser.IsValid)
            {
                throw new ArgumentException("Server user is not valid.", nameof(serverUser));
            }
            ServerUser = serverUser;
            serverUser.OnUsernameUpdated += () => OnUsernameUpdated?.Invoke();
            serverUser.OnUserLobbyColorUpdated += () => OnUserLobbyColorUpdated?.Invoke();
            serverUser.OnClientTicked += (entityDeltas) => OnClientTicked?.Invoke(entityDeltas);
            serverUser.OnServerTicked += (time, entityDeltas) => OnServerTicked?.Invoke(time, entityDeltas);
        }

        /// <summary>
        /// Updates username
        /// </summary>
        /// <param name="name">Username</param>
        public virtual void UpdateUsername(string name) => ServerUser.UpdateUsername(name);

        /// <summary>
        /// Updates user lobby color
        /// </summary>
        /// <param name="lobbyColor">User lobby color</param>
        public virtual void UpdateUserLobbyColor(Color lobbyColor) => ServerUser.UpdateUserLobbyColor(lobbyColor);

        /// <summary>
        /// Disconnects this user
        /// </summary>
        /// <param name="reason">Disconnection reason</param>
        public virtual void Disconnect(EDisconnectionReason reason) => ServerUser.Disconnect(reason);

        /// <summary>
        /// Bans this user
        /// </summary>
        /// <param name="reason">Ban reason</param>
        public virtual void Ban(string reason) => ServerUser.Ban(reason);

        /// <summary>
        /// Sets the new game color
        /// </summary>
        /// <param name="newGameColor">New game color</param>
        public virtual void SetGameColor(EGameColor newGameColor) => ServerUser.SetGameColor(newGameColor);

        /// <summary>
        /// Set the new position
        /// </summary>
        /// <param name="newPosition">New position</param>
        public virtual void SetPosition(Vector3 newPosition) => ServerUser.SetPosition(newPosition);

        /// <summary>
        /// Sets the new rotation
        /// </summary>
        /// <param name="newRotation">New rotation</param>
        public virtual void SetRotation(Quaternion newRotation) => ServerUser.SetRotation(newRotation);

        /// <summary>
        /// Sets the new velocity
        /// </summary>
        /// <param name="newVelocity">New velocity</param>
        public virtual void SetVelocity(Vector3 newVelocity) => ServerUser.SetVelocity(newVelocity);

        /// <summary>
        /// Sets the new angular velocity
        /// </summary>
        /// <param name="newAngularVelocity">New angular velocity</param>
        public virtual void SetAngularVelocity(Vector3 newAngularVelocity) => ServerUser.SetAngularVelocity(newAngularVelocity);

        /// <summary>
        /// Sets the new game actions
        /// </summary>
        /// <param name="newActions">New game actions</param>
        /// <returns>Number of game actions set</returns>
        public virtual uint SetActions(IEnumerable<EGameAction> newActions) => ServerUser.SetActions(newActions);
    }
}
