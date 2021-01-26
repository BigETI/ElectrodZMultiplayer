using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer JSON converters namespace
/// </summary>
namespace ElectrodZMultiplayer.JSONConverters
{
    /// <summary>
    /// A class used for converting GUIDs to JSON and vice versa
    /// </summary>
    internal class GUIDJSONConverter : JsonConverter
    {
        /// <summary>
        /// Is type nullable
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>"true" if type is nullable, otherwise "false"</returns>
        private static bool IsTypeNullable(Type type) => type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);

        /// <summary>
        /// Can convert the specified object type
        /// </summary>
        /// <param name="objectType">Object type</param>
        /// <returns>"true" if the specified object can be converted, otherwise "false"</returns>
        public override bool CanConvert(Type objectType) => (IsTypeNullable(objectType) ? Nullable.GetUnderlyingType(objectType) : objectType).IsEnum;

        /// <summary>
        /// Reads JSON
        /// </summary>
        /// <param name="reader">JSON reader</param>
        /// <param name="objectType">Object type</param>
        /// <param name="existingValue">Existing value</param>
        /// <param name="serializer">JSON serializer</param>
        /// <returns>Read object</returns>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer) => ((reader.TokenType == JsonToken.String) && Guid.TryParse(reader.Value.ToString(), out Guid guid)) ? guid : (IsTypeNullable(objectType) ? (object)null : Guid.Empty);

        /// <summary>
        /// Write JSON
        /// </summary>
        /// <param name="writer">JSON writer</param>
        /// <param name="value">Value</param>
        /// <param name="serializer">JSON serializer</param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) => writer.WriteValue(value.ToString());
    }
}
