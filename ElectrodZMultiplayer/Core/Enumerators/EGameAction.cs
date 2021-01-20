using ElectrodZMultiplayer.JSONConverters;
using Newtonsoft.Json;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Game action enumerator
    /// </summary>
    [JsonConverter(typeof(GameActionJSONConverter))]
    public enum EGameAction
    {
        /// <summary>
        /// Unknown game action
        /// </summary>
        Unknown,

        /// <summary>
        /// Change world
        /// </summary>
        ChangeWorld,

        /// <summary>
        /// Change color
        /// </summary>
        ChangeColor,

        /// <summary>
        /// Jump
        /// </summary>
        Jump
    }
}
