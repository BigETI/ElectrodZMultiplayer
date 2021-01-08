using Newtonsoft.Json;

/// <summary>
/// ElectrodZ multiplayer data namespace
/// </summary>
namespace ElectrodZMultiplayer.Data
{
    /// <summary>
    /// A class that describes 3D vector data
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class Vector3FloatData
    {
        /// <summary>
        /// X component of the 3D vector
        /// </summary>
        [JsonProperty("x")]
        public float X { get; set; }

        /// <summary>
        /// Y component of the 3D vector
        /// </summary>
        [JsonProperty("y")]
        public float Y { get; set; }

        /// <summary>
        /// Z component of the 3D vector
        /// </summary>
        [JsonProperty("z")]
        public float Z { get; set; }

        /// <summary>
        /// Constructs 3D vector data for deserializers
        /// </summary>
        public Vector3FloatData()
        {
            // ...
        }

        /// <summary>
        /// Constructs 3D vector data
        /// </summary>
        /// <param name="x">X component of the 3D vector</param>
        /// <param name="y">Y component of the 3D vector</param>
        /// <param name="z">Z component of the 3D vector</param>
        public Vector3FloatData(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Explicitly casts a 3D vector to 3D vector data
        /// </summary>
        /// <param name="vector">3D Vector</param>
        public static explicit operator Vector3FloatData(Vector3<float> vector) => new Vector3FloatData(vector.X, vector.Y, vector.Z);
    }
}
