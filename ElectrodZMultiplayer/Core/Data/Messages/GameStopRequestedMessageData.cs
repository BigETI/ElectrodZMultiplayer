using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a game stop request message
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class GameStopRequestedMessageData : BaseMessageData
    {
        /// <summary>
        /// Time to stop game in seconds
        /// </summary>
        [JsonProperty("time")]
        public float Time { get; set; }

        /// <summary>
        /// Is valid
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (Time >= 0.0f);

        /// <summary>
        /// Constructs a game stop request message for deserializers
        /// </summary>
        public GameStopRequestedMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a game stop request message
        /// </summary>
        /// <param name="time">Time to stop game in seconds</param>
        public GameStopRequestedMessageData(float time) : base(Naming.GetMessageTypeNameFromMessageDataType<GameStopRequestedMessageData>())
        {
            if (time < 0.0f)
            {
                throw new ArgumentException("Time must be positive.", nameof(time));
            }
            Time = time;
        }
    }
}
