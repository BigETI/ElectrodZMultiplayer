using System;

/// <summary>
/// ElectrodZ server namespace
/// </summary>
namespace ElectrodZServer
{
    /// <summary>
    /// An interface that represents a logger
    /// </summary>
    internal interface ILogger : IDisposable
    {
        /// <summary>
        /// Write
        /// </summary>
        /// <param name="obj">Object</param>
        void Write(object obj);

        /// <summary>
        /// Write line
        /// </summary>
        /// <param name="obj">Object</param>
        void WriteLine(object obj);
    }
}
