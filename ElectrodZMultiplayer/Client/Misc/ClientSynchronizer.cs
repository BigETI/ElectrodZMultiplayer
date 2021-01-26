﻿using ElectrodZMultiplayer.Data;
using ElectrodZMultiplayer.Data.Messages;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

/// <summary>
/// ElectrodZ multiplayer client namespace
/// </summary>
namespace ElectrodZMultiplayer.Client
{
    /// <summary>
    /// A class that describes a synchronizer specific to a client
    /// </summary>
    internal class ClientSynchronizer : ASynchronizer, IInternalClientSynchronizer
    {
        /// <summary>
        /// Client user
        /// </summary>
        private IInternalClientUser user;

        /// <summary>
        /// Peer
        /// </summary>
        public IPeer Peer { get; private set; }

        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; private set; }

        /// <summary>
        /// Client user
        /// </summary>
        public IClientUser User => user;

        /// <summary>
        /// Is peer connected
        /// </summary>
        public bool IsConnected => Peer != null;

        /// <summary>
        /// Is user authentificated
        /// </summary>
        public bool IsAuthentificated => user != null;

        /// <summary>
        /// This event will be invoked when an authentification was acknowledged.
        /// </summary>
        public override event AuthentificationAcknowledgedDelegate OnAuthentificationAcknowledged;

        /// <summary>
        /// This event will be invoked when an authentification has failed.
        /// </summary>
        public override event AuthentificationFailedDelegate OnAuthentificationFailed;

        /// <summary>
        /// This event will be invoked when lobbies have been listed.
        /// </summary>
        public override event LobbiesListedDelegate OnLobbiesListed;

        /// <summary>
        /// This event will be invoked when listing lobbies has failed.
        /// </summary>
        public override event ListLobbiesFailedDelegate OnListLobbiesFailed;

        /// <summary>
        /// This event will be invoked when available game modes have been listed.
        /// </summary>
        public override event AvailableGameModesListedDelegate OnAvailableGameModesListed;

        /// <summary>
        /// This event will be invoked when listing available game modes has failed.
        /// </summary>
        public override event ListAvailableGameModesFailedDelegate OnListAvailableGameModesFailed;

        /// <summary>
        /// On lobby join acknowledged
        /// </summary>
        public event LobbyJoinAcknowledgedDelegate OnLobbyJoinAcknowledged;

        /// <summary>
        /// This event will be invoked when joining a lobby has failed.
        /// </summary>
        public override event JoinLobbyFailedDelegate OnJoinLobbyFailed;

        /// <summary>
        /// This event will be invoked when creating a lobby has failed.
        /// </summary>
        public override event CreateLobbyFailedDelegate OnCreateLobbyFailed;

        /// <summary>
        /// This event will be invoked when changing username has failed.
        /// </summary>
        public override event ChangeUsernameFailedDelegate OnChangeUsernameFailed;

        /// <summary>
        /// This event will be invoked when changing user lobby color has failed.
        /// </summary>
        public override event ChangeUserLobbyColorFailedDelegate OnChangeUserLobbyColorFailed;

        /// <summary>
        /// This event will be invoked when changing lobby rules have failed.
        /// </summary>
        public override event ChangeLobbyRulesFailedDelegate OnChangeLobbyRulesFailed;

        /// <summary>
        /// This event will be invoked when kicking a user has failed.
        /// </summary>
        public override event KickUserFailedDelegate OnKickUserFailed;

        /// <summary>
        /// This event will be invoked when starting a game has failed.
        /// </summary>
        public override event StartGameFailedDelegate OnStartGameFailed;

        /// <summary>
        /// This event will be invoked when restarting a game has failed.
        /// </summary>
        public override event RestartGameFailedDelegate OnRestartGameFailed;

        /// <summary>
        /// This event will be invoked when stopping a game has failed.
        /// </summary>
        public override event StopGameFailedDelegate OnStopGameFailed;

