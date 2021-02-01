using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Used to signal a server tick
    /// </summary>
    /// <param name="time">Elapsed time in seconds since game start</param>
    /// <param name="entityDeltas">Entity deltas</param>>
    /// <param name="Hits">Hits</param>>
    public delegate void ServerTickedDelegate(double time, IEnumerable<IEntityDelta> entityDeltas, IEnumerable<IHit> hits);
}
