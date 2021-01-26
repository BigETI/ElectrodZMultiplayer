using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a user changing their username as a message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ChangeUsernameMessageData : BaseMessageData
    {
        /// <summary>
        /// New username
        /// </summary>
        [JsonProperty("newUsername")]
        public string NewUsername { get; set; }

        /// <summary>
        /// Is valid
        /// </summary>
        /// <returns>"true" if valid, otherwise "false"</returns>
        public override bool IsValid =>
            base.IsValid &&
            (NewUsername != null) &&
            (NewUsername.Trim().Length >= Defaults.minimalUsernameLength) &&
            (NewUsername.Trim().Length <= Defaults.maximalUsernameLength);

        /// <summary>
        /// Constructs a username change message for deserializers
        /// </summary>
        public ChangeUsernameMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a username change message
        /// </summary>
        /// <param name="newUsername">New username</param>
        public ChangeUsernameMessageData(string newUsername) : base(Naming.GetMessageTypeNameFromMessageDataType<ChangeUsernameMessageData>())
        {
            if (newUsername == null)
            {
                throw new ArgumentNullException(nameof(newUsername));
            }
            string new_username = newUsername.Trim();
            if ((new_username.Length < Defaults.minimalUsernameLength) || (new_username.Length > Defaults.maximalUsernameLength))
            {
                throw new ArgumentException($"Username must be between { Defaults.minimalUsernameLength } and { Defaults.maximalUsernameLength } characters long.", nameof(newUsername));
            }
            NewUsername = new_username;
        }
    }
}
