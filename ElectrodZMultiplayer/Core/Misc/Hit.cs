using System;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// A structure that describes a hit
    /// </summary>
    public readonly struct Hit : IHit
    {
        /// <summary>
        /// Issuer
        /// </summary>
        public IEntity Issuer { get; }

        /// <summary>
        /// Victim
        /// </summary>
        public IEntity Victim { get; }

        /// <summary>
        /// Weapon name
        /// </summary>
        public string WeaponName { get; }

        /// <summary>
        /// Hit position
        /// </summary>
        public Vector3 HitPosition { get; }

        /// <summary>
        /// Hit force
        /// </summary>
        public Vector3 HitForce { get; }

        /// <summary>
        /// Damage
        /// </summary>
        public float Damage { get; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public bool IsValid =>
            ((Issuer == null) || Issuer.IsValid) &&
            (Victim != null) &&
            Victim.IsValid &&
            !string.IsNullOrWhiteSpace(WeaponName) &&
            (Damage >= 0.0f);

        /// <summary>
        /// Constructs a hit
        /// </summary>
        /// <param name="victim">Victim</param>
        /// <param name="weaponName">Weapon name</param>
        /// <param name="hitPosition">Hit position</param>
        /// <param name="hitForce">Hit force</param>
        /// <param name="damage">Damage</param>
        public Hit(IEntity victim, string weaponName, Vector3 hitPosition, Vector3 hitForce, float damage)
        {
            if (victim == null)
            {
                throw new ArgumentNullException(nameof(victim));
            }
            if (!victim.IsValid)
            {
                throw new ArgumentException("Issuer is not valid.", nameof(victim));
            }
            if (string.IsNullOrWhiteSpace(weaponName))
            {
                throw new ArgumentNullException(nameof(weaponName));
            }
            if (damage < 0.0f)
            {
                throw new ArgumentException("Damage can't be negative.", nameof(damage));
            }
            Issuer = null;
            Victim = victim;
            WeaponName = weaponName;
            HitPosition = hitPosition;
            HitForce = hitForce;
            Damage = damage;
        }

        /// <summary>
        /// Constructs a hit
        /// </summary>
        /// <param name="issuer">Issuer</param>
        /// <param name="victim">Victim</param>
        /// <param name="weaponName">Weapon name</param>
        /// <param name="hitPosition">Hit position</param>
        /// <param name="hitForce">Hit force</param>
        /// <param name="damage">Damage</param>
        public Hit(IEntity issuer, IEntity victim, string weaponName, Vector3 hitPosition, Vector3 hitForce, float damage)
        {
            if (issuer == null)
            {
                throw new ArgumentNullException(nameof(issuer));
            }
            if (!issuer.IsValid)
            {
                throw new ArgumentException("Issuer is not valid.", nameof(issuer));
            }
            if (victim == null)
            {
                throw new ArgumentNullException(nameof(victim));
            }
            if (!victim.IsValid)
            {
                throw new ArgumentException("Issuer is not valid.", nameof(victim));
            }
            if (string.IsNullOrWhiteSpace(weaponName))
            {
                throw new ArgumentNullException(nameof(weaponName));
            }
            if (damage < 0.0f)
            {
                throw new ArgumentException("Damage can't be negative.", nameof(damage));
            }
            Issuer = issuer;
            Victim = victim;
            WeaponName = weaponName;
            HitPosition = hitPosition;
            HitForce = hitForce;
            Damage = damage;
        }
    }
}
