/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Used to signal a fail at parsing a message
    /// </summary>
    /// <param name="peer">Sending peer</param>
    /// <param name="expectedMessageType">Expected message type</param>
    /// <param name="json">Message JSON</param>
    public delegate void MessageParseFailedDelegate(IPeer peer, string expectedMessageType, string json);
}
