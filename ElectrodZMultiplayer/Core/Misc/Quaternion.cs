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

        /// <summary>
        /// Compares two quaternions
        /// </summary>
        /// <param name="left">Left</param>
        /// <param name="right">Right</param>
        /// <returns>"true" if both quaternions are equivalent, otherwise "false"</returns>
        public static bool operator ==(Quaternion left, Quaternion right) => (left.X == right.X) && (left.Y == right.Y) && (left.Z == right.Z) && (left.W == right.W);

        /// <summary>
        /// Compares two quaternions
        /// </summary>
        /// <param name="left">Left</param>
        /// <param name="right">Right</param>
        /// <returns>"true" if both quaternions are not equivalent, otherwise "false"</returns>
        public static bool operator !=(Quaternion left, Quaternion right) => (left.X != right.X) || (left.Y != right.Y) || (left.Z != right.Z) || (left.W != right.W);

        /// <summary>
        /// Checks if the specified object is equal to this object
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>"true" if both objects are equal, otherwise "false"</returns>
        public override bool Equals(object obj) =>
            (obj is Quaternion quaternion) &&
            (X == quaternion.X) &&
            (Y == quaternion.Y) &&
            (Z == quaternion.Z) &&
            (W == quaternion.W);

        /// <summary>
        /// Gets the hash code for this object
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            int hashCode = 767639859;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Z.GetHashCode();
            hashCode = hashCode * -1521134295 + W.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Gets the string representation of this object
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString() => $"(x: \"{ X }\", y: \"{ Y }\", z: \"{ Z }\", w: \"{ W }\")";
    }
}