        /// <summary>
        /// This event will be invoked when a client tick has failed.
        /// </summary>
        public override event ClientTickFailedDelegate OnClientTickFailed;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="peer">Peer</param>
        /// <param name="token">Token</param>
        public ClientSynchronizer(IPeer peer, string token) : base()
        {
            Peer = peer ?? throw new ArgumentNullException(nameof(peer));
            AddAutomaticMessageParserWithFatality<AuthentificationAcknowledgedMessageData>
            (
                (_, message, __) =>
                {
                    if (IsAuthentificated)
                    {
                        SendErrorMessageToPeer<AuthentificationAcknowledgedMessageData>(peer, EErrorType.InvalidMessageContext, "Authentification has been already acknowledged.", true);
                    }
                    else
                    {
                        Token = message.Token;
                        user = new ClientUser(message.GUID);
                        OnAuthentificationAcknowledged?.Invoke(User);
                    }
                }
            );
            AddAutomaticMessageParser<AuthentificationFailedMessageData>((currentPeer, message, _) => OnAuthentificationFailed?.Invoke(currentPeer, message.Message, message.Reason));
            AddAutomaticMessageParser<ListLobbyResultsMessageData>
            (
                (_, message, __) =>
                {
                    ILobbyView[] lobbies = new ILobbyView[message.Lobbies.Count];
                    Parallel.For(0, lobbies.Length, (index) => lobbies[index] = (LobbyView)message.Lobbies[index]);
                    OnLobbiesListed?.Invoke(lobbies);
                }
            );
            AddAutomaticMessageParser<ListLobbiesFailedMessageData>((currentPeer, message, _) => OnListLobbiesFailed?.Invoke(currentPeer, message.Message, message.Reason));
            AddAutomaticMessageParser<ListAvailableGameModeResultsMessageData>((_, message, __) => OnAvailableGameModesListed?.Invoke(message.GameModes));
            AddAutomaticMessageParser<ListAvailableGameModesFailedMessageData>((currentPeer, message, _) => OnListAvailableGameModesFailed?.Invoke(currentPeer, message.Message, message.Reason));
            AddAutomaticMessageParser<JoinLobbyAcknowledgedMessageData>
            (
                (_, message, __) => AssertIsUserAuthentificated<JoinLobbyAcknowledgedMessageData>
                (
                    (clientUser) =>
                    {
                        if (clientUser.Lobby == null)
                        {
                            Dictionary<string, IUser> users = new Dictionary<string, IUser>();
                            List<IInternalClientUser> user_list = new List<IInternalClientUser>();
                            foreach (UserData user in message.Users)
                            {
                                IInternalClientUser client_user;
                                if (user.GUID == clientUser.GUID)
                                {
                                    client_user = clientUser;
                                    this.user.SetNameInternally(user.Name);
                                    this.user.SetLobbyColorInternally(user.LobbyColor);
                                }
                                else
                                {
                                    client_user = new ClientUser(user.GUID, user.GameColor, user.Name, user.LobbyColor);
                                }
                                users.Add(user.GUID.ToString(), client_user);
                                user_list.Add(client_user);
                            }
                            IInternalClientLobby client_lobby = new ClientLobby(this, message.Rules.LobbyCode, message.Rules.Name, message.Rules.MinimalUserCount, message.Rules.MaximalUserCount, message.Rules.IsStartingGameAutomatically, message.Rules.GameMode, message.Rules.GameModeRules, users[message.OwnerGUID.ToString()], users);
                            user.ClientLobby = client_lobby;
                            foreach (IInternalClientUser client_user in user_list)
                            {
                                client_user.ClientLobby = client_lobby;
                            }
                            user_list.Clear();
                            OnLobbyJoinAcknowledged?.Invoke(client_lobby);
                        }
                        else
                        {
                            SendInvalidMessageContextErrorMessageToPeer<JoinLobbyAcknowledgedMessageData>(peer, $"User is already in lobby \"{ clientUser.Lobby.Name }\" with lobby code \"{ clientUser.Lobby.LobbyCode }\".");
                        }
                    }
                )
            );
            AddAutomaticMessageParser<JoinLobbyFailedMessageData>((currentPeer, message, _) => OnJoinLobbyFailed?.Invoke(currentPeer, message.Message, message.Reason));
            AddAutomaticMessageParser<CreateLobbyFailedMessageData>((currentPeer, message, _) => OnCreateLobbyFailed?.Invoke(currentPeer, message.Message, message.Reason));
            AddAutomaticMessageParser<LobbyRulesChangedMessageData>
            (
                (_, message, __) => AssertIsUserInLobby<LobbyRulesChangedMessageData>
                (
                    (clientUser, clientLobby) =>
                    {
                        LobbyRulesData rules = message.Rules;
                        clientLobby.UpdateGameModeRulesInternally
                        (
                            rules.LobbyCode,
                            rules.Name,
                            rules.GameMode,
                            rules.MinimalUserCount,
                            rules.MaximalUserCount,
                            rules.IsStartingGameAutomatically,
                            rules.GameModeRules
                        );
                    }
                )
            );
            AddAutomaticMessageParser<UserJoinedMessageData>
            (
                (_, message, __) => AssertIsUserInLobby<UserJoinedMessageData>
                (
                    (clientUser, clientLobby) =>
                    {
                        IUser user = new ClientUser(message.GUID, message.GameColor, message.Name, message.LobbyColor);
                        if (!clientLobby.AddUserInternally(user))
                        {
                            SendInvalidMessageContextErrorMessageToPeer<UserJoinedMessageData>(peer, $"Failed to add user \"{ user.Name }\" with GUID \"{ user.GUID }\" to lobby \"{ clientLobby.Name }\" with lobby code \"{ clientLobby.LobbyCode }\".");
                        }
                    }
                )
            );
            AddAutomaticMessageParser<UserLeftMessageData>
            (
                (_, message, __) => AssertTargetLobbyUser<UserLeftMessageData>
                (
                    message.GUID,
                    (clientUser, clientLobby, targetUser) =>
                    {
                        if (!clientLobby.RemoveUserInternally(targetUser, message.Reason, message.Message))
                        {
                            SendInvalidMessageContextErrorMessageToPeer<UserLeftMessageData>(peer, $"Failed to remove user \"{ targetUser.Name }\" with GUID \"{ targetUser.GUID }\" from lobby \"{ clientLobby.Name }\" with lobby code \"{ clientLobby.LobbyCode }\".");
                        }
                        targetUser.ClientLobby = null;
                    }
                )
            );
            AddAutomaticMessageParser<UsernameChangedMessageData>((_, message, __) => AssertTargetLobbyUser<UsernameChangedMessageData>(message.GUID, (clientUser, clientLobby, targetUser) => targetUser.SetNameInternally(message.NewUsername)));
            AddAutomaticMessageParser<ChangeUsernameFailedMessageData>((currentPeer, message, _) => OnChangeUsernameFailed?.Invoke(currentPeer, message.Message, message.Reason));
            AddAutomaticMessageParser<UserLobbyColorChangedMessageData>((_, message, __) => AssertTargetLobbyUser<UserLobbyColorChangedMessageData>(message.GUID, (clientUser, clientLobby, targetUser) => targetUser.SetLobbyColorInternally(message.NewLobbyColor)));
            AddAutomaticMessageParser<ChangeUserLobbyColorFailedMessageData>((currentPeer, message, _) => OnChangeUserLobbyColorFailed?.Invoke(currentPeer, message.Message, message.Reason));
            AddAutomaticMessageParser<ChangeLobbyRulesFailedMessageData>((currentPeer, message, _) => OnChangeLobbyRulesFailed?.Invoke(currentPeer, message.Message, message.Reason));
            AddAutomaticMessageParser<KickUserFailedMessageData>((currentPeer, message, _) => OnKickUserFailed?.Invoke(currentPeer, message.Message, message.Reason));
            AddAutomaticMessageParser<GameStartRequestedMessageData>((_, message, __) => AssertIsUserInLobby<GameStartRequestedMessageData>((clientUser, clientLobby) => clientLobby.InvokeGameStartRequestedEventInternally(message.Time)));
            AddAutomaticMessageParser<GameStartedMessageData>((_, message, __) => AssertIsUserInLobby<GameStartedMessageData>((clientUser, clientLobby) => clientLobby.InvokeGameStartedEventInternally()));
            AddAutomaticMessageParser<StartGameFailedMessageData>((currentPeer, message, _) => OnStartGameFailed?.Invoke(currentPeer, message.Message, message.Reason));
            AddAutomaticMessageParser<GameRestartRequestedMessageData>((_, message, __) => AssertIsUserInLobby<GameRestartRequestedMessageData>((clientUser, clientLobby) => clientLobby.InvokeGameRestartRequestedEventInternally(message.Time)));
            AddAutomaticMessageParser<GameRestartedMessageData>((_, message, js__on) => AssertIsUserInLobby<GameRestartedMessageData>((clientUser, clientLobby) => clientLobby.InvokeGameRestartedEventInternally()));
            AddAutomaticMessageParser<RestartGameFailedMessageData>((currentPeer, message, _) => OnRestartGameFailed?.Invoke(currentPeer, message.Message, message.Reason));
            AddAutomaticMessageParser<GameStopRequestedMessageData>((_, message, __) => AssertIsUserInLobby<GameStopRequestedMessageData>((clientUser, clientLobby) => clientLobby.InvokeGameStopRequestedEventInternally(message.Time)));
            AddAutomaticMessageParser<GameStoppedMessageData>
            (
                (_, message, __) => AssertIsUserInLobby<GameStoppedMessageData>
                (
                    (clientUser, clientLobby) =>
                    {
                        Dictionary<string, UserWithResults> users = new Dictionary<string, UserWithResults>();
                        foreach (GameEndUserData user in message.Users)
                        {
                            string key = user.GUID.ToString();
                            if (clientLobby.Users.ContainsKey(key))
                            {
                                users.Add(key, new UserWithResults(clientLobby.Users[key], user.Results));
                            }
                            else
                            {
                                SendErrorMessage<GameStoppedMessageData>(EErrorType.InvalidMessageContext, $"User with GUID \"{ key }\" is not in lobby \"{ clientLobby.Name }\" with lobby code \"{ clientLobby.LobbyCode }\".");
                            }
                        }
                        clientLobby.InvokeGameStoppedEventInternally(users, message.Results);
                    }
                )
            );
            AddAutomaticMessageParser<StopGameFailedMessageData>((currentPeer, message, _) => OnStopGameFailed?.Invoke(currentPeer, message.Message, message.Reason));
            AddAutomaticMessageParser<ServerTickMessageData>((_, message, __) => AssertIsUserInLobby<ServerTickMessageData>((clientUser, clientLobby) => clientLobby.ProcessServerTickInternally(message.Time, message.Entities)));
            AddAutomaticMessageParser<ClientTickFailedMessageData>((currentPeer, message, _) => OnClientTickFailed?.Invoke(currentPeer, message.Message, message.Reason));
            OnPeerConnected += (_) => SendAuthenticateMessage(token);
        }

