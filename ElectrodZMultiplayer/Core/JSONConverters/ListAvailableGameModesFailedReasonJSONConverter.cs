/// <summary>
/// ElectrodZ multiplayer JSON converters namespace
/// </summary>
namespace ElectrodZMultiplayer.JSONConverters
{
    /// <summary>
    /// A class used for converting list available game modes failed reason to JSON and vice versa
    /// </summary>
    internal class ListAvailableGameModesFailedReasonJSONConverter : EnumeratorValueJSONConverter<EListAvailableGameModesFailedReason>
    {
        /// <summary>
        /// Constructs a list available game modes failed reason JSON converter
        /// </summary>
        public ListAvailableGameModesFailedReasonJSONConverter() : base(EListAvailableGameModesFailedReason.Invalid)
        {
            // ...
        }
    }
}
