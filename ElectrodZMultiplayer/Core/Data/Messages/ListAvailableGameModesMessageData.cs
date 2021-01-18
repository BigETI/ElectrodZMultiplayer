using Newtonsoft.Json;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a list available game modes message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ListAvailableGameModesMessageData : BaseMessageData
    {
        /// <summary>
        /// Game mode name filter
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Constructs a list available game modes message for serializers
        /// </summary>
        public ListAvailableGameModesMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a list available game modes message
        /// </summary>
        /// <param name="name">Game mode name filter</param>
        public ListAvailableGameModesMessageData(string name) : base(Naming.GetMessageTypeNameFromMessageDataType<ListAvailableGameModesMessageData>()) => Name = name;
    }
}
