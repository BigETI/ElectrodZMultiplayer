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
        /// Sets the new game actions
        /// </summary>
        /// <param name="newActions">New game actions</param>
        /// <returns>Number of game actions set</returns>
        public virtual uint SetActions(IEnumerable<string> newActions) => ServerEntity.SetActions(newActions);

        /// <summary>
        /// Sets the new game color
        /// </summary>
        /// <param name="newGameColor">New game color</param>
        public virtual void SetGameColor(EGameColor newGameColor) => SetGameColor(newGameColor);

        /// <summary>
        /// Sets the new position
        /// </summary>
        /// <param name="newPosition">New position</param>
        public virtual void SetPosition(Vector3 newPosition) => SetPosition(newPosition);

        /// <summary>
        /// Sets the new rotation
        /// </summary>
        /// <param name="newRotation">New rotation</param>
        public virtual void SetRotation(Quaternion newRotation) => SetRotation(newRotation);

        /// <summary>
        /// Sets the new velocity
        /// </summary>
        /// <param name="newVelocity">New velocity</param>
        public virtual void SetVelocity(Vector3 newVelocity) => SetVelocity(newVelocity);

        /// <summary>
        /// Sets the new angular velocity
        /// </summary>
        /// <param name="newAngularVelocity">New angular velocity</param>
        public virtual void SetAngularVelocity(Vector3 newAngularVelocity) => ServerEntity.SetAngularVelocity(newAngularVelocity);
    }
}
