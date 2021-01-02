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
    public delegate void UserLeftDelegate(IUser user, string reason);
}
