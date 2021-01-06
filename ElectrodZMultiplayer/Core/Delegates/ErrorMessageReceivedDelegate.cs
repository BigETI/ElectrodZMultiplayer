/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Used to signal that an error message has been received
    /// </summary>
    /// <param name="errorType">Error type</param>
    /// <param name="message">Error message</param>
    public delegate void ErrorMessageReceivedDelegate(EErrorType errorType, string message);
}
