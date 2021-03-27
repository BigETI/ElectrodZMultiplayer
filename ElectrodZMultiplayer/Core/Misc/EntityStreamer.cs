using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// A class that describes an entity streamer
    /// </summary>
    internal class EntityStreamer : IEntityStreamer
    {
        /// <summary>
        /// Entities
        /// </summary>
        private readonly Dictionary<string, (Entity, IEntityDelta)> entities = new Dictionary<string, (Entity, IEntityDelta)>();

        /// <summary>
        /// Remove entities
        /// </summary>
        private readonly HashSet<string> removeEntities = new HashSet<string>();

        /// <summary>
        /// This event will be invoked when an entity has been created.
        /// </summary>
        public event EntityCreatedDelegate OnEntityCreated;

        /// <summary>
        /// This event will be invoked when an entity has been updated.
        /// </summary>
        public event EntityUpdatedDelegate OnEntityUpdated;

        /// <summary>
        /// This event will be invoked when an entity has been destroyed.
        /// </summary>
        public event EntityDestroyedDelegate OnEntityDestroyed;

        /// <summary>
        /// Gets the delta for the specified entity
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Entity delta</returns>
        private IEntityDelta GetEntityDelta(IEntity entity)
        {
            IEntityDelta ret;
            string key = entity.GUID.ToString();
            if (removeEntities.Remove(key))
            {
                (Entity, IEntityDelta) base_entity = entities[key];
                if (Entity.TryGetDelta(base_entity.Item1, entity, out ret))
                {
                    if (!string.IsNullOrWhiteSpace(ret.EntityType))
                    {
                        base_entity.Item1.SetEntityTypeInternally(ret.EntityType);
                    }
                    if (ret.GameColor != null)
                    {
                        base_entity.Item1.SetGameColorInternally(ret.GameColor.Value);
                    }
                    if (ret.Position != null)
                    {
                        base_entity.Item1.SetPositionInternally(ret.Position.Value);
                    }
                    if (ret.Rotation != null)
                    {
                        base_entity.Item1.SetRotationInternally(ret.Rotation.Value);
                    }
                    if (ret.Velocity != null)
                    {
                        base_entity.Item1.SetVelocityInternally(ret.Velocity.Value);
                    }
                    if (ret.AngularVelocity != null)
                    {
                        base_entity.Item1.SetAngularVelocityInternally(ret.AngularVelocity.Value);
                    }
                    if (ret.Actions != null)
                    {
                        base_entity.Item1.SetActionsInternally(ret.Actions);
                    }
                    if (ret.IsResyncRequested != null)
                    {
                        base_entity.Item1.SetResyncRequestedStateInternally(ret.IsResyncRequested.Value);
                    }
                }
                else
                {
                    ret = base_entity.Item2;
                }
                OnEntityUpdated?.Invoke(entity);
            }
            else
            {
                Entity new_entity = new Entity(entity.GUID, entity.EntityType, entity.GameColor, entity.Position, entity.Rotation, entity.Velocity, entity.AngularVelocity, entity.Actions, entity.IsResyncRequested);
                ret = new EntityDelta(new_entity.GUID, new_entity.EntityType, new_entity.GameColor, new_entity.Position, new_entity.Rotation, new_entity.Velocity, new_entity.AngularVelocity, new_entity.Actions, new_entity.IsResyncRequested);
                entities.Add(key, (new_entity, new EntityDelta(new_entity.GUID)));
                OnEntityCreated?.Invoke(new_entity);
            }
            return ret;
        }

        /// <summary>
        /// Gets the entity deltas from users and entities
        /// </summary>
        /// <param name="users">Users</param>
        /// <param name="entities">Entities</param>
        /// <returns>Entity deltas</returns>
        public IList<IEntityDelta> GetEntityDeltas(IEnumerable<IUser> users, IEnumerable<IEntity> entities, ref IList<IEntityDelta> entityDeltas)
        {
            if (users == null)
            {
                throw new ArgumentNullException(nameof(users));
            }
            if (!Protection.IsValid(users))
            {
                throw new ArgumentException("Users are not valid.", nameof(users));
            }
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }
            if (!Protection.IsValid(entities))
            {
                throw new ArgumentException("Entities are not valid.", nameof(entities));
            }
            entityDeltas.Clear();
            removeEntities.UnionWith(this.entities.Keys);
            foreach (IUser user in users)
            {
                entityDeltas.Add(GetEntityDelta(user));
            }
            foreach (IEntity entity in entities)
            {
                entityDeltas.Add(GetEntityDelta(entity));
            }
            foreach (string remove_entity in removeEntities)
            {
                OnEntityDestroyed?.Invoke(this.entities[remove_entity].Item1);
                this.entities.Remove(remove_entity);
            }
            removeEntities.Clear();
            return entityDeltas;
        }
    }
}
