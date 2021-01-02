/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// An interface that describes a base message
    /// </summary>
    public interface IBaseMessageParser
    {
        /// <summary>
        /// Message type
        /// </summary>
        string MessageType { get; }

        /// <summary>
        /// Parses incoming message
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="json">JSON</param>
        void ParseMessage(IPeer peer, string json);
    }
}
