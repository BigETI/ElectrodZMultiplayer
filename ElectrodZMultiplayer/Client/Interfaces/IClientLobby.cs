/// <summary>
/// ElectrodZ multiplayer client namespace
/// </summary>
namespace ElectrodZMultiplayer.Client
{
    /// <summary>
    /// An interface that represents a lobby specific to a client
    /// </summary>
    public interface IClientLobby : ILobby
    {
        /// <summary>
        /// Client
        /// </summary>
        IClientSynchronizer Client { get; }

        /// <summary>
        /// Leave lobby
        /// </summary>
        void Leave();
    }
}
