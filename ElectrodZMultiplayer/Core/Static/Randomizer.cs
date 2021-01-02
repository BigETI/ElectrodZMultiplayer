using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// A class used for obtaining randomized outputs
    /// </summary>
    internal static class Randomizer
    {
        /// <summary>
        /// Random number generator
        /// </summary>
        private static readonly Random random = new Random();

        /// <summary>
        /// Gets a random string of the specified length and provided characters to select random characters from
        /// </summary>
        /// <param name="length">Random string length</param>
        /// <param name="characters">CHaractewrs to choose random characters from</param>
        /// <returns>Randomly generated string</returns>
        public static string GetRandomString(uint length, IReadOnlyList<char> characters)
        {
            if (characters == null)
            {
                throw new ArgumentNullException(nameof(characters));
            }
            if (characters.Count <= 0)
            {
                throw new ArgumentException($"\"{ nameof(characters) }\" is empty.", nameof(characters));
            }
            char[] result = new char[length];
            for (uint index = 0U; index != length; index++)
            {
                result[index] = characters[random.Next(characters.Count)];
            }
            return new string(result);
        }
    }
}
