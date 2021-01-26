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
    public delegate void ClientTickedDelegate(IEnumerable<IEntityDelta> entityDeltas);
}
