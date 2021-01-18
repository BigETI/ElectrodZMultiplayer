/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// Quaternion structure
    /// </summary>
    public readonly struct Quaternion
    {
        /// <summary>
        /// Identity
        /// </summary>
        public static Quaternion Identity { get; } = new Quaternion(0.0f, 0.0f, 0.0f, 1.0f);

        /// <summary>
        /// X component of the quaternion
        /// </summary>
        public float X { get; }

        /// <summary>
        /// Y component of the quaternion
        /// </summary>
        public float Y { get; }

        /// <summary>
        /// Z component of the quaternion
        /// </summary>
        public float Z { get; }

        /// <summary>
        /// W component of the quaternion
        /// </summary>
        public float W { get; }

        /// <summary>
        /// Constructs a quaternion
        /// </summary>
        /// <param name="x">X component of the quaternion</param>
        /// <param name="y">Y component of the quaternion</param>
        /// <param name="z">Z component of the quaternion</param>
        /// <param name="w">W component of the quaternion</param>
        public Quaternion(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }
    }
}
