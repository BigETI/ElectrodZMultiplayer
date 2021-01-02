/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Used to signal a fail at parsing a message
    /// </summary>
    /// <typeparam name="T">Message type</typeparam>
    /// <param name="peer">Sending peer</param>
    /// <param name="expectedMessageType">Expected message type</param>
    /// <param name="message">Message</param>
    /// <param name="json">Message JSON</param>
    public delegate void MessageParseFailedDelegate<T>(IPeer peer, string expectedMessageType, T message, string json) where T : IBaseMessageData;
}
