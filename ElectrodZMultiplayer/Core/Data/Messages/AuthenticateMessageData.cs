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
    public class AuthenticateMessageData : BaseMessageData
    {
        /// <summary>
        /// Used API version
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; }

        /// <summary>
        /// Existing authentification token
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (Version == Defaults.apiVersion);

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
        public AuthenticateMessageData(string token = null) : base(Naming.GetMessageTypeNameFromMessageDataType<AuthenticateMessageData>())
        {
            Version = Defaults.apiVersion;
            Token = token;
        }
    }
}
