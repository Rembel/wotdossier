using System;
using System.IO;
using System.Text;
using WotDossier.Common;
using WotDossier.Domain;

namespace WotDossier.Applications
{
    public class SettingsReader
    {
        private static readonly object _syncObject = new object();

        private const string PATH_WEB_BIN = @".\..";

        private readonly string _filePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public SettingsReader(string filePath)
        {
            _filePath = filePath;
        }

        /// <summary>
        /// Reads this instance.
        /// </summary>
        /// <returns></returns>
        public AppSettings Read()
        {
            var filePath = GetFilePath();

            if (File.Exists(filePath))
            {
                using (StreamReader stream = File.OpenText(filePath))
                {
                    var readToEnd = stream.ReadToEnd();
                    return XmlSerializer.LoadObjectFromXml<AppSettings>(readToEnd);
                }
            }
            AppSettings settingsDto = new AppSettings();
            Save(settingsDto);
            return settingsDto;
        }

        private string GetFilePath()
        {
            return Environment.CurrentDirectory + _filePath;
        }

        /// <summary>
        /// Saves the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public void Save(AppSettings settings)
        {
            var filePath = GetFilePath();

            lock (_syncObject)
            {
                using (FileStream stream = File.Open(filePath, FileMode.Create))
                {
                    StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
                    string xml = XmlSerializer.StoreObjectInXml(settings);
                    writer.Write(xml);
                    writer.Flush();
                }
            }
        }
    }
}
