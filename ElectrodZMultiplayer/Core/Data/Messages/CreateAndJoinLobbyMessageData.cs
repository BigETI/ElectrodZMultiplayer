﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;

/// <summary>
/// ElectrodZ multiplayer data messages namespace
/// </summary>
namespace ElectrodZMultiplayer.Data.Messages
{
    /// <summary>
    /// A class that describes a message to create and join a new lobby
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class CreateAndJoinLobbyMessageData : BaseMessageData
    {
        /// <summary>
        /// Username
        /// </summary>
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// Lobby name
        /// </summary>
        [JsonProperty("lobbyName")]
        public string LobbyName { get; set; }

        /// <summary>
        /// Game mode
        /// </summary>
        [JsonProperty("gameMode")]
        public string GameMode { get; set; }

        /// <summary>
        /// Is lobby private
        /// </summary>
        [JsonProperty("isPrivate")]
        public bool? IsPrivate { get; set; }

        /// <summary>
        /// Minimal user count
        /// </summary>
        [JsonProperty("minUserCount")]
        public uint? MinimalUserCount { get; set; }

        /// <summary>
        /// Maximal user count
        /// </summary>
        [JsonProperty("maxUserCount")]
        public uint? MaximalUserCount { get; set; }

        /// <summary>
        /// Is starting game automatically
        /// </summary>
        [JsonProperty("isStartingGameAutomatically")]
        public bool? IsStartingGameAutomatically { get; set; }

        /// <summary>
        /// Game mode rules
        /// </summary>
        [JsonProperty("gameModeRules")]
        public Dictionary<string, object> GameModeRules { get; set; }

        /// <summary>
        /// Is object in a valid state
        /// </summary>
        public override bool IsValid =>
            base.IsValid &&
            (Username != null) &&
            (Username.Trim().Length >= Defaults.minimalUsernameLength) &&
            (Username.Trim().Length <= Defaults.maximalUsernameLength) &&
            (LobbyName != null) &&
            (LobbyName.Trim().Length >= Defaults.minimalLobbyNameLength) &&
            (LobbyName.Trim().Length <= Defaults.maximalLobbyNameLength) &&
            (GameMode != null) &&
            !string.IsNullOrWhiteSpace(GameMode) &&
            ((GameModeRules == null) || !GameModeRules.ContainsValue(null));

        /// <summary>
        /// Constructs a message for creating and joining a new lobby for deserializers
        /// </summary>
        public CreateAndJoinLobbyMessageData() : base()
        {
            // ...
        }

        /// <summary>
        /// Constructs a message for creating and joining a new lobby
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="lobbyName">Lobby name</param>
        /// <param name="gameMode">Game mode</param>
        /// <param name="isPrivate">Is lobby private</param>
        /// <param name="minimalUserCount">Minimal user count</param>
        /// <param name="maximalUserCount">Maximal user count</param>
        /// <param name="isStartingGameAutomatically">Is starting game automatically</param>
        /// <param name="gameModeRules">Game mode rules</param>
        public CreateAndJoinLobbyMessageData(string username, string lobbyName, string gameMode, bool? isPrivate = null, uint? minimalUserCount = null, uint? maximalUserCount = null, bool? isStartingGameAutomatically = null, IReadOnlyDictionary<string, object> gameModeRules = null) : base(Naming.GetMessageTypeNameFromMessageDataType<CreateAndJoinLobbyMessageData>())
        {
            if (username == null)
            {
                throw new ArgumentNullException(nameof(username));
            }
            string new_username = username.Trim();
            if ((new_username.Length < Defaults.minimalUsernameLength) || (new_username.Length > Defaults.maximalUsernameLength))
            {
                throw new ArgumentException($"Username must be between { Defaults.minimalUsernameLength } and { Defaults.maximalUsernameLength } characters long.", nameof(username));
            }
            if (lobbyName == null)
            {
                throw new ArgumentNullException(nameof(lobbyName));
            }
            string new_lobby_name = lobbyName.Trim();
            if ((new_lobby_name.Length < Defaults.minimalLobbyNameLength) || (new_lobby_name.Length > Defaults.maximalLobbyNameLength))
            {
                throw new ArgumentException($"Lobby name must be between { Defaults.minimalLobbyNameLength } and { Defaults.maximalLobbyNameLength } characters long.", nameof(lobbyName));
            }
            if (string.IsNullOrWhiteSpace(gameMode))
            {
                throw new ArgumentNullException(nameof(gameMode));
            }
            if ((minimalUserCount != null) && (maximalUserCount != null) && (minimalUserCount > maximalUserCount))
            {
                throw new ArgumentException("Minimal user count can't be greater than maximal user count.", nameof(minimalUserCount));
            }
            if ((gameModeRules != null) && Protection.IsContained(gameModeRules.Values, (value) => value == null))
            {
                throw new ArgumentException("Game mode rules contains null.", nameof(gameModeRules));
            }
            Username = new_username;
            LobbyName = new_lobby_name;
            GameMode = gameMode;
            IsPrivate = isPrivate;
            MinimalUserCount = minimalUserCount;
            MaximalUserCount = maximalUserCount;
            IsStartingGameAutomatically = isStartingGameAutomatically;
            if (gameModeRules != null)
            {
                GameModeRules = new Dictionary<string, object>();
                foreach (KeyValuePair<string, object> game_mode_rule in gameModeRules)
                {
                    GameModeRules.Add(game_mode_rule.Key, game_mode_rule.Value);
                }
            }
        }
    }
}
