using ElectrodZMultiplayer.Data.Messages;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// This is being used to signal when listing lobbies has failed
    /// </summary>
    /// <param name="peer">Peer</param>
    /// <param name="message">Received essage</param>
    /// <param name="reason">Reason</param>
    public delegate void ListAvailableGameModesFailedDelegate(IPeer peer, ListAvailableGameModesMessageData message, EListAvailableGameModesFailedReason reason);
}
