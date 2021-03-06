﻿/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// A class containing all defaults
    /// </summary>
    public static class Defaults
    {
        /// <summary>
        /// API version
        /// </summary>
        public static readonly string apiVersion = "1.0";

        /// <summary>
        /// Port
        /// </summary>
        public static readonly ushort networkPort = 6789;

        /// <summary>
        /// Timeout time in seconds
        /// </summary>
        public static readonly uint timeoutTime = 15U;

        /// <summary>
        /// Maximal pipe stream count to listen to incoming connections
        /// </summary>
        public static readonly uint maximalListeningPipeStreamCount = 4U;

        /// <summary>
        /// Default is lobby private
        /// </summary>
        public static readonly bool isLobbyPrivate = false;

        /// <summary>
        /// Default minimal user count
        /// </summary>
        public static readonly uint minimalUserCount = 2U;

        /// <summary>
        /// Default maximal user count
        /// </summary>
        public static readonly uint maximalUserCount = 6U;

        /// <summary>
        /// Default is starting game automatically
        /// </summary>
        public static readonly bool isStartingGameAutomatically = false;

        /// <summary>
        /// Minimal username length
        /// </summary>
        public static readonly uint minimalUsernameLength = 1U;

        /// <summary>
        /// Maximal username length
        /// </summary>
        public static readonly uint maximalUsernameLength = 32U;

        /// <summary>
        /// Minimal lobby name length
        /// </summary>
        public static readonly uint minimalLobbyNameLength = 1U;

        /// <summary>
        /// Maximal lobby name length
        /// </summary>
        public static readonly uint maximalLobbyNameLength = 32U;

        /// <summary>
        /// Default player entity type
        /// </summary>
        public static readonly string playerEntityType = "Player";
    }
}
