/// <summary>
/// ElectrodZ multiplayer JSON converters namespace
/// </summary>
namespace ElectrodZMultiplayer.JSONConverters
{
    /// <summary>
    /// A class used for converting quit lobby failed reason to JSON and vice versa
    /// </summary>
    internal class QuitLobbyFailedReasonJSONConverter : EnumeratorValueJSONConverter<EQuitLobbyFailedReason>
    {
        /// <summary>
        /// Constructs a quit lobby failed reason JSON converter
        /// </summary>
        public QuitLobbyFailedReasonJSONConverter() : base(EQuitLobbyFailedReason.Invalid)
        {
            // ...
        }
    }
}
