/// <summary>
/// ElectrodZ multiplayer JSON converters namespace
/// </summary>
namespace ElectrodZMultiplayer.JSONConverters
{
    /// <summary>
    /// A class used for converting client tick failed reason to JSON and vice versa
    /// </summary>
    internal class ClientTickFailedReasonJSONConverter : EnumeratorValueJSONConverter<EClientTickFailedReason>
    {
        /// <summary>
        /// Constructs a client tick failed reason JSON converter
        /// </summary>
        public ClientTickFailedReasonJSONConverter() : base(EClientTickFailedReason.Invalid)
        {
            // ...
        }
    }
}
