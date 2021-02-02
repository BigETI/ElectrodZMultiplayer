using ElectrodZMultiplayer;
using ElectrodZMultiplayer.Server;
using ElectrodZServer.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

/// <summary>
/// ElectrodZ server namespace
/// </summary>
namespace ElectrodZServer
{
    /// <summary>
    /// A class that represents this program
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Server JSON path
        /// </summary>
        private static readonly string serverJSONFilePath = "./server.json";

        /// <summary>
        /// Bans JSON file path
        /// </summary>
        private static readonly string bansJSONFilePath = "./bans.json";

        /// <summary>
        /// Output logger
        /// </summary>
        private static Logger outputLogger;

        /// <summary>
        /// Error logger
        /// </summary>
        private static Logger errorLogger;

        /// <summary>
        /// Command line commands
        /// </summary>
        private static readonly ICommands commandLineCommands = new Commands();

        /// <summary>
        /// Console commands
        /// </summary>
        private static readonly ICommands consoleCommands = new Commands();

        /// <summary>
        /// Server configuration data
        /// </summary>
        private static ServerConfigurationData serverConfigurationData;

        /// <summary>
        /// Keep server running
        /// </summary>
        private static bool keepServerRunning = true;

        /// <summary>
        /// Server
        /// </summary>
        private static IServerSynchronizer server;

        /// <summary>
        /// "help" command line command executed event
        /// </summary>
        /// <param name="arguments">Arguments</param>
        private static void HelpCommandLineCommandExecutedEvent(IReadOnlyList<string> arguments)
        {
            keepServerRunning = false;
            PrintHelpTopic(commandLineCommands, "-", (arguments.Count > 0) ? arguments[0] : string.Empty);
        }

        /// <summary>
        /// "port" command line command executed event
        /// </summary>
        /// <param name="arguments">Arguments</param>
        private static void PortCommandLineCommandExecutedEvent(IReadOnlyList<string> arguments)
        {
            if (arguments.Count > 0)
            {
                string port_name = arguments[0];
                if (ushort.TryParse(port_name, out ushort port))
                {
                    if (port == 0)
                    {
                        keepServerRunning = false;
                        WriteErrorLogLine("\"0\" is not a valid port.");
                    }
                    else
                    {
                        serverConfigurationData.NetworkPort = port;
                    }
                }
                else
                {
                    keepServerRunning = false;
                    WriteErrorLogLine($"\"{ port_name }\" is not a valid port.");
                }
            }
        }

        /// <summary>
        /// "tickrate" command line command executed event
        /// </summary>
        /// <param name="arguments">Arguments</param>
        private static void TickRateCommandLineCommandExecutedEvent(IReadOnlyList<string> arguments)
        {
            if (arguments.Count > 0)
            {
                string tick_rate_name = arguments[0];
                if (ushort.TryParse(tick_rate_name, out ushort tick_rate))
                {
                    if ((tick_rate <= 0) || (tick_rate > 1000))
                    {
                        keepServerRunning = false;
                        WriteErrorLogLine($"\"{ tick_rate }\" is not a valid tick rate.");
                    }
                    else
                    {
                        serverConfigurationData.TickRate = tick_rate;
                    }
                }
                else
                {
                    keepServerRunning = false;
                    WriteErrorLogLine($"\"{ tick_rate_name }\" is not a valid tick rate.");
                }
            }
        }

        /// <summary>
        /// "help" console command executed event
        /// </summary>
        /// <param name="arguments">Arguments</param>
        private static void HelpConsoleCommmandExecutedEvent(IReadOnlyList<string> arguments) => PrintHelpTopic(consoleCommands, string.Empty, (arguments.Count > 0) ? arguments[0] : string.Empty);