        /// <summary>
        /// Asserts that user is authentificated
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="onUserIsAuthentificated">On user is authentificated</param>
        private void AssertIsUserAuthentificated<T>(UserIsAuthentificatedDelegate onUserIsAuthentificated) where T : IBaseMessageData
        {
            if (User == null)
            {
                SendErrorMessage<T>(EErrorType.InvalidMessageContext, "User has not been authentificated yet.", true);
            }
            else if (User is IInternalClientUser client_user)
            {
                onUserIsAuthentificated(client_user);
            }
            else
            {
                SendErrorMessage<T>(EErrorType.Unknown, $"User \"{ User.Name }\" with GUID \"{ User.GUID }\" does not inherit from \"{ nameof(IInternalClientUser) }\".");
            }
        }

        /// <summary>
        /// Asserts that user is in lobby
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="onUserIsInLobby">On user is in lobby</param>
        private void AssertIsUserInLobby<T>(UserIsInLobbyDelegate onUserIsInLobby) where T : IBaseMessageData =>
            AssertIsUserAuthentificated<T>
            (
                (clientUser) =>
                {
                    if (clientUser.ClientLobby == null)
                    {
                        SendErrorMessage<T>(EErrorType.InvalidMessageContext, "User is not in any lobby yet.");
                    }
                    else
                    {
                        onUserIsInLobby(clientUser, clientUser.ClientLobby);
                    }
                }
            );

