using ElectrodZMultiplayer.JSONConverters;
using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a message informing about an user leaving the lobby
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class UserLeftMessageData : BaseMessageData
    {
        /// <summary>
        /// Leaving user GUID
        /// </summary>
        [JsonProperty("guid")]
        [JsonConverter(typeof(GUIDJSONConverter))]
        public Guid GUID { get; set; }

        /// <summary>
        /// Reason for leaving the lobby
        /// </summary>
        [JsonProperty("reason")]
        public string Reason { get; set; }

        /// <summary>
        /// Is valid
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (GUID != Guid.Empty) &&
            (Reason != null);

        /// <summary>
        /// Constructs a message informing about an useer leaving the lobby for deserializers
        /// </summary>
        public UserLeftMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a message informing about an useer leaving the lobby
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="reason">Reason</param>
        public UserLeftMessageData(IUser user, string reason) : base(Naming.GetMessageTypeNameFromMessageDataType<UserLeftMessageData>())
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (!user.IsValid)
            {
                throw new ArgumentException("User is not valid.", nameof(user));
            }
            GUID = user.GUID;
            Reason = reason ?? throw new ArgumentNullException(nameof(reason));
        }
    }
}
