/// <summary>
/// ElectrodZ multiplayer JSON converters namespace
/// </summary>
namespace ElectrodZMultiplayer.JSONConverters
{
    /// <summary>
    /// A class used for converting authentification failed reason to JSON and vice versa
    /// </summary>
    internal class AuthentificationFailedReasonJSONConverter : EnumeratorValueJSONConverter<EAuthentificationFailedReason>
    {
        /// <summary>
        /// Constructs an authenticate failed reason JSON converter
        /// </summary>
        public AuthentificationFailedReasonJSONConverter() : base(EAuthentificationFailedReason.Invalid)
        {
            // ...
        }
    }
}
