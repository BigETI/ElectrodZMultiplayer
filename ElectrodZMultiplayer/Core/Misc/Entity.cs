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
        public string EntityType { get; private set; } = "UnknownEntityType";

        /// <summary>
        /// Game color
        /// </summary>
        public EGameColor GameColor { get; private set; }

        /// <summary>
        /// Current position
        /// </summary>
        public Vector3<float> Position { get; private set; }

        /// <summary>
        /// Current rotation
        /// </summary>
        public Quaternion<float> Rotation { get; private set; }

        /// <summary>
        /// Current velocity
        /// </summary>
        public Vector3<float> Velocity { get; private set; }

        /// <summary>
        /// Current angular velocity
        /// </summary>
        public Vector3<float> AngularVelocity { get; private set; }

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
        /// Constructs a game entity object
        /// </summary>
        /// <param name="guid">Entity GUID</param>
        /// <param name="gameColor">Entity game color</param>
        public Entity(Guid guid, EGameColor gameColor)
        {
            if (guid == Guid.Empty)
            {
                throw new ArgumentException("Entity GUID can't be empty.", nameof(guid));
            }
            if (gameColor == EGameColor.Unknown)
            {
                throw new ArgumentException("Entity game color is unknown.", nameof(gameColor));
            }
            GUID = guid;
            GameColor = gameColor;
        }

        /// <summary>
        /// Constructs a game entity object
        /// </summary>
        /// <param name="guid">Entity GUID</param>
        /// <param name="position">Position</param>
        /// <param name="rotation">Rotation</param>
        /// <param name="velocity">Velocity</param>
        /// <param name="angularVelocity">Angular velocity</param>
        /// <param name="actions">Game actions</param>
        public Entity(Guid guid, string entityType, Vector3<float> position, Quaternion<float> rotation, Vector3<float> velocity, Vector3<float> angularVelocity, IEnumerable<EGameAction> actions)
        {
            if (guid == Guid.Empty)
            {
                throw new ArgumentException("Entity GUID can't be empty.", nameof(guid));
            }
            if (string.IsNullOrWhiteSpace(entityType))
            {
                throw new ArgumentNullException(nameof(entityType));
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
        public void SetPositionInternally(Vector3<float> position) => Position = position;

        /// <summary>
        /// Sets a new rotation internally
        /// </summary>
        /// <param name="rotation">New rotation</param>
        public void SetRotationInternally(Quaternion<float> rotation) => Rotation = rotation;

        /// <summary>
        /// Sets a new velocity internally
        /// </summary>
        /// <param name="velocity">New velocity</param>
        public void SetVelocityInternally(Vector3<float> velocity) => Velocity = velocity;

        /// <summary>
        /// Sets a new angular velocity
        /// </summary>
        /// <param name="angularVelocity">New angular velocity</param>
        public void SetAngularVelocityInternally(Vector3<float> angularVelocity) => AngularVelocity = angularVelocity;

        /// <summary>
        /// Adds a new game action to the current game actions.
        /// </summary>
        /// <param name="action">New game action</param>
        /// <returns>"true" if new game action was successfully added, otherwise "false"</returns>
        public bool AddGameActionInternally(EGameAction action)
        {
            bool ret = false;
            if (action != EGameAction.Unknown)
            {
                ret = actions.Add(action);
            }
            return ret;
        }

        /// <summary>
        /// Removes the specified game action from the current game actions
        /// </summary>
        /// <param name="action">Game action</param>
        /// <returns>"true" if the specified game action was successfully removed, otherwise "false"</returns>
        public bool RemoveActionInternally(EGameAction action)
        {
            bool ret = false;
            if (action != EGameAction.Unknown)
            {
                ret = actions.Remove(action);
            }
            return ret;
        }

        /// <summary>
        /// Clears all set game actions
        /// </summary>
        public void ClearGameActionsInternally() => actions.Clear();

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
            return new Entity(entity.GUID, entity.EntityType, new Vector3<float>(entity.Position.X, entity.Position.Y, entity.Position.Z), new Quaternion<float>(entity.Rotation.W, entity.Rotation.X, entity.Rotation.Y, entity.Rotation.Z), new Vector3<float>(entity.Velocity.X, entity.Velocity.Y, entity.Velocity.Z), new Vector3<float>(entity.AngularVelocity.X, entity.AngularVelocity.Y, entity.AngularVelocity.Z), entity.Actions);
        }
    }
}
