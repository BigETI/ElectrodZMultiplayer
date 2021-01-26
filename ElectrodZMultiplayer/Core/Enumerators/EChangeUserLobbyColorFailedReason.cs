using ElectrodZMultiplayer.JSONConverters;
using Newtonsoft.Json;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Change user lobby color failed reason enumerator
    /// </summary>
    [JsonConverter(typeof(ChangeUserLobbyColorFailedReasonJSONConverter))]
    public enum EChangeUserLobbyColorFailedReason
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
