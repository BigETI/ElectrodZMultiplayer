/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// A delegate to describe a method to check if a collection contains atleast the specified element
    /// </summary>
    /// <typeparam name="T">Element type</typeparam>
    /// <param name="element">Element</param>
    /// <returns>"true" if element is contained in collection, otherwise "false"</returns>
    public delegate bool ContainsDelegate<T>(T element);
}
