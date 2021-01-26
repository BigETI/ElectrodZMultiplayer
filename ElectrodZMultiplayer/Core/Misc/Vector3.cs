using System;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// 3D vector structure
    /// </summary>
    public readonly struct Vector3
    {
        /// <summary>
        /// Zero
        /// </summary>
        public static Vector3 Zero { get; } = new Vector3();

        /// <summary>
        /// X component of a 3D vector
        /// </summary>
        public float X { get; }

        /// <summary>
        /// Y component of a 3D vector
        /// </summary>
        public float Y { get; }

        /// <summary>
        /// Z component of a 3D vector
        /// </summary>
        public float Z { get; }

        /// <summary>
        /// Magnitude squared
        /// </summary>
        public float MagnitudeSquared => (X * X) + (Y * Y) + (Z * Z);

        /// <summary>
        /// Magnitude squared
        /// </summary>
        public float Magnitude => (float)Math.Sqrt(MagnitudeSquared);

        /// <summary>
        /// COnstructs a 3D vector
        /// </summary>
        /// <param name="x">X component of a 3D vector</param>
        /// <param name="y">Y component of a 3D vector</param>
        /// <param name="z">Z component of a 3D vector</param>
        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Adds two vectors together
        /// </summary>
        /// <param name="left">Left</param>
        /// <param name="right">Right</param>
        /// <returns>Added vector</returns>
        public static Vector3 operator +(Vector3 left, Vector3 right) => new Vector3(left.X + right.X, left.Y + right.Y, left.Z + right.Z);

        /// <summary>
        /// Subtracts two vectors together
        /// </summary>
        /// <param name="left">Left</param>
        /// <param name="right">Right</param>
        /// <returns>Subtracted vector</returns>
        public static Vector3 operator -(Vector3 left, Vector3 right) => new Vector3(left.X - right.X, left.Y - right.Y, left.Z - right.Z);

        /// <summary>
        /// Compares two vectors
        /// </summary>
        /// <param name="left">Left</param>
        /// <param name="right">Right</param>
        /// <returns>"true" if both vectors are equivalent, otherwise "false"</returns>
        public static bool operator ==(Vector3 left, Vector3 right) => (left.X == right.X) && (left.Y == right.Y) && (left.Z == right.Z);

        /// <summary>
        /// Compares two vectors
        /// </summary>
        /// <param name="left">Left</param>
        /// <param name="right">Right</param>
        /// <returns>"true" if both vectors are not equivalent, otherwise "false"</returns>
        public static bool operator !=(Vector3 left, Vector3 right) => (left.X != right.X) || (left.Y != right.Y) || (left.Z != right.Z);

        /// <summary>
        /// Checks if the specified object is equal to this object
        /// </summary>
        /// <param name="obj">Object</param>
        /// <returns>"true" if both objects are equal, otherwise "false"</returns>
        public override bool Equals(object obj) =>
            (obj is Vector3 vector) &&
            (X == vector.X) &&
            (Y == vector.Y) &&
            (Z == vector.Z);

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
            return hashCode;
        }
    }
}
