/// <summary>
/// ElectrodZ multiplayer JSON converters namespace
/// </summary>
namespace ElectrodZMultiplayer.JSONConverters
{
    /// <summary>
    /// A class used for converting game actions to JSON and vice versa
    /// </summary>
    internal class GameActionJSONConverter : EnumeratorValueJSONConverter<EGameAction>
    {
        /// <summary>
        /// Constructs a game action JSON converter
        /// </summary>
        public GameActionJSONConverter() : base(EGameAction.Unknown)
        {
            // ...
        }
    }
}
