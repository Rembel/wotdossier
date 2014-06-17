using System;
using System.IO;
using System.Security.Cryptography;

namespace WotDossier.Common
{
    public static class FileExtensions
    {
        public static string MD5(this FileInfo file)
        {
            if (!File.Exists(file.FullName))
            {
                return string.Empty;
            }
            using (FileStream stream = File.OpenRead(file.FullName))
            {
                byte[] buffer = new MD5CryptoServiceProvider().ComputeHash(stream);
                stream.Close();
                return BitConverter.ToString(buffer).Replace("-", "").ToLower();
            }
        }

        public static byte[] Read(this Stream stream, int count)
        {
            byte[] buffer = new byte[count];
            stream.Read(buffer, 0, count);
            return buffer;
        }
    }
}
