using ElectrodZMultiplayer.JSONConverters;
using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a message to kick an user
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class KickUserMessageData : BaseMessageData
    {
        /// <summary>
        /// User GUID to kick
        /// </summary>
        [JsonProperty("userGUID")]
        [JsonConverter(typeof(GUIDJSONConverter))]
        public Guid UserGUID { get; set; }

        /// <summary>
        /// Kick reason
        /// </summary>
        [JsonProperty("reason")]
        public string Reason { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (UserGUID != Guid.Empty) &&
            (Reason != null);

        /// <summary>
        /// Constructs a message to kick an user for deserializers
        /// </summary>
        public KickUserMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a message to kick an user
        /// </summary>
        /// <param name="userGUID">User GUID to kick</param>
        /// <param name="reason">Kick reason</param>
        public KickUserMessageData(Guid userGUID, string reason) : base(Naming.GetMessageTypeNameFromMessageDataType<KickUserMessageData>())
        {
            if (userGUID == Guid.Empty)
            {
                throw new ArgumentException("User GUID is empty.", nameof(userGUID));
            }
            UserGUID = userGUID;
            Reason = reason ?? throw new ArgumentNullException(nameof(reason));
        }
    }
}
