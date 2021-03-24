using ElectrodZMultiplayer.Data;
using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Entity class
    /// </summary>
    internal class Entity : IInternalEntity
    {
        /// <summary>
        /// Current game actions
        /// </summary>
        private readonly HashSet<string> actions = new HashSet<string>();

        /// <summary>
        /// Entity GUID
        /// </summary>
        public Guid GUID { get; private set; }

        /// <summary>
        /// Entity type
        /// </summary>
        public string EntityType { get; private set; }

        /// <summary>
        /// Game color
        /// </summary>
        public EGameColor GameColor { get; private set; } = EGameColor.Default;

        /// <summary>
        /// Current position
        /// </summary>
        public Vector3 Position { get; private set; }

        /// <summary>
        /// Current rotation
        /// </summary>
        public Quaternion Rotation { get; private set; }

        /// <summary>
        /// Current velocity
        /// </summary>
        public Vector3 Velocity { get; private set; }

        /// <summary>
        /// Current angular velocity
        /// </summary>
        public Vector3 AngularVelocity { get; private set; }

        /// <summary>
        /// Current game actions
        /// </summary>
        public IEnumerable<string> Actions => actions;

        /// <summary>
        /// Is resynchronization requested
        /// </summary>
        public virtual bool IsResyncRequested { get; private set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        /// <returns>"true" if valid, otherwise "false"</returns>
        public bool IsValid =>
            (GUID != Guid.Empty) &&
            !string.IsNullOrWhiteSpace(EntityType) &&
            (GameColor != EGameColor.Invalid) &&
            Protection.IsValid(Actions);

        /// <summary>
        /// Constructs an entity object
        /// </summary>
        /// <param name="guid">Entity GUID</param>
        /// <param name="gameColor">Entity game color</param>
        public Entity(Guid guid, string entityType)
        {
            if (guid == Guid.Empty)
            {
                throw new ArgumentException("Entity GUID can't be empty.", nameof(guid));
            }
            if (string.IsNullOrWhiteSpace(entityType))
            {
                throw new ArgumentNullException(nameof(entityType));
            }
            GUID = guid;
            EntityType = entityType;
        }

        /// <summary>
        /// Constructs an entity object
        /// </summary>
        /// <param name="guid">Entity GUID</param>
        /// <param name="entityType">Entity type</param>
        /// <param name="gameColor">Game color</param>
        /// <param name="position">Position</param>
        /// <param name="rotation">Rotation</param>
        /// <param name="velocity">Velocity</param>
        /// <param name="angularVelocity">Angular velocity</param>
        /// <param name="actions">Game actions</param>
        /// <param name="isResyncRequested">Is resynchronization requested</param>
        public Entity(Guid guid, string entityType, EGameColor gameColor, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity, IEnumerable<string> actions, bool isResyncRequested)
        {
            if (guid == Guid.Empty)
            {
                throw new ArgumentException("Entity GUID can't be empty.", nameof(guid));
            }
            if (string.IsNullOrWhiteSpace(entityType))
            {
                throw new ArgumentNullException(nameof(entityType));
            }
            if (gameColor == EGameColor.Invalid)
            {
                throw new ArgumentException("Game color can't be invalid.", nameof(gameColor));
            }
            if (actions == null)
            {
                throw new ArgumentNullException(nameof(actions));
            }
            if (Protection.IsContained(actions, (action) => action == null))
            {
                throw new ArgumentException($"\"{ nameof(actions) }\" contains invalid game actions.", nameof(guid));
            }
            GUID = guid;
            EntityType = entityType;
            GameColor = gameColor;
            Position = position;
            Rotation = rotation;
            Velocity = velocity;
            AngularVelocity = angularVelocity;
            this.actions.UnionWith(actions);
            IsResyncRequested = isResyncRequested;
        }

        /// <summary>
        /// Gets the delta for the specified entities
        /// </summary>
        /// <param name="baseEntity">Base entity</param>
        /// <param name="patchEntity">Patch entity</param>
        /// <returns>Entity delta if there are differences between the specified entities, otherwise "null"</returns>
        public static IEntityDelta GetDelta(IEntity baseEntity, IEntity patchEntity) => TryGetDelta(baseEntity, patchEntity, out IEntityDelta ret) ? ret : null;

        /// <summary>
        /// Tries to get entity delta from the specified entities
        /// </summary>
        /// <param name="baseEntity">Base entity</param>
        /// <param name="patchEntity">Patch entity</param>
        /// <param name="entityDelta">Entity data</param>
        /// <returns>"true" if differences between base and patch entities exist, otherwise "false"</returns>
        public static bool TryGetDelta(IEntity baseEntity, IEntity patchEntity, out IEntityDelta entityDelta)
        {
            if (baseEntity == null)
            {
                throw new ArgumentNullException(nameof(baseEntity));
            }
            if (!baseEntity.IsValid)
            {
                throw new ArgumentException("Base entity is not valid.", nameof(baseEntity));
            }
            if (patchEntity == null)
            {
                throw new ArgumentNullException(nameof(patchEntity));
            }
            if (!patchEntity.IsValid)
            {
                throw new ArgumentException("Patch entity is not valid.", nameof(patchEntity));
            }
            if (baseEntity.GUID != patchEntity.GUID)
            {
                throw new ArgumentException($"Base entity GUID \"{ baseEntity.GUID }\" does not match patch entity GUID \"{ patchEntity.GUID }\".", nameof(patchEntity));
            }
            bool ret = false;
            string entity_type = null;
            EGameColor? game_color = null;
            Vector3? position = null;
            Quaternion? rotation = null;
            Vector3? velocity = null;
            Vector3? angular_velocity = null;
            IEnumerable<string> actions = null;
            bool? is_resync_requested = null;
            if (baseEntity != patchEntity)
            {
                if (baseEntity.EntityType != patchEntity.EntityType)
                {
                    ret = true;
                    entity_type = patchEntity.EntityType;
                }
                if (baseEntity.EntityType != patchEntity.EntityType)
                {
                    ret = true;
                    entity_type = patchEntity.EntityType;
                }
                if (baseEntity.GameColor != patchEntity.GameColor)
                {
                    ret = true;
                    game_color = patchEntity.GameColor;
                }
                if (baseEntity.Position != patchEntity.Position)
                {
                    ret = true;
                    position = patchEntity.Position;
                }
                if (baseEntity.Rotation != patchEntity.Rotation)
                {
                    ret = true;
                    rotation = patchEntity.Rotation;
                }
                if (baseEntity.Velocity != patchEntity.Velocity)
                {
                    ret = true;
                    velocity = patchEntity.Velocity;
                }
                if (baseEntity.AngularVelocity != patchEntity.AngularVelocity)
                {
                    ret = true;
                    angular_velocity = patchEntity.AngularVelocity;
                }
                foreach (string action in baseEntity.Actions)
                {
                    if (!Protection.IsContained(patchEntity.Actions, (patch_entity_action) => patch_entity_action == action))
                    {
                        actions = patchEntity.Actions;
                        break;
                    }
                }
                if (baseEntity.IsResyncRequested != patchEntity.IsResyncRequested)
                {
                    ret = true;
                    is_resync_requested = patchEntity.IsResyncRequested;
                }
                if (actions == null)
                {
                    foreach (string action in patchEntity.Actions)
                    {
                        if (!Protection.IsContained(baseEntity.Actions, (base_entity_action) => base_entity_action == action))
                        {
                            actions = patchEntity.Actions;
                            break;
                        }
                    }
                }
            }
            entityDelta = ret ? (IEntityDelta)new EntityDelta(baseEntity.GUID, entity_type, game_color, position, rotation, velocity, angular_velocity, actions, is_resync_requested) : null;
            return ret;
        }

        /// <summary>
        /// Sets a new entity GUID internally
        /// </summary>
        /// <param name="guid">New entity GUID</param>
        public void SetGUIDInternally(Guid guid)
        {
            if (guid == Guid.Empty)
            {
                throw new ArgumentException("Entity GUID is empty.", nameof(guid));
            }
            GUID = guid;
        }

        /// <summary>
        /// Set a new entity type internally
        /// </summary>
        /// <param name="entityType">New entity type</param>
        public void SetEntityTypeInternally(string entityType)
        {
            if (string.IsNullOrWhiteSpace(entityType))
            {
                throw new ArgumentNullException(nameof(entityType));
            }
            EntityType = entityType;
        }

        /// <summary>
        /// Sets a new game color internally
        /// </summary>
        /// <param name="gameColor">New game color</param>
        public void SetGameColorInternally(EGameColor gameColor)
        {
            if (gameColor == EGameColor.Invalid)
            {
                throw new ArgumentException("Game color can't be invalid.", nameof(gameColor));
            }
            GameColor = gameColor;
        }

        /// <summary>
        /// Sets a new position internally
        /// </summary>
        /// <param name="position">New position</param>
        public void SetPositionInternally(Vector3 position) => Position = position;

        /// <summary>
        /// Sets a new rotation internally
        /// </summary>
        /// <param name="rotation">New rotation</param>
        public void SetRotationInternally(Quaternion rotation) => Rotation = rotation;

        /// <summary>
        /// Sets a new velocity internally
        /// </summary>
        /// <param name="velocity">New velocity</param>
        public void SetVelocityInternally(Vector3 velocity) => Velocity = velocity;

        /// <summary>
        /// Sets a new angular velocity
        /// </summary>
        /// <param name="angularVelocity">New angular velocity</param>
        public void SetAngularVelocityInternally(Vector3 angularVelocity) => AngularVelocity = angularVelocity;

        /// <summary>
        /// Sets the game actions internally
        /// </summary>
        /// <param name="actions">Game actions</param>
        /// <returns>Number of actions added</returns>
        public uint SetActionsInternally(IEnumerable<string> actions)
        {
            if (actions == null)
            {
                throw new ArgumentNullException(nameof(actions));
            }
            if (Protection.IsContained(actions, (action) => action == null))
            {
                throw new ArgumentException("Game actions contain null.");
            }
            uint ret = 0U;
            this.actions.Clear();
            foreach (string action in actions)
            {
                ret += this.actions.Add(action) ? 1U : 0U;
            }
            return ret;
        }

        /// <summary>
        /// Sets the resynchronization requested state internally
        /// </summary>
        /// <param name="isResyncRequested">Is resynchronization requested</param>
        public void SetResyncRequestedStateInternally(bool isResyncRequested) => IsResyncRequested = isResyncRequested;

        /// <summary>
        /// Casts the specified entity data to an entity object
        /// </summary>
        /// <param name="entity">Entity data</param>
        public static explicit operator Entity(EntityData entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (!entity.IsValid)
            {
                throw new ArgumentException("Entity is not valid.", nameof(entity));
            }
            if (string.IsNullOrWhiteSpace(entity.EntityType))
            {
                throw new ArgumentException("Entity type can't be empty.", nameof(entity));
            }
            return new Entity(entity.GUID, entity.EntityType, (entity.GameColor == null) ? EGameColor.Default : entity.GameColor.Value, (entity.Position == null) ? Vector3.Zero : new Vector3(entity.Position.X, entity.Position.Y, entity.Position.Z), (entity.Rotation == null) ? Quaternion.Identity : new Quaternion(entity.Rotation.X, entity.Rotation.Y, entity.Rotation.Z, entity.Rotation.W), (entity.Velocity == null) ? Vector3.Zero : new Vector3(entity.Velocity.X, entity.Velocity.Y, entity.Velocity.Z), (entity.AngularVelocity == null) ? Vector3.Zero : new Vector3(entity.AngularVelocity.X, entity.AngularVelocity.Y, entity.AngularVelocity.Z), entity.Actions ?? (IEnumerable<string>)Array.Empty<string>(), entity.IsResyncRequested ?? false);
        }
    }
}
