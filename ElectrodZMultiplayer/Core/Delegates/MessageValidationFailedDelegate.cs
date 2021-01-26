/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Used to signal when validation on a message has failed
    /// </summary>
    /// <typeparam name="T">Message tyoe</typeparam>
    /// <param name="peer">Peer</param>
    /// <param name="message">Message</param>
    /// <param name="json">JSON</param>
    public delegate void MessageValidationFailedDelegate<T>(IPeer peer, T message, string json) where T : IBaseMessageData;
}
