using ElectrodZMultiplayer.JSONConverters;
using Newtonsoft.Json;
using System;
using System.Drawing;

/// <summary>
/// ElectrodZ multiplayer data namespace
/// </summary>
namespace ElectrodZMultiplayer.Data
{
    /// <summary>
    /// A class that describes user data
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class UserData : IValidable
    {
        /// <summary>
        /// User GUID
        /// </summary>
        [JsonProperty("guid")]
        [JsonConverter(typeof(GUIDJSONConverter))]
        public Guid GUID { get; set; }

        /// <summary>
        /// Current user game color
        /// </summary>
        [JsonProperty("gameColor")]
        [JsonConverter(typeof(GameColorJSONConverter))]
        public EGameColor GameColor { get; set; }

        /// <summary>
        /// Current username
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Current user lobby color
        /// </summary>
        [JsonProperty("lobbyColor")]
        [JsonConverter(typeof(ColorJSONConverter))]
        public Color LobbyColor { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public bool IsValid =>
            (GUID != Guid.Empty) &&
            (GameColor != EGameColor.Unknown) &&
            (Name != null) &&
            (Name.Trim().Length >= Defaults.minimalUsernameLength) &&
            (Name.Trim().Length <= Defaults.maximalUsernameLength);

        /// <summary>
        /// Constructs user data for deserializers
        /// </summary>
        public UserData()
        {
            // ...
        }

        /// <summary>
        /// Constructs user data
        /// </summary>
        /// <param name="guid">User GUID</param>
        /// <param name="gameColor">Current user game color</param>
        /// <param name="name">Current username</param>
        /// <param name="lobbyColor">Current user lobby color</param>
        public UserData(Guid guid, EGameColor gameColor, string name, Color lobbyColor)
        {
            if (guid == Guid.Empty)
            {
                throw new ArgumentException("User GUID is empty.", nameof(guid));
            }
            if (gameColor == EGameColor.Unknown)
            {
                throw new ArgumentException("User game color is unknown.", nameof(gameColor));
            }
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            string new_name = name.Trim();
            if ((new_name.Length < Defaults.minimalUsernameLength) || (new_name.Length > Defaults.maximalUsernameLength))
            {
                throw new ArgumentException($"Username must be between { Defaults.minimalUsernameLength } and { Defaults.maximalUsernameLength } characters long.", nameof(name));
            }
            GUID = guid;
            GameColor = gameColor;
            Name = new_name;
            LobbyColor = lobbyColor;
        }
    }
}
