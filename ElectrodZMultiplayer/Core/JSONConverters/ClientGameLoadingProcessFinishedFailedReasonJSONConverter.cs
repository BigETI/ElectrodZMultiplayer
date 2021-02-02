/// <summary>
/// ElectrodZ multiplayer JSON converters namespace
/// </summary>
namespace ElectrodZMultiplayer.JSONConverters
{
    /// <summary>
    /// A class used for converting client game loading process finished failed reason to JSON and vice versa
    /// </summary>
    internal class ClientGameLoadingProcessFinishedFailedReasonJSONConverter : EnumeratorValueJSONConverter<EClientGameLoadingProcessFinishedFailedReason>
    {
        /// <summary>
        /// Constructs a client game loading process finished failed reason JSON converter
        /// </summary>
        public ClientGameLoadingProcessFinishedFailedReasonJSONConverter() : base(EClientGameLoadingProcessFinishedFailedReason.Invalid)
        {
            // ...
        }
    }
}
