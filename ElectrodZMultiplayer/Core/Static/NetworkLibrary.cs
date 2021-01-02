using ENet;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// A class to deal with ENet's initialization functions
    /// </summary>
    internal static class NetworkLibrary
    {
        /// <summary>
        /// Initialize counter
        /// </summary>
        private static uint initializeCounter;

        /// <summary>
        /// Initialize
        /// </summary>
        public static bool Initialize()
        {
            bool ret = false;
            if (initializeCounter == 0U)
            {
                if (Library.Initialize())
                {
                    initializeCounter = 1U;
                    ret = true;
                }
            }
            else
            {
                ++initializeCounter;
                ret = true;
            }
            return ret;
        }

        /// <summary>
        /// Deinitialize
        /// </summary>
        public static void Deinitialize()
        {
            if (initializeCounter > 0U)
            {
                --initializeCounter;
                if (initializeCounter == 0U)
                {
                    Library.Deinitialize();
                }
            }
        }
    }
}
