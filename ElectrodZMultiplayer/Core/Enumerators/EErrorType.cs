﻿/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Error type enumerator
    /// </summary>
    public enum EErrorType
    {
        /// <summary>
        /// Unknown error
        /// </summary>
        Unknown,

        /// <summary>
        /// Unknown message
        /// </summary>
        UnknownMessage,

        /// <summary>
        /// Malformed message
        /// </summary>
        MalformedMessage,

        /// <summary>
        /// Not supported message
        /// </summary>
        NotSupportedMessage,

        /// <summary>
        /// Invalid message parameter
        /// </summary>
        InvalidMessageParameters,

        /// <summary>
        /// Message comes from an invalid context
        /// </summary>
        InvalidMessageContext,

        /// <summary>
        /// Requested entity has not been found
        /// </summary>
        NotFound,

        /// <summary>
        /// Requested entity is full
        /// </summary>
        Full,

        /// <summary>
        /// Internal error
        /// </summary>
        InternalError
    }
}
