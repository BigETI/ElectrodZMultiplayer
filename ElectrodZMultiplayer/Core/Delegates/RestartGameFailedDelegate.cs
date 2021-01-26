using ElectrodZMultiplayer.Data.Messages;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// This is being used to signal when restarting a game has failed
    /// </summary>
    /// <param name="peer">Peer</param>
    /// <param name="message">Received message</param>
    /// <param name="reason">Reason</param>
    public delegate void RestartGameFailedDelegate(IPeer peer, RestartGameMessageData message, ERestartGameFailedReason reason);
}