        /// <summary>
        /// "connectors" console command executed event
        /// </summary>
        /// <param name="arguments"></param>
        private static void ConnectorsConsoleCommmandExecutedEvent(IReadOnlyList<string> arguments)
        {
            bool has_no_connectors = true;
            foreach (IConnector connector in server.Connectors)
            {
                if (has_no_connectors)
                {
                    has_no_connectors = false;
                    WriteOutputLogLine("Connectors: ");
                }
                WriteOutputLogLine($"\t{ connector.GetType().FullName }");
            }
            if (has_no_connectors)
            {
                WriteOutputLogLine("There are no connectors.");
            }
        }

        /// <summary>
        /// "gameresources" console command executed event
        /// </summary>
        /// <param name="arguments">Arguments</param>
        private static void GameResourcesConsoleCommmandExecutedEvent(IReadOnlyList<string> arguments)
        {
            bool has_no_game_resources = true;
            foreach (string game_resource in server.GameResources.Keys)
            {
                if (has_no_game_resources)
                {
                    has_no_game_resources = false;
                    WriteOutputLogLine("Loaded game resources: ");
                }
                WriteOutputLogLine($"\t{ game_resource }");
            }
            if (has_no_game_resources)
            {
                WriteOutputLogLine("There are no game resources loaded.");
            }
        }

        /// <summary>
        /// "lobbies" console command executed event
        /// </summary>
        /// <param name="arguments">Arguments</param>
        private static void LobbiesConsoleCommmandExecutedEvent(IReadOnlyList<string> arguments)
        {
            if (server.Lobbies.Count > 0)
            {
                WriteOutputLogLine("Lobbies: ");
                foreach (string lobby_guid in server.Lobbies.Keys)
                {
                    WriteOutputLogLine($"\t{ lobby_guid }");
                }
            }
            else
            {
                WriteOutputLogLine("There are no lobbies.");
            }
        }

        /// <summary>
        /// "users" console command executed event
        /// </summary>
        /// <param name="arguments">Arguments</param>
        private static void UsersConsoleCommmandExecutedEvent(IReadOnlyList<string> arguments)
        {
            bool are_no_user_left = true;
            if (arguments.Count > 0)
            {
                string lobby_guid = arguments[0];
                if (server.Lobbies.ContainsKey(lobby_guid))
                {
                    ILobby lobby = server.Lobbies[lobby_guid];
                    if (lobby.UserCount > 0U)
                    {
                        are_no_user_left = false;
                        WriteOutputLogLine($"\tUsers in lobby \"{ lobby.Name }\" with lobby code \"{ lobby.LobbyCode }\":");
                        foreach (IUser user in lobby.Users.Values)
                        {
                            WriteOutputLogLine($"\t\t\"{ user.Name }\" : \"{ user.GUID }\"");
                        }
                    }
                }
                else
                {
                    WriteErrorLogLine($"\"{ lobby_guid }\" is not a valid lobby GUID.");
                }
            }
            else
            {
                foreach (ILobby lobby in server.Lobbies.Values)
                {
                    if (lobby.UserCount > 0U)
                    {
                        if (are_no_user_left)
                        {
                            WriteOutputLogLine("Users: ");
                            are_no_user_left = false;
                        }
                        WriteOutputLogLine($"\tIn lobby \"{ lobby.Name }\" with lobby code \"{ lobby.LobbyCode }\":");
                        foreach (IUser user in lobby.Users.Values)
                        {
                            WriteOutputLogLine($"\t\t\"{ user.Name }\" : \"{ user.GUID }\"");
                        }
                    }
                }
                bool are_all_in_lobby = true;
                foreach (IUser user in server.Users.Values)
                {
                    if (user.Lobby == null)
                    {
                        if (are_no_user_left)
                        {
                            WriteOutputLogLine("Users: ");
                            are_no_user_left = false;
                        }
                        if (are_all_in_lobby)
                        {
                            WriteOutputLogLine("\tNot in any lobby right now:");
                            are_all_in_lobby = false;
                        }
                        WriteOutputLogLine($"\t\t\"{ user.Name }\" : \"{ user.GUID }\"");
                    }
                }
            }
            if (are_no_user_left)
            {
                WriteOutputLogLine("There are no users.");
            }
        }

