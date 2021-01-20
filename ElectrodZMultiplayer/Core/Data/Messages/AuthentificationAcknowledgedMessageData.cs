using ElectrodZMultiplayer.JSONConverters;
using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes an authentification acknowledgment message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class AuthentificationAcknowledgedMessageData : BaseMessageData
    {
        /// <summary>
        /// User GUID
        /// </summary>
        [JsonProperty("guid")]
        [JsonConverter(typeof(GUIDJSONConverter))]
        public Guid GUID { get; set; }

        /// <summary>
        /// Authentification token
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
        /// Constructs an authentification acknowledgment message for deserializers
        /// </summary>
        public AuthentificationAcknowledgedMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs an authentification acknowledgment message
        /// </summary>
        /// <param name="guid">User GUID</param>
        /// <param name="token">User token</param>
        public AuthentificationAcknowledgedMessageData(Guid guid, string token) : base(Naming.GetMessageTypeNameFromMessageDataType<AuthentificationAcknowledgedMessageData>())
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
