using System;
using System.IO;
using System.Text;

/// <summary>
/// ElectrodZ server namespace
/// </summary>
namespace ElectrodZServer
{
    /// <summary>
    /// Logger class
    /// </summary>
    internal class Logger : ILogger
    {
        /// <summary>
        /// Stream writer
        /// </summary>
        private readonly StreamWriter streamWriter;

        /// <summary>
        /// Open log file
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <returns>Logger if successful, otherwise "null"</returns>
        public static Logger Open(string filePath)
        {
            Logger ret = null;
            try
            {
                FileStream file_stream = File.OpenWrite(filePath);
                if (file_stream != null)
                {
                    ret = new Logger(new StreamWriter(file_stream, Encoding.UTF8));
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
            }
            return ret;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="streamWriter">Stream writer</param>
        private Logger(StreamWriter streamWriter) => this.streamWriter = streamWriter ?? throw new ArgumentNullException(nameof(streamWriter));

        /// <summary>
        /// Write
        /// </summary>
        /// <param name="obj">Object</param>
        public void Write(object obj) => streamWriter.Write((obj == null) ? "null" : obj.ToString());

        /// <summary>
        /// Write line
        /// </summary>
        /// <param name="obj">Object</param>
        public void WriteLine(object obj) => streamWriter.WriteLine((obj == null) ? "null" : obj.ToString());

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose() => streamWriter.Dispose();
    }
}
