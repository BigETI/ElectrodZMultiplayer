using Newtonsoft.Json;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a client game loading process finished message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ClientGameLoadingProcessFinishedMessageData : BaseMessageData
    {
        /// <summary>
        /// Constructs a client game loading process finished message
        /// </summary>
        public ClientGameLoadingProcessFinishedMessageData() : base(Naming.GetMessageTypeNameFromMessageDataType<ClientGameLoadingProcessFinishedMessageData>())
        {
            // ...
        }
    }
}
