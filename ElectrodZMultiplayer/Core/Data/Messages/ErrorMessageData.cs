using ElectrodZMultiplayer.JSONConverters;
using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes an error message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class ErrorMessageData : BaseMessageData
    {
        /// <summary>
        /// Error type
        /// </summary>
        [JsonProperty("errorType")]
        public EErrorType ErrorType { get; set; } = EErrorType.Invalid;

        /// <summary>
        /// Error message type
        /// </summary>
        [JsonProperty("errorMessageType")]
        public string ErrorMessageType { get; set; }

        /// <summary>
        /// Error message
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Is valid
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (ErrorType != EErrorType.Invalid) &&
            !string.IsNullOrWhiteSpace(ErrorMessageType) &&
            (Message != null);

        /// <summary>
        /// Constructs an error message for deserializers
        /// </summary>
        public ErrorMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs an error message
        /// </summary>
        /// <param name="errorType">Error type</param>
        /// <param name="errorMessageType">Error message</param>
        /// <param name="message">Error message</param>
        public ErrorMessageData(EErrorType errorType, string errorMessageType, string message) : base(Naming.GetMessageTypeNameFromMessageDataType<ErrorMessageData>())
        {
            if (errorType == EErrorType.Invalid)
            {
                throw new ArgumentException("Error type can't be invalid.", nameof(errorType));
            }
            if (string.IsNullOrWhiteSpace(errorMessageType))
            {
                throw new ArgumentNullException(nameof(errorMessageType));
            }
            ErrorType = errorType;
            ErrorMessageType = errorMessageType;
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }
    }
}
