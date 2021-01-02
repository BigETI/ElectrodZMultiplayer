using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// User with results structure
    /// </summary>
    public readonly struct UserWithResults : IValidable
    {
        /// <summary>
        /// User
        /// </summary>
        public IUser User { get; }

        /// <summary>
        /// User results
        /// </summary>
        public IReadOnlyDictionary<string, object> Results { get; }

        /// <summary>
        /// Is valid
        /// </summary>
        public bool IsValid =>
            (User != null) &&
            (Results != null) &&
            !Protection.ContainsNullOrInvalid(Results.Values);

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="results">User results</param>
        public UserWithResults(IUser user, IReadOnlyDictionary<string, object> results)
        {
            if (results == null)
            {
                throw new ArgumentNullException(nameof(results));
            }
            User = user ?? throw new ArgumentNullException(nameof(user));
            Dictionary<string, object> user_results = new Dictionary<string, object>();
            foreach (KeyValuePair<string, object> result in results)
            {
                if (result.Value == null)
                {
                    throw new ArgumentException($"Value of user result key \"{ result.Key }\" is null.");
                }
                user_results.Add(result.Key, result.Value);
            }
            Results = user_results;
        }
    }
}
