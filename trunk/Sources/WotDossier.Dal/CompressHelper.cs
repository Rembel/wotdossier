﻿using System.IO;
using System.IO.Compression;
using System.Text;
using Newtonsoft.Json;

namespace WotDossier.Dal
{
    /// <summary>
    /// Utility class for compress\decompress data
    /// </summary>
    public class CompressHelper
    {
        /// <summary>
        /// Compresses the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static byte[] Compress(string value)
        {
            var memoryStream = new MemoryStream();
            var zip = new GZipStream(memoryStream, CompressionMode.Compress);
            using (var writer = new StreamWriter(zip, Encoding.UTF8))
            {
                writer.Write(value);
            }
            return memoryStream.ToArray();
        }

        /// <summary>
        /// Decompresses the specified byte array.
        /// </summary>
        /// <param name="byteArray">The byte array.</param>
        /// <returns></returns>
        public static string Decompress(byte[] byteArray)
        {
            var zip = new GZipStream(new MemoryStream(byteArray), CompressionMode.Decompress);
            using (var sr = new StreamReader(zip, Encoding.UTF8))
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        /// Decompresses the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="byteArray">The byte array.</param>
        /// <returns></returns>
        public static T DecompressObject<T>(byte[] byteArray)
        {
            string json = Decompress(byteArray);
            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Compresses the object.
        /// </summary>
        /// <param name="tank">The tank.</param>
        /// <returns></returns>
        public static byte[] CompressObject(object tank)
        {
            return Compress(JsonConvert.SerializeObject(tank));
        }
    }


}