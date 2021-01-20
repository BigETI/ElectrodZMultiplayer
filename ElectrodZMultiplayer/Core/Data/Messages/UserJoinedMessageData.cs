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
    /// A class that describes a message that informs an user joining the current lobby
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class UserJoinedMessageData : BaseMessageData
    {
        /// <summary>
        /// Joining user GUID
        /// </summary>
        [JsonProperty("guid")]
        [JsonConverter(typeof(GUIDJSONConverter))]
        public Guid GUID { get; set; }

        /// <summary>
        /// Joining user game color
        /// </summary>
        [JsonProperty("gameColor")]
        public EGameColor GameColor { get; set; }

        /// <summary>
        /// Joining username
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Joining user lobby color
        /// </summary>
        [JsonProperty("lobbyColor")]
        [JsonConverter(typeof(ColorJSONConverter))]
        public Color LobbyColor { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (GUID != Guid.Empty) &&
            (GameColor != EGameColor.Unknown) &&
            (Name != null);

        /// <summary>
        /// Constructs a message informing a user joining the lobby for deserializers
        /// </summary>
        public UserJoinedMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a message informing a user joining the lobby
        /// </summary>
        /// <param name="user">Joining user</param>
        public UserJoinedMessageData(IUser user) : base(Naming.GetMessageTypeNameFromMessageDataType<UserJoinedMessageData>())
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
            GameColor = user.GameColor;
            Name = user.Name;
            LobbyColor = user.LobbyColor;
        }
    }
}
