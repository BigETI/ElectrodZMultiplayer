using Newtonsoft.Json;
using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a message informing a game being stopped
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class GameStoppedMessageData : BaseMessageData
    {
        /// <summary>
        /// Current users
        /// </summary>
        [JsonProperty("users")]
        public List<GameEndUserData> Users { get; set; }

        /// <summary>
        /// Current results
        /// </summary>
        [JsonProperty("results")]
        public Dictionary<string, object> Results { get; set; }

        /// <summary>
        /// Constructs a message that informs a game being stopped
        /// </summary>
        public GameStoppedMessageData(IEnumerable<(IUser, IReadOnlyDictionary<string, object>)> users, IReadOnlyDictionary<string, object> results) : base(Naming.GetMessageTypeNameFromMessageDataType<GameStoppedMessageData>())
        {
            if (users == null)
            {
                throw new ArgumentNullException(nameof(users));
            }
            if (results == null)
            {
                throw new ArgumentNullException(nameof(results));
            }
            if (!Protection.IsValid(users))
            {
                throw new ArgumentException($"\"{ nameof(users) }\" contains null.");
            }
            Users = new List<GameEndUserData>();
            foreach ((IUser, IReadOnlyDictionary<string, object>) user in users)
            {
                Users.Add(new GameEndUserData(user.Item1.GUID, user.Item2));
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
