using Newtonsoft.Json;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a message informing a game being restarted
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class GameRestartedMessageData : BaseMessageData
    {
        /// <summary>
        /// Constructs a message that informs a game being restarted
        /// </summary>
        public GameRestartedMessageData() : base(Naming.GetMessageTypeNameFromMessageDataType<GameRestartedMessageData>())
        {
            // ...
        }
    }
}
