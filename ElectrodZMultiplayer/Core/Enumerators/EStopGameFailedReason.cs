using ElectrodZMultiplayer.JSONConverters;
using Newtonsoft.Json;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Stop game failed reason enumerator
    /// </summary>
    [JsonConverter(typeof(StopGameFailedReasonJSONConverter))]
    public enum EStopGameFailedReason
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