        /// <summary>
        /// "kick" console command executed event
        /// </summary>
        /// <param name="arguments">Arguments</param>
        private static void KickConsoleCommmandExecutedEvent(IReadOnlyList<string> arguments)
        {
            bool failed = true;
            string user_guid = arguments[0];
            foreach (ILobby lobby in server.Lobbies.Values)
            {
                if (lobby.Users.ContainsKey(user_guid))
                {
                    if (lobby.Users[user_guid] is IServerUser server_user)
                    {
                        server_user.Disconnect(EDisconnectionReason.Kicked);
                        failed = false;
                    }
                    break;
                }
            }
            if (failed)
            {
                WriteErrorLogLine($"User with GUID \"{ user_guid }\" does not exist or could not been kicked.");
            }
        }

        /// <summary>
        /// "ban" console command executed event
        /// </summary>
        /// <param name="arguments">Arguments</param>
        private static void BanConsoleCommmandExecutedEvent(IReadOnlyList<string> arguments)
        {
            IServerUser banned_server_user = null;
            string user_guid = arguments[0];
            foreach (ILobby lobby in server.Lobbies.Values)
            {
                if (lobby.Users.ContainsKey(user_guid))
                {
                    banned_server_user = lobby.Users[user_guid] as IServerUser;
                    if (banned_server_user != null)
                    {
                        server.Bans.AddPeer(banned_server_user.Peer, "Banned by admin");
                        banned_server_user.Disconnect(EDisconnectionReason.Banned);
                        server.Bans.WriteToFile(bansJSONFilePath);
                        break;
                    }
                }
            }
            if (banned_server_user == null)
            {
                WriteErrorLogLine($"User with GUID \"{ user_guid }\" does not exist or could not been banned.");
            }
            else
            {
                foreach (ILobby lobby in server.Lobbies.Values)
                {
                    foreach (IUser user in lobby.Users.Values)
                    {
                        if (user is IServerUser server_user)
                        {
                            if ((banned_server_user != server_user) && (server.Bans.IsPeerBanned(server_user.Peer, out _)))
                            {
                                server_user.Disconnect(EDisconnectionReason.Banned);
                            }
                        }
                    }
                }
                WriteOutputLogLine($"User \"{ banned_server_user.Name }\" with GUID \"{ banned_server_user.GUID }\" at \"{ banned_server_user.Peer.Secret }\" has been banned.");
            }
        }

        /// <summary>
        /// "bansecret" console command executed event
        /// </summary>
        /// <param name="arguments">Arguments</param>
        private static void BanSecretConsoleCommmandExecutedEvent(IReadOnlyList<string> arguments)
        {
            string secret = arguments[0];
            server.Bans.AddSecret(arguments[0], "Banned by admin");
            server.Bans.WriteToFile(bansJSONFilePath);
            foreach (ILobby lobby in server.Lobbies.Values)
            {
                foreach (IUser user in lobby.Users.Values)
                {
                    if (user is IServerUser server_user)
                    {
                        if (server.Bans.IsPeerBanned(server_user.Peer, out _))
                        {
                            server_user.Disconnect(EDisconnectionReason.Banned);
                        }
                    }
                }
            }
            WriteOutputLogLine($"Secret \"{ secret }\" has been banned.");
        }

        /// <summary>
        /// "unbansecret" console command executed event
        /// </summary>
        /// <param name="arguments">Arguments</param>
        private static void UnbanSecretConsoleCommmandExecutedEvent(IReadOnlyList<string> arguments)
        {
            string secret = arguments[0];
            if (server.Bans.RemoveSecret(arguments[0]))
            {
                server.Bans.WriteToFile(bansJSONFilePath);
                WriteOutputLogLine($"Secret \"{ secret }\" has been unbanned.");
            }
            else
            {
                WriteOutputLogLine($"Secret \"{ secret }\" is not banned.");
            }
        }

