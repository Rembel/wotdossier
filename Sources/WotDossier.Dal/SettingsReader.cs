using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using WotDossier.Common;
using WotDossier.Domain;

namespace WotDossier.Dal
{
    public static class SettingsReader
    {
        private static readonly object _syncObject = new object();

        private static readonly string _filePath = AppConfigSettings.SettingsPath;

        /// <summary>
        /// Reads this instance.
        /// </summary>
        /// <returns></returns>
        public static AppSettings Get()
        {
            var filePath = GetFilePath();

            if (File.Exists(filePath))
            {
                FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using (StreamReader reader = new StreamReader(stream))
                {
                    var readToEnd = reader.ReadToEnd();
                    return Deserialize<AppSettings>(readToEnd);
                }
            }
            
            //create settings file if not exists
            AppSettings settingsDto = new AppSettings();
            settingsDto.DossierCachePath = Folder.GetDefaultDossierCacheFolder();
            Save(settingsDto);
            return settingsDto;
        }

        public static T Get<T>() where T : class, new()
        {
            var filePath = GetFilePath();

            if (File.Exists(filePath))
            {
                FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using (StreamReader reader = new StreamReader(stream))
                {
                    var readToEnd = reader.ReadToEnd();
                    return Deserialize<T>(readToEnd);
                }
            }

            //create settings file if not exists
            T settingsDto = new T();
            Save(settingsDto);
            return settingsDto;
        }

        /// <summary>
        /// Gets the file path.
        /// </summary>
        /// <returns></returns>
        public static string GetFilePath()
        {
            return Environment.CurrentDirectory + _filePath;
        }

        /// <summary>
        /// Saves the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public static void Save(object settings)
        {
            var filePath = GetFilePath();

            lock (_syncObject)
            {
                using (FileStream stream = File.Open(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.Read))
                {
                    StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
                    string xml = Serialize(settings);
                    writer.Write(xml);
                    writer.Flush();
                }
            }
        }

        private static string Serialize(object settings)
        {
            //return XmlSerializer.StoreObjectInXml(settings);
            return JsonConvert.SerializeObject(settings, Formatting.Indented);
        }

        private static T Deserialize<T>(string serializedObject) where T : class
        {
            //return XmlSerializer.LoadObjectFromXml<T>(serializedObject);
            return JsonConvert.DeserializeObject<T>(serializedObject);
        }
    }
}
