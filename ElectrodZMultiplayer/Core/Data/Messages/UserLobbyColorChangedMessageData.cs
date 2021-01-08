using ElectrodZMultiplayer.JSONConverters;
using Newtonsoft.Json;
using System;
using System.Drawing;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a message informing about an user changing their lobby color
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class UserLobbyColorChangedMessageData : BaseMessageData
    {
        /// <summary>
        /// User GUID
        /// </summary>
        [JsonProperty("guid")]
        [JsonConverter(typeof(GUIDJSONConverter))]
        public Guid GUID { get; set; }

        /// <summary>
        /// New user lobby color
        /// </summary>
        [JsonProperty("newLobbyColor")]
        [JsonConverter(typeof(ColorJSONConverter))]
        public Color NewLobbyColor { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (GUID != Guid.Empty);

        /// <summary>
        /// Constructs a message informing about an user changing their lobby color for deserializers
        /// </summary>
        public UserLobbyColorChangedMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a message informing about an user changing their lobby color
        /// </summary>
        /// <param name="user">User</param>
        public UserLobbyColorChangedMessageData(IUser user) : base(Naming.GetMessageTypeNameFromMessageDataType<UserLobbyColorChangedMessageData>())
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
            NewLobbyColor = user.LobbyColor;
        }
    }
}
