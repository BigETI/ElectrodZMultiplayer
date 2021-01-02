/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Used to signal a peer connection attempt
    /// </summary>
    /// <param name="peer">Connecting peer</param>
    public delegate void PeerConnectionAttemptedDelegate(IPeer peer);
}
