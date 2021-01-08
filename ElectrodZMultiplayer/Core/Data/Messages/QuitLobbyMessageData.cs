using Newtonsoft.Json;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a quit lobby request message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class QuitLobbyMessageData : BaseMessageData
    {
        /// <summary>
        /// Constructs a quit lobby request message
        /// </summary>
        public QuitLobbyMessageData() : base(Naming.GetMessageTypeNameFromMessageDataType<QuitLobbyMessageData>())
        {
            // ...
        }
    }
}
