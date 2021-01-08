using System.Text.RegularExpressions;

/// <summary>
/// ElectrodZ multiplayer server namespace
/// </summary>
namespace ElectrodZMultiplayer.Server
{
    /// <summary>
    /// An interface that represents a ban
    /// </summary>
    public interface IBan
    {
        /// <summary>
        /// Pattern
        /// </summary>
        Regex Pattern { get; }

        /// <summary>
        /// Reason
        /// </summary>
        string Reason { get; }
    }
}
