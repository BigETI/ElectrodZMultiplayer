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
    public class ServerTickMessageData : BaseMessageData
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
        /// Hits
        /// </summary>
        [JsonProperty("hits")]
        public List<ServerHitData> Hits { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (Time >= 0.0) &&
            Protection.IsValid(Entities) &&
            ((Hits == null) || Protection.IsValid(Hits));

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
        /// <param name="hits">Hits</param>
        public ServerTickMessageData(double time, IEnumerable<IEntityDelta> entityDeltas, IEnumerable<IHit> hits) : base(Naming.GetMessageTypeNameFromMessageDataType<ServerTickMessageData>())
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
                throw new ArgumentException("Entity deltas contain invalid entity deltas.", nameof(entityDeltas));
            }
            if ((hits != null) && !Protection.IsValid(hits))
            {
                throw new ArgumentException("Hits contain invalid hits.", nameof(hits));
            }
            Time = time;
            Entities = new List<EntityData>();
            foreach (IEntityDelta entity_delta in entityDeltas)
            {
                Entities.Add(new EntityData(entity_delta.GUID, entity_delta.EntityType, entity_delta.GameColor, entity_delta.IsSpectating, entity_delta.Position, entity_delta.Rotation, entity_delta.Velocity, entity_delta.AngularVelocity, entity_delta.Actions, entity_delta.IsResyncRequested));
            }
            if (hits != null)
            {
                foreach (IHit hit in hits)
                {
                    Hits = Hits ?? new List<ServerHitData>();
                    Hits.Add(new ServerHitData(hit));
                }
            }
        }
    }
}
