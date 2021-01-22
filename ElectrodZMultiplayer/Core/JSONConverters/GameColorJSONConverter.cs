/// <summary>
/// ElectrodZ multiplayer JSON converters namespace
/// </summary>
namespace ElectrodZMultiplayer.JSONConverters
{
    /// <summary>
    /// A class used for converting game colors to JSON and vice versa
    /// </summary>
    internal class GameColorJSONConverter : EnumeratorValueJSONConverter<EGameColor>
    {
        /// <summary>
        /// Constructs a game color JSON converter
        /// </summary>
        public GameColorJSONConverter() : base(EGameColor.Unknown)
        {
            // ...
        }
    }
}
