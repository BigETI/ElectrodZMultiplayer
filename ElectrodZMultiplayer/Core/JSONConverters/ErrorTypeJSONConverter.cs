using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer JSON converters namespace
/// </summary>
namespace ElectrodZMultiplayer.JSONConverters
{
    /// <summary>
    /// A class used for convert error types to JSON and vice versa
    /// </summary>
    internal class ErrorTypeJSONConverter : JsonConverter<EErrorType>
    {
        /// <summary>
        /// Read JSON
        /// </summary>
        /// <param name="reader">JSON reader</param>
        /// <param name="objectType">Object type</param>
        /// <param name="existingValue">Existing value</param>
        /// <param name="hasExistingValue">Has existing value</param>
        /// <param name="serializer">JSON serializer</param>
        /// <returns>Error type</returns>
        public override EErrorType ReadJson(JsonReader reader, Type objectType, EErrorType existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            EErrorType ret = existingValue;
            if (reader.Value is string value)
            {
                if (!Enum.TryParse(value, out ret))
                {
                    ret = existingValue;
                }
            }
            return ret;
        }

        /// <summary>
        /// Write JSON
        /// </summary>
        /// <param name="writer">JSON writer</param>
        /// <param name="value">Value</param>
        /// <param name="serializer">JSON serializer</param>
        public override void WriteJson(JsonWriter writer, EErrorType value, JsonSerializer serializer) => writer.WriteValue(value.ToString());
    }
}