        /// <summary>
        /// "reloadbans" console command executed
        /// </summary>
        /// <param name="arguments">Arguments</param>
        private static void ReloadBansConsoleCommmandExecutedEvent(IReadOnlyList<string> arguments)
        {
            server.Bans.Clear();
            server.Bans.AppendFromFile(bansJSONFilePath);
            WriteOutputLogLine($"Bans have been reloaded from \"{ bansJSONFilePath }\".");
        }

        /// <summary>
        /// "closelobby" command executed event
        /// </summary>
        /// <param name="arguments">Arguments</param>
        private static void CloseLobbyConsoleCommmandExecutedEvent(IReadOnlyList<string> arguments)
        {
            string lobby_code = arguments[0];
            if (server.Lobbies.ContainsKey(lobby_code))
            {
                if (server.Lobbies[lobby_code] is IServerLobby server_lobby)
                {
                    server_lobby.Close();
                }
                WriteOutputLogLine($"Lobby with lobby code \"{ lobby_code }\" has been closed.");
            }
            else
            {
                WriteErrorLogLine($"Lobby with lobby code \"{ lobby_code }\" does not exist.");
            }
        }

        /// <summary>
        /// "exit" console command executed event
        /// </summary>
        /// <param name="arguments">Arguments</param>
        private static void ExitConsoleCommmandExecutedEvent(IReadOnlyList<string> arguments)
        {
            WriteOutputLogLine("Exiting process...");
            keepServerRunning = false;
        }

        /// <summary>
        /// Print help topic
        /// </summary>
        /// <param name="commands">Commands</param>
        /// <param name="prefix">Command prefix</param>
        private static void PrintHelpTopic(ICommands commands, string prefix) => PrintHelpTopic(commands, string.Empty, prefix);

        /// <summary>
        /// Print help topic
        /// </summary>
        /// <param name="commands">Commands</param>
        /// <param name="prefix">Command prefix</param>
        /// <param name="commandName">Command name</param>
        private static void PrintHelpTopic(ICommands commands, string prefix, string commandName)
        {
            if (commands == null)
            {
                throw new ArgumentNullException(nameof(commands));
            }
            if (prefix == null)
            {
                throw new ArgumentNullException(nameof(prefix));
            }
            if (commandName == null)
            {
                throw new ArgumentNullException(nameof(commandName));
            }
            Console.WriteLine(commands.GetHelpTopic(commandName, prefix));
        }

        /// <summary>
        /// Write output log line
        /// </summary>
        /// <param name="obj">Object</param>
        private static void WriteOutputLogLine(object obj)
        {
            if (outputLogger != null)
            {
                outputLogger.WriteLine(obj);
            }
            Console.WriteLine((obj == null) ? "null" : obj.ToString());
        }

        /// <summary>
        /// Write error log line
        /// </summary>
        /// <param name="obj"></param>
        private static void WriteErrorLogLine(object obj)
        {
            if (errorLogger != null)
            {
                errorLogger.WriteLine(obj);
            }
            Console.Error.WriteLine((obj == null) ? "null" : obj.ToString());
        }

