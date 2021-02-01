using ElectrodZMultiplayer.Data.Messages;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Used to signal a cancel start, restart or stop game timer fail.
    /// </summary>
    /// <param name="peer">Peer</param>
    /// <param name="message">Received message</param>
    /// <param name="reason">Reason</param>
    public delegate void CancelStartRestartStopGameTimerFailedDelegate(IPeer peer, CancelStartRestartStopGameTimerMessageData message, ECancelStartRestartStopGameTimerFailedReason reason);
}
