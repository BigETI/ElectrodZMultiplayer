using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Used to signal a server tick
    /// </summary>
    /// <param name="user">User</param>
    /// <param name="time">Elapsed time in seconds since game start</param>
    /// <param name="entityDeltas">Entity deltas</param>>
    /// <param name="hits">Hits</param>>
    public delegate void UserServerTickedDelegate(IUser user, double time, IEnumerable<IEntityDelta> entityDeltas, IEnumerable<IHit> hits);
}
