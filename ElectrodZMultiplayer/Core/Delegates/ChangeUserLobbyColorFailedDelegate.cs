using ElectrodZMultiplayer.Data.Messages;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// This is being used to signal when changing user lobby color has failed
    /// </summary>
    /// <param name="peer">Peer</param>
    /// <param name="message">Received message</param>
    /// <param name="reason">Reason</param>
    public delegate void ChangeUserLobbyColorFailedDelegate(IPeer peer, ChangeUserLobbyColorMessageData message, EChangeUserLobbyColorFailedReason reason);
}
