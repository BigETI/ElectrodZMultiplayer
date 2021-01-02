using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// An interface describing a lobby
    /// </summary>
    public interface ILobby : ILobbyView, IDisposable
    {
        /// <summary>
        /// Lobby owner
        /// </summary>
        IUser Owner { get; }

        /// <summary>
        /// Users
        /// </summary>
        IReadOnlyDictionary<string, IUser> Users { get; }

        /// <summary>
        /// Entities
        /// </summary>
        IReadOnlyDictionary<string, IEntity> Entities { get; }
    }
}
