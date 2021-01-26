using ElectrodZMultiplayer.JSONConverters;
using Newtonsoft.Json;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Start game failed reason enumerator
    /// </summary>
    [JsonConverter(typeof(StartGameFailedReasonJSONConverter))]
    public enum EStartGameFailedReason
    {
        /// <summary>
        /// Invalid
        /// </summary>
        Invalid,

        /// <summary>
        /// Time is negative
        /// </summary>
        NegativeTime,

        /// <summary>
        /// Game mode is already running
        /// </summary>
        GameModeIsAlreadyRunning,

        /// <summary>
        /// Unknown reason
        /// </summary>
        Unknown
    }
}
