/// <summary>
/// ElectrodZ multiplayer client namespace
/// </summary>
namespace ElectrodZMultiplayer.Client
{
    /// <summary>
    /// This is being used to assert an useer being in a lobby
    /// </summary>
    /// <param name="clientUser">Client user</param>
    /// <param name="clientLobby">Client lobby</param>
    internal delegate void UserIsInLobbyDelegate(IInternalClientUser clientUser, IInternalClientLobby clientLobby);
}