        /// <summary>
        /// Main entry point
        /// </summary>
        /// <param name="args">Command line arguments</param>
        private static void Main(string[] args)
        {
            try
            {
                commandLineCommands.AddCommand("-help", "Show help topics", "This command shows help topics.", HelpCommandLineCommandExecutedEvent, CommandArgument.Optional("helpTopic"));
                commandLineCommands.AddCommand("-port", "Set port", "This command sets the port the server should be available from.", PortCommandLineCommandExecutedEvent, "port");
                commandLineCommands.AddCommand("-tick-rate", "Set tick rate", "This command sets the tick rate the server should run on.", TickRateCommandLineCommandExecutedEvent, "tickRate");
                commandLineCommands.AddAlias("h", "-help");
                commandLineCommands.AddAlias("p", "-port");
                commandLineCommands.AddAlias("t", "-tick-rate");
                consoleCommands.AddCommand("help", "Show help topic", "This command shows help topics.", HelpConsoleCommmandExecutedEvent, CommandArgument.Optional("helpTopic"));
                consoleCommands.AddCommand("connectors", "List connectors", "This command lists all connectors.", ConnectorsConsoleCommmandExecutedEvent);
                consoleCommands.AddCommand("gameresources", "List game resources", "This command lists all game resources.", GameResourcesConsoleCommmandExecutedEvent);
                consoleCommands.AddCommand("lobbies", "List lobbies", "This command lists all lobbies.", LobbiesConsoleCommmandExecutedEvent);
                consoleCommands.AddCommand("users", "List users", "This command lists all users.", UsersConsoleCommmandExecutedEvent, CommandArgument.Optional("lobbyGUID"));
                consoleCommands.AddCommand("kick", "Kick user", "This command kicks the specified user.", KickConsoleCommmandExecutedEvent, "userGUID");
                consoleCommands.AddCommand("ban", "Ban user", "This command bans the specified user.", BanConsoleCommmandExecutedEvent, "userGUID");
                consoleCommands.AddCommand("bansecret", "Ban IP address", "This command bans the specified peer secret.", BanSecretConsoleCommmandExecutedEvent, "secret");
                consoleCommands.AddCommand("unbansecret", "Unban IP address", "This command unbans the specified peer secret.", UnbanSecretConsoleCommmandExecutedEvent, "secret");
                consoleCommands.AddCommand("reloadbans", $"Reload bans from \"{ bansJSONFilePath }\"", $"This command reloads the bans from \"{ bansJSONFilePath }\".", ReloadBansConsoleCommmandExecutedEvent);
                consoleCommands.AddCommand("closelobby", "Close lobby", "This command closes the specified lobby.", CloseLobbyConsoleCommmandExecutedEvent, "lobbyCode");
                consoleCommands.AddCommand("exit", "Exit process", "This command exists the current process.", ExitConsoleCommmandExecutedEvent);
                consoleCommands.AddAlias("commands", "help");
                consoleCommands.AddAlias("cmds", "commands");
                consoleCommands.AddAlias("cmd", "cmds");
                consoleCommands.AddAlias("h", "help");
                consoleCommands.AddAlias("?", "help");
                consoleCommands.AddAlias("c", "connectors");
                consoleCommands.AddAlias("resources", "gameresources");
                consoleCommands.AddAlias("gr", "gameresources");
                consoleCommands.AddAlias("r", "resources");
                consoleCommands.AddAlias("l", "lobbies");
                consoleCommands.AddAlias("u", "users");
                consoleCommands.AddAlias("k", "kick");
                consoleCommands.AddAlias("b", "ban");
                consoleCommands.AddAlias("bsecret", "bansecret");
                consoleCommands.AddAlias("ubsecret", "unbansecret");
                consoleCommands.AddAlias("cl", "closelobby");
                consoleCommands.AddAlias("quit", "exit");
                consoleCommands.AddAlias("q", "quit");
                if (File.Exists(serverJSONFilePath))
                {
                    try
                    {
                        using FileStream file_stream = File.OpenRead(serverJSONFilePath);
                        using StreamReader stream_reader = new StreamReader(file_stream);
                        serverConfigurationData = JsonConvert.DeserializeObject<ServerConfigurationData>(stream_reader.ReadToEnd());
                    }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine(e);
                    }
                }
                else
                {
                    serverConfigurationData = new ServerConfigurationData();
                    try
                    {
                        using FileStream file_stream = File.Open(serverJSONFilePath, FileMode.Create, FileAccess.Write);
                        using StreamWriter stream_writer = new StreamWriter(file_stream);
                        stream_writer.Write(JsonConvert.SerializeObject(serverConfigurationData));
                    }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine(e);
                    }
                }
                if (serverConfigurationData == null)
                {
                    serverConfigurationData = new ServerConfigurationData();
                }
                if (serverConfigurationData.NetworkPort == 0)
                {
                    Console.Error.WriteLine($"Port is set to \"0\". Port will be set to default port \"{ ServerConfigurationData.defaultNetworkPort }\".");
                    serverConfigurationData.NetworkPort = ServerConfigurationData.defaultNetworkPort;
                }
                if ((serverConfigurationData.TickRate <= 0) || (serverConfigurationData.TickRate > 1000))
                {
                    Console.Error.WriteLine($"Tick rate is set to \"{ serverConfigurationData.TickRate }\". Tick rate will be set to default tick rate \"{ ServerConfigurationData.defaultTickRate }\".");
                    serverConfigurationData.TickRate = ServerConfigurationData.defaultTickRate;
                }
                if (string.IsNullOrWhiteSpace(serverConfigurationData.OutputLogPath))
                {
                    Console.Error.WriteLine($"Output log path is not defined. Output log path will be set to default output log path \"{ ServerConfigurationData.defaultOutputLogPath }\".");
                    serverConfigurationData.OutputLogPath = ServerConfigurationData.defaultOutputLogPath;
                }
                if (string.IsNullOrWhiteSpace(serverConfigurationData.ErrorLogPath))
                {
                    Console.Error.WriteLine($"Error log path is not defined. Error log path will be set to default error log path \"{ ServerConfigurationData.defaultErrorLogPath }\".");
                    serverConfigurationData.ErrorLogPath = ServerConfigurationData.defaultErrorLogPath;
                }
                StringBuilder command_line_arguments_string_builder = new StringBuilder();
                bool first = true;
                foreach (string arg in args)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        command_line_arguments_string_builder.Append(' ');
                    }
                    command_line_arguments_string_builder.Append(arg);
                }
                if (commandLineCommands.ParseCommands(command_line_arguments_string_builder.ToString(), "-"))
                {
                    outputLogger = Logger.Open(serverConfigurationData.OutputLogPath);
                    if (serverConfigurationData.OutputLogPath == serverConfigurationData.ErrorLogPath)
                    {
                        errorLogger = outputLogger;
                    }
                    else
                    {
                        errorLogger = Logger.Open(serverConfigurationData.ErrorLogPath);
                    }
                    if (keepServerRunning)
                    {
                        using (server = Servers.Create(serverConfigurationData.NetworkPort, serverConfigurationData.TimeoutTime))
                        {
                            if (server == null)
                            {
                                WriteErrorLogLine($"Failed to create server at port { serverConfigurationData.NetworkPort }.");
                            }
                            else
                            {
                                TimeSpan tick_time_span = TimeSpan.FromMilliseconds(1000 / serverConfigurationData.TickRate);
                                Stopwatch stopwatch = new Stopwatch();
                                uint lag_count = 0U;
                                ConsoleKeyInfo console_key_information;
                                StringBuilder input_string_builder = new StringBuilder();
                                foreach (IConnector connector in server.Connectors)
                                {
                                    connector.OnPeerConnectionAttempted += PeerConnectionAttemptedEvent;
                                    connector.OnPeerConnected += PeerConnectedEvent;
                                    connector.OnPeerDisconnected += PeerDisconnectedEvent;
                                }
                                server.Bans.AppendFromFile(bansJSONFilePath);
                                try
                                {
                                    foreach (string game_resources_file_path in Directory.GetFiles("./GameResources/", "*.dll"))
                                    {
                                        WriteOutputLogLine($"Loading game resources file  \"{ game_resources_file_path }\"...");
                                        try
                                        {
                                            Assembly assembly = Assembly.LoadFrom(game_resources_file_path);
                                            if (assembly == null)
                                            {
                                                WriteErrorLogLine($"Failed to load game resource assembly from \"{ game_resources_file_path }\"");
                                            }
                                            else
                                            {
                                                foreach (Type type in assembly.GetTypes())
                                                {
                                                    if (type.IsClass && !type.IsAbstract && typeof(IGameResource).IsAssignableFrom(type))
                                                    {
                                                        bool has_default_constructor = true;
                                                        foreach (ConstructorInfo constructor in type.GetConstructors(BindingFlags.Public))
                                                        {
                                                            has_default_constructor = false;
                                                            if (constructor.GetParameters().Length <= 0)
                                                            {
                                                                has_default_constructor = true;
                                                                break;
                                                            }
                                                        }
                                                        if (has_default_constructor)
                                                        {
                                                            if (server.AddGameResource(type))
                                                            {
                                                                WriteOutputLogLine($"Loaded game resource \"{ type.FullName }\" from \"{ game_resources_file_path }\".");
                                                            }
                                                            else
                                                            {
                                                                WriteErrorLogLine($"Failed to load game resource \"{ type.FullName }\" from \"{ game_resources_file_path }\".");
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            WriteErrorLogLine(e);
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    WriteErrorLogLine(e);
                                }
                                WriteOutputLogLine($"Starting server at port { serverConfigurationData.NetworkPort }...");
                                while (keepServerRunning)
                                {
                                    stopwatch.Restart();
                                    server.ProcessEvents();
                                    while (Console.KeyAvailable)
                                    {
                                        console_key_information = Console.ReadKey();
                                        if (console_key_information.KeyChar != '\0')
                                        {
                                            if (console_key_information.Key == ConsoleKey.Enter)
                                            {
                                                string input = input_string_builder.ToString();
                                                WriteOutputLogLine(input);
                                                if (!consoleCommands.ParseCommand(input))
                                                {
                                                    WriteErrorLogLine("Failed to execute command.");
                                                }
                                                input_string_builder.Clear();
                                            }
                                            else
                                            {
                                                input_string_builder.Append(console_key_information.KeyChar);
                                            }
                                        }
                                    }
                                    stopwatch.Stop();
                                    if (tick_time_span > stopwatch.Elapsed)
                                    {
                                        lag_count = (lag_count > 0U) ? (lag_count - 1U) : 0U;
                                        Thread.Sleep(tick_time_span - stopwatch.Elapsed);
                                    }
                                    else
                                    {
                                        ++lag_count;
                                        if (lag_count >= serverConfigurationData.TickRate)
                                        {
                                            lag_count = 0U;
                                            WriteErrorLogLine($"Server can't keep up. Try lowering the tick rate in \"{ serverJSONFilePath }\" or run the server with a lower tick rate.");
                                        }
                                    }
                                }
                                server.Bans.WriteToFile(bansJSONFilePath);
                                WriteOutputLogLine("Server has shut down.");
                            }
                        }
                    }
                }
                else
                {
                    PrintHelpTopic(commandLineCommands, "-");
                }
                command_line_arguments_string_builder.Clear();
                if (outputLogger != null)
                {
                    outputLogger.Dispose();
                    if (outputLogger == errorLogger)
                    {
                        outputLogger = null;
                        errorLogger = null;
                    }
                }
                if (errorLogger != null)
                {
                    errorLogger.Dispose();
                    errorLogger = null;
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
        }

        /// <summary>
        /// Peer connection attempted event
        /// </summary>
        /// <param name="peer">Peer</param>
        private static void PeerConnectionAttemptedEvent(IPeer peer) => WriteOutputLogLine($"Connection attempt by peer with GUID \"{ peer.GUID }\" and secret \"{ peer.Secret }\"...");

        /// <summary>
        /// Peer connected event
        /// </summary>
        /// <param name="peer">Peer</param>
        private static void PeerConnectedEvent(IPeer peer) => WriteOutputLogLine($"Peer with GUID \"{ peer.GUID }\" and secret \"{ peer.Secret }\" has connected successfully.");

        /// <summary>
        /// Peer disconnected event
        /// </summary>
        /// <param name="peer">Peer</param>
        private static void PeerDisconnectedEvent(IPeer peer) => WriteOutputLogLine($"Peer with GUID \"{ peer.GUID }\" and secret \"{ peer.Secret }\" has disconnected.");
    }
}
