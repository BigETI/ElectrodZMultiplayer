/// <summary>
/// ElectrodZ multiplayer JSON converters namespace
/// </summary>
namespace ElectrodZMultiplayer.JSONConverters
{
    /// <summary>
    /// A class used for converting change username failed reason to JSON and vice versa
    /// </summary>
    internal class ChangeUsernameFailedReasonJSONConverter : EnumeratorValueJSONConverter<EChangeUsernameFailedReason>
    {
        /// <summary>
        /// Constructs a change username failed reason JSON converter
        /// </summary>
        public ChangeUsernameFailedReasonJSONConverter() : base(EChangeUsernameFailedReason.Invalid)
        {
            // ...
        }
    }
}
