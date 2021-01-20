using Newtonsoft.Json;
using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a server tick message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ServerTickMessageData : BaseMessageData
    {
        /// <summary>
        /// Elapsed time in seconds since game start
        /// </summary>
        [JsonProperty("time")]
        public double Time { get; set; }

        /// <summary>
        /// Entities to update
        /// </summary>
        [JsonProperty("entities")]
        public List<EntityData> Entities { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (Time >= 0.0) &&
            Protection.IsValid(Entities);

        /// <summary>
        /// Constructs a server tick message for deserializers
        /// </summary>
        public ServerTickMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a server tick message
        /// </summary>
        /// <param name="time">Time</param>
        /// <param name="entities">Entities to update</param>
        public ServerTickMessageData(double time, IEnumerable<IEntityDelta> entities) : base(Naming.GetMessageTypeNameFromMessageDataType<ServerTickMessageData>())
        {
            if (time < 0.0)
            {
                throw new ArgumentException("Time must be positive.", nameof(time));
            }
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }
            if (!Protection.IsValid(entities))
            {
                throw new ArgumentException($"Entities are not valid.", nameof(entities));
            }
            Time = time;
            Entities = new List<EntityData>();
            foreach (IEntity entity in entities)
            {
                Entities.Add(new EntityData(entity.GUID, entity.EntityType, entity.GameColor, entity.Position, entity.Rotation, entity.Velocity, entity.AngularVelocity, entity.Actions));
            }
        }
    }
}
