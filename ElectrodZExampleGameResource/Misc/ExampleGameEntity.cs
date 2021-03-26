using ElectrodZMultiplayer.Server;
using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// ElectrodZ example game resource namespace
/// </summary>
namespace ElectrodZExampleGameResource
{
    /// <summary>
    /// A class that describes an example game entity
    /// </summary>
    internal class ExampleGameEntity : AGameEntity
    {
        /// <summary>
        /// Constructs an example game entity
        /// </summary>
        /// <param name="serverEntity">Server entity</param>
        public ExampleGameEntity(IServerEntity serverEntity) : base(serverEntity)
        {
            // ...
        }
    }
}
