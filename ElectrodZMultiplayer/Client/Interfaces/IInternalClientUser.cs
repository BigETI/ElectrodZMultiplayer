using System.Collections.Generic;
using System.Drawing;

/// <summary>
/// ElectrodZ multiplayer client namespace
/// </summary>
namespace ElectrodZMultiplayer.Client
{
    /// <summary>
    /// An interface that represents internally an user specific for a client
    /// </summary>
    internal interface IInternalClientUser : IInternalEntity, IClientUser
    {
        /// <summary>
        /// Client lobby
        /// </summary>
        IInternalClientLobby ClientLobby { get; set; }

        /// <summary>
        /// Sets a new username internally
        /// </summary>
        /// <param name="name">Username</param>
        void SetNameInternally(string name);

        /// <summary>
        /// Sets a new username internally
        /// </summary>
        /// <param name="name">Username</param>
        /// <param name="isEventInvoked">Is event invoked</param>
        void SetNameInternally(string name, bool isEventInvoked);

        /// <summary>
        /// Sets a new lobby color internally
        /// </summary>
        /// <param name="lobbyColor">Lobby color</param>
        void SetLobbyColorInternally(Color lobbyColor);

        /// <summary>
        /// Sets a new lobby color internally
        /// </summary>
        /// <param name="lobbyColor">Lobby color</param>
        /// <param name="isEventInvoked">Is event invoked</param>
        void SetLobbyColorInternally(Color lobbyColor, bool isEventInvoked);

        /// <summary>
        /// Invokes the username updated event
        /// </summary>
        void InvokeUsernameUpdatedEvent();

        /// <summary>
        /// Invokes the user lobby color updated event
        /// </summary>
        void InvokeUserLobbyColorUpdatedEvent();

        /// <summary>
        /// Invokes the client ticked event
        /// </summary>
        /// <param name="entityDeltas">Entity deltas</param>
        /// <param name="hits">Hits</param>
        void InvokeClientTickedEvent(IEnumerable<IEntityDelta> entityDeltas, IEnumerable<IHit> hits);

        /// <summary>
        /// Invokes the server game loading process finished event
        /// </summary>
        void InvokeServerGameLoadingProcessFinishedEvent();

        /// <summary>
        /// Invokes the server ticked event
        /// </summary>
        /// <param name="time">Time in seconds elapsed since game start</param>
        /// <param name="entityDeltas">Entity deltas</param>
        /// <param name="hits">Hits</param>
        void InvokeServerTickedEvent(double time, IEnumerable<IEntityDelta> entityDeltas, IEnumerable<IHit> hits);
    }
}
