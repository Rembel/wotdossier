using System;
using System.IO;
using System.Reflection;

namespace WotDossier.Common
{
    public static class AssemblyExtensions
    {
        public static byte[] GetEmbeddedResource(string resourceName, Assembly assembly)
        {
            using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                    return null;

                int length = Convert.ToInt32(resourceStream.Length); // get strem length
                byte[] byteArr = new byte[length]; // create a byte array
                resourceStream.Read(byteArr, 0, length);

                return byteArr;
            }
        }
    }
}