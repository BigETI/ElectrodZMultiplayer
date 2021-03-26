using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer server namespace
/// </summary>
namespace ElectrodZMultiplayer.Server
{
    /// <summary>
    /// Game entity abstract class
    /// </summary>
    public abstract class AGameEntity : IGameEntity
    {
        /// <summary>
        /// Server entity
        /// </summary>
        public IServerEntity ServerEntity { get; }

        /// <summary>
        /// Game entity GUID
        /// </summary>
        public virtual Guid GUID => ServerEntity.GUID;

        /// <summary>
        /// Game entity type
        /// </summary>
        public virtual string EntityType => ServerEntity.EntityType;

        /// <summary>
        /// Game entity game color
        /// </summary>
        public virtual EGameColor GameColor => ServerEntity.GameColor;

        /// <summary>
        /// Game entity position
        /// </summary>
        public virtual Vector3 Position => ServerEntity.Position;

        /// <summary>
        /// Game entity rotation
        /// </summary>
        public virtual Quaternion Rotation => ServerEntity.Rotation;

        /// <summary>
        /// Game entity velocity
        /// </summary>
        public virtual Vector3 Velocity => ServerEntity.Velocity;

        /// <summary>
        /// Game entity angular velocity
        /// </summary>
        public virtual Vector3 AngularVelocity => ServerEntity.AngularVelocity;

        /// <summary>
        /// Game entity game actions
        /// </summary>
        public virtual IEnumerable<string> Actions => ServerEntity.Actions;

        /// <summary>
        /// Is resynchronization requested
        /// </summary>
        public bool IsResyncRequested => ServerEntity.IsResyncRequested;

        /// <summary>
        /// Is client game color set
        /// </summary>
        public bool IsClientGameColorSet => ServerEntity.IsClientGameColorSet;

        /// <summary>
        /// Is client position set
        /// </summary>
        public bool IsClientPositionSet => ServerEntity.IsClientPositionSet;

        /// <summary>
        /// Is client rotation set
        /// </summary>
        public bool IsClientRotationSet => ServerEntity.IsClientRotationSet;

        /// <summary>
        /// Is client velocity set
        /// </summary>
        public bool IsClientVelocitySet => ServerEntity.IsClientVelocitySet;

        /// <summary>
        /// Is client angular velocity set
        /// </summary>
        public bool IsClientAngularVelocitySet => ServerEntity.IsClientAngularVelocitySet;

        /// <summary>
        /// Are client game actions set
        /// </summary>
        public bool AreClientActionsSet => ServerEntity.AreClientActionsSet;

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public virtual bool IsValid =>
            (ServerEntity != null) &&
            ServerEntity.IsValid;

        /// <summary>
        /// Constructs a game entity
        /// </summary>
        /// <param name="serverEntity"></param>
        public AGameEntity(IServerEntity serverEntity)
        {
            if (serverEntity == null)
            {
                throw new ArgumentNullException(nameof(serverEntity));
            }
            if (!serverEntity.IsValid)
            {
                throw new ArgumentException("Server entity is not valid.", nameof(serverEntity));
            }
            ServerEntity = serverEntity;
        }

        /// <summary>
        /// Sets the new game color
        /// </summary>
        /// <param name="newGameColor">New game color</param>
        public virtual void SetGameColor(EGameColor newGameColor) => ServerEntity.SetGameColor(newGameColor);

        /// <summary>
        /// Sets the new game color of game entity
        /// </summary>
        /// <param name="newGameColor">New game entity game color</param>
        /// <param name="isValueFromClient">Is value from client</param>
        public virtual void SetGameColor(EGameColor newGameColor, bool isValueFromClient) => ServerEntity.SetGameColor(newGameColor, isValueFromClient);

        /// <summary>
        /// Sets the new position
        /// </summary>
        /// <param name="newPosition">New position</param>
        public virtual void SetPosition(Vector3 newPosition) => ServerEntity.SetPosition(newPosition);

        /// <summary>
        /// Sets the new position of game entity
        /// </summary>
        /// <param name="newPosition">New game entity position</param>
        /// <param name="isValueFromClient">Is value from client</param>
        public virtual void SetPosition(Vector3 newPosition, bool isValueFromClient) => ServerEntity.SetPosition(newPosition, isValueFromClient);

        /// <summary>
        /// Sets the new rotation
        /// </summary>
        /// <param name="newRotation">New rotation</param>
        public virtual void SetRotation(Quaternion newRotation) => ServerEntity.SetRotation(newRotation);

        /// <summary>
        /// Sets the new rotation of game entity
        /// </summary>
        /// <param name="newRotation">New game entity rotation</param>
        /// <param name="isValueFromClient">Is value from client</param>
        public virtual void SetRotation(Quaternion newRotation, bool isValueFromClient) => ServerEntity.SetRotation(newRotation, isValueFromClient);

        /// <summary>
        /// Sets the new velocity
        /// </summary>
        /// <param name="newVelocity">New velocity</param>
        public virtual void SetVelocity(Vector3 newVelocity) => ServerEntity.SetVelocity(newVelocity);

        /// <summary>
        /// Sets the new velocity of game entity
        /// </summary>
        /// <param name="newVelocity">New game entity velocity</param>
        /// <param name="isValueFromClient">Is value from client</param>
        public virtual void SetVelocity(Vector3 newVelocity, bool isValueFromClient) => ServerEntity.SetVelocity(newVelocity, isValueFromClient);

        /// <summary>
        /// Sets the new angular velocity
        /// </summary>
        /// <param name="newAngularVelocity">New angular velocity</param>
        public virtual void SetAngularVelocity(Vector3 newAngularVelocity) => ServerEntity.SetAngularVelocity(newAngularVelocity);

        /// <summary>
        /// Sets the new angular velocity of game entity
        /// </summary>
        /// <param name="newAngularVelocity">New game entity angular velocity</param>
        /// <param name="isValueFromClient">Is value from client</param>
        public virtual void SetAngularVelocity(Vector3 newAngularVelocity, bool isValueFromClient) => ServerEntity.SetAngularVelocity(newAngularVelocity, isValueFromClient);

        /// <summary>
        /// Sets the new game actions
        /// </summary>
        /// <param name="newActions">New game actions</param>
        /// <returns>Number of game actions set</returns>
        public virtual uint SetActions(IEnumerable<string> newActions) => ServerEntity.SetActions(newActions);

        /// <summary>
        /// Sets the new game actions of game entity
        /// </summary>
        /// <param name="newActions">New game entity game actions</param>
        /// <param name="isValueFromClient">Is value from client</param>
        /// <returns>Number of actions set</returns>
        public virtual uint SetActions(IEnumerable<string> newActions, bool isValueFromClient) => ServerEntity.SetActions(newActions, isValueFromClient);
    }
}
