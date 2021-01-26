using Newtonsoft.Json;
using System;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes an request to restart a game
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    internal class GameRestartRequestedMessageData : BaseMessageData
    {
        /// <summary>
        /// Time to restart game in seconds
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
        /// Constructs a game restart request message for deserializers
        /// </summary>
        public GameRestartRequestedMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a game restart request message
        /// </summary>
        /// <param name="time">Time to restart game in seconds</param>
        public GameRestartRequestedMessageData(double time) : base(Naming.GetMessageTypeNameFromMessageDataType<GameRestartRequestedMessageData>())
        {
            if (time < 0.0)
            {
                throw new ArgumentException("Time must be positive.", nameof(time));
            }
            Time = time;
        }
    }
}
