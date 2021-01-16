using ElectrodZMultiplayer.JSONConverters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a client tick message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ClientTickMessageData : BaseMessageData
    {
        /// <summary>
        /// Current game color (optional)
        /// </summary>
        [JsonProperty("color")]
        [JsonConverter(typeof(GameColorJSONConverter))]
        public EGameColor? Color { get; set; }

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
        /// Current game actions (optional)
        /// </summary>
        [JsonProperty("actions")]
        [JsonConverter(typeof(GameActionJSONConverter))]
        public List<EGameAction> Actions { get; set; }

        /// <summary>
        /// Entities to update (optional)
        /// </summary>
        [JsonProperty("entities")]
        public List<EntityData> Entities { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            ((Color == null) || (Color != EGameColor.Unknown)) &&
            ((Actions == null) || !Actions.Contains(EGameAction.Unknown)) &&
            ((Entities == null) || Protection.IsValid(Entities));

        /// <summary>
        /// Constructs a client tick message for deserializers
        /// </summary>
        public ClientTickMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a client tick message
        /// </summary>
        /// <param name="color">Current game color (optional)</param>
        /// <param name="position">Current position (optional)</param>
        /// <param name="rotation">Current rotation (optional)</param>
        /// <param name="velocity">Current velocity (optional)</param>
        /// <param name="actions">Current game actions (optional)</param>
        /// <param name="entities">Entities to update (optional)</param>
        public ClientTickMessageData(EGameColor? color, Vector3<float>? position, Quaternion<float>? rotation, Vector3<float>? velocity, IEnumerable<EGameAction> actions, IEnumerable<IEntityDelta> entities) : base(Naming.GetMessageTypeNameFromMessageDataType<ClientTickMessageData>())
        {
            if ((color != null) && (color == EGameColor.Unknown))
            {
                throw new ArgumentException($"Game color can't be unknown.", nameof(color));
            }
            if ((actions != null) && Protection.IsContained(actions, (action) => action == EGameAction.Unknown))
            {
                throw new ArgumentException("Game actions contains unknown game action.", nameof(actions));
            }
            if ((entities != null) && Protection.IsValid(entities))
            {
                throw new ArgumentException("Entities contains invalid entities.", nameof(entities));
            }
            Color = color;
            Position = (position == null) ? null : (Vector3FloatData)position;
            Rotation = (rotation == null) ? null : (QuaternionFloatData)rotation;
            Velocity = (velocity == null) ? null : (Vector3FloatData)velocity;
            Actions = (actions == null) ? null : new List<EGameAction>(actions);
            if (entities != null)
            {
                Entities = new List<EntityData>();
                foreach (IEntityDelta entity in entities)
                {
                    Entities.Add(new EntityData(entity.GUID, entity.EntityType, entity.GameColor, entity.Position, entity.Rotation, entity.Velocity, entity.AngularVelocity, entity.Actions));
                }
            }
        }
    }
}
