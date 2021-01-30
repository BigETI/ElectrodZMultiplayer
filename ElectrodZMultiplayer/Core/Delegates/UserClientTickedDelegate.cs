﻿using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Used to signal a client tick
    /// </summary>
    /// <param name="user">user</param>
    /// <param name="entityDeltas">Entity deltas</param>
    public delegate void UserClientTickedDelegate(IUser user, IEnumerable<IEntityDelta> entityDeltas);
}
