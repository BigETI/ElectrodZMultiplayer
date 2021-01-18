﻿using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// An interface that represents a game entity
    /// </summary>
    public interface IEntity : IValidable
    {
        /// <summary>
        /// Entity GUID
        /// </summary>
        Guid GUID { get; }

        /// <summary>
        /// Entity type
        /// </summary>
        string EntityType { get; }

        /// <summary>
        /// Game color
        /// </summary>
        EGameColor GameColor { get; }

        /// <summary>
        /// Current position
        /// </summary>
        Vector3 Position { get; }

        /// <summary>
        /// Current rotation
        /// </summary>
        Quaternion Rotation { get; }

        /// <summary>
        /// Current velocity
        /// </summary>
        Vector3 Velocity { get; }

        /// <summary>
        /// Current angular velocity
        /// </summary>
        Vector3 AngularVelocity { get; }

        /// <summary>
        /// Current game actions
        /// </summary>
        IEnumerable<EGameAction> Actions { get; }
    }
}
