using System;
using System.Collections.Generic;
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
        /// This event will be invoked when the username changes.
        /// </summary>
        public event UsernameUpdatedDelegate OnUsernameUpdated;

        /// <summary>
        /// This event will be invoked when the user lobby color changes.
        /// </summary>
        public event UserLobbyColorUpdatedDelegate OnUserLobbyColorUpdated;

        /// <summary>
        /// This event will be invoked when the user finished loading their game.
        /// </summary>
        public event GameLoadingFinishedDelegate OnGameLoadingFinished;

        /// <summary>
        /// This event will be invoked when a client tick has been performed.
        /// </summary>
        public event ClientTickedDelegate OnClientTicked;

        /// <summary>
        /// This event will be invoked when a server tick has been performed.
        /// </summary>
        public event ServerTickedDelegate OnServerTicked;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="guid">Entity GUID</param>
        public ClientUser(Guid guid) : base(guid, Defaults.playerEntityType)
        {
            // ...
        }

        /// <summary>
        /// Constructs a client user
        /// </summary>
        /// <param name="guid">Entity GUID</param>
        /// <param name="gameColor">Entity game color</param>
        /// <param name="name">Username</param>
        /// <param name="lobbyColor">User lobby color</param>
        public ClientUser(Guid guid, EGameColor gameColor, string name, Color lobbyColor) : base(guid, Defaults.playerEntityType, gameColor, Vector3.Zero, Quaternion.Identity, Vector3.Zero, Vector3.Zero, Array.Empty<string>(), false)
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
            OnUsernameUpdated?.Invoke();
        }

        /// <summary>
        /// Sets a new lobby color internally
        /// </summary>
        /// <param name="lobbyColor">Lobby color</param>
        public void SetLobbyColorInternally(Color lobbyColor)
        {
            LobbyColor = lobbyColor;
            OnUserLobbyColorUpdated?.Invoke();
        }

        /// <summary>
        /// Invokes the client ticked event
        /// </summary>
        /// <param name="entityDeltas">Entity deltas</param>
        /// <param name="hits">Hits</param>
        public void InvokeClientTickedEvent(IEnumerable<IEntityDelta> entityDeltas, IEnumerable<IHit> hits) => OnClientTicked?.Invoke(entityDeltas, hits);

        /// <summary>
        /// Invokes the server game loading process finished event
        /// </summary>
        public void InvokeServerGameLoadingProcessFinishedEvent() => OnGameLoadingFinished?.Invoke();

        /// <summary>
        /// Invokes the server ticked event
        /// </summary>
        /// <param name="time">Time in seconds elapsed since game start</param>
        /// <param name="entityDeltas">Entity deltas</param>
        /// <param name="hits">Hits</param>
        public void InvokeServerTickedEvent(double time, IEnumerable<IEntityDelta> entityDeltas, IEnumerable<IHit> hits) => OnServerTicked?.Invoke(time, entityDeltas, hits);
    }
}
