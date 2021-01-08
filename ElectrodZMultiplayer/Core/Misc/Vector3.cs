/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// 3D vector structure
    /// </summary>
    /// <typeparam name="T">Vector component type</typeparam>
    public readonly struct Vector3<T>
    {
        /// <summary>
        /// X component of a 3D vector
        /// </summary>
        public T X { get; }

        /// <summary>
        /// Y component of a 3D vector
        /// </summary>
        public T Y { get; }

        /// <summary>
        /// Z component of a 3D vector
        /// </summary>
        public T Z { get; }

        /// <summary>
        /// COnstructs a 3D vector
        /// </summary>
        /// <param name="x">X component of a 3D vector</param>
        /// <param name="y">Y component of a 3D vector</param>
        /// <param name="z">Z component of a 3D vector</param>
        public Vector3(T x, T y, T z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
