using ElectrodZMultiplayer.Server.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// ElectrodZ multiplayer server namespace
/// </summary>
namespace ElectrodZMultiplayer.Server
{
    /// <summary>
    /// A class that represents a set of bans
    /// </summary>
    internal class Bans : IBans
    {
        /// <summary>
        /// Ban lookup
        /// </summary>
        private readonly Dictionary<string, IBan> banLookup = new Dictionary<string, IBan>();

        /// <summary>
        /// Ban lookup
        /// </summary>
        public IReadOnlyDictionary<string, IBan> BanLookup => banLookup;

        /// <summary>
        /// Appends bans from file
        /// </summary>
        /// <param name="path">Path</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        public bool AppendFromFile(string path)
        {
            bool ret = false;
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            if (File.Exists(path))
            {
                try
                {
                    using (FileStream file_stream = File.OpenRead(path))
                    {
                        ret = AppendFromStream(file_stream);
                    }
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine(e);
                }
            }
            return ret;
        }

        /// <summary>
        /// Appends bans from stream
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <returns>"true" if successful, otherwise "false"</returns>
        public bool AppendFromStream(Stream stream)
        {
            bool ret = false;
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            if (stream.CanRead)
            {
                using (StreamReader stream_reader = new StreamReader(stream, Encoding.UTF8))
                {
                    try
                    {
                        BanData[] bans_data = JsonConvert.DeserializeObject<BanData[]>(stream_reader.ReadToEnd());
                        if (bans_data != null)
                        {
                            foreach (BanData ban_data in bans_data)
                            {
                                if (ban_data != null)
                                {
                                    if ((ban_data.Pattern != null) && (ban_data.Reason != null))
                                    {
                                        try
                                        {
                                            Regex regex = new Regex(ban_data.Pattern, RegexOptions.Compiled);
                                            if (banLookup.ContainsKey(ban_data.Pattern))
                                            {
                                                banLookup[ban_data.Pattern] = new Ban(regex, ban_data.Reason);
                                            }
                                            else
                                            {
                                                banLookup.Add(ban_data.Pattern, new Ban(regex, ban_data.Reason));
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            Console.Error.WriteLine(e);
                                        }
                                    }
                                }
                            }
                            ret = true;
                        }
                    }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine(e);
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Writes bans to file
        /// </summary>
        /// <param name="path">File path</param>
        public void WriteToFile(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }
            try
            {
                using (FileStream file_stream = File.Open(path, FileMode.Create, FileAccess.Write))
                {
                    WriteToStream(file_stream);
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
        }

        /// <summary>
        /// Writes bans to stream
        /// </summary>
        /// <param name="stream">Stream</param>
        public void WriteToStream(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }
            if (stream.CanWrite)
            {
                using (StreamWriter stream_writer = new StreamWriter(stream))
                {
                    BanData[] bans = new BanData[banLookup.Count];
                    uint index = 0U;
                    foreach (IBan ban in banLookup.Values)
                    {
                        bans[index] = new BanData(ban.Pattern.ToString(), ban.Reason);
                    }
                    stream_writer.Write(JsonConvert.SerializeObject(bans));
                }
            }
        }

        /// <summary>
        /// Adds a ban as a pattern
        /// </summary>
        /// <param name="pattern">Pattern</param>
        /// <param name="reason">Reason</param>
        public void AddPattern(string pattern, string reason)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }
            if (reason == null)
            {
                throw new ArgumentNullException(nameof(reason));
            }
            try
            {
                Regex regex = new Regex(pattern, RegexOptions.Compiled);
                if (banLookup.ContainsKey(pattern))
                {
                    banLookup[pattern] = new Ban(regex, reason);
                }
                else
                {
                    banLookup.Add(pattern, new Ban(regex, reason));
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
        }

        /// <summary>
        /// Adds a peer secret
        /// </summary>
        /// <param name="secret">Peer secret</param>
        /// <param name="reason">Reason</param>
        public void AddSecret(string secret, string reason)
        {
            if (string.IsNullOrWhiteSpace(secret))
            {
                throw new ArgumentNullException(nameof(secret));
            }
            if (reason == null)
            {
                throw new ArgumentNullException(nameof(reason));
            }
            AddPattern(Regex.Escape(secret), reason);
        }

        /// <summary>
        /// Adds a peer
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="reason">Reason</param>
        public void AddPeer(IPeer peer, string reason)
        {
            if (peer == null)
            {
                throw new ArgumentNullException(nameof(peer));
            }
            if (!peer.IsValid)
            {
                throw new ArgumentException("Peer is not valid.", nameof(peer));
            }
            if (reason == null)
            {
                throw new ArgumentNullException(nameof(reason));
            }
            AddSecret(peer.Secret, reason);
        }

        /// <summary>
        /// Removes pattern
        /// </summary>
        /// <param name="pattern">Pattern</param>
        /// <returns>"true" if pattern was successfully removed, otherwise "false"</returns>
        public bool RemovePattern(string pattern)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }
            return banLookup.Remove(pattern);
        }

        /// <summary>
        /// Removes peer secret
        /// </summary>
        /// <param name="secret">Peer secret</param>
        /// <returns>"true" if peer secret was successfully removed, otherwise "false"</returns>
        public bool RemoveSecret(string secret)
        {
            if (secret == null)
            {
                throw new ArgumentNullException(nameof(secret));
            }
            return RemovePattern(Regex.Escape(secret));
        }

        /// <summary>
        /// Is banned
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="reason">Reason</param>
        /// <returns>"true" if IP address is banned, otherwise "false"</returns>
        public bool IsPeerBanned(IPeer peer, out string reason)
        {
            if (peer == null)
            {
                throw new ArgumentNullException(nameof(peer));
            }
            if (!peer.IsValid)
            {
                throw new ArgumentException("Peer is not valid.", nameof(peer));
            }
            bool ret = false;
            reason = string.Empty;
            foreach (IBan ban in banLookup.Values)
            {
                if (ban != null)
                {
                    if (ban.Pattern.IsMatch(peer.Secret))
                    {
                        reason = ban.Reason;
                        ret = true;
                        break;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Clears bans
        /// </summary>
        public void Clear()
        {
            banLookup.Clear();
        }
    }
}
