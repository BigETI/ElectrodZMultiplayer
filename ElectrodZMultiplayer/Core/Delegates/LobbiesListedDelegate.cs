using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// This is being used to signal that lobbies are being listed
    /// </summary>
    /// <param name="lobbies">Lobbies</param>
    public delegate void LobbiesListedDelegate(IEnumerable<ILobbyView> lobbies);
}
