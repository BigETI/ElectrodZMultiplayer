using ElectrodZMultiplayer.JSONConverters;
using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a message informing about an user changing their game color
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class UserGameColorChangedMessageData : BaseMessageData
    {
        /// <summary>
        /// User GUID
        /// </summary>
        [JsonProperty("guid")]
        [JsonConverter(typeof(GUIDJSONConverter))]
        public Guid GUID { get; set; }

        /// <summary>
        /// New game lobby color
        /// </summary>
        [JsonProperty("newGameColor")]
        [JsonConverter(typeof(GameColorJSONConverter))]
        public EGameColor NewGameColor { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (GUID != Guid.Empty) &&
            (NewGameColor != EGameColor.Unknown);

        /// <summary>
        /// Constructs a message informing about an user changing their game color for deserializers
        /// </summary>
        public UserGameColorChangedMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a message informing about an user changing their game color
        /// </summary>
        /// <param name="user">User</param>
        public UserGameColorChangedMessageData(IUser user) : base(Naming.GetMessageTypeNameFromMessageDataType<UserGameColorChangedMessageData>())
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
            NewGameColor = user.GameColor;
        }
    }
}
