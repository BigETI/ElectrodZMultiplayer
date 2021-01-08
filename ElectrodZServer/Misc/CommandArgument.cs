using System;

/// <summary>
/// ElectrodZ server namespace
/// </summary>
namespace ElectrodZServer
{
    /// <summary>
    /// A class that describes a command argument
    /// </summary>
    internal struct CommandArgument : ICommandArgument
    {
        /// <summary>
        /// Command argument name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Is required
        /// </summary>
        public bool IsRequired { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="isRequired">Is required</param>
        private CommandArgument(string name, bool isRequired)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            IsRequired = isRequired;
        }

        /// <summary>
        /// Optional argument
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Command argument</returns>
        public static CommandArgument Optional(string name) => new CommandArgument(name, false);

        /// <summary>
        /// Required argument
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>Command argument</returns>
        public static CommandArgument Required(string name) => new CommandArgument(name, true);

        /// <summary>
        /// Implicitly casts a string to a command argument
        /// </summary>
        /// <param name="name">Command argument name</param>
        public static implicit operator CommandArgument(string name) => Required(name);
    }
}
