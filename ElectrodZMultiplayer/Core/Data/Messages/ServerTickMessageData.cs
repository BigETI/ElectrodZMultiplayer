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
        /// <param name="entityDeltas">Entities to update</param>
        public ServerTickMessageData(double time, IEnumerable<IEntityDelta> entityDeltas) : base(Naming.GetMessageTypeNameFromMessageDataType<ServerTickMessageData>())
        {
            if (time < 0.0)
            {
                throw new ArgumentException("Time must be positive.", nameof(time));
            }
            if (entityDeltas == null)
            {
                throw new ArgumentNullException(nameof(entityDeltas));
            }
            if (!Protection.IsValid(entityDeltas))
            {
                throw new ArgumentException($"Entities are not valid.", nameof(entityDeltas));
            }
            Time = time;
            Entities = new List<EntityData>();
            foreach (IEntityDelta entity_delta in entityDeltas)
            {
                Entities.Add(new EntityData(entity_delta.GUID, entity_delta.EntityType, entity_delta.GameColor, entity_delta.Position, entity_delta.Rotation, entity_delta.Velocity, entity_delta.AngularVelocity, entity_delta.Actions));
            }
        }
    }
}
