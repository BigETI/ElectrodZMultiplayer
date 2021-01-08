/// <summary>
/// ElectrodZ multiplayeer client namespace
/// </summary>
namespace ElectrodZMultiplayer.Client
{
    /// <summary>
    /// This is being used to assert a user being targeted in a lobby
    /// </summary>
    /// <param name="clientUser">Client user</param>
    /// <param name="clientLobby">Client lobby</param>
    /// <param name="targetUser">Target user</param>
    internal delegate void TargetLobbyUserDelegate(IInternalClientUser clientUser, IInternalClientLobby clientLobby, IInternalClientUser targetUser);
}
