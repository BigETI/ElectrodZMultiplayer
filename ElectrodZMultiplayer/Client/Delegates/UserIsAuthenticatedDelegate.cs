/// <summary>
/// ElectrodZ multiplayer client namespace
/// </summary>
namespace ElectrodZMultiplayer.Client
{
    /// <summary>
    /// This is being used to assert an user being authenticated
    /// </summary>
    /// <param name="clientUser">Client user</param>
    internal delegate void UserIsAuthenticatedDelegate(IInternalClientUser clientUser);
}
