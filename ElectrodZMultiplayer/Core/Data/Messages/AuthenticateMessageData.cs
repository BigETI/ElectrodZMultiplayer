using Newtonsoft.Json;
using System;

/// <summary>
/// ElectordZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes an authentication message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class AuthenticateMessageData : BaseMessageData
    {
        /// <summary>
        /// Existing authentication token
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }

        /// <summary>
        /// Constructs an authentication message for deserializers
        /// </summary>
        public AuthenticateMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs an authentication message
        /// </summary>
        /// <param name="token">Existing authentication token</param>
        public AuthenticateMessageData(string token) : base(Naming.GetMessageTypeNameFromMessageDataType<AuthenticateMessageData>()) => Token = token;
    }
}
