using System.Collections.Generic;

/// <summary>
/// ElectrodZ server namespace
/// </summary>
namespace ElectrodZServer
{
    /// <summary>
    /// Used to signal that a command has been executed
    /// </summary>
    /// <param name="arguments">Arguments</param>
    internal delegate void CommandExecutedDelegate(IReadOnlyList<string> arguments);
}
