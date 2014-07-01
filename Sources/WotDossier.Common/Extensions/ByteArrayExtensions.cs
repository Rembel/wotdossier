using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WotDossier.Common.Extensions
{
    public static class ByteArrayExtensions
    {
        public static void Dump(this byte[] array, string path)
        {
            using (FileStream fileStream = File.Open(path, FileMode.Create))
            {
                fileStream.Write(array, 0, array.Length);
                fileStream.Flush();
            }
        }

        public static void Dump(this string array, string path)
        {
            using (FileStream fileStream = File.Open(path, FileMode.Create))
            {
                TextWriter writer = new StreamWriter(fileStream);
                writer.Write(array);
                writer.Flush();
            }
        }

        public static ulong ConvertLittleEndian(this byte[] array)
        {
            int pos = 0;
            ulong result = 0;
            foreach (byte by in array)
            {
                result |= (ulong)(by << pos);
                pos += 8;
            }
            return result;
        }

        public static string GetUtf8String(this byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public static string GetAsciiString(this byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes);
        }

        public static byte[] GetBytes(this string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static List<int> Unpack(this byte[] array, string format)
        {
            List<int> result = new List<int>();
            using (var stream = new MemoryStream(array))
            {
                for (int i = 0; i < format.Length; i++)
                {
                    switch (format[i])
                    {
                        case 'B':
                            var bytes1 = stream.Read(1);
                            result.Add((int)bytes1.ConvertLittleEndian());
                            break;
                        case 'H':
                            var bytes = stream.Read(2);
                            result.Add((int)bytes.ConvertLittleEndian());
                            break;
                        case 'Q':
                            var read = stream.Read(4);
                            result.Add((int)read.ConvertLittleEndian());
                            break;
                    }
                }
            }
            return result;
        }

        public static byte[] Xor(byte[] result, byte[] matchValue)
        {
            if (result.Length == 0)
            {
                return matchValue;
            }

            byte[] newResult = new byte[matchValue.Length > result.Length ? matchValue.Length : result.Length];

            for (int i = 1; i < newResult.Length + 1; i++)
            {
                //Use XOR on the LSBs until we run out
                if (i > result.Length)
                {
                    newResult[newResult.Length - i] = matchValue[matchValue.Length - i];
                }
                else if (i > matchValue.Length)
                {
                    newResult[newResult.Length - i] = result[result.Length - i];
                }
                else
                {
                    newResult[newResult.Length - i] =
                        (byte)(matchValue[matchValue.Length - i] ^ result[result.Length - i]);
                }
            }
            return newResult;
        }
    }
}