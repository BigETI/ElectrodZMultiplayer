/// <summary>
/// ElectrodZ multiplayer JSON converters namespace
/// </summary>
namespace ElectrodZMultiplayer.JSONConverters
{
    /// <summary>
    /// A class used for converting change lobby rules failed reason to JSON and vice versa
    /// </summary>
    internal class ChangeLobbyRulesFailedReasonJSONConverter : EnumeratorValueJSONConverter<EChangeLobbyRulesFailedReason>
    {
        /// <summary>
        /// Constructs a change lobby rules failed reason JSON converter
        /// </summary>
        public ChangeLobbyRulesFailedReasonJSONConverter() : base(EChangeLobbyRulesFailedReason.Invalid)
        {
            // ...
        }
    }
}
