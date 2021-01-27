/// <summary>
/// ElectrodZ multiplayer JSON converters namespace
/// </summary>
namespace ElectrodZMultiplayer.JSONConverters
{
    /// <summary>
    /// A class used for converting server tick failed reason to JSON and vice versa
    /// </summary>
    internal class ServerTickFailedReasonJSONConverter : EnumeratorValueJSONConverter<EServerTickFailedReason>
    {
        /// <summary>
        /// Constructs a server tick failed reason JSON converter
        /// </summary>
        public ServerTickFailedReasonJSONConverter() : base(EServerTickFailedReason.Invalid)
        {
            // ...
        }
    }
}
