using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer client namespace
/// </summary>
namespace ElectrodZMultiplayer.Client
{
    /// <summary>
    /// This is being used to signal that lobbies are being listed
    /// </summary>
    /// <param name="lobbies">Lobbies</param>
    public delegate void LobbiesListedDelegate(IEnumerable<ILobbyView> lobbies);
}
