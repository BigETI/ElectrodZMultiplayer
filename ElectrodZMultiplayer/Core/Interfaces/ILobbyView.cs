using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// An interface that represents a view of a lobby
    /// </summary>
    public interface ILobbyView : IValidable
    {
        /// <summary>
        /// Lobby code
        /// </summary>
        string LobbyCode { get; }

        /// <summary>
        /// Name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Game mode
        /// </summary>
        string GameMode { get; }

        /// <summary>
        /// Is lobby private
        /// </summary>
        bool IsPrivate { get; }

        /// <summary>
        /// Minimal user count
        /// </summary>
        uint MinimalUserCount { get; }

        /// <summary>
        /// Maximal user count
        /// </summary>
        uint MaximalUserCount { get; }

        /// <summary>
        /// Is starting game automatically
        /// </summary>
        bool IsStartingGameAutomatically { get; }

        /// <summary>
        /// Game mode rules
        /// </summary>
        IReadOnlyDictionary<string, object> GameModeRules { get; }

        /// <summary>
        /// User count
        /// </summary>
        uint UserCount { get; }
    }
}
