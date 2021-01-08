using System;
using System.Text.RegularExpressions;

/// <summary>
/// ElectrodZ multiplayer server namespace
/// </summary>
namespace ElectrodZMultiplayer.Server
{
    /// <summary>
    /// A class that describes a ban
    /// </summary>
    internal readonly struct Ban : IBan
    {
        /// <summary>
        /// Pattern
        /// </summary>
        public Regex Pattern { get; }

        /// <summary>
        /// Reason
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// Constructs a ban
        /// </summary>
        /// <param name="pattern">Pattern</param>
        /// <param name="reason">Reason</param>
        public Ban(Regex pattern, string reason)
        {
            Pattern = pattern ?? throw new ArgumentNullException(nameof(pattern));
            Reason = reason ?? throw new ArgumentNullException(nameof(reason));
        }
    }
}
