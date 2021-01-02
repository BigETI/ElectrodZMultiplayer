/// <summary>
/// ElectrodZ multiplayer namesapce
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Used to signal a message being successfully parsed
    /// </summary>
    /// <typeparam name="T">Message type</typeparam>
    /// <param name="peer">Peer</param>
    /// <param name="message">Message</param>
    /// <param name="json">JSON</param>
    public delegate void MessageParsedDelegate<T>(IPeer peer, T message, string json) where T : IBaseMessageData;
}
