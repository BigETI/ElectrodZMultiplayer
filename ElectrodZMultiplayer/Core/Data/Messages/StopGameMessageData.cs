using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a stop game request message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class StopGameMessageData : BaseMessageData
    {
        /// <summary>
        /// Time to stop game in seconds
        /// </summary>
        [JsonProperty("time")]
        public float Time { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (Time >= 0.0f);

        /// <summary>
        /// Constructs a game stop request message for deserializers
        /// </summary>
        public StopGameMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a game stop request message
        /// </summary>
        /// <param name="time">Time to start game in seconds</param>
        public StopGameMessageData(float time) : base(Naming.GetMessageTypeNameFromMessageDataType<StopGameMessageData>())
        {
            if (time < 0.0f)
            {
                throw new ArgumentException("Time must be positive", nameof(time));
            }
            Time = time;
        }
    }
}
