using Newtonsoft.Json;

/// <summary>
/// ElectrodZ multiplayer data namespace
/// </summary>
namespace ElectrodZMultiplayer.Data
{
    /// <summary>
    /// A class that describes quaternion data
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class QuaternionFloatData
    {
        /// <summary>
        /// W component of the quaternion
        /// </summary>
        [JsonProperty("w")]
        public float W { get; set; }

        /// <summary>
        /// X component of the quaternion
        /// </summary>
        [JsonProperty("x")]
        public float X { get; set; }

        /// <summary>
        /// Y component of the quaternion
        /// </summary>
        [JsonProperty("y")]
        public float Y { get; set; }

        /// <summary>
        /// Z component of the quaternion
        /// </summary>
        [JsonProperty("z")]
        public float Z { get; set; }

        /// <summary>
        /// Constructs quaternion data for deserializers
        /// </summary>
        public QuaternionFloatData()
        {
            // ...
        }

        /// <summary>
        /// Constructs quaternion data
        /// </summary>
        /// <param name="w">W component of the quaternion</param>
        /// <param name="x">X component of the quaternion</param>
        /// <param name="y">Y component of the quaternion</param>
        /// <param name="z">Z component of the quaternion</param>
        public QuaternionFloatData(float w, float x, float y, float z)
        {
            W = w;
            X = x;
            Y = y;
            Z = z;
        }

        /// <summary>
        /// Explicitly casts a quaternion to quaternion data
        /// </summary>
        /// <param name="quaternion">Quaternion</param>
        public static explicit operator QuaternionFloatData(Quaternion<float> quaternion) => new QuaternionFloatData(quaternion.W, quaternion.X, quaternion.Y, quaternion.Z);
    }
}
