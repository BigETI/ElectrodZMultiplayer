/// <summary>
/// ElectrodZ multiplayer JSON converters namespace
/// </summary>
namespace ElectrodZMultiplayer.JSONConverters
{
    /// <summary>
    /// A class used for converting change user lobby color failed reason to JSON and vice versa
    /// </summary>
    internal class ChangeUserLobbyColorFailedReasonJSONConverter : EnumeratorValueJSONConverter<EChangeUserLobbyColorFailedReason>
    {
        /// <summary>
        /// Constructs a change user lobby color failed reason JSON converter
        /// </summary>
        public ChangeUserLobbyColorFailedReasonJSONConverter() : base(EChangeUserLobbyColorFailedReason.Invalid)
        {
            // ...
        }
    }
}
