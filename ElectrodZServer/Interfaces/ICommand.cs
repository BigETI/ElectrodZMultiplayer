using System.Collections.Generic;

/// <summary>
/// ElectrodZ server namespace
/// </summary>
namespace ElectrodZServer
{
    /// <summary>
    /// An interface that represents a command
    /// </summary>
    internal interface ICommand
    {
        /// <summary>
        /// Command name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Command arguments
        /// </summary>
        IReadOnlyList<CommandArgument> Arguments { get; }

        /// <summary>
        /// Command description
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Help topic
        /// </summary>
        string HelpTopic { get; }

        /// <summary>
        /// Alias to command
        /// </summary>
        ICommand AliasTo { get; }

        /// <summary>
        /// Is alias of command
        /// </summary>
        /// <param name="command">Command</param>
        /// <returns>"true" if this command is an alias of the specified command, otherwise "false"</returns>
        bool IsAliasOf(ICommand command);

        /// <summary>
        /// Execute command
        /// </summary>
        /// <param name="arguments">Command arguments</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        bool Execute(IReadOnlyList<string> arguments);
    }
}
