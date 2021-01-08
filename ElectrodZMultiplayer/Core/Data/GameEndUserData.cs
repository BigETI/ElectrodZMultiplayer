using ElectrodZMultiplayer.JSONConverters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer data namespace
/// </summary>
namespace ElectrodZMultiplayer.Data
{
    /// <summary>
    /// A class describing user data at the end of a game
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class GameEndUserData : IValidable
    {
        /// <summary>
        /// User GUID
        /// </summary>
        [JsonProperty("guid")]
        [JsonConverter(typeof(GUIDJSONConverter))]
        public Guid GUID { get; set; }

        /// <summary>
        /// Game end results
        /// </summary>
        [JsonProperty("results")]
        public Dictionary<string, object> Results { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public bool IsValid =>
            (GUID != Guid.Empty) &&
            (Results != null) &&
            !Results.ContainsValue(null);

        /// <summary>
        /// Constructs user data after the end of a game for deserializers
        /// </summary>
        public GameEndUserData()
        {
            // ...
        }

        /// <summary>
        /// Constructs user data after the end of a game
        /// </summary>
        /// <param name="guid">User GUID</param>
        /// <param name="results">Game end results</param>
        public GameEndUserData(Guid guid, IReadOnlyDictionary<string, object> results)
        {
            if (guid == Guid.Empty)
            {
                throw new ArgumentException("User GUID can't be empty.", nameof(guid));
            }
            if (results == null)
            {
                throw new ArgumentNullException(nameof(results));
            }
            Results = new Dictionary<string, object>();
            foreach (KeyValuePair<string, object> result in results)
            {
                if (result.Value == null)
                {
                    throw new ArgumentException($"Value of game end result key \"{ result.Key }\" is null.", nameof(results));
                }
                Results.Add(result.Key, result.Value);
            }
        }
    }
}
