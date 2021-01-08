using Newtonsoft.Json;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a message informing a game being started
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class GameStartedMessageData : BaseMessageData
    {
        /// <summary>
        /// Constructs a message informaing a game being started
        /// </summary>
        public GameStartedMessageData() : base(Naming.GetMessageTypeNameFromMessageDataType<GameStartedMessageData>())
        {
            // ...
        }
    }
}
