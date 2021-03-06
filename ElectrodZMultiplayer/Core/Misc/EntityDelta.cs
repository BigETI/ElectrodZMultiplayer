﻿using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// A structure that describes an entity delta
    /// </summary>
    public readonly struct EntityDelta : IEntityDelta
    {
        /// <summary>
        /// Entity GUID
        /// </summary>
        public Guid GUID { get; }

        /// <summary>
        /// Entity type (optional)
        /// </summary>
        public string EntityType { get; }

        /// <summary>
        /// Game color (optional)
        /// </summary>
        public EGameColor? GameColor { get; }

        /// <summary>
        /// Is spectating (optional)
        /// </summary>
        public bool? IsSpectating { get; }

        /// <summary>
        /// Current position (optional)
        /// </summary>
        public Vector3? Position { get; }

        /// <summary>
        /// Current rotation (optional)
        /// </summary>
        public Quaternion? Rotation { get; }

        /// <summary>
        /// Current velocity (optional)
        /// </summary>
        public Vector3? Velocity { get; }

        /// <summary>
        /// Current angular velocity (optional)
        /// </summary>
        public Vector3? AngularVelocity { get; }

        /// <summary>
        /// Current game actions (optional)
        /// </summary>
        public IEnumerable<string> Actions { get; }

        /// <summary>
        /// Is resynchronization requested
        /// </summary>
        public bool? IsResyncRequested { get; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public bool IsValid =>
            (GUID != Guid.Empty) &&
            ((GameColor == null) || GameColor.Value != EGameColor.Invalid) &&
            ((Actions == null) || !Protection.IsContained(Actions, (action) => action == null));

        /// <summary>
        /// COnstructs an entity delta
        /// </summary>
        /// <param name="guid">GUID</param>
        /// <param name="entityType">Entity type</param>
        /// <param name="gameColor">Game color</param>
        /// <param name="isSpectating">Is spectating</param>
        /// <param name="position">Position</param>
        /// <param name="rotation">Rotation</param>
        /// <param name="velocity">Velocity</param>
        /// <param name="angularVelocity">Angular velocity</param>
        /// <param name="actions">Actions</param>
        /// <param name="isResyncRequested">Is resynchronization requested</param>
        public EntityDelta(Guid guid, string entityType = null, EGameColor? gameColor = null, bool? isSpectating = null, Vector3? position = null, Quaternion? rotation = null, Vector3? velocity = null, Vector3? angularVelocity = null, IEnumerable<string> actions = null, bool? isResyncRequested = null)
        {
            if (guid == Guid.Empty)
            {
                throw new ArgumentException("Entity GUID can't be empty.", nameof(guid));
            }
            GUID = guid;
            EntityType = entityType;
            GameColor = gameColor;
            IsSpectating = isSpectating;
            Position = position;
            Rotation = rotation;
            Velocity = velocity;
            AngularVelocity = angularVelocity;
            Actions = actions;
            IsResyncRequested = isResyncRequested;
        }

        /// <summary>
        /// Combines two entity deltas to create a new entity delta
        /// </summary>
        /// <param name="baseEntityDelta">Base entity delta</param>
        /// <param name="patchEntityDelta">Patch entity delta</param>
        /// <returns>Combined entity delta</returns>
        public static EntityDelta Combine(IEntityDelta baseEntityDelta, IEntityDelta patchEntityDelta)
        {
            if (baseEntityDelta == null)
            {
                throw new ArgumentNullException(nameof(baseEntityDelta));
            }
            if (!baseEntityDelta.IsValid)
            {
                throw new ArgumentException("Base entity delta is not valid.", nameof(baseEntityDelta));
            }
            if (patchEntityDelta == null)
            {
                throw new ArgumentNullException(nameof(patchEntityDelta));
            }
            if (!patchEntityDelta.IsValid)
            {
                throw new ArgumentException("Patch entity delta is not valid.", nameof(patchEntityDelta));
            }
            if (baseEntityDelta.GUID != patchEntityDelta.GUID)
            {
                throw new ArgumentException($"Base entity delta GUID \"{ baseEntityDelta.GUID }\" does not match patch entity delta GUID \"{ patchEntityDelta.GUID }\".", nameof(patchEntityDelta));
            }
            HashSet<string> actions = (baseEntityDelta.Actions == null) ? null : new HashSet<string>(baseEntityDelta.Actions);
            if (patchEntityDelta.Actions != null)
            {
                if (actions == null)
                {
                    actions = new HashSet<string>(patchEntityDelta.Actions);
                }
                else
                {
                    actions.UnionWith(patchEntityDelta.Actions);
                }
            }
            return new EntityDelta
            (
                baseEntityDelta.GUID,
                patchEntityDelta.EntityType ?? baseEntityDelta.EntityType,
                patchEntityDelta.GameColor ?? baseEntityDelta.GameColor,
                patchEntityDelta.IsSpectating ?? baseEntityDelta.IsSpectating,
                patchEntityDelta.Position ?? baseEntityDelta.Position,
                patchEntityDelta.Rotation ?? baseEntityDelta.Rotation,
                patchEntityDelta.Velocity ?? baseEntityDelta.Velocity,
                patchEntityDelta.AngularVelocity ?? baseEntityDelta.AngularVelocity,
                actions,
                patchEntityDelta.IsResyncRequested ?? baseEntityDelta.IsResyncRequested
            );
        }
    }
}
