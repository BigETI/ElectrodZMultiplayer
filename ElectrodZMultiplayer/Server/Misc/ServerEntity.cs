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
        /// Client error margin squared
        /// </summary>
        private static readonly float clientErrorMarginSquared = 0.0078125f;

        /// <summary>
        /// Is client game color set
        /// </summary>
        public bool IsClientGameColorSet { get; private set; } = true;

        /// <summary>
        /// Is client spectating state set
        /// </summary>
        public bool IsClientSpectatingStateSet { get; private set; } = true;

        /// <summary>
        /// Is client position set
        /// </summary>
        public bool IsClientPositionSet { get; private set; } = true;

        /// <summary>
        /// Is client rotation set
        /// </summary>
        public bool IsClientRotationSet { get; private set; } = true;

        /// <summary>
        /// Is client velocity set
        /// </summary>
        public bool IsClientVelocitySet { get; private set; } = true;

        /// <summary>
        /// Is client angular velocity set
        /// </summary>
        public bool IsClientAngularVelocitySet { get; private set; } = true;

        /// <summary>
        /// Are client game actions set
        /// </summary>
        public bool AreClientActionsSet { get; private set; } = true;

        /// <summary>
        /// Is resynchronization requested
        /// </summary>
        public override bool IsResyncRequested =>
            !IsClientGameColorSet ||
            !IsClientSpectatingStateSet ||
            !IsClientPositionSet ||
            !IsClientRotationSet ||
            !IsClientVelocitySet ||
            !IsClientAngularVelocitySet ||
            !AreClientActionsSet;

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
        /// <param name="isSpectating">Is spectating</param>
        /// <param name="position">Position</param>
        /// <param name="rotation">Rotation</param>
        /// <param name="velocity">Velocity</param>
        /// <param name="angularVelocity">Angular velocity</param>
        /// <param name="actions">Game actions</param>
        /// <param name="isResyncRequested">Is resynchronization requested</param>
        public ServerEntity(Guid guid, string entityType, EGameColor gameColor, bool isSpectating, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity, IEnumerable<string> actions, bool isResyncRequested) : base(guid, entityType, gameColor, isSpectating, position, rotation, velocity, angularVelocity, actions, isResyncRequested)
        {
            // ...
        }

        /// <summary>
        /// Sets the new game color of game entity
        /// </summary>
        /// <param name="newGameColor">New game entity game color</param>
        public void SetGameColor(EGameColor newGameColor) => SetGameColor(newGameColor, false);

        /// <summary>
        /// Sets the new game color of game entity
        /// </summary>
        /// <param name="newGameColor">New game entity game color</param>
        /// <param name="isValueFromClient">Is value from client</param>
        public void SetGameColor(EGameColor newGameColor, bool isValueFromClient)
        {
            if (newGameColor == EGameColor.Invalid)
            {
                throw new ArgumentException("Game color can't be invalid.", nameof(newGameColor));
            }
            if (isValueFromClient)
            {
                if (IsClientGameColorSet)
                {
                    SetGameColorInternally(newGameColor);
                }
                else
                {
                    IsClientGameColorSet = GameColor == newGameColor;
                }
            }
            else
            {
                IsClientGameColorSet = false;
                SetGameColorInternally(newGameColor);
            }
        }

        /// <summary>
        /// Sets the new spectating state
        /// </summary>
        /// <param name="newSpectatingState">New spectating state</param>
        public void SetSpectatingState(bool newSpectatingState) => SetSpectatingState(newSpectatingState, false);

        /// <summary>
        /// Sets the new spectating state
        /// </summary>
        /// <param name="newSpectatingState">New spectating state</param>
        /// <param name="isValueFromClient">Is value from client</param>
        public void SetSpectatingState(bool newSpectatingState, bool isValueFromClient)
        {
            if (isValueFromClient)
            {
                IsClientSpectatingStateSet = IsSpectating == newSpectatingState;
            }
            else
            {
                IsClientSpectatingStateSet = false;
                SetSpectatingStateInternally(newSpectatingState);
            }
        }

        /// <summary>
        /// Sets the new position of game entity
        /// </summary>
        /// <param name="newPosition">New game entity position</param>
        public void SetPosition(Vector3 newPosition) => SetPosition(newPosition, false);

        /// <summary>
        /// Sets the new position of game entity
        /// </summary>
        /// <param name="newPosition">New game entity position</param>
        /// <param name="isValueFromClient">Is value from client</param>
        public void SetPosition(Vector3 newPosition, bool isValueFromClient)
        {
            if (isValueFromClient)
            {
                if (IsClientPositionSet)
                {
                    SetPositionInternally(newPosition);
                }
                else
                {
                    IsClientPositionSet = (Position - newPosition).MagnitudeSquared <= clientErrorMarginSquared;
                }
            }
            else
            {
                IsClientPositionSet = false;
                SetPositionInternally(newPosition);
            }
        }

        /// <summary>
        /// Sets the new rotation of game entity
        /// </summary>
        /// <param name="newRotation">New game entity rotation</param>
        public void SetRotation(Quaternion newRotation) => SetRotation(newRotation, false);

        /// <summary>
        /// Sets the new rotation of game entity
        /// </summary>
        /// <param name="newRotation">New game entity rotation</param>
        /// <param name="isValueFromClient">Is value from client</param>
        public void SetRotation(Quaternion newRotation, bool isValueFromClient)
        {
            if (isValueFromClient)
            {
                Quaternion delta = new Quaternion(Rotation.X - newRotation.X, Rotation.Y - newRotation.Y, Rotation.Z - newRotation.Z, Rotation.W - newRotation.W);
                if (IsClientRotationSet)
                {
                    SetRotationInternally(newRotation);
                }
                else
                {
                    IsClientRotationSet = ((delta.X * delta.X) + (delta.Y * delta.Y) + (delta.Z * delta.Z) + (delta.W * delta.W)) <= clientErrorMarginSquared;
                }
            }
            else
            {
                IsClientRotationSet = false;
                SetRotationInternally(newRotation);
            }
        }

        /// <summary>
        /// Sets the new velocity of game entity
        /// </summary>
        /// <param name="newVelocity">New game entity velocity</param>
        public void SetVelocity(Vector3 newVelocity) => SetVelocity(newVelocity, false);

        /// <summary>
        /// Sets the new velocity of game entity
        /// </summary>
        /// <param name="newVelocity">New game entity velocity</param>
        /// <param name="isValueFromClient">Is value from client</param>
        public void SetVelocity(Vector3 newVelocity, bool isValueFromClient)
        {
            if (isValueFromClient)
            {
                if (IsClientVelocitySet)
                {
                    SetVelocityInternally(newVelocity);
                }
                else
                {
                    IsClientVelocitySet = (Velocity - newVelocity).MagnitudeSquared <= clientErrorMarginSquared;
                }
            }
            else
            {
                IsClientVelocitySet = false;
                SetVelocityInternally(newVelocity);
            }
        }

        /// <summary>
        /// Sets the new angular velocity of game entity
        /// </summary>
        /// <param name="newAngularVelocity">New game entity angular velocity</param>
        public void SetAngularVelocity(Vector3 newAngularVelocity) => SetAngularVelocity(newAngularVelocity, false);

        /// <summary>
        /// Sets the new angular velocity of game entity
        /// </summary>
        /// <param name="newAngularVelocity">New game entity angular velocity</param>
        /// <param name="isValueFromClient">Is value from client</param>
        public void SetAngularVelocity(Vector3 newAngularVelocity, bool isValueFromClient)
        {
            if (isValueFromClient)
            {
                if (IsClientAngularVelocitySet)
                {
                    SetAngularVelocityInternally(newAngularVelocity);
                }
                else
                {
                    IsClientAngularVelocitySet = (AngularVelocity - newAngularVelocity).MagnitudeSquared <= clientErrorMarginSquared;
                }
            }
            else
            {
                IsClientAngularVelocitySet = false;
                SetAngularVelocityInternally(newAngularVelocity);
            }
        }

        /// <summary>
        /// Sets the new game actions of game entity
        /// </summary>
        /// <param name="newActions">New game entity game actions</param>
        /// <returns>Number of actions set</returns>
        public uint SetActions(IEnumerable<string> newActions) => SetActionsInternally(newActions);

        /// <summary>
        /// Sets the new game actions of game entity
        /// </summary>
        /// <param name="newActions">New game entity game actions</param>
        /// <param name="isValueFromClient">Is value from client</param>
        /// <returns>Number of actions set</returns>
        public uint SetActions(IEnumerable<string> newActions, bool isValueFromClient)
        {
            if (!Protection.IsValid(newActions))
            {
                throw new ArgumentException("Game actions can't contain invalid actions.", nameof(newActions));
            }
            uint ret = 0U;
            if (isValueFromClient)
            {
                if (AreClientActionsSet)
                {
                    ret = SetActionsInternally(newActions);
                }
                else
                {
                    if (Actions is HashSet<string> actions)
                    {
                        AreClientActionsSet = actions.SetEquals(newActions);
                    }
                    else
                    {
                        HashSet<string> current_actions = new HashSet<string>(Actions);
                        AreClientActionsSet = current_actions.SetEquals(newActions);
                        current_actions.Clear();
                    }
                }
            }
            else
            {
                AreClientActionsSet = false;
                ret = SetActionsInternally(newActions);
            }
            return ret;
        }
    }
}
