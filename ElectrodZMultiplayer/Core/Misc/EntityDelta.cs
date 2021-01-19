using System;
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
        public IEnumerable<EGameAction> Actions { get; }

        public bool IsValid =>
            (GUID != Guid.Empty) &&
            ((GameColor == null) || GameColor.Value != EGameColor.Unknown) &&
            ((Actions == null) || !Protection.IsContained(Actions, (action) => action == EGameAction.Unknown));

        /// <summary>
        /// COnstructs an entity delta
        /// </summary>
        /// <param name="guid">GUID</param>
        /// <param name="entityType">Entity type</param>
        /// <param name="gameColor">Game color</param>
        /// <param name="position">Position</param>
        /// <param name="rotation">Rotation</param>
        /// <param name="velocity">Velocity</param>
        /// <param name="angularVelocity">Angular velocity</param>
        /// <param name="actions">Actions</param>
        public EntityDelta(Guid guid, string entityType = null, EGameColor? gameColor = null, Vector3? position = null, Quaternion? rotation = null, Vector3? velocity = null, Vector3? angularVelocity = null, IEnumerable<EGameAction> actions = null)
        {
            GUID = guid;
            EntityType = entityType;
            GameColor = gameColor;
            Position = position;
            Rotation = rotation;
            Velocity = velocity;
            AngularVelocity = angularVelocity;
            Actions = actions;
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
                throw new ArgumentException($"Base entity delta GUID \"{ baseEntityDelta.GUID }\" does not match path entity delta GUID \"{ patchEntityDelta.GUID }\".", nameof(patchEntityDelta));
            }
            HashSet<EGameAction> actions = (baseEntityDelta.Actions == null) ? null : new HashSet<EGameAction>(baseEntityDelta.Actions);
            if (patchEntityDelta.Actions != null)
            {
                if (actions == null)
                {
                    actions = new HashSet<EGameAction>(patchEntityDelta.Actions);
                }
                else
                {
                    actions.IntersectWith(patchEntityDelta.Actions);
                }
            }
            return new EntityDelta
            (
                baseEntityDelta.GUID,
                patchEntityDelta.EntityType ?? baseEntityDelta.EntityType,
                patchEntityDelta.GameColor ?? baseEntityDelta.GameColor,
                patchEntityDelta.Position ?? baseEntityDelta.Position,
                patchEntityDelta.Rotation ?? baseEntityDelta.Rotation,
                patchEntityDelta.Velocity ?? baseEntityDelta.Velocity,
                patchEntityDelta.AngularVelocity ?? baseEntityDelta.AngularVelocity,
                actions
            );
        }
    }
}