        /// <summary>
        /// Asserts that a user is being targeted in a lobby
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="targetUserGUID">Target user GUID</param>
        /// <param name="onUserIsInLobby">On user is in lobby</param>
        private void AssertTargetLobbyUser<T>(Guid targetUserGUID, TargetLobbyUserDelegate onTargetLobbyUser) where T : IBaseMessageData =>
            AssertIsUserInLobby<T>
            (
                (clientUser, clientLobby) =>
                {
                    IReadOnlyDictionary<string, IUser> users = clientLobby.Users;
                    string key = targetUserGUID.ToString();
                    if (users.ContainsKey(key))
                    {
                        if (users[key] is IInternalClientUser client_user)
                        {
                            onTargetLobbyUser(clientUser, clientLobby, client_user);
                        }
                        else
                        {
                            SendErrorMessage<T>(EErrorType.Unknown, $"User \"{ users[key].Name }\" with GUID \"{ key }\" does not inherit from \"{ nameof(IInternalClientUser) }\".");
                        }
                    }
                    else
                    {
                        SendErrorMessage<T>(EErrorType.InvalidMessageContext, $"User with GUID \"{ key }\" is not in lobby \"{ User.Lobby.Name }\" with lobby code \"{ User.Lobby.LobbyCode }\".");
                    }
                }
            );

