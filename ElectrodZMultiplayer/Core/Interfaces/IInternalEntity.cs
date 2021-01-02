using System;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// An interface specifically used for the internal representation of a game entity
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
        void SetPositionInternally(Vector3<float> position);

        /// <summary>
        /// Sets a new rotation internally
        /// </summary>
        /// <param name="rotation">New rotation</param>
        void SetRotationInternally(Quaternion<float> rotation);

        /// <summary>
        /// Sets a new velocity internally
        /// </summary>
        /// <param name="velocity">New velocity</param>
        void SetVelocityInternally(Vector3<float> velocity);

        /// <summary>
        /// Sets a new angular velocity
        /// </summary>
        /// <param name="angularVelocity">New angular velocity</param>
        void SetAngularVelocityInternally(Vector3<float> angularVelocity);

        /// <summary>
        /// Adds a new game action to the current game actions internally.
        /// </summary>
        /// <param name="action">New game action</param>
        /// <returns>"true" if new game action was successfully added, otherwise "false"</returns>
        bool AddGameActionInternally(EGameAction action);

        /// <summary>
        /// Removes the specified game action from the current game actions internally.
        /// </summary>
        /// <param name="action">Game action</param>
        /// <returns>"true" if the specified game action was successfully removed, otherwise "false"</returns>
        bool RemoveActionInternally(EGameAction action);

        /// <summary>
        /// Clears all set game actions internally.
        /// </summary>
        void ClearGameActionsInternally();
    }
}
