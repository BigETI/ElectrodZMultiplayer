using ElectrodZMultiplayer.JSONConverters;
using Newtonsoft.Json;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Game color enumerator
    /// </summary>
    [JsonConverter(typeof(GameColorJSONConverter))]
    public enum EGameColor
    {
        /// <summary>
        /// Invalid game color
        /// </summary>
        Invalid,

        /// <summary>
        /// Default game color
        /// </summary>
        Default,

        /// <summary>
        /// World 0, variant 0
        /// </summary>
        World00,

        /// <summary>
        /// World 0, variant 1
        /// </summary>
        World01,

        /// <summary>
        /// World 1, variant 0
        /// </summary>
        World10,

        /// <summary>
        /// World 1, variant 1
        /// </summary>
        World11
    }
}
