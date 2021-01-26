using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// ElectrodZ server namespace
/// </summary>
namespace ElectrodZServer
{
    /// <summary>
    /// A class that describes a set of commands
    /// </summary>
    internal class Commands : ICommands
    {
        /// <summary>
        /// Command lookup
        /// </summary>
        private readonly Dictionary<string, ICommand> commandLookup = new Dictionary<string, ICommand>();

        /// <summary>
        /// Command lookup
        /// </summary>
        public IReadOnlyDictionary<string, ICommand> CommandLookup => commandLookup;

        /// <summary>
        /// Adds a command
        /// </summary>
        /// <param name="name">Command name</param>
        /// <param name="description">Description</param>
        /// <param name="helpTopic">Help topic</param>
        /// <param name="onCommandExecuted">On command executed</param>
        /// <param name="arguments">Arguments</param>
        /// <returns>Command if successful, otherwise "null"</returns>
        public ICommand AddCommand(string name, string description, string helpTopic, CommandExecutedDelegate onCommandExecuted, params CommandArgument[] arguments)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (description == null)
            {
                throw new ArgumentNullException(nameof(description));
            }
            if (helpTopic == null)
            {
                throw new ArgumentNullException(nameof(helpTopic));
            }
            if (onCommandExecuted == null)
            {
                throw new ArgumentNullException(nameof(onCommandExecuted));
            }
            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }
            ICommand ret = null;
            string trimmed_name = name.Trim().ToLower();
            if (!commandLookup.ContainsKey(trimmed_name))
            {
                ret = new Command(trimmed_name, description, helpTopic, onCommandExecuted, arguments);
                commandLookup.Add(trimmed_name, ret);
            }
            return ret;
        }

        /// <summary>
        /// Adds a command alias
        /// </summary>
        /// <param name="name">Command name</param>
        /// <param name="aliasToName">Alias to command name</param>
        /// <returns>Command if successful, otherwise "null"</returns>
        public ICommand AddAlias(string name, string aliasToName)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            if (aliasToName == null)
            {
                throw new ArgumentNullException(nameof(aliasToName));
            }
            ICommand ret = null;
            string trimmed_name = name.Trim().ToLower();
            string trimmed_alias_to_name = aliasToName.Trim().ToLower();
            if ((trimmed_name != trimmed_alias_to_name) && commandLookup.ContainsKey(trimmed_alias_to_name) && !commandLookup.ContainsKey(trimmed_name))
            {
                ret = new CommandAlias(trimmed_name, commandLookup[trimmed_alias_to_name]);
                commandLookup.Add(trimmed_name, ret);
            }
            return ret;
        }

        /// <summary>
        /// Clears all commands
        /// </summary>
        public void Clear() => commandLookup.Clear();

        /// <summary>
        /// Parses command
        /// </summary>
        /// <param name="command">Command</param>
        /// <returns>"true" if success, otherwise "false"</returns>
        public bool ParseCommand(string command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            bool ret = false;
            List<string> command_parts = new List<string>(command.Split());
            if (command_parts.Count > 0)
            {
                string command_name = command_parts[0].Trim().ToLower();
                command_parts.RemoveAt(0);
                if (commandLookup.ContainsKey(command_name))
                {
                    ret = commandLookup[command_name].Execute(command_parts);
                }
            }
            command_parts.Clear();
            return ret;
        }

        /// <summary>
        /// Clears execute command list
        /// </summary>
        /// <param name="executeCommandList">Execute command list</param>
        private static void ClearExecuteCommandList(List<Tuple<ICommand, List<string>>> executeCommandList)
        {
            Parallel.ForEach(executeCommandList ?? throw new ArgumentNullException(nameof(executeCommandList)), (execute_command) => execute_command.Item2.Clear());
            executeCommandList.Clear();
        }

        /// <summary>
        /// Parses commands
        /// </summary>
        /// <param name="commands">Commands</param>
        /// <param name="prefix">Prefix</param>
        /// <returns></returns>
        public bool ParseCommands(string commands, string prefix)
        {
            if (commands == null)
            {
                throw new ArgumentNullException(nameof(commands));
            }
            if (prefix == null)
            {
                throw new ArgumentNullException(nameof(prefix));
            }
            bool ret = true;
            string[] commands_parts = commands.Split(' ');
            bool parse_argument = false;
            List<Tuple<ICommand, List<string>>> execute_command_list = new List<Tuple<ICommand, List<string>>>();
            foreach (string commands_part in commands_parts)
            {
                if (parse_argument)
                {
                    Tuple<ICommand, List<string>> execute_command = execute_command_list[^1];
                    execute_command.Item2.Add(commands_part);
                    if (execute_command.Item2.Count >= execute_command.Item1.Arguments.Count)
                    {
                        parse_argument = false;
                    }
                }
                else
                {
                    if (commands_part.StartsWith(prefix) && (commands_part.Length > prefix.Length))
                    {
                        string key = commands_part[prefix.Length..].Trim().ToLower();
                        if (commandLookup.ContainsKey(key))
                        {
                            ICommand command = commandLookup[key];
                            execute_command_list.Add(new Tuple<ICommand, List<string>>(command, new List<string>()));
                            if (command.Arguments.Count > 0)
                            {
                                parse_argument = true;
                            }
                        }
                        else
                        {
                            ClearExecuteCommandList(execute_command_list);
                            ret = false;
                            break;
                        }
                    }
                    else if (commands_part.Length > 0)
                    {
                        ClearExecuteCommandList(execute_command_list);
                        ret = false;
                        break;
                    }
                }
            }
            foreach (Tuple<ICommand, List<string>> execute_command in execute_command_list)
            {
                execute_command.Item1.Execute(execute_command.Item2);
                execute_command.Item2.Clear();
            }
            execute_command_list.Clear();
            return ret;
        }

        /// <summary>
        /// Gets help topic
        /// </summary>
        /// <param name="commandName">Command name</param>
        /// <param name="prefix">Prefix</param>
        /// <returns>Help topic</returns>
        public string GetHelpTopic(string commandName, string prefix)
        {
            if (commandName == null)
            {
                throw new ArgumentNullException(nameof(commandName));
            }
            if (prefix == null)
            {
                throw new ArgumentNullException(nameof(prefix));
            }
            StringBuilder help_topic_string_builder = new StringBuilder();
            string key = commandName.Trim();
            if (key.Length > 0)
            {
                if (commandLookup.ContainsKey(key))
                {
                    ICommand command = commandLookup[key];
                    help_topic_string_builder.AppendLine($"Help topic for \"{ prefix }{ command.Name }\":");
                    help_topic_string_builder.AppendLine();
                    help_topic_string_builder.AppendLine($"\tDescription: { command.Description }");
                    help_topic_string_builder.AppendLine($"\tHelp topic: { command.HelpTopic }");
                    help_topic_string_builder.Append($"\tUsage: { command.Name }");
                    foreach (CommandArgument argument in command.Arguments)
                    {
                        help_topic_string_builder.Append($" <{ argument.Name }{ (argument.IsRequired ? " (optional)>" : ">") }");
                    }
                }
                else
                {
                    help_topic_string_builder.Append($"There is no help topic for \"{ commandName }\".");
                }
            }
            else
            {
                foreach (ICommand command in commandLookup.Values)
                {
                    if (command is Command)
                    {
                        help_topic_string_builder.Append($"\"{ prefix }{ command.Name }\"");
                        foreach (ICommand command_alias in commandLookup.Values)
                        {
                            if (command_alias is CommandAlias && command_alias.IsAliasOf(command))
                            {
                                help_topic_string_builder.Append($" -> \"{ prefix }{ command_alias.Name }\"");
                            }
                        }
                        help_topic_string_builder.AppendLine($" { command.Description }");
                        help_topic_string_builder.AppendLine($"\t{ command.HelpTopic }");
                        help_topic_string_builder.Append($"\tUsage: { prefix }{ command.Name }");
                        foreach (CommandArgument argument in command.Arguments)
                        {
                            help_topic_string_builder.Append($" <{ argument.Name }{ (argument.IsRequired ? " (optional)>" : ">") }");
                        }
                        help_topic_string_builder.AppendLine();
                        help_topic_string_builder.AppendLine();
                    }
                }
            }
            string ret = help_topic_string_builder.ToString();
            help_topic_string_builder.Clear();
            return ret;
        }
    }
}
