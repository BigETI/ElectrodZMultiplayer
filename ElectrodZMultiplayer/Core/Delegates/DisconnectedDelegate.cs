/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Used to signal a peer being disconnected
    /// </summary>
    /// <param name="peer">Disconnected peer</param>
    /// <param name="reason">Disconnection reason</param>
    public delegate void DisconnectedDelegate(IPeer peer, EDisconnectionReason reason);
}
