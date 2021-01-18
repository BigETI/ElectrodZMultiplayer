﻿using Newtonsoft.Json;
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
        /// Entities to update (optional)
        /// </summary>
        [JsonProperty("entities")]
        public List<EntityData> Entities { get; set; }

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
        /// <param name="color">Current game color (optional)</param>
        /// <param name="position">Current position (optional)</param>
        /// <param name="rotation">Current rotation (optional)</param>
        /// <param name="velocity">Current velocity (optional)</param>
        /// <param name="actions">Current game actions (optional)</param>
        /// <param name="entities">Entities to update (optional)</param>
        public ClientTickMessageData(IEnumerable<IEntityDelta> entities) : base(Naming.GetMessageTypeNameFromMessageDataType<ClientTickMessageData>())
        {
            if ((entities != null) && Protection.IsValid(entities))
            {
                throw new ArgumentException("Entities contains invalid entities.", nameof(entities));
            }
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
