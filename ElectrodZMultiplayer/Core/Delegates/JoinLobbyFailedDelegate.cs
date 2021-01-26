using ElectrodZMultiplayer.Data.Messages;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// This is being used to signal when joining a lobby has failed
    /// </summary>
    /// <param name="peer">Peer</param>
    /// <param name="message">Received message</param>
    /// <param name="reason">Reason</param>
    public delegate void JoinLobbyFailedDelegate(IPeer peer, JoinLobbyMessageData message, EJoinLobbyFailedReason reason);
}
