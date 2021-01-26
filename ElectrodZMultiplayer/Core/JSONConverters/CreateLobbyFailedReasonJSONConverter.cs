/// <summary>
/// ElectrodZ multiplayer JSON converters namespace
/// </summary>
namespace ElectrodZMultiplayer.JSONConverters
{
    /// <summary>
    /// A class used for converting create lobby failed reason to JSON and vice versa
    /// </summary>
    internal class CreateLobbyFailedReasonJSONConverter : EnumeratorValueJSONConverter<ECreateLobbyFailedReason>
    {
        /// <summary>
        /// Constructs an create lobby failed reason JSON converter
        /// </summary>
        public CreateLobbyFailedReasonJSONConverter() : base(ECreateLobbyFailedReason.Invalid)
        {
            // ...
        }
    }
}
