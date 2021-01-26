using System;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// ElectrodZ multiplayer server namespace
/// </summary>
namespace ElectrodZMultiplayer.Server
{
    /// <summary>
    /// AN abstract class that describes a game resource
    /// </summary>
    public abstract class AGameResource : IGameResource
    {
        /// <summary>
        /// Available game mode types
        /// </summary>
        private readonly Dictionary<string, Type> availableGameModeTypes = new Dictionary<string, Type>();

        /// <summary>
        /// Server
        /// </summary>
        public IServerSynchronizer Server { get; private set; }

        /// <summary>
        /// Creates a new game user factory
        /// </summary>
        /// <returns>Game user factory</returns>
        public abstract IGameUserFactory CreateNewGameUserFactory();

        /// <summary>
        /// Creates a new game entity factory
        /// </summary>
        /// <returns>Game entity factory</returns>
        public abstract IGameEntityFactory CreateNewGameEntityFactory();

        /// <summary>
        /// Available game mode types
        /// </summary>
        public IReadOnlyDictionary<string, Type> AvailableGameModeTypes => availableGameModeTypes;

        /// <summary>
        /// Constructs a game resource
        /// </summary>
        public AGameResource()
        {
            foreach (Type type in GetType().Assembly.GetTypes())
            {
                if (type.IsClass && !type.IsAbstract)
                {
                    if (typeof(IGameMode).IsAssignableFrom(type))
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
                            string key = type.FullName;
                            if (availableGameModeTypes.ContainsKey(key))
                            {
                                Console.Error.WriteLine($"Game mode of type \"{ key }\" has been already registered.");
                            }
                            else
                            {
                                availableGameModeTypes.Add(key, type);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Game resource has been initialized
        /// </summary>
        /// <param name="server">Server</param>
        public virtual void OnInitialized(IServerSynchronizer server) => Server = server ?? throw new ArgumentNullException(nameof(server));

        /// <summary>
        /// Game resource has been closed
        /// </summary>
        public virtual void OnClosed() => availableGameModeTypes.Clear();
    }
}
