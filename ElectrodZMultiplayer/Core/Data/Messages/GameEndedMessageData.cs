using Newtonsoft.Json;
using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer data messages class
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes message informing a game being ended
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class GameEndedMessageData : BaseMessageData
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
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            !Protection.ContainsNullOrInvalid(Users) &&
            (Results != null) &&
            !Results.ContainsValue(null);

        /// <summary>
        /// Constructs a message that informs a game being ended for deserializers
        /// </summary>
        public GameEndedMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a message that informs a game being ended
        /// </summary>
        /// <param name="users">Users</param>
        public GameEndedMessageData(IEnumerable<(IUser, IReadOnlyDictionary<string, object>)> users, IReadOnlyDictionary<string, object> results) : base(Naming.GetMessageTypeNameFromMessageDataType<GameEndedMessageData>())
        {
            if (users == null)
            {
                throw new ArgumentNullException(nameof(users));
            }
            if (results == null)
            {
                throw new ArgumentNullException(nameof(results));
            }
            if (Protection.ContainsNullOrInvalid(users))
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
