using ElectrodZMultiplayer.JSONConverters;
using Newtonsoft.Json;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// List lobbies failed reason enumerator
    /// </summary>
    [JsonConverter(typeof(ListLobbiesFailedReasonJSONConverter))]
    public enum EListLobbiesFailedReason
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
