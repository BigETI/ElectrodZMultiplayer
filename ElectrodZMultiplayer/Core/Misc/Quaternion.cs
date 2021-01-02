/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Quaternion structure
    /// </summary>
    /// <typeparam name="T">Quaternion component type</typeparam>
    public readonly struct Quaternion<T>
    {
        /// <summary>
        /// W component of the quaternion
        /// </summary>
        public T W { get; }

        /// <summary>
        /// X component of the quaternion
        /// </summary>
        public T X { get; }

        /// <summary>
        /// Y component of the quaternion
        /// </summary>
        public T Y { get; }

        /// <summary>
        /// Z component of the quaternion
        /// </summary>
        public T Z { get; }

        /// <summary>
        /// Constructs a quaternion
        /// </summary>
        /// <param name="w">W component of the quaternion</param>
        /// <param name="x">X component of the quaternion</param>
        /// <param name="y">Y component of the quaternion</param>
        /// <param name="z">Z component of the quaternion</param>
        public Quaternion(T w, T x, T y, T z)
        {
            W = w;
            X = x;
            Y = y;
            Z = z;
        }
    }
}
