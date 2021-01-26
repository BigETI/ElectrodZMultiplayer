/// <summary>
/// ElectrodZ multiplayer JSON converters namespace
/// </summary>
namespace ElectrodZMultiplayer.JSONConverters
{
    /// <summary>
    /// A class used for converting stop game failed reason to JSON and vice versa
    /// </summary>
    internal class StopGameFailedReasonJSONConverter : EnumeratorValueJSONConverter<EStopGameFailedReason>
    {
        /// <summary>
        /// Constructs a stop game failed reason JSON converter
        /// </summary>
        public StopGameFailedReasonJSONConverter() : base(EStopGameFailedReason.Invalid)
        {
            // ...
        }
    }
}
