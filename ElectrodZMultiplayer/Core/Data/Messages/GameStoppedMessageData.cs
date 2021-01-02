using Newtonsoft.Json;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a message informing a game being stopped
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class GameStoppedMessageData : BaseMessageData
    {
        /// <summary>
        /// Constructs a message that informs a game being stopped
        /// </summary>
        public GameStoppedMessageData() : base(Naming.GetMessageTypeNameFromMessageDataType<GameStoppedMessageData>())
        {
            // ...
        }
    }
}
