/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// A class used to determine names using naming conventions
    /// </summary>
    internal static class Naming
    {
        /// <summary>
        /// Message data suffix
        /// </summary>
        private static readonly string messageDataSuffix = "MessageData";

        /// <summary>
        /// Get message type name from message data type
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <returns>Message type</returns>
        public static string GetMessageTypeNameFromMessageDataType<T>() where T : IBaseMessageData
        {
            string ret = typeof(T).Name;
            if (ret.EndsWith(messageDataSuffix))
            {
                ret = ret.Substring(0, ret.Length - messageDataSuffix.Length);
            }
            return ret;
        }
    }
}
