/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Used to handle peer connection attempts
    /// </summary>
    /// <param name="peer">Connecting peer</param>
    /// <returns>"true" if connection should be established, otherwise "false"</returns>
    public delegate bool HandlePeerConnectionAttemptDelegate(IPeer peer);
}
