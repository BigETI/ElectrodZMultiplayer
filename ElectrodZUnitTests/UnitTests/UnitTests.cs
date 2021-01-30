using ElectrodZExampleGameResource.GameModes;
using ElectrodZExampleGameResource.Resources;
using ElectrodZMultiplayer;
using ElectrodZMultiplayer.Client;
using ElectrodZMultiplayer.Server;
using ElectrodZUnitTests.Data;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

/// <summary>
/// ElectrodZ unit tests namespace
/// </summary>
namespace ElectrodZUnitTests
{
    /// <summary>
    /// A class that describes all unit tests
    /// </summary>
    public class UnitTests
    {
        /// <summary>
        /// Unit tests configuration path
        /// </summary>
        private static readonly string unitTestsConfigurationPath = "./unit-tests.json";

        /// <summary>
        /// Tests connections between a server and clients
        /// </summary>
        /// <param name="port">Network port</param>
        /// <param name="onCreateConnection">On create connection</param>
        public void TestConnections(ushort port, CreateClientConnectionDelegate onCreateConnection)
        {
            if (port == 0)
            {
                throw new ArgumentException("Port can't be zero.", nameof(port));
            }
            if (onCreateConnection == null)
            {
                throw new ArgumentNullException(nameof(onCreateConnection));
            }
            UnitTestsConfigurationData unit_tests_configuration = null;
            if (File.Exists(unitTestsConfigurationPath))
            {
                using FileStream file_stream = File.OpenRead(unitTestsConfigurationPath);
                using StreamReader file_stream_reader = new StreamReader(file_stream);
                unit_tests_configuration = JsonConvert.DeserializeObject<UnitTestsConfigurationData>(file_stream_reader.ReadToEnd());
            }
            unit_tests_configuration ??= new UnitTestsConfigurationData();
            Assert.IsTrue(unit_tests_configuration.IsValid);
            bool is_running = true;
            uint perform_ticks = unit_tests_configuration.PerformTicks;
            using IServerSynchronizer server = Servers.Create(port, Defaults.timeoutTime);
            Assert.IsNotNull(server);
            server.AddGameResource<ExampleGameResource>();
            server.OnPeerConnectionAttempted += (peer) => Console.WriteLine($"[SERVER] Peer GUID \"{ peer.GUID }\" with secret \"{ peer.Secret }\" attempted to connect.");
            server.OnPeerConnected += (peer) => Console.WriteLine($"[SERVER] Peer GUID \"{ peer.GUID }\" with secret \"{ peer.Secret }\" is connect.");
            server.OnPeerDisconnected += (peer) => Console.WriteLine($"[SERVER] Peer GUID \"{ peer.GUID }\" with secret \"{ peer.Secret }\" has been disconnect.");
            server.OnUnknownMessageReceived += (message, json) =>
            {
                Console.Error.WriteLine($"[SERVER] Message type: \"{ message.MessageType }\"");
                Console.Error.WriteLine("[SERVER] JSON:");
                Console.Error.WriteLine();
                Console.Error.WriteLine(json);
                Assert.Fail("Server has received an unknown message.", message, json);
            };
            server.OnPeerMessageReceived += (peer, message) => Console.WriteLine($"[SERVER] Peer GUID \"{ peer.GUID }\" sent a message of length \"{ message.Length }\".");
            IClientSynchronizer[] clients = new IClientSynchronizer[Defaults.maximalUserCount + 1U];
            IUser[] users = new IUser[clients.Length];
            ILobby[] lobbies = new ILobby[clients.Length];
            for (int index = 0; index < clients.Length; index++)
            {
                int current_index = index;
                IClientSynchronizer client = onCreateConnection(server);
                Assert.IsNotNull(client);
                clients[index] = client;
                client.OnPeerConnectionAttempted += (peer) => Console.WriteLine($"[CLIENT] Peer GUID \"{ peer.GUID }\" with secret \"{ peer.Secret }\" attempted to connect.");
                client.OnPeerConnected += (peer) => Console.WriteLine($"[CLIENT] Peer GUID \"{ peer.GUID }\" with secret \"{ peer.Secret }\" is connect.");
                client.OnPeerDisconnected += (peer) => Console.WriteLine($"[CLIENT] Peer GUID \"{ peer.GUID }\" with secret \"{ peer.Secret }\" has been disconnect.");
                client.OnUnknownMessageReceived += (message, json) =>
                {
                    Console.Error.WriteLine($"Message type: \"{ message.MessageType }\"");
                    Console.Error.WriteLine("JSON:");
                    Console.Error.WriteLine();
                    Console.Error.WriteLine(json);
                    Assert.Fail($"Client index { current_index } has received an unknown message.", message, json, current_index);
                };
                client.OnPeerMessageReceived += (peer, message) => Console.WriteLine($"[CLIENT] Peer GUID \"{ peer.GUID }\" sent a message of length \"{ message.Length }\".");
                client.OnLobbyJoinAcknowledged += (lobby) => lobbies[current_index] = lobby;
                client.OnAuthentificationAcknowledged += (user) =>
                {
                    Assert.IsNotNull(user);
                    users[current_index] = user;
                    if (current_index == 0)
                    {
                        client.OnLobbyJoinAcknowledged += (lobby) =>
                        {
                            for (int client_index = 1; client_index < clients.Length; client_index++)
                            {
                                clients[client_index].JoinLobby(lobby.LobbyCode, $"Client_{ client_index }");
                            }
                        };
                        client.CreateAndJoinLobby($"Client_{ current_index }", "Test lobby", typeof(ExampleGameMode).FullName);
                    }
                };
                client.OnErrorMessageReceived += (errorType, message) => Console.Error.WriteLine($"Error at client index { current_index }: [{ errorType }] { message }");
            };
            TimeSpan tick_time_span = TimeSpan.FromSeconds(1.0 / unit_tests_configuration.TickRate);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (is_running && (perform_ticks > 0U))
            {
                stopwatch.Restart();
                server.ProcessEvents();
                foreach (IClientSynchronizer client in clients)
                {
                    client.ProcessEvents();
                }
                stopwatch.Stop();
                TimeSpan elapsed_time = stopwatch.Elapsed;
                if (elapsed_time > tick_time_span)
                {
                    Console.Error.WriteLine($"Can't keep up server and clients. Please lower the tick rate. Current tick rate: { unit_tests_configuration.TickRate }");
                    is_running = false;
                }
                else if (elapsed_time < tick_time_span)
                {
                    Thread.Sleep(tick_time_span - elapsed_time);
                }
                --perform_ticks;
            }
            server.Dispose();
            foreach (IClientSynchronizer client in clients)
            {
                client.Dispose();
            }
            Assert.AreEqual(0U, perform_ticks);
            uint no_lobby_client_count = 0U;
            for (int index = 0; index < clients.Length; index++)
            {
                Assert.IsNotNull(users[index], $"User at client index { index } was not authentificated.", index);
                if (lobbies[index] == null)
                {
                    ++no_lobby_client_count;
                }
            }
            if (no_lobby_client_count != 1U)
            {
                if (no_lobby_client_count == 0U)
                {
                    Assert.Fail("Every client is in a lobby, which is wrong.");
                }
                else
                {
                    Assert.Fail($"{ no_lobby_client_count } clients are not in a lobby.");
                }
            }
            Assert.Pass();
        }

        /// <summary>
        /// Tests connections between a server and local clients
        /// </summary>
        [Test]
        public void TestLocalConnections() => TestConnections(Defaults.networkPort, (server) => server.TryGetConnectorOfType(out ILocalConnector local_connector) ? Clients.ConnectToLocalInstance(local_connector, string.Empty) : null);

        /// <summary>
        /// Tests connections between a server and network clients
        /// </summary>
        [Test]
        public void TestNetworkConnections() => TestConnections((ushort)(Defaults.networkPort + 1), (_) => Clients.ConnectToNetwork("127.0.0.1", (ushort)(Defaults.networkPort + 1), string.Empty, Defaults.timeoutTime));
    }
}
