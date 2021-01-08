using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ server namespace
/// </summary>
namespace ElectrodZServer
{
    /// <summary>
    /// A class that describes a command alias
    /// </summary>
    internal class CommandAlias : ACommand
    {
        /// <summary>
        /// Command arguments
        /// </summary>
        public override IReadOnlyList<CommandArgument> Arguments => AliasTo.Arguments;

        /// <summary>
        /// Command description
        /// </summary>
        public override string Description => AliasTo.Description;

        /// <summary>
        /// Help topic
        /// </summary>
        public override string HelpTopic => AliasTo.HelpTopic;

        /// <summary>
        /// Alias to command
        /// </summary>
        public override ICommand AliasTo { get; }

        /// <summary>
        /// Constructs a command alias
        /// </summary>
        /// <param name="name">Command name</param>
        /// <param name="aliasTo">Alias to command</param>
        public CommandAlias(string name, ICommand aliasTo) : base(name) => AliasTo = aliasTo ?? throw new ArgumentNullException(nameof(aliasTo));

        /// <summary>
        /// Executes command
        /// </summary>
        /// <param name="arguments">Arguments</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        public override bool Execute(IReadOnlyList<string> arguments) => AliasTo.Execute(arguments);
    }
}
