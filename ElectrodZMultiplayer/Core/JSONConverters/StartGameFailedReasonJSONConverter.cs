/// <summary>
/// ElectrodZ multiplayer JSON converters namespace
/// </summary>
namespace ElectrodZMultiplayer.JSONConverters
{
    /// <summary>
    /// A class used for converting start game failed reason to JSON and vice versa
    /// </summary>
    internal class StartGameFailedReasonJSONConverter : EnumeratorValueJSONConverter<EStartGameFailedReason>
    {
        /// <summary>
        /// Constructs a start game failed reason JSON converter
        /// </summary>
        public StartGameFailedReasonJSONConverter() : base(EStartGameFailedReason.Invalid)
        {
            // ...
        }
    }
}
