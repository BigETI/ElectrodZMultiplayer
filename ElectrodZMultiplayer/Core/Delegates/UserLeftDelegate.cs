/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Used to signal an user leaving the lobby
    /// </summary>
    /// <param name="user">Leaving user</param>
    /// <param name="reason">Reason to leave</param>
    /// <param name="message">Leave message</param>
    public delegate void UserLeftDelegate(IUser user, EDisconnectionReason reason, string message);
}
