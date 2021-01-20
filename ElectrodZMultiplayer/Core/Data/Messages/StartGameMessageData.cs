using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a start game request message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class StartGameMessageData : BaseMessageData
    {
        /// <summary>
        /// Time to start game in seconds
        /// </summary>
        [JsonProperty("time")]
        public double Time { get; set; }

        /// <summary>
        /// Is valid
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (Time >= 0.0);

        /// <summary>
        /// Constructs a start game request message for deserializers
        /// </summary>
        public StartGameMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a start game request message
        /// </summary>
        /// <param name="time">Time to start game in seconds</param>
        public StartGameMessageData(double time) : base(Naming.GetMessageTypeNameFromMessageDataType<StartGameMessageData>())
        {
            if (time < 0.0)
            {
                throw new ArgumentException("Time must be positive", nameof(time));
            }
            Time = time;
        }
    }
}
