/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Used to signal recieving a non-meaningful messge from a peer
    /// </summary>
    /// <param name="message">Message</param>
    /// <param name="json">Message JSON</param>
    public delegate void UnknownMessageReceivedDelegate(IBaseMessageData message, string json);
}
