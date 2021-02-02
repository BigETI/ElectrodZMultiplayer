using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a server game loading process finished message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ServerGameLoadingProcessFinishedMessageData : BaseMessageData
    {
        /// <summary>
        /// User GUID
        /// </summary>
        [JsonProperty("userGUID")]
        public Guid UserGUID { get; set; }

        /// <summary>
        /// Constructs a server game loading process finished message for deserializers
        /// </summary>
        public ServerGameLoadingProcessFinishedMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a server game loading process finished message
        /// </summary>
        /// <param name="user">User</param>
        public ServerGameLoadingProcessFinishedMessageData(IUser user) : base(Naming.GetMessageTypeNameFromMessageDataType<ServerGameLoadingProcessFinishedMessageData>())
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if (!user.IsValid)
            {
                throw new ArgumentException("User is not valid.", nameof(user));
            }
            UserGUID = user.GUID;
        }
    }
}
