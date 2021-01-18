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
        private readonly HashSet<EGameAction> actions = new HashSet<EGameAction>();

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
        public IEnumerable<EGameAction> Actions => actions;

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        /// <returns>"true" if valid, otherwise "false"</returns>
        public bool IsValid =>
            (GUID != Guid.Empty) &&
            (GameColor != EGameColor.Unknown);

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
        public Entity(Guid guid, string entityType, EGameColor gameColor, Vector3 position, Quaternion rotation, Vector3 velocity, Vector3 angularVelocity, IEnumerable<EGameAction> actions)
        {
            if (guid == Guid.Empty)
            {
                throw new ArgumentException("Entity GUID can't be empty.", nameof(guid));
            }
            if (string.IsNullOrWhiteSpace(entityType))
            {
                throw new ArgumentNullException(nameof(entityType));
            }
            if (gameColor == EGameColor.Unknown)
            {
                throw new ArgumentException("Game color can't be unknown.", nameof(gameColor));
            }
            if (actions == null)
            {
                throw new ArgumentNullException(nameof(actions));
            }
            if (Protection.IsContained(actions, (action) => action == EGameAction.Unknown))
            {
                throw new ArgumentException($"\"{ nameof(actions) }\" contains unknown game actions.", nameof(guid));
            }
            GUID = guid;
            EntityType = entityType;
            GameColor = gameColor;
            Position = position;
            Rotation = rotation;
            Velocity = velocity;
            AngularVelocity = angularVelocity;
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
            if (gameColor == EGameColor.Unknown)
            {
                throw new ArgumentException("Game color can't be unknown.", nameof(gameColor));
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
        public uint SetActionsInternally(IEnumerable<EGameAction> actions)
        {
            if (actions == null)
            {
                throw new ArgumentNullException(nameof(actions));
            }
            if (Protection.IsContained(actions, (action) => action == EGameAction.Unknown))
            {
                throw new ArgumentException("Game actions contain invalid invalid actions.");
            }
            uint ret = 0U;
            this.actions.Clear();
            foreach (EGameAction action in actions)
            {
                ret += this.actions.Add(action) ? 1U : 0U;
            }
            return ret;
        }

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
            return new Entity(entity.GUID, entity.EntityType, (entity.Color == null) ? EGameColor.Default : entity.Color.Value, (entity.Position == null) ? Vector3.Zero : new Vector3(entity.Position.X, entity.Position.Y, entity.Position.Z), (entity.Rotation == null) ? Quaternion.Identity : new Quaternion(entity.Rotation.X, entity.Rotation.Y, entity.Rotation.Z, entity.Rotation.W), (entity.Velocity == null) ? Vector3.Zero : new Vector3(entity.Velocity.X, entity.Velocity.Y, entity.Velocity.Z), (entity.AngularVelocity == null) ? Vector3.Zero : new Vector3(entity.AngularVelocity.X, entity.AngularVelocity.Y, entity.AngularVelocity.Z), entity.Actions ?? (IEnumerable<EGameAction>)Array.Empty<EGameAction>());
        }
    }
}
