using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer JSON converters namespace
/// </summary>
namespace ElectrodZMultiplayer.JSONConverters
{
    /// <summary>
    /// A class used for convert game colors to JSON and vice versa
    /// </summary>
    internal class GameColorJSONConverter : JsonConverter<EGameColor>
    {
        /// <summary>
        /// Read JSON
        /// </summary>
        /// <param name="reader">JSON reader</param>
        /// <param name="objectType">Object type</param>
        /// <param name="existingValue">Existing value</param>
        /// <param name="hasExistingValue">Has existing value</param>
        /// <param name="serializer">JSON serializer</param>
        /// <returns>Game color</returns>
        public override EGameColor ReadJson(JsonReader reader, Type objectType, EGameColor existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            EGameColor ret = existingValue;
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
        public override void WriteJson(JsonWriter writer, EGameColor value, JsonSerializer serializer) => writer.WriteValue(value.ToString());
    }
}
