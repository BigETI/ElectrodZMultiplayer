/// <summary>
/// ElectrodZ multiplayer JSON converters namespace
/// </summary>
namespace ElectrodZMultiplayer.JSONConverters
{
    /// <summary>
    /// A class used for converting cancel start, restart or stop game timer failed reason to JSON and vice versa
    /// </summary>
    internal class CancelStartRestartStopGameTimerFailedReasonJSONConverter : EnumeratorValueJSONConverter<ECancelStartRestartStopGameTimerFailedReason>
    {
        /// <summary>
        /// Constructs a cancel start, restart or stop game timer failed reason JSON converter
        /// </summary>
        public CancelStartRestartStopGameTimerFailedReasonJSONConverter() : base(ECancelStartRestartStopGameTimerFailedReason.Invalid)
        {
            // ...
        }
    }
}
