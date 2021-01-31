using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Used to signal a client tick
    /// </summary>
    /// <param name="entityDeltas">Entity deltas</param>
    /// <param name="hits">Hits</param>
    public delegate void ClientTickedDelegate(IEnumerable<IEntityDelta> entityDeltas, IEnumerable<IHit> hits);
}
