using ElectrodZMultiplayer.JSONConverters;
using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes an acknowledgment message for an authentication attempt
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class AuthenticationAcknowledgedMessageData : BaseMessageData
    {
        /// <summary>
        /// User GUID
        /// </summary>
        [JsonProperty("guid")]
        [JsonConverter(typeof(GUIDJSONConverter))]
        public Guid GUID { get; set; }

        /// <summary>
        /// Authentication token
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (Token != null) &&
            (GUID != Guid.Empty);

        /// <summary>
        /// Constructs an acknowledgment message for an authentication attempt for deserializers
        /// </summary>
        public AuthenticationAcknowledgedMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs an acknowledgment message for an authentication attempt
        /// </summary>
        /// <param name="guid">User GUID</param>
        /// <param name="user">User being authenticated</param>
        public AuthenticationAcknowledgedMessageData(Guid guid, string token) : base(Naming.GetMessageTypeNameFromMessageDataType<AuthenticationAcknowledgedMessageData>())
        {
            if (guid == Guid.Empty)
            {
                throw new ArgumentException("User GUID is empty.", nameof(guid));
            }
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentNullException(nameof(token));
            }
            GUID = guid;
            Token = token;
        }
    }
}
