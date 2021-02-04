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
    public class ClientTickMessageData : BaseMessageData
    {
        /// <summary>
        /// Entities to update (optional)
        /// </summary>
        [JsonProperty("entities")]
        public List<EntityData> Entities { get; set; }

        /// <summary>
        /// Hits
        /// </summary>
        public List<ClientHitData> Hits { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
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
        /// <param name="entities">Entities (optional)</param>
        /// <param name="hits">Hits (optional)</param>
        public ClientTickMessageData(IEnumerable<IEntityDelta> entities = null, IEnumerable<IHit> hits = null) : base(Naming.GetMessageTypeNameFromMessageDataType<ClientTickMessageData>())
        {
            if ((entities != null) && !Protection.IsValid(entities))
            {
                throw new ArgumentException("Entities contains invalid entities.", nameof(entities));
            }
            if ((hits != null) && !Protection.IsValid(hits))
            {
                throw new ArgumentException("Hits contains invalid hits.", nameof(hits));
            }
            if (entities != null)
            {
                Entities = new List<EntityData>();
                foreach (IEntityDelta entity in entities)
                {
                    Entities.Add(new EntityData(entity.GUID, entity.EntityType, entity.GameColor, entity.Position, entity.Rotation, entity.Velocity, entity.AngularVelocity, entity.Actions));
                }
            }
            if (hits != null)
            {
                foreach (IHit hit in hits)
                {
                    if (hit.Issuer != null)
                    {
                        throw new ArgumentException("Hits can't specify issuers", nameof(hits));
                    }
                    Hits = Hits ?? new List<ClientHitData>();
                    Hits.Add(new ClientHitData(hit));
                }
            }
        }
    }
}
