using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer JSON converters namespace
/// </summary>
namespace ElectrodZMultiplayer.JSONConverters
{
    /// <summary>
    /// A class used for convert GUIDs to JSON and vice versa
    /// </summary>
    internal class GUIDJSONConverter : JsonConverter<Guid>
    {
        /// <summary>
        /// Read JSON
        /// </summary>
        /// <param name="reader">JSON reader</param>
        /// <param name="objectType">Object type</param>
        /// <param name="existingValue">Existing value</param>
        /// <param name="hasExistingValue">Has existing value</param>
        /// <param name="serializer">JSON serializer</param>
        /// <returns></returns>
        public override Guid ReadJson(JsonReader reader, Type objectType, Guid existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            Guid ret = existingValue;
            if (reader.Value is string value)
            {
                if (!Guid.TryParse(value, out ret))
                {
                    ret = Guid.Empty;
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
        public override void WriteJson(JsonWriter writer, Guid value, JsonSerializer serializer) => writer.WriteValue(value.ToString());
    }
}
