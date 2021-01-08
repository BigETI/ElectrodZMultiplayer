/// <summary>
/// ElectrodZ multiplayer client namespace
/// </summary>
namespace ElectrodZMultiplayer.Client
{
    /// <summary>
    /// This is being used to signal when an user authentication was acknowledged
    /// </summary>
    /// <param name="user">Authenticated user</param>
    public delegate void AuthenticationAcknowledgedDelegate(IUser user);
}
