using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// A class that implements methods for object validity protection
    /// </summary>
    internal static class Protection
    {
        /// <summary>
        /// Does collection contain null or invalid elements
        /// </summary>
        /// <typeparam name="T">Element type</typeparam>
        /// <param name="collection">Collection</param>
        /// <returns>"true" if collection contains any null or invalid elements, otherwise "false"</returns>
        public static bool ContainsNullOrInvalid<T>(IEnumerable<T> collection)
        {
            bool ret = false;
            if (collection == null)
            {
                ret = true;
            }
            else
            {
                Parallel.ForEach(collection, (element, parallel_loop_state) =>
                {
                    if ((element == null) || ((element is IValidable validable_element) && !validable_element.IsValid))
                    {
                        ret = true;
                        parallel_loop_state.Break();
                    }
                });
            }
            return ret;
        }

        /// <summary>
        /// Does collection contain the specified element
        /// </summary>
        /// <typeparam name="T">Element type</typeparam>
        /// <param name="collection">Collection</param>
        /// <param name="findElement">Find element</param>
        /// <returns>"true" if collection contains the specified element, otherwise "false"</returns>
        public static bool Contains<T>(IEnumerable<T> collection, T findElement)
        {
            bool ret = false;
            if (collection != null)
            {
                Parallel.ForEach(collection, (element, parallel_loop_state) =>
                {
                    if (Equals(element, findElement))
                    {
                        ret = true;
                        parallel_loop_state.Break();
                    }
                });
            }
            return ret;
        }

        /// <summary>
        /// Does collection contain the specified element
        /// </summary>
        /// <typeparam name="T">Element type</typeparam>
        /// <param name="collection">Collection</param>
        /// <param name="onContains">On contains</param>
        /// <returns>"true" if collection contains specified element in method, otherwise "false"</returns>
        public static bool Contains<T>(IEnumerable<T> collection, ContainsDelegate<T> onContains)
        {
            bool ret = false;
            if (collection != null)
            {
                Parallel.ForEach(collection, (element, parallel_loop_state) =>
                {
                    if (onContains(element))
                    {
                        ret = true;
                        parallel_loop_state.Break();
                    }
                });
            }
            return ret;
        }

        /// <summary>
        /// Is valid
        /// </summary>
        /// <typeparam name="T">Validable type</typeparam>
        /// <param name="validable">Validable</param>
        /// <returns>"true" if valid, otherwise "false"</returns>
        public static bool IsValid<T>(T validable) where T : IValidable => (validable != null) && validable.IsValid;
    }
}
