using ElectrodZMultiplayer.JSONConverters;
using Newtonsoft.Json;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Authentification failed reason enumerator
    /// </summary>
    [JsonConverter(typeof(KickUserFailedReasonJSONConverter))]
    public enum EKickUserFailedReason
    {
        /// <summary>
        /// Invalid
        /// </summary>
        Invalid,

        /// <summary>
        /// Invalid user GUID
        /// </summary>
        InvalidUserGUID,

        /// <summary>
        /// Failed to execute command
        /// </summary>
        FailedExecution,

        /// <summary>
        /// Unknown reason
        /// </summary>
        Unknown
    }
}
