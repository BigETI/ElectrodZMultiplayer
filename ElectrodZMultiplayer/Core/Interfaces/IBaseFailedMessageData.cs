/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// An interface that represents the base of any sent or received data that describe a failure
    /// </summary>
    internal interface IBaseFailedMessageData<TMessage, TReason> : IBaseMessageData where TMessage : IBaseMessageData where TReason : struct
    {
        /// <summary>
        /// Received message
        /// </summary>
        TMessage Message { get; }

        /// <summary>
        /// Reason
        /// </summary>
        TReason Reason { get; }
    }
}
