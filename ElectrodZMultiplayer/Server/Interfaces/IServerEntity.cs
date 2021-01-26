using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer server namespace
/// </summary>
namespace ElectrodZMultiplayer.Server
{
    /// <summary>
    /// Server entity class
    /// </summary>
    public interface IServerEntity : IEntity
    {
        /// <summary>
        /// Sets the new game color of game entity
        /// </summary>
        /// <param name="newGameColor">New game entity game color</param>
        void SetGameColor(EGameColor newGameColor);

        /// <summary>
        /// Sets the new position of game entity
        /// </summary>
        /// <param name="newPosition">New game entity position</param>
        void SetPosition(Vector3 newPosition);

        /// <summary>
        /// Sets the new rotation of game entity
        /// </summary>
        /// <param name="newRotation">New game entity rotation</param>
        void SetRotation(Quaternion newRotation);

        /// <summary>
        /// Sets the new velocity of game entity
        /// </summary>
        /// <param name="newVelocity">New game entity velocity</param>
        void SetVelocity(Vector3 newVelocity);

        /// <summary>
        /// Sets the new angular velocity of game entity
        /// </summary>
        /// <param name="newAngularVelocity">New game entity angular velocity</param>
        void SetAngularVelocity(Vector3 newAngularVelocity);

        /// <summary>
        /// Sets the new game actions of game entity
        /// </summary>
        /// <param name="newActions">New game entity game actions</param>
        /// <returns>Number of actions set</returns>
        uint SetActions(IEnumerable<EGameAction> newActions);
    }
}
