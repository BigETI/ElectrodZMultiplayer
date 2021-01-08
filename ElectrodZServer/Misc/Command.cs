using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ server namespace
/// </summary>
namespace ElectrodZServer
{
    /// <summary>
    /// A class that describes a command
    /// </summary>
    internal class Command : ACommand
    {
        /// <summary>
        /// Command arguments
        /// </summary>
        public override IReadOnlyList<CommandArgument> Arguments { get; }

        /// <summary>
        /// Description
        /// </summary>
        public override string Description { get; }

        /// <summary>
        /// Help topic
        /// </summary>
        public override string HelpTopic { get; }

        /// <summary>
        /// Alias to
        /// </summary>
        public override ICommand AliasTo => null;

        /// <summary>
        /// Command executed delegate
        /// </summary>
        public event CommandExecutedDelegate OnCommandExecuted;

        /// <summary>
        /// Constructs a command
        /// </summary>
        /// <param name="name">Command name</param>
        /// <param name="description">Description</param>
        /// <param name="helpTopic">Help topic</param>
        /// <param name="onCommandExecuted">On command executed</param>
        /// <param name="arguments">Command arguments</param>
        public Command(string name, string description, string helpTopic, CommandExecutedDelegate onCommandExecuted, params CommandArgument[] arguments) : base(name)
        {
            if (onCommandExecuted == null)
            {
                throw new ArgumentNullException(nameof(onCommandExecuted));
            }
            Description = description ?? throw new ArgumentNullException(nameof(description));
            HelpTopic = helpTopic ?? throw new ArgumentNullException(nameof(helpTopic));
            OnCommandExecuted += onCommandExecuted;
            Arguments = arguments ?? throw new ArgumentNullException(nameof(arguments));
        }

        /// <summary>
        /// Executes command
        /// </summary>
        /// <param name="arguments">Command arguments</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        public override bool Execute(IReadOnlyList<string> arguments)
        {
            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }
            bool ret = false;
            if (OnCommandExecuted != null)
            {
                List<string> command_arguments = new List<string>();
                for (int index = 0; index < Arguments.Count; index++)
                {
                    CommandArgument command_argument = Arguments[index];
                    if (index >= arguments.Count)
                    {
                        if (command_argument.IsRequired)
                        {
                            command_arguments.Clear();
                            command_arguments = null;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        command_arguments.Add(arguments[index]);
                    }
                }
                if (command_arguments != null)
                {
                    OnCommandExecuted(command_arguments);
                    ret = true;
                }
            }
            return ret;
        }
    }
}
