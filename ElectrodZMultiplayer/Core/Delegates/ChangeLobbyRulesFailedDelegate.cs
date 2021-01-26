using ElectrodZMultiplayer.Data.Messages;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// This is being used to signal when changing lobby rules have failed
    /// </summary>
    /// <param name="peer">Peer</param>
    /// <param name="message">Received message</param>
    /// <param name="reason">Reason</param>
    public delegate void ChangeLobbyRulesFailedDelegate(IPeer peer, ChangeLobbyRulesMessageData message, EChangeLobbyRulesFailedReason reason);
}
