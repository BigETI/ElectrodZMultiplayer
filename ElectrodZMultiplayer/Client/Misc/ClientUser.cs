using System;
using System.Drawing;

/// <summary>
/// ElectrodZ multiplayer client namespace
/// </summary>
namespace ElectrodZMultiplayer.Client
{
    /// <summary>
    /// A class that describes an user specific to a client
    /// </summary>
    internal class ClientUser : Entity, IInternalClientUser
    {
        /// <summary>
        /// Client lobby
        /// </summary>
        public IInternalClientLobby ClientLobby { get; set; }

        /// <summary>
        /// Lobby
        /// </summary>
        public ILobby Lobby => ClientLobby;

        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; private set; } = string.Empty;

        /// <summary>
        /// Lobby color
        /// </summary>
        public Color LobbyColor { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="guid">Entity GUID</param>
        /// <param name="gameColor">Game color</param>
        public ClientUser(Guid guid, EGameColor gameColor) : base(guid, gameColor)
        {
            // ...
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="guid">Entity GUID</param>
        /// <param name="gameColor">Entity game color</param>
        /// <param name="name">Username</param>
        /// <param name="lobbyColor">User lobby color</param>
        public ClientUser(Guid guid, EGameColor gameColor, string name, Color lobbyColor) : base(guid, gameColor)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            LobbyColor = lobbyColor;
        }

        /// <summary>
        /// Sets a new username internally
        /// </summary>
        /// <param name="name">Username</param>
        public void SetNameInternally(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            string new_name = name.Trim();
            if ((new_name.Length < Defaults.minimalUsernameLength) || (new_name.Length > Defaults.maximalUsernameLength))
            {
                throw new ArgumentException($"Username must be between { Defaults.minimalUsernameLength } and { Defaults.maximalUsernameLength } characters long.", nameof(name));
            }
            Name = new_name;
        }

        /// <summary>
        /// Sets a new lobby color internally
        /// </summary>
        /// <param name="lobbyColor">Lobby color</param>
        public void SetLobbyColorInternally(Color lobbyColor) => LobbyColor = lobbyColor;
    }
}
