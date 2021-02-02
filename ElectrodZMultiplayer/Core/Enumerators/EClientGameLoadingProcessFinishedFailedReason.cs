using ElectrodZMultiplayer.JSONConverters;
using Newtonsoft.Json;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Client game loading process finished failed reason enumerator
    /// </summary>
    [JsonConverter(typeof(ClientGameLoadingProcessFinishedFailedReasonJSONConverter))]
    public enum EClientGameLoadingProcessFinishedFailedReason
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
