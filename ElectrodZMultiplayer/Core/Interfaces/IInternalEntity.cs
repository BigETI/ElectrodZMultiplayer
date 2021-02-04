using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// An interface that represents a game entity
    /// </summary>
    internal interface IInternalEntity : IEntity
    {
        /// <summary>
        /// Sets a new entity GUID internally
        /// </summary>
        /// <param name="guid">Entity GUID</param>
        void SetGUIDInternally(Guid guid);

        /// <summary>
        /// Set a new entity type internally
        /// </summary>
        /// <param name="entityType">New entity type</param>
        void SetEntityTypeInternally(string entityType);

        /// <summary>
        /// Sets a new game color internally
        /// </summary>
        /// <param name="gameColor">New game color</param>
        void SetGameColorInternally(EGameColor gameColor);

        /// <summary>
        /// Sets a new position internally
        /// </summary>
        /// <param name="position">New position</param>
        void SetPositionInternally(Vector3 position);

        /// <summary>
        /// Sets a new rotation internally
        /// </summary>
        /// <param name="rotation">New rotation</param>
        void SetRotationInternally(Quaternion rotation);

        /// <summary>
        /// Sets a new velocity internally
        /// </summary>
        /// <param name="velocity">New velocity</param>
        void SetVelocityInternally(Vector3 velocity);

        /// <summary>
        /// Sets a new angular velocity
        /// </summary>
        /// <param name="angularVelocity">New angular velocity</param>
        void SetAngularVelocityInternally(Vector3 angularVelocity);

        /// <summary>
        /// Sets the game actions internally
        /// </summary>
        /// <param name="actions">Game actions</param>
        /// <returns>Number of actions added</returns>
        uint SetActionsInternally(IEnumerable<string> actions);
    }
}
