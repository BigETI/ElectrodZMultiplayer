/// <summary>
/// ElectrodZ multiplayer client namespace
/// </summary>
namespace ElectrodZMultiplayer.Client
{
    /// <summary>
    /// This is being used to assert an user being authentificated
    /// </summary>
    /// <param name="clientUser">Client user</param>
    internal delegate void UserIsAuthentificatedDelegate(IInternalClientUser clientUser);
}
