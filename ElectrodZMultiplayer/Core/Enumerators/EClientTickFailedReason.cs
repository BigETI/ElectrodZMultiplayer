using ElectrodZMultiplayer.JSONConverters;
using Newtonsoft.Json;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Client tick failed reason enumerator
    /// </summary>
    [JsonConverter(typeof(ClientTickFailedReasonJSONConverter))]
    public enum EClientTickFailedReason
    {
        /// <summary>
        /// Invalid
        /// </summary>
        Invalid,

        /// <summary>
        /// Entities contain invalid entries
        /// </summary>
        InvalidEntities,

        /// <summary>
        /// Unknown reason
        /// </summary>
        Unknown
    }
}