        /// <summary>
        /// Sends a request to create and join a lobby
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="lobbyName">Lobby name</param>
        /// <param name="gameMode">Game mode</param>
        /// <param name="minimalUserCount">Minimal user count</param>
        /// <param name="maximalUserCount">Maximal user count</param>
        /// <param name="isStartingGameAutomatically">Is starting game automatically</param>
        /// <param name="gameModeRules">Game mode rules</param>
        public void CreateAndJoinLobby(string username, string lobbyName, string gameMode, uint? minimalUserCount = null, uint? maximalUserCount = null, bool? isStartingGameAutomatically = null, IReadOnlyDictionary<string, object> gameModeRules = null)
        {
            if (User.Lobby == null)
            {
                SendCreateAndJoinLobbyMessage(username, lobbyName, gameMode, minimalUserCount, maximalUserCount, isStartingGameAutomatically, gameModeRules);
            }
        }

        /// <summary>
        /// Joins a lobby with the specified lobby code
        /// </summary>
        /// <param name="lobbyCode">Lobby code</param>
        /// <param name="username">Username</param>
        public void JoinLobby(string lobbyCode, string username)
        {
            if (User.Lobby == null)
            {
                SendJoinLobbyMessage(lobbyCode, username);
            }
        }

        /// <summary>
        /// Process events
        /// </summary>
        public void ProcessEvents()
        {
            foreach (IConnector connector in Connectors)
            {
                connector.ProcessEvents();
            }
        }

        /// <summary>
        /// Sends a message to peer
        /// </summary>
        /// <typeparam name="T">Object type</typeparam>
        /// <param name="message">Message</param>
        public void SendMessage<T>(T message) where T : IBaseMessageData => SendMessageToPeer(Peer, message);

