using Newtonsoft.Json;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes an authenticate message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class AuthenticateMessageData : BaseMessageData
    {
        /// <summary>
        /// Existing authentification token
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }

        /// <summary>
        /// Constructs an authenticate message for deserializers
        /// </summary>
        public AuthenticateMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs an authenticate message
        /// </summary>
        /// <param name="token">Existing authentification token</param>
        public AuthenticateMessageData(string token) : base(Naming.GetMessageTypeNameFromMessageDataType<AuthenticateMessageData>()) => Token = token;
    }
}
