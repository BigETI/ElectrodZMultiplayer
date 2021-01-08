using System.Collections.Generic;
using System.IO;

/// <summary>
/// ElectrodZ multiplayer server namespace
/// </summary>
namespace ElectrodZMultiplayer.Server
{
    /// <summary>
    /// An interface that represents a set of bans
    /// </summary>
    public interface IBans
    {
        /// <summary>
        /// Ban lookup
        /// </summary>
        IReadOnlyDictionary<string, IBan> BanLookup { get; }

        /// <summary>
        /// Appends bans from file
        /// </summary>
        /// <param name="path">Path</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        bool AppendFromFile(string path);

        /// <summary>
        /// Appends bans from stream
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        bool AppendFromStream(Stream stream);

        /// <summary>
        /// Writes bans to file
        /// </summary>
        /// <param name="path">File path</param>
        void WriteToFile(string path);

        /// <summary>
        /// Writes bans to stream
        /// </summary>
        /// <param name="stream">Stream</param>
        void WriteToStream(Stream stream);

        /// <summary>
        /// Adds a ban as a pattern
        /// </summary>
        /// <param name="pattern">Pattern</param>
        /// <param name="reason">Reason</param>
        void AddPattern(string pattern, string reason);

        /// <summary>
        /// Adds a peer secret
        /// </summary>
        /// <param name="secret">Peer secret</param>
        /// <param name="reason">Reason</param>
        void AddSecret(string secret, string reason);

        /// <summary>
        /// Adds a peer
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="reason">Reason</param>
        void AddPeer(IPeer peer, string reason);

        /// <summary>
        /// Removes pattern
        /// </summary>
        /// <param name="pattern">Pattern</param>
        /// <returns>"true" if pattern was successfully removed, otherwise "false"</returns>
        bool RemovePattern(string pattern);

        /// <summary>
        /// Removes peer secret
        /// </summary>
        /// <param name="secret">Peer secret</param>
        /// <returns>"true" if pattern was successfully removed, otherwise "false"</returns>
        bool RemoveSecret(string secret);

        /// <summary>
        /// Is peer banned
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="reason">Reason</param>
        /// <returns>"true" if IP address is banned, otherwise "false"</returns>
        bool IsPeerBanned(IPeer peer, out string reason);

        /// <summary>
        /// Clears bans
        /// </summary>
        void Clear();
    }
}
