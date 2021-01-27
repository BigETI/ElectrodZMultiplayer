using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Used to signal a game being stopped
    /// </summary>
    /// <param name="lobby">Lobby</param>
    /// <param name="users">Participating users</param>
    /// <param name="results">Game end results</param>
    public delegate void LobbyGameStoppedDelegate(ILobby lobby, IReadOnlyDictionary<string, UserWithResults> users, IReadOnlyDictionary<string, object> results);
}