        /// <summary>
        /// Sends an authenticate message to peer
        /// </summary>
        public void SendAuthenticateMessage() => SendAuthenticateMessage(null);

        /// <summary>
        /// Sends an authenticate message to peer
        /// </summary>
        /// <param name="token">Existing authentification token</param>
        public void SendAuthenticateMessage(string token) => SendMessage(new AuthenticateMessageData(token));

        /// <summary>
        /// Sends a list available game modes message
        /// </summary>
        /// <param name="name">Game mode name filter</param>
        public void SendListAvailableGameModesMessage(string name = null) => SendMessage(new ListAvailableGameModesMessageData(name));

        /// <summary>
        /// Sends a list lobbies message to peer
        /// </summary>
        /// <param name="excludeFull">Exclude full lobbies</param>
        /// <param name="name">Lobby name</param>
        /// <param name="minimalUserCount">Minimal user count</param>
        /// <param name="maximalUserCount">Maximal user count</param>
        /// <param name="isStartingGameAutomatically">Is starting game automatically</param>
        /// <param name="gameMode">Game mode</param>
        /// <param name="gameModeRules">Game mode rules</param>
        public void SendListLobbiesMessage(bool? excludeFull = null, string name = null, uint? minimalUserCount = null, uint? maximalUserCount = null, bool? isStartingGameAutomatically = null, string gameMode = null, IReadOnlyDictionary<string, object> gameModeRules = null)
        {
            Dictionary<string, object> game_mode_rules = null;
            if (gameModeRules != null)
            {
                game_mode_rules = new Dictionary<string, object>();
                foreach (KeyValuePair<string, object> game_mode_rule in gameModeRules)
                {
                    game_mode_rules.Add(game_mode_rule.Key, game_mode_rule.Value);
                }
            }
            SendMessage(new ListLobbiesMessageData(excludeFull, name, minimalUserCount, maximalUserCount, isStartingGameAutomatically, gameMode, game_mode_rules));
        }

        /// <summary>
        /// Sends a join lobby message to peer
        /// </summary>
        /// <param name="lobbyCode">Lobby code</param>
        /// <param name="username">Username</param>
        public void SendJoinLobbyMessage(string lobbyCode, string username) => SendMessage(new JoinLobbyMessageData(lobbyCode, username));

