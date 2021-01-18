/// <summary>
/// ElectrodZ multiplayer client namespace
/// </summary>
namespace ElectrodZMultiplayer.Client
{
    /// <summary>
    /// An interface that represents internally a generalized synchronizer specific to a client
    /// </summary>
    internal interface IInternalClientSynchronizer : IClientSynchronizer
    {
        /// <summary>
        /// Sends a message to peer
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="message">Message</param>
        void SendMessage<T>(T message) where T : IBaseMessageData;

        /// <summary>
        /// Sends a authentication message to peer
        /// </summary>
        void SendAuthenticationMessage();

        /// <summary>
        /// Sends a authentication message to peer
        /// </summary>
        /// <param name="token">Existing authentication token</param>
        void SendAuthenticationMessage(string token);
    }
}
