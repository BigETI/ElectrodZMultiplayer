using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ server namespace
/// </summary>
namespace ElectrodZServer
{
    /// <summary>
    /// An abstract class that describes a generalized command
    /// </summary>
    internal abstract class ACommand : ICommand
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Command arguments
        /// </summary>
        public abstract IReadOnlyList<CommandArgument> Arguments { get; }

        /// <summary>
        /// Description
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// Help topic
        /// </summary>
        public abstract string HelpTopic { get; }

        /// <summary>
        /// Alias to
        /// </summary>
        public abstract ICommand AliasTo { get; }

        /// <summary>
        /// Constructs a generalized command
        /// </summary>
        /// <param name="name">Command name</param>
        public ACommand(string name) => Name = name ?? throw new ArgumentNullException(nameof(name));

        /// <summary>
        /// Is alias of command
        /// </summary>
        /// <param name="command">Command</param>
        /// <returns>"true" if this command is an alias of the specified command, otherwise "false"</returns>
        public bool IsAliasOf(ICommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            bool ret = false;
            if (command.Name != Name)
            {
                ICommand alias_to = AliasTo;
                while (alias_to != null)
                {
                    if (command.Name == alias_to.Name)
                    {
                        ret = true;
                        break;
                    }
                    alias_to = alias_to.AliasTo;
                }
            }
            return ret;
        }

        /// <summary>
        /// Executes command
        /// </summary>
        /// <param name="arguments">Command arguments</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        public abstract bool Execute(IReadOnlyList<string> arguments);
    }
}
