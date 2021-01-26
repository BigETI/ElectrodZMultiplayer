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
    [JsonConverter(typeof(AuthentificationFailedReasonJSONConverter))]
    public enum EAuthentificationFailedReason
    {
        /// <summary>
        /// Invalid
        /// </summary>
        Invalid,

        /// <summary>
        /// Version is null
        /// </summary>
        VersionIsNull,

        /// <summary>
        /// Version is not supported
        /// </summary>
        NotSupportedVersion,

        /// <summary>
        /// Already authenticated
        /// </summary>
        AlreadyAuthenticated,

        /// <summary>
        /// Token is being already in use
        /// </summary>
        TokenIsAlreadyInUse,

        /// <summary>
        /// Unknown reason
        /// </summary>
        Unknown
    }
}
