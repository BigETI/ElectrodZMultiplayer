/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// An interface that represents a hit
    /// </summary>
    public interface IHit : IValidable
    {
        /// <summary>
        /// Issuer
        /// </summary>
        IEntity Issuer { get; }

        /// <summary>
        /// Victim
        /// </summary>
        IEntity Victim { get; }

        /// <summary>
        /// Weapon name
        /// </summary>
        string WeaponName { get; }

        /// <summary>
        /// Hit position
        /// </summary>
        Vector3 HitPosition { get; }

        /// <summary>
        /// Hit force
        /// </summary>
        Vector3 HitForce { get; }

        /// <summary>
        /// Damage
        /// </summary>
        float Damage { get; }
    }
}
