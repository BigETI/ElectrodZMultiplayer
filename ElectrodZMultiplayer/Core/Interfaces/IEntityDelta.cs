using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// An interface that represents an entity delta
    /// </summary>
    public interface IEntityDelta : IValidable
    {
        /// <summary>
        /// Entity GUID
        /// </summary>
        Guid GUID { get; }

        /// <summary>
        /// Entity type (optional)
        /// </summary>
        string EntityType { get; }

        /// <summary>
        /// Game color (optional)
        /// </summary>
        EGameColor? GameColor { get; }

        /// <summary>
        /// Current position (optional)
        /// </summary>
        Vector3<float>? Position { get; }

        /// <summary>
        /// Current rotation (optional)
        /// </summary>
        Quaternion<float>? Rotation { get; }

        /// <summary>
        /// Current velocity (optional)
        /// </summary>
        Vector3<float>? Velocity { get; }

        /// <summary>
        /// Current angular velocity (optional)
        /// </summary>
        Vector3<float>? AngularVelocity { get; }

        /// <summary>
        /// Current game actions (optional)
        /// </summary>
        IEnumerable<EGameAction> Actions { get; }
    }
}
