using ElectrodZMultiplayer.JSONConverters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer data namespace
/// </summary>
namespace ElectrodZMultiplayer.Data
{
    /// <summary>
    /// A class that describes entity data
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class EntityData : IValidable
    {
        /// <summary>
        /// Entity GUID
        /// </summary>
        [JsonProperty("guid")]
        [JsonConverter(typeof(GUIDJSONConverter))]
        public Guid GUID { get; set; }

        /// <summary>
        /// Current entity type (optional)
        /// </summary>
        [JsonProperty("entityType")]
        public string EntityType { get; set; }

        /// <summary>
        /// Entity game color (optional)
        /// </summary>
        [JsonProperty("color")]
        public EGameColor? GameColor { get; set; }

        /// <summary>
        /// Current position (optional)
        /// </summary>
        [JsonProperty("position")]
        public Vector3FloatData Position { get; set; }

        /// <summary>
        /// Current rotation (optional)
        /// </summary>
        [JsonProperty("rotation")]
        public QuaternionFloatData Rotation { get; set; }

        /// <summary>
        /// Current velocity (optional)
        /// </summary>
        [JsonProperty("velocity")]
        public Vector3FloatData Velocity { get; set; }

        /// <summary>
        /// Curent angular velocity (optional)
        /// </summary>
        [JsonProperty("angularVelocity")]
        public Vector3FloatData AngularVelocity { get; set; }

        /// <summary>
        /// Current game actions (optional)
        /// </summary>
        [JsonProperty("actions")]
        public List<EGameAction> Actions { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public bool IsValid =>
            (GUID != Guid.Empty) &&
            ((EntityType == null) || !string.IsNullOrWhiteSpace(EntityType)) &&
            ((GameColor == null) || (GameColor != EGameColor.Unknown)) &&
            ((Position == null) || (Position != null)) &&
            ((Rotation == null) || (Rotation != null)) &&
            ((AngularVelocity == null) || (AngularVelocity != null)) &&
            ((Actions == null) || !Actions.Contains(EGameAction.Unknown));

        /// <summary>
        /// Constructs entity data for deserializers
        /// </summary>
        public EntityData()
        {
            // ...
        }

        /// <summary>
        /// Constructs entity data
        /// </summary>
        /// <param name="guid">Entity GUID</param>
        /// <param name="entityType">Current entity type (optional)</param>
        /// <param name="color">Current game color (optional)</param>
        /// <param name="position">Current position (optional)</param>
        /// <param name="rotation">Current rotation (optional)</param>
        /// <param name="velocity">Current velocity (optional)</param>
        /// <param name="angularVelocity">Current angular velocity (optional)</param>
        /// <param name="actions">Current game actions (optional)</param>
        public EntityData(Guid guid, string entityType, EGameColor? color, Vector3? position, Quaternion? rotation, Vector3? velocity, Vector3? angularVelocity, IEnumerable<EGameAction> actions)
        {
            if (guid == Guid.Empty)
            {
                throw new ArgumentException("Entity GUID can't be empty.", nameof(guid));
            }
            if ((entityType != null) && string.IsNullOrWhiteSpace(entityType))
            {
                throw new ArgumentException("Entity type can't be empty.", nameof(entityType));
            }
            if ((color != null) && (color == EGameColor.Unknown))
            {
                throw new ArgumentException("Game color can't be unknown.", nameof(color));
            }
            if ((actions != null) && Protection.IsContained(actions, (action) => action == EGameAction.Unknown))
            {
                throw new ArgumentException($"\"{ nameof(actions) }\" contains unknown game actions");
            }
            GUID = guid;
            EntityType = entityType;
            GameColor = color;
            Position = (position == null) ? null : (Vector3FloatData)position;
            Rotation = (rotation == null) ? null : (QuaternionFloatData)rotation;
            Velocity = (velocity == null) ? null : (Vector3FloatData)velocity;
            AngularVelocity = (angularVelocity == null) ? null : (Vector3FloatData)angularVelocity;
            GameColor = color;
            Actions = (actions == null) ? null : new List<EGameAction>(actions ?? throw new ArgumentNullException(nameof(actions)));
        }

        /// <summary>
        /// Explicitly casts entity data to entity delta
        /// </summary>
        /// <param name="entity">Entity data</param>
        public static explicit operator EntityDelta(EntityData entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            return new EntityDelta
            (
                entity.GUID,
                entity.EntityType,
                entity.GameColor,
                new Vector3(entity.Position.X, entity.Position.Y, entity.Position.Z),
                new Quaternion(entity.Rotation.X, entity.Rotation.Y, entity.Rotation.Z, entity.Rotation.W),
                new Vector3(entity.Velocity.X, entity.Velocity.Y, entity.Velocity.Z),
                new Vector3(entity.AngularVelocity.X, entity.AngularVelocity.Y, entity.AngularVelocity.Z),
                entity.Actions
            );
        }
    }
}
