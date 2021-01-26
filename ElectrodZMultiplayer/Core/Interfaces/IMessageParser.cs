/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// An interface that represents a generalized message parser
    /// </summary>
    /// <typeparam name="T">Message type</typeparam>
    public interface IMessageParser<T> : IBaseMessageParser where T : IBaseMessageData
    {
        /// <summary>
        /// On message parsed
        /// </summary>
        event MessageParsedDelegate<T> OnMessageParsed;

        /// <summary>
        /// On message validation failed
        /// </summary>
        event MessageValidationFailedDelegate<T> OnMessageValidationFailed;

        /// <summary>
        /// Om message parse failed
        /// </summary>
        event MessageParseFailedDelegate OnMessageParseFailed;
    }
}
