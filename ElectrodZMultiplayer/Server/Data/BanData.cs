using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer server data namespace
/// </summary>
namespace ElectrodZMultiplayer.Server.Data
{
    /// <summary>
    /// A class that contains ban data
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class BanData
    {
        /// <summary>
        /// Pattern
        /// </summary>
        [JsonProperty("pattern")]
        public string Pattern { get; set; }

        /// <summary>
        /// Ban reason
        /// </summary>
        [JsonProperty("reason")]
        public string Reason { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public BanData()
        {
            // ...
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pattern">Pattern</param>
        /// <param name="reason">Reason</param>
        public BanData(string pattern, string reason)
        {
            Pattern = pattern ?? throw new ArgumentNullException(nameof(pattern));
            Reason = reason ?? throw new ArgumentNullException(nameof(reason));
        }
    }
}
