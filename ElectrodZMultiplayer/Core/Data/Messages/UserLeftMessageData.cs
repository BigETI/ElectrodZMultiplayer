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
        /// Kicked by user GUID
        /// </summary>
        [JsonProperty("kickedByGUID")]
        [JsonConverter(typeof(GUIDJSONConverter))]
        public Guid? KickedByGUID { get; set; }

        /// <summary>
        /// Reason for leaving the lobby
        /// </summary>
        [JsonProperty("reason")]
        public EDisconnectionReason Reason { get; set; }

        /// <summary>
        /// Leave message
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }

        /// <summary>
        /// Is valid
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (GUID != Guid.Empty) &&
            ((KickedByGUID == null) || (KickedByGUID != Guid.Empty)) &&
            (Reason != EDisconnectionReason.Invalid);

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
        /// <param name="message">Message</param>
        public UserLeftMessageData(IUser user, EDisconnectionReason reason, string message) : base(Naming.GetMessageTypeNameFromMessageDataType<UserLeftMessageData>())
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (!user.IsValid)
            {
                throw new ArgumentException("User is not valid.", nameof(user));
            }
            if (reason == EDisconnectionReason.Invalid)
            {
                throw new ArgumentException("Reason can't be invalid.", nameof(reason));
            }
            GUID = user.GUID;
            Reason = reason;
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }
    }
}
