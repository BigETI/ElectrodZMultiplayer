/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// An interface that describes the base of any sent or received data
    /// </summary>
    public interface IBaseMessageData : IValidable
    {
        /// <summary>
        /// Message type
        /// </summary>
        string MessageType { get; }
    }
}
