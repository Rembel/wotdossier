using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace WotDossier.Common
{
    /// <summary>
    /// Helper class
    /// </summary>
    public static class AssemblyExtensions
    {
        public static byte[] GetEmbeddedResource(this Assembly assembly, string resourceName)
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

        public static string GetTextEmbeddedResource(this Assembly assembly, string resourceName)
        {
            using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                    return null;

                var streamReader = new StreamReader(resourceStream);
                return streamReader.ReadToEnd();
            }
        }

        /// <summary>
        /// Use as first line in ad hoc tests (needed by XNA specifically)
        /// </summary>
        public static void SetEntryAssembly()
        {
            Assembly.GetCallingAssembly().SetEntryAssembly();
        }

        /// <summary>
        /// Allows setting the Entry Assembly when needed. 
        /// Use AssemblyUtilities.SetEntryAssembly() as first line in XNA ad hoc tests
        /// </summary>
        /// <param name="assembly">Assembly to set as entry assembly</param>
        public static void SetEntryAssembly(this Assembly assembly)
        {
            AppDomainManager manager = new AppDomainManager();
            FieldInfo entryAssemblyfield = manager.GetType().GetField("m_entryAssembly", BindingFlags.Instance | BindingFlags.NonPublic);
            entryAssemblyfield.SetValue(manager, assembly);

            AppDomain domain = AppDomain.CurrentDomain;
            FieldInfo domainManagerField = domain.GetType().GetField("_domainManager", BindingFlags.Instance | BindingFlags.NonPublic);
            domainManagerField.SetValue(domain, manager);
        }

        public static IEnumerable<string> GetResourcesByMask(this Assembly assembly, string extension)
        {
            return assembly.GetManifestResourceNames().Where(x => x.EndsWith(extension));
        }
    }
}