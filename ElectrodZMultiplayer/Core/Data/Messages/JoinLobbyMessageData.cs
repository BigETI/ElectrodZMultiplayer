using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a join lobby request message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class JoinLobbyMessageData : BaseMessageData
    {
        /// <summary>
        /// Lobby code
        /// </summary>
        [JsonProperty("lobbyCode")]
        public string LobbyCode { get; set; }

        /// <summary>
        /// Username
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (LobbyCode != null) &&
            (Username != null) &&
            (Username.Trim().Length >= Defaults.minimalUsernameLength) &&
            (Username.Trim().Length <= Defaults.maximalUsernameLength);

        /// <summary>
        /// Constructs a join lobby request message for deserializers
        /// </summary>
        public JoinLobbyMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a join lobby request message
        /// </summary>
        /// <param name="lobbyCode">Lobby code</param>
        /// <param name="username">Username</param>
        public JoinLobbyMessageData(string lobbyCode, string username) : base(Naming.GetMessageTypeNameFromMessageDataType<JoinLobbyMessageData>())
        {
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }
            string new_username = username.Trim();
            if ((new_username.Length < Defaults.minimalUsernameLength) || (new_username.Length > Defaults.maximalUsernameLength))
            {
                throw new ArgumentException($"Username must be between { Defaults.minimalUsernameLength } and { Defaults.maximalUsernameLength } characters long.", nameof(username));
            }
            LobbyCode = lobbyCode ?? throw new ArgumentNullException(nameof(lobbyCode));
            Username = new_username;
        }
    }
}
