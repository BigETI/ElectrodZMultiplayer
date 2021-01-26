using ElectrodZMultiplayer.JSONConverters;
using Newtonsoft.Json;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Create lobby failed reason enumerator
    /// </summary>
    [JsonConverter(typeof(CreateLobbyFailedReasonJSONConverter))]
    public enum ECreateLobbyFailedReason
    {
        /// <summary>
        /// Invalid
        /// </summary>
        Invalid,

        /// <summary>
        /// Username is null
        /// </summary>
        UsernameIsNull,

        /// <summary>
        /// Invalid username length
        /// </summary>
        InvalidUsernameLength,

        /// <summary>
        /// Lobby name is null
        /// </summary>
        LobbyNameIsNull,

        /// <summary>
        /// Invalid lobby name length
        /// </summary>
        InvalidLobbyNameLength,

        /// <summary>
        /// Invalid game mode
        /// </summary>
        InvalidGameMode,

        /// <summary>
        /// Minimal user count is bigger than maximal user count
        /// </summary>
        MinimalUserCountIsBiggerThanMaximalUserCount,

        /// <summary>
        /// Game mode rules contain null
        /// </summary>
        GameModeRulesContainNull,

        /// <summary>
        /// Specified game mode is not available
        /// </summary>
        GameModeIsNotAvailable,

        /// <summary>
        /// Unknown reason
        /// </summary>
        Unknown
    }
}
