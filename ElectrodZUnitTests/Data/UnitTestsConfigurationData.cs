using ElectrodZMultiplayer;
using Newtonsoft.Json;

/// <summary>
/// ElectrodZ unit tests data namespace
/// </summary>
namespace ElectrodZUnitTests.Data
{
    /// <summary>
    /// A class that contains all configurations for unit tests
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class UnitTestsConfigurationData : IValidable
    {
        /// <summary>
        /// Default tick rate per second
        /// </summary>
        private static readonly uint defaultTickRate = 60;

        /// <summary>
        /// Default value for how many ticks should be performed till connections are being closed
        /// </summary>
        private static readonly uint defaultPerformTicks = 600;

        /// <summary>
        /// Tick rate per second
        /// </summary>
        [JsonProperty("tickRate")]
        public uint TickRate { get; set; } = defaultTickRate;

        /// <summary>
        /// How many ticks should be performed till connections are being closed
        /// </summary>
        [JsonProperty("performTicks")]
        public uint PerformTicks { get; set; } = defaultPerformTicks;

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public bool IsValid =>
            (TickRate > 0U) &&
            (PerformTicks > 0U);
    }
}
