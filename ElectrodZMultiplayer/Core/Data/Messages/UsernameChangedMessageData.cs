using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a message informing about an user changing their username
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class UsernameChangedMessageData : BaseMessageData
    {
        /// <summary>
        /// User GUID
        /// </summary>
        [JsonProperty("guid")]
        public Guid GUID { get; set; }

        /// <summary>
        /// New username
        /// </summary>
        [JsonProperty("newUsername")]
        public string NewUsername { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (GUID != Guid.Empty) &&
            (NewUsername != null);

        /// <summary>
        /// Constructs a message informing about an user changing their username for deserializers
        /// </summary>
        public UsernameChangedMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a message informing about an user changing their username
        /// </summary>
        /// <param name="user">User</param>
        public UsernameChangedMessageData(IUser user) : base(Naming.GetMessageTypeNameFromMessageDataType<UsernameChangedMessageData>())
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
            NewUsername = user.Name;
        }
    }
}
