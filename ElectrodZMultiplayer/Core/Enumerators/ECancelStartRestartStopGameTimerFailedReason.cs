using ElectrodZMultiplayer.JSONConverters;
using Newtonsoft.Json;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Cancel start, restart or stop game timer failed reason enumerator
    /// </summary>
    [JsonConverter(typeof(CancelStartRestartStopGameTimerFailedReasonJSONConverter))]
    public enum ECancelStartRestartStopGameTimerFailedReason
    {
        /// <summary>
        /// Invalid
        /// </summary>
        Invalid,

        /// <summary>
        /// Game start timer is not running
        /// </summary>
        GameStartTimerIsNotRunning,

        /// <summary>
        /// Game restart and stop timers are not running
        /// </summary>
        GameRestartStopTimersAreNotRunning,

        /// <summary>
        /// Unknown reason
        /// </summary>
        Unknown
    }
}