        /// <summary>
        /// Sends a create and join lobby message to peer
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="lobbyName">Lobby name</param>
        /// <param name="gameMode">Game mode</param>
        /// <param name="minimalUserCount">Minimal user count</param>
        /// <param name="maximalUserCount">Maximal user count</param>
        /// <param name="isStartingGameAutomatically">Is starting game automatically</param>
        /// <param name="gameModeRules">Game mode rules</param>
        public void SendCreateAndJoinLobbyMessage(string username, string lobbyName, string gameMode, uint? minimalUserCount = null, uint? maximalUserCount = null, bool? isStartingGameAutomatically = null, IReadOnlyDictionary<string, object> gameModeRules = null)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentNullException(nameof(username));
            }
            if (string.IsNullOrWhiteSpace(lobbyName))
            {
                throw new ArgumentNullException(nameof(lobbyName));
            }
            if (string.IsNullOrWhiteSpace(gameMode))
            {
                throw new ArgumentNullException(nameof(gameMode));
            }
            Dictionary<string, object> game_mode_rules = null;
            if (gameModeRules != null)
            {
                game_mode_rules = new Dictionary<string, object>();
                foreach (KeyValuePair<string, object> game_mode_rule in gameModeRules)
                {
                    game_mode_rules.Add(game_mode_rule.Key, game_mode_rule.Value);
                }
            }
            SendMessage(new CreateAndJoinLobbyMessageData(username, lobbyName, gameMode, minimalUserCount, maximalUserCount, isStartingGameAutomatically, game_mode_rules));
        }

        /// <summary>
        /// Sends a quit lobbym message to peer
        /// </summary>
        public void SendQuitLobbyMessage() => SendMessage(new QuitLobbyMessageData());

        /// <summary>
        /// Sends a change username message to peer
        /// </summary>
        /// <param name="newUsername">New username</param>
        public void SendChangeUsernameMessage(string newUsername) => SendMessage(new ChangeUsernameMessageData(newUsername));

        /// <summary>
        /// Send change lobby color message
        /// </summary>
        /// <param name="lobbyColor">Lobby color</param>
        public void SendChangeLobbyColorMessage(Color lobbyColor) => SendMessage(new ChangeUserLobbyColorMessageData(lobbyColor));

        /// <summary>
        /// Sends a change lobby rules message to peer
        /// </summary>
        /// <param name="name">Lobby name (optional)</param>
        /// <param name="gameMode">Game mode (optional)</param>
        /// <param name="minimalUserCount">Minimal user count (optional)</param>
        /// <param name="maximalUserCount">Maximal user count (optional)</param>
        /// <param name="isStartingGameAutomatically">Is starting game automatically (optional)</param>
        /// <param name="gameModeRules">Game mode rules (optional)</param>
        public void SendChangeLobbyRules(string name = null, string gameMode = null, uint? minimalUserCount = null, uint? maximalUserCount = null, bool? isStartingGameAutomatically = null, IReadOnlyDictionary<string, object> gameModeRules = null)
        {
            Dictionary<string, object> game_mode_rules = null;
            if (gameModeRules != null)
            {
                game_mode_rules = new Dictionary<string, object>();
                foreach (KeyValuePair<string, object> game_mode_rule in gameModeRules)
                {
                    game_mode_rules.Add(game_mode_rule.Key, game_mode_rule.Value);
                }
            }
            SendMessage(new ChangeLobbyRulesMessageData(name, gameMode, minimalUserCount, maximalUserCount, isStartingGameAutomatically, game_mode_rules));
        }

        /// <summary>
        /// Sends a kick user message to peer
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="reason">Reason</param>
        public void SendKickUserMessage(IUser user, string reason)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            SendMessage(new KickUserMessageData(user.GUID, reason));
        }

        /// <summary>
        /// Sends a start game message to peer
        /// </summary>
        /// <param name="time">Time to start game in seconds</param>
        public void SendStartGameMessage(double time) => SendMessage(new StartGameMessageData(time));

        /// <summary>
        /// Sends a restart game message to peer
        /// </summary>
        /// <param name="time">Time to restart game in seconds</param>
        public void SendRestartGameMessage(double time) => SendMessage(new RestartGameMessageData(time));

        /// <summary>
        /// Sends a stop game message to peer
        /// </summary>
        /// <param name="time">Time to stop game in seconds</param>
        public void SendStopGameMessage(double time) => SendMessage(new StopGameMessageData(time));

        /// <summary>
        /// Sends a client tick message
        /// </summary>
        /// <param name="entities">Entities to update</param>
        public void SendClientTickMessage(IEnumerable<IEntityDelta> entities = null) => SendMessage(new ClientTickMessageData(entities));

        /// <summary>
        /// Sends an error message
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="errorType">Error type</param>
        /// <param name="errorMessage">Error message</param>
        public void SendErrorMessage<T>(EErrorType errorType, string errorMessage) where T : IBaseMessageData => SendErrorMessageToPeer<T>(Peer, errorType, errorMessage);

        /// <summary>
        /// Sends an error message
        /// </summary>
        /// <typeparam name="T">Message type</typeparam>
        /// <param name="errorType">Error tyoe</param>
        /// <param name="errorMessage">Error message</param>
        /// <param name="isFatal">Is error fatal</param>
        public void SendErrorMessage<T>(EErrorType errorType, string errorMessage, bool isFatal) where T : IBaseMessageData => SendErrorMessageToPeer<T>(Peer, errorType, errorMessage, isFatal);
    }
}
