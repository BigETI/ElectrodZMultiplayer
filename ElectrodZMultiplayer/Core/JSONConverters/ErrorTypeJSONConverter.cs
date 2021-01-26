/// <summary>
/// ElectrodZ multiplayer JSON converters namespace
/// </summary>
namespace ElectrodZMultiplayer.JSONConverters
{
    /// <summary>
    /// A class used for converting error types to JSON and vice versa
    /// </summary>
    internal class ErrorTypeJSONConverter : EnumeratorValueJSONConverter<EErrorType>
    {
        /// <summary>
        /// Constructs an error type JSON converter
        /// </summary>
        public ErrorTypeJSONConverter() : base(EErrorType.Invalid)
        {
            // ...
        }
    }
}
