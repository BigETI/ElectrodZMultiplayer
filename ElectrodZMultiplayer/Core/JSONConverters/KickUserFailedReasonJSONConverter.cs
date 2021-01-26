/// <summary>
/// ElectrodZ multiplayer JSON converters namespace
/// </summary>
namespace ElectrodZMultiplayer.JSONConverters
{
    /// <summary>
    /// A class used for converting kick user failed reason to JSON and vice versa
    /// </summary>
    internal class KickUserFailedReasonJSONConverter : EnumeratorValueJSONConverter<EKickUserFailedReason>
    {
        /// <summary>
        /// Constructs a kick user failed reason JSON converter
        /// </summary>
        public KickUserFailedReasonJSONConverter() : base(EKickUserFailedReason.Invalid)
        {
            // ...
        }
    }
}
