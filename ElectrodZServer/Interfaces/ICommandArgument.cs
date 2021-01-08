/// <summary>
/// ElectrodZ server namespace
/// </summary>
namespace ElectrodZServer
{
    /// <summary>
    /// An interface that represents a command argument
    /// </summary>
    internal interface ICommandArgument
    {
        /// <summary>
        /// Command argument name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Is required
        /// </summary>
        bool IsRequired { get; }
    }
}
