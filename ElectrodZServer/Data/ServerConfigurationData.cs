using Newtonsoft.Json;
using System.Reflection;

/// <summary>
/// ElectrodZ server data namespace
/// </summary>
namespace ElectrodZServer.Data
{
    /// <summary>
    /// A class that contains all server configuration data
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ServerConfigurationData
    {
        /// <summary>
        /// Default network port
        /// </summary>
        public static readonly ushort defaultNetworkPort = 6789;

        /// <summary>
        /// Default tick rate
        /// </summary>
        public static readonly ushort defaultTickRate = 60;

        /// <summary>
        /// Default timeout time in seconds
        /// </summary>
        public static readonly uint defaultTimeoutTime = 15U;

        /// <summary>
        /// Default output log path
        /// </summary>
        public static readonly string defaultOutputLogPath = "./output.txt";

        /// <summary>
        /// Default error log path
        /// </summary>
        public static readonly string defaultErrorLogPath = "./error.txt";

        /// <summary>
        /// Network port
        /// </summary>
        [JsonProperty("networkPort")]
        public ushort NetworkPort { get; set; } = defaultNetworkPort;

        /// <summary>
        /// Tick rate
        /// </summary>
        [JsonProperty("tickRate")]
        public ushort TickRate { get; set; } = defaultTickRate;

        /// <summary>
        /// Timeout time in seconds
        /// </summary>
        [JsonProperty("timeoutTime")]
        public uint TimeoutTime { get; set; } = defaultTimeoutTime;

        /// <summary>
        /// Output log path
        /// </summary>
        [JsonProperty("outputLogPath")]
        public string OutputLogPath { get; set; } = defaultOutputLogPath;

        /// <summary>
        /// Error log path
        /// </summary>
        [JsonProperty("errorLogPath")]
        public string ErrorLogPath { get; set; } = defaultErrorLogPath;
    }
}
