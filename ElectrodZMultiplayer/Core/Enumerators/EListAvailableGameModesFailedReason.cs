using ElectrodZMultiplayer.JSONConverters;
using Newtonsoft.Json;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// List available game modes failed reason enumerator
    /// </summary>
    [JsonConverter(typeof(ListAvailableGameModesFailedReasonJSONConverter))]
    public enum EListAvailableGameModesFailedReason
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
