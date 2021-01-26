using ElectrodZMultiplayer.JSONConverters;
using Newtonsoft.Json;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Disconnection reason enumerator
    /// </summary>
    [JsonConverter(typeof(DisconnectionReasonJSONConverter))]
    public enum EDisconnectionReason
    {
        /// <summary>
        /// Invalid disconnection reason
        /// </summary>
        Invalid,

        /// <summary>
        /// Error
        /// </summary>
        Error,

        /// <summary>
        /// Object has been disposed
        /// </summary>
        Disposed,

        /// <summary>
        /// User has been disconnected
        /// </summary>
        Disconnected,

        /// <summary>
        /// User has quit the lobby
        /// </summary>
        Quit,

        /// <summary>
        /// Kicked
        /// </summary>
        Kicked,

        /// <summary>
        /// Banned
        /// </summary>
        Banned,

        /// <summary>
        /// Lobby has been closed
        /// </summary>
        LobbyClosed
    }
}
