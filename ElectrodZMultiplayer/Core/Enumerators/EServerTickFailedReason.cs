using ElectrodZMultiplayer.JSONConverters;
using Newtonsoft.Json;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Server tick failed reason enumerator
    /// </summary>
    [JsonConverter(typeof(ServerTickFailedReasonJSONConverter))]
    public enum EServerTickFailedReason
    {
        /// <summary>
        /// Invalid
        /// </summary>
        Invalid,

        /// <summary>
        /// Unknown reason
        /// </summary>
        Unknown
    }
}
