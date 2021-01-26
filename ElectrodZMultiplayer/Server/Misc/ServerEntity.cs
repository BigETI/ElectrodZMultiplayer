using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer server namespace
/// </summary>
namespace ElectrodZMultiplayer.Server
{
    /// <summary>
    /// A class that describes a server entity
    /// </summary>
    internal class ServerEntity : Entity, IServerEntity
    {
        /// <summary>
        /// Constructs a server entity object
        /// </summary>
        /// <param name="guid">Entity GUID</param>
        /// <param name="gameColor">Entity game color</param>
        public ServerEntity(Guid guid, string entityType) : base(guid, entityType)
        {
            // ...
        }

        /// <summary>
        /// Constructs a server entity object
        /// </summary>
        /// <param name="guid">Entity GUID</param>
        /// <param name="entityType">Entity type</param>
        /// <param name="gameColor">Game color</param>
        /// <param name="position">Position</param>
        /// <param name="rotation">Rotation</param>
        /// <param name="velocity">Velocity</param>
        /// <param name="angularVelocity">Angular velocity</param>
        /// <param name="actions">Game actions</param>
        public ServerEntity(Guid guid, string entityType, EGameColor gameColor, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity, IEnumerable<EGameAction> actions) : base(guid, entityType, gameColor, position, rotation, velocity, angularVelocity, actions)
        {
            // ...
        }

        /// <summary>
        /// Sets the new game color of game entity
        /// </summary>
        /// <param name="newGameColor">New game entity game color</param>
        public void SetGameColor(EGameColor newGameColor) => SetGameColorInternally(newGameColor);

        /// <summary>
        /// Sets the new position of game entity
        /// </summary>
        /// <param name="newPosition">New game entity position</param>
        public void SetPosition(Vector3 newPosition) => SetPositionInternally(newPosition);

        /// <summary>
        /// Sets the new rotation of game entity
        /// </summary>
        /// <param name="newRotation">New game entity rotation</param>
        public void SetRotation(Quaternion newRotation) => SetRotationInternally(newRotation);

        /// <summary>
        /// Sets the new velocity of game entity
        /// </summary>
        /// <param name="newVelocity">New game entity velocity</param>
        public void SetVelocity(Vector3 newVelocity) => SetVelocityInternally(newVelocity);

        /// <summary>
        /// Sets the new angular velocity of game entity
        /// </summary>
        /// <param name="newAngularVelocity">New game entity angular velocity</param>
        public void SetAngularVelocity(Vector3 newAngularVelocity) => SetAngularVelocityInternally(newAngularVelocity);

        /// <summary>
        /// Sets the new game actions of game entity
        /// </summary>
        /// <param name="newActions">New game entity game actions</param>
        /// <returns>Number of actions set</returns>
        public uint SetActions(IEnumerable<EGameAction> newActions) => SetActionsInternally(newActions);
    }
}
