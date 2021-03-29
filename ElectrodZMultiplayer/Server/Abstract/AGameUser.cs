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
        /// Is spectating
        /// </summary>
        public virtual bool IsSpectating => ServerUser.IsSpectating;

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
        public virtual IEnumerable<string> Actions => ServerUser.Actions;

        /// <summary>
        /// Is resynchronization requested
        /// </summary>
        public virtual bool IsResyncRequested => ServerUser.IsResyncRequested;

        /// <summary>
        /// Is client game color set
        /// </summary>
        public bool IsClientGameColorSet => ServerUser.IsClientGameColorSet;

        /// <summary>
        /// Is client spectating state set
        /// </summary>
        public bool IsClientSpectatingStateSet => ServerUser.IsClientSpectatingStateSet;

        /// <summary>
        /// Is client position set
        /// </summary>
        public bool IsClientPositionSet => ServerUser.IsClientPositionSet;

        /// <summary>
        /// Is client rotation set
        /// </summary>
        public bool IsClientRotationSet => ServerUser.IsClientRotationSet;

        /// <summary>
        /// Is client velocity set
        /// </summary>
        public bool IsClientVelocitySet => ServerUser.IsClientVelocitySet;

        /// <summary>
        /// Is client angular velocity set
        /// </summary>
        public bool IsClientAngularVelocitySet => ServerUser.IsClientAngularVelocitySet;

        /// <summary>
        /// Are client game actions set
        /// </summary>
        public bool AreClientActionsSet => ServerUser.AreClientActionsSet;

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
            serverUser.OnGameLoadingFinished += () => OnGameLoadingFinished?.Invoke();
            serverUser.OnClientTicked += (entityDeltas, hits) => OnClientTicked?.Invoke(entityDeltas, hits);
            serverUser.OnServerTicked += (time, entityDeltas, hits) => OnServerTicked?.Invoke(time, entityDeltas, hits);
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
        /// Sets the new game color
        /// </summary>
        /// <param name="newGameColor">New game entity game color</param>
        /// <param name="isValueFromClient">Is value from client</param>
        public virtual void SetGameColor(EGameColor newGameColor, bool isValueFromClient) => ServerUser.SetGameColor(newGameColor, isValueFromClient);

        /// <summary>
        /// Sets the new spectating state
        /// </summary>
        /// <param name="newSpectatingState">New spectating state</param>
        public virtual void SetSpectatingState(bool newSpectatingState) => ServerUser.SetSpectatingState(newSpectatingState);

        /// <summary>
        /// Sets the new spectating state
        /// </summary>
        /// <param name="newSpectatingState">New spectating state</param>
        /// <param name="isValueFromClient">Is value from client</param>
        public virtual void SetSpectatingState(bool newSpectatingState, bool isValueFromClient) => ServerUser.SetSpectatingState(newSpectatingState, isValueFromClient);

        /// <summary>
        /// Set the new position
        /// </summary>
        /// <param name="newPosition">New position</param>
        public virtual void SetPosition(Vector3 newPosition) => ServerUser.SetPosition(newPosition);

        /// <summary>
        /// Sets the new position
        /// </summary>
        /// <param name="newPosition">New game entity position</param>
        /// <param name="isValueFromClient">Is value from client</param>
        public virtual void SetPosition(Vector3 newPosition, bool isValueFromClient) => ServerUser.SetPosition(newPosition, isValueFromClient);

        /// <summary>
        /// Sets the new rotation
        /// </summary>
        /// <param name="newRotation">New rotation</param>
        public virtual void SetRotation(Quaternion newRotation) => ServerUser.SetRotation(newRotation);

        /// <summary>
        /// Sets the new rotation
        /// </summary>
        /// <param name="newRotation">New game entity rotation</param>
        /// <param name="isValueFromClient">Is value from client</param>
        public virtual void SetRotation(Quaternion newRotation, bool isValueFromClient) => ServerUser.SetRotation(newRotation, isValueFromClient);

        /// <summary>
        /// Sets the new velocity
        /// </summary>
        /// <param name="newVelocity">New velocity</param>
        public virtual void SetVelocity(Vector3 newVelocity) => ServerUser.SetVelocity(newVelocity);

        /// <summary>
        /// Sets the new velocity
        /// </summary>
        /// <param name="newVelocity">New game entity velocity</param>
        /// <param name="isValueFromClient">Is value from client</param>
        public virtual void SetVelocity(Vector3 newVelocity, bool isValueFromClient) => ServerUser.SetVelocity(newVelocity, isValueFromClient);

        /// <summary>
        /// Sets the new angular velocity
        /// </summary>
        /// <param name="newAngularVelocity">New angular velocity</param>
        public virtual void SetAngularVelocity(Vector3 newAngularVelocity) => ServerUser.SetAngularVelocity(newAngularVelocity);

        /// <summary>
        /// Sets the new angular velocity of game entity
        /// </summary>
        /// <param name="newAngularVelocity">New game entity angular velocity</param>
        /// <param name="isValueFromClient">Is value from client</param>
        public virtual void SetAngularVelocity(Vector3 newAngularVelocity, bool isValueFromClient) => ServerUser.SetAngularVelocity(newAngularVelocity, isValueFromClient);

        /// <summary>
        /// Sets the new game actions
        /// </summary>
        /// <param name="newActions">New game actions</param>
        /// <returns>Number of game actions set</returns>
        public virtual uint SetActions(IEnumerable<string> newActions) => ServerUser.SetActions(newActions);

        /// <summary>
        /// Sets the new game actions of game entity
        /// </summary>
        /// <param name="newActions">New game entity game actions</param>
        /// <param name="isValueFromClient">Is value from client</param>
        /// <returns>Number of actions set</returns>
        public virtual uint SetActions(IEnumerable<string> newActions, bool isValueFromClient) => ServerUser.SetActions(newActions, isValueFromClient);
    }
}
