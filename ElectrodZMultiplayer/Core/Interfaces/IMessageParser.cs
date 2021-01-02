/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// An interface that describes a generalized message parser
    /// </summary>
    /// <typeparam name="T">Message type</typeparam>
    public interface IMessageParser<T> : IBaseMessageParser where T : IBaseMessageData
    {
        /// <summary>
        /// On message parsed
        /// </summary>
        event MessageParsedDelegate<T> OnMessageParsed;

        /// <summary>
        /// Om message parse failed
        /// </summary>
        event MessageParseFailedDelegate<T> OnMessageParseFailed;
    }
}
