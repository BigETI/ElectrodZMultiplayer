using ElectrodZMultiplayer.JSONConverters;
using Newtonsoft.Json;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Join lobby failed reason enumerator
    /// </summary>
    [JsonConverter(typeof(JoinLobbyFailedReasonJSONConverter))]
    public enum EJoinLobbyFailedReason
    {
        /// <summary>
        /// Invalid
        /// </summary>
        Invalid,

        /// <summary>
        /// Lobby code is null
        /// </summary>
        LobbyCodeIsNull,

        /// <summary>
        /// Username is null
        /// </summary>
        UsernameIsNull,

        /// <summary>
        /// Invalid username length
        /// </summary>
        InvalidUsernameLength,

        /// <summary>
        /// Lobby has not been found
        /// </summary>
        NotFound,

        /// <summary>
        /// Lobby is full
        /// </summary>
        Full,

        /// <summary>
        /// Unknown reason
        /// </summary>
        Unknown
    }
}
