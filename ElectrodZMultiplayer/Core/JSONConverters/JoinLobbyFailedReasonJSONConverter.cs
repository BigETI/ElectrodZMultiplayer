/// <summary>
/// ElectrodZ multiplayer JSON converters namespace
/// </summary>
namespace ElectrodZMultiplayer.JSONConverters
{
    /// <summary>
    /// A class used for converting join lobby failed reason to JSON and vice versa
    /// </summary>
    internal class JoinLobbyFailedReasonJSONConverter : EnumeratorValueJSONConverter<EJoinLobbyFailedReason>
    {
        /// <summary>
        /// Constructs a join lobby failed reason JSON converter
        /// </summary>
        public JoinLobbyFailedReasonJSONConverter() : base(EJoinLobbyFailedReason.Invalid)
        {
            // ...
        }
    }
}
