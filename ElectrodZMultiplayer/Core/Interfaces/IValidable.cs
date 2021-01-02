/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// An interface that describes an object that can be validated
    /// </summary>
    public interface IValidable
    {
        /// <summary>
        /// Is object in a valid state
        /// </summary>
        bool IsValid { get; }
    }
}
