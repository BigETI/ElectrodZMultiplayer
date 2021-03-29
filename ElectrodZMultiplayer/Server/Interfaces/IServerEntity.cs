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
        /// Is client game color set
        /// </summary>
        bool IsClientGameColorSet { get; }

        /// <summary>
        /// Is client spectating state set
        /// </summary>
        bool IsClientSpectatingStateSet { get; }

        /// <summary>
        /// Is client position set
        /// </summary>
        bool IsClientPositionSet { get; }

        /// <summary>
        /// Is client rotation set
        /// </summary>
        bool IsClientRotationSet { get; }

        /// <summary>
        /// Is client velocity set
        /// </summary>
        bool IsClientVelocitySet { get; }

        /// <summary>
        /// Is client angular velocity set
        /// </summary>
        bool IsClientAngularVelocitySet { get; }

        /// <summary>
        /// Are client game actions set
        /// </summary>
        bool AreClientActionsSet { get; }

        /// <summary>
        /// Sets the new game color of game entity
        /// </summary>
        /// <param name="newGameColor">New game entity game color</param>
        void SetGameColor(EGameColor newGameColor);

        /// <summary>
        /// Sets the new game color of game entity
        /// </summary>
        /// <param name="newGameColor">New game entity game color</param>
        /// <param name="isValueFromClient">Is value from client</param>
        void SetGameColor(EGameColor newGameColor, bool isValueFromClient);

        /// <summary>
        /// Sets the new spectating state
        /// </summary>
        /// <param name="newSpectatingState">New spectating state</param>
        void SetSpectatingState(bool newSpectatingState);

        /// <summary>
        /// Sets the new spectating state
        /// </summary>
        /// <param name="newSpectatingState">New spectating state</param>
        /// <param name="isValueFromClient">Is value from client</param>
        void SetSpectatingState(bool newSpectatingState, bool isValueFromClient);

        /// <summary>
        /// Sets the new position of game entity
        /// </summary>
        /// <param name="newPosition">New game entity position</param>
        void SetPosition(Vector3 newPosition);

        /// <summary>
        /// Sets the new position of game entity
        /// </summary>
        /// <param name="newPosition">New game entity position</param>
        /// <param name="isValueFromClient">Is value from client</param>
        void SetPosition(Vector3 newPosition, bool isValueFromClient);

        /// <summary>
        /// Sets the new rotation of game entity
        /// </summary>
        /// <param name="newRotation">New game entity rotation</param>
        void SetRotation(Quaternion newRotation);

        /// <summary>
        /// Sets the new rotation of game entity
        /// </summary>
        /// <param name="newRotation">New game entity rotation</param>
        /// <param name="isValueFromClient">Is value from client</param>
        void SetRotation(Quaternion newRotation, bool isValueFromClient);

        /// <summary>
        /// Sets the new velocity of game entity
        /// </summary>
        /// <param name="newVelocity">New game entity velocity</param>
        void SetVelocity(Vector3 newVelocity);

        /// <summary>
        /// Sets the new velocity of game entity
        /// </summary>
        /// <param name="newVelocity">New game entity velocity</param>
        /// <param name="isValueFromClient">Is value from client</param>
        void SetVelocity(Vector3 newVelocity, bool isValueFromClient);

        /// <summary>
        /// Sets the new angular velocity of game entity
        /// </summary>
        /// <param name="newAngularVelocity">New game entity angular velocity</param>
        void SetAngularVelocity(Vector3 newAngularVelocity);

        /// <summary>
        /// Sets the new angular velocity of game entity
        /// </summary>
        /// <param name="newAngularVelocity">New game entity angular velocity</param>
        /// <param name="isValueFromClient">Is value from client</param>
        void SetAngularVelocity(Vector3 newAngularVelocity, bool isValueFromClient);

        /// <summary>
        /// Sets the new game actions of game entity
        /// </summary>
        /// <param name="newActions">New game entity game actions</param>
        /// <returns>Number of actions set</returns>
        uint SetActions(IEnumerable<string> newActions);

        /// <summary>
        /// Sets the new game actions of game entity
        /// </summary>
        /// <param name="newActions">New game entity game actions</param>
        /// <param name="isValueFromClient">Is value from client</param>
        /// <returns>Number of actions set</returns>
        uint SetActions(IEnumerable<string> newActions, bool isValueFromClient);
    }
}
