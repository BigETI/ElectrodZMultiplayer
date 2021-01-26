/// <summary>
/// ElectrodZ multiplayer JSON converters namespace
/// </summary>
namespace ElectrodZMultiplayer.JSONConverters
{
    /// <summary>
    /// A class used for converting list lobbies failed reason to JSON and vice versa
    /// </summary>
    internal class ListLobbiesFailedReasonJSONConverter : EnumeratorValueJSONConverter<EListLobbiesFailedReason>
    {
        /// <summary>
        /// Constructs a list lobbies failed reason JSON converter
        /// </summary>
        public ListLobbiesFailedReasonJSONConverter() : base(EListLobbiesFailedReason.Invalid)
        {
            // ...
        }
    }
}
