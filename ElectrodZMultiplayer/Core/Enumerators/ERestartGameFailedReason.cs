using ElectrodZMultiplayer.JSONConverters;
using Newtonsoft.Json;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Restart game failed reason enumerator
    /// </summary>
    [JsonConverter(typeof(RestartGameFailedReasonJSONConverter))]
    public enum ERestartGameFailedReason
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
        /// Game mode is not running yet
        /// </summary>
        GameModeIsNotRunning,

        /// <summary>
        /// Unknown reason
        /// </summary>
        Unknown
    }
}
