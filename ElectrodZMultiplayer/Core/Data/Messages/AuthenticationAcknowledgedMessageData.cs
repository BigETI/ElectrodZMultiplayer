using ElectrodZMultiplayer.JSONConverters;
using ElectrodZMultiplayer.Server;
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
        /// Authentication token
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }

        /// <summary>
        /// User GUID
        /// </summary>
        [JsonProperty("guid")]
        [JsonConverter(typeof(GUIDJSONConverter))]
        public Guid GUID { get; set; }

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
        /// <param name="user">User being authenticated</param>
        public AuthenticationAcknowledgedMessageData(IServerUser user) : base(Naming.GetMessageTypeNameFromMessageDataType<AuthenticationAcknowledgedMessageData>())
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (!user.IsValid)
            {
                throw new ArgumentException("User is not valid", nameof(user));
            }
            Token = user.Token;
            GUID = user.GUID;
        }
    }
}
