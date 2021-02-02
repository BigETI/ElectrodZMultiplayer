using ElectrodZMultiplayer.JSONConverters;
using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a lobby ownership changed message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class LobbyOwnershipChangedMessageData : BaseMessageData
    {
        /// <summary>
        /// New owner GUID
        /// </summary>
        [JsonProperty("newOwnerGUID")]
        [JsonConverter(typeof(GUIDJSONConverter))]
        public Guid NewOwnerGUID { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (NewOwnerGUID != Guid.Empty);

        /// <summary>
        /// Constructs a lobby ownership changed message for deserializers
        /// </summary>
        public LobbyOwnershipChangedMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a lobby ownership changed message
        /// </summary>
        /// <param name="newOwner">New lobby owner</param>
        public LobbyOwnershipChangedMessageData(IUser newOwner) : base(Naming.GetMessageTypeNameFromMessageDataType<LobbyOwnershipChangedMessageData>())
        {
            if (newOwner == null)
            {
                throw new ArgumentNullException(nameof(newOwner));
            }
            if (!newOwner.IsValid)
            {
                throw new ArgumentException("New owner is not valid.", nameof(newOwner));
            }
            NewOwnerGUID = newOwner.GUID;
        }
    }
}
