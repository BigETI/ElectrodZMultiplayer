/// <summary>
/// ElectrodZ multiplayer JSON converters namespace
/// </summary>
namespace ElectrodZMultiplayer.JSONConverters
{
    /// <summary>
    /// A class used for converting disconnection reason to JSON and vice versa
    /// </summary>
    internal class DisconnectionReasonJSONConverter : EnumeratorValueJSONConverter<EDisconnectionReason>
    {
        /// <summary>
        /// Constructs an disconnection reason JSON converter
        /// </summary>
        public DisconnectionReasonJSONConverter() : base(EDisconnectionReason.Invalid)
        {
            // ...
        }
    }
}
