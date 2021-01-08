using System.Collections.Generic;

/// <summary>
/// ElectrodZ server namespace
/// </summary>
namespace ElectrodZServer
{
    /// <summary>
    /// An interface that represents a set of commands
    /// </summary>
    internal interface ICommands
    {
        /// <summary>
        /// Command lookup
        /// </summary>
        IReadOnlyDictionary<string, ICommand> CommandLookup { get; }

        /// <summary>
        /// Adds a command
        /// </summary>
        /// <param name="name">Command name</param>
        /// <param name="description">Description</param>
        /// <param name="helpTopic">Help topic</param>
        /// <param name="onCommandExecuted">On command executed</param>
        /// <param name="arguments">Arguments</param>
        /// <returns>Command if successful, otherwise "null"</returns>
        ICommand AddCommand(string name, string description, string helpTopic, CommandExecutedDelegate onCommandExecuted, params CommandArgument[] arguments);

        /// <summary>
        /// Adds a command alias
        /// </summary>
        /// <param name="name">Command name</param>
        /// <param name="aliasToName">Alias to command name</param>
        /// <returns>COmmand if successful, otherwise "null"</returns>
        ICommand AddAlias(string name, string aliasToName);

        /// <summary>
        /// Clears all commands
        /// </summary>
        void Clear();

        /// <summary>
        /// Parses command
        /// </summary>
        /// <param name="command">Command</param>
        /// <returns>"true" if success, otherwise "false"</returns>
        bool ParseCommand(string command);

        /// <summary>
        /// Parses commands
        /// </summary>
        /// <param name="commands">Commands</param>
        /// <param name="prefix">Prefix</param>
        /// <returns></returns>
        bool ParseCommands(string commands, string prefix);

        /// <summary>
        /// Gets help topic
        /// </summary>
        /// <param name="commandName">Command name</param>
        /// <param name="prefix">Command prefix</param>
        /// <returns>Help topic</returns>
        string GetHelpTopic(string commandName, string prefix);
    }
}
