using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer data namespace
/// </summary>
namespace ElectrodZMultiplayer.Data
{
    /// <summary>
    /// A class that describes client hit data
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ClientHitData : IValidable
    {
        /// <summary>
        /// Victim GUID
        /// </summary>
        [JsonProperty("victimGUID")]
        public Guid VictimGUID { get; set; }

        /// <summary>
        /// Weapon name
        /// </summary>
        [JsonProperty("weaponName")]
        public string WeaponName { get; set; }

        /// <summary>
        /// Hit position
        /// </summary>
        [JsonProperty("hitPosition")]
        public Vector3FloatData HitPosition { get; set; }

        /// <summary>
        /// Hit force
        /// </summary>
        [JsonProperty("hitForce")]
        public Vector3FloatData HitForce { get; set; }

        /// <summary>
        /// Damage
        /// </summary>
        [JsonProperty("damage")]
        public float Damage { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public bool IsValid =>
            (VictimGUID != Guid.Empty) &&
            !string.IsNullOrWhiteSpace(WeaponName) &&
            (HitPosition != null) &&
            (HitForce != null) &&
            (Damage >= 0.0f);

        /// <summary>
        /// Constructs client hit data for deserializers
        /// </summary>
        public ClientHitData()
        {
            // ...
        }

        /// <summary>
        /// Constructs client hit data
        /// </summary>
        /// <param name="entities">Entities (optional)</param>
        public ClientHitData(IHit hit)
        {
            if (hit == null)
            {
                throw new ArgumentNullException(nameof(hit));
            }
            if (!hit.IsValid)
            {
                throw new ArgumentException("Hit is not valid.", nameof(hit));
            }
            VictimGUID = hit.Victim.GUID;
            WeaponName = hit.WeaponName;
            HitPosition = (Vector3FloatData)hit.HitPosition;
            HitForce = (Vector3FloatData)hit.HitForce;
            Damage = hit.Damage;
        }
    }
}
