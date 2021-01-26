using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// An interface that represents an entity streamer
    /// </summary>
    public interface IEntityStreamer
    {
        /// <summary>
        /// This event will be invoked when an entity has been created.
        /// </summary>
        event EntityCreatedDelegate OnEntityCreated;

        /// <summary>
        /// This event will be invoked when an entity has been updated.
        /// </summary>
        event EntityUpdatedDelegate OnEntityUpdated;

        /// <summary>
        /// This event will be invoked when an entity has been destroyed.
        /// </summary>
        event EntityDestroyedDelegate OnEntityDestroyed;

        /// <summary>
        /// Gets the entity deltas from users and entities
        /// </summary>
        /// <param name="users">Users</param>
        /// <param name="entities">Entities</param>
        /// <returns>Entity deltas</returns>
        IList<IEntityDelta> GetEntityDeltas(IEnumerable<IUser> users, IEnumerable<IEntity> entities, ref IList<IEntityDelta> entityDeltas);
    }
}
