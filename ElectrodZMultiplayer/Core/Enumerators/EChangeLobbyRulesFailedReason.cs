using ElectrodZMultiplayer.JSONConverters;
using Newtonsoft.Json;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Change lobby rules failed reason enumerator
    /// </summary>
    [JsonConverter(typeof(ChangeLobbyRulesFailedReasonJSONConverter))]
    public enum EChangeLobbyRulesFailedReason
    {
        /// <summary>
        /// Invalid
        /// </summary>
        Invalid,

        /// <summary>
        /// Invalid lobby name length
        /// </summary>
        InvalidLobbyNameLength,

        /// <summary>
        /// Minimal user count is bigger than maximal user count
        /// </summary>
        MinimalUserCountIsBiggerThanMaximalUserCount,

        /// <summary>
        /// Invalid game mode
        /// </summary>
        InvalidGameMode,

        /// <summary>
        /// Game mode rules contain null
        /// </summary>
        GameModeRulesContainNull,

        /// <summary>
        /// Game mode is not available
        /// </summary>
        GameModeIsNotAvailable,

        /// <summary>
        /// Unknown reason
        /// </summary>
        Unknown
    }
}
