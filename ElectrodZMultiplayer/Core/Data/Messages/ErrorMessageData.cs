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
        [JsonConverter(typeof(ErrorTypeJSONConverter))]
        public EErrorType ErrorType { get; set; } = EErrorType.Unknown;

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
            (ErrorType != EErrorType.Unknown) &&
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
        /// <param name="message">Error message</param>
        public ErrorMessageData(EErrorType errorType, string message) : base(Naming.GetMessageTypeNameFromMessageDataType<ErrorMessageData>())
        {
            if (errorType == EErrorType.Unknown)
            {
                throw new ArgumentException(nameof(errorType));
            }
            ErrorType = errorType;
            Message = message ?? throw new ArgumentNullException(nameof(message));
        }
    }
}
