using System;
using System.IO;
using System.IO.Compression;

/// <summary>
/// ElectrodZ multiplayer namespace
/// </summary>
namespace ElectrodZMultiplayer
{
    /// <summary>
    /// A class used for compressing or decompressing data
    /// </summary>
    internal static class Compression
    {
        /// <summary>
        /// Decompresses the specified data
        /// </summary>
        /// <param name="data">Data</param>
        /// <returns>Decompressed data</returns>
        public static byte[] Compress(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            return Compress(data, 0U, (uint)data.Length);
        }

        /// <summary>
        /// Decompresses the specified data
        /// </summary>
        /// <param name="data">Data</param>
        /// <param name="index">Starting index</param>
        /// <param name="length">Data length</param>
        /// <returns>Decompressed data</returns>
        public static byte[] Compress(byte[] data, uint index, uint length)
        {
            byte[] ret = null;
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            if (index >= data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Data index is out of rage.");
            }
            if ((index + length) > data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Data index plus length is larger that data length.");
            }
            using (MemoryStream input_memory_stream = new MemoryStream(data, (int)index, (int)length))
            {
                using (MemoryStream output_memory_stream = new MemoryStream())
                {
                    using (GZipStream gzip_stream = new GZipStream(output_memory_stream, CompressionLevel.Optimal, true))
                    {
                        input_memory_stream.CopyTo(gzip_stream);
                    }
                    output_memory_stream.Seek(0L, SeekOrigin.Begin);
                    ret = output_memory_stream.ToArray();
                }
            }
            return ret ?? Array.Empty<byte>();
        }

        /// <summary>
        /// Decompresses the specified data
        /// </summary>
        /// <param name="data">Data</param>
        /// <returns>Decompressed data</returns>
        public static byte[] Decompress(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            return Decompress(data, 0U, (uint)data.Length);
        }

        /// <summary>
        /// Decompresses the specified data
        /// </summary>
        /// <param name="data">Data</param>
        /// <param name="index">Starting index</param>
        /// <param name="length">Data length</param>
        /// <returns>Decompressed data</returns>
        public static byte[] Decompress(byte[] data, uint index, uint length)
        {
            byte[] ret = null;
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            if (index >= data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Data index is out of rage.");
            }
            if ((index + length) > data.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Data index plus length is larger that data length.");
            }
            using (MemoryStream input_memory_stream = new MemoryStream(data, (int)index, (int)length))
            {
                using (MemoryStream output_memory_stream = new MemoryStream())
                {
                    using (GZipStream gzip_stream = new GZipStream(input_memory_stream, CompressionMode.Decompress, true))
                    {
                        gzip_stream.CopyTo(output_memory_stream);
                    }
                    output_memory_stream.Seek(0L, SeekOrigin.Begin);
                    ret = output_memory_stream.ToArray();
                }
            }
            return ret ?? Array.Empty<byte>();
        }
    }
}
