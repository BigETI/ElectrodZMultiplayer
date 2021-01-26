using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// This is being used to signal when available game modes have been listed
    /// </summary>
    /// <param name="availableGameModes">Available game modes</param>
    public delegate void AvailableGameModesListedDelegate(IEnumerable<string> availableGameModes);
}
