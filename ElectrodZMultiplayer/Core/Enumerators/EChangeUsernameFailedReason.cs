using ElectrodZMultiplayer.JSONConverters;
using Newtonsoft.Json;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Change username failed reason enumerator
    /// </summary>
    [JsonConverter(typeof(ChangeUsernameFailedReasonJSONConverter))]
    public enum EChangeUsernameFailedReason
    {
        /// <summary>
        /// Invalid
        /// </summary>
        Invalid,

        /// <summary>
        /// Username is null
        /// </summary>
        UsernameIsNull,

        /// <summary>
        /// Invalid username length
        /// </summary>
        InvalidUsernameLength,

        /// <summary>
        /// Unknown reason
        /// </summary>
        Unknown
    }
}
