﻿using System;
using System.IO;
using System.Text;
using WotDossier.Common;
using WotDossier.Dal;
using WotDossier.Domain;

namespace WotDossier.Applications
{
    public static class SettingsReader
    {
        private static readonly object _syncObject = new object();

        private const string PATH_WEB_BIN = @".\..";

        private static readonly string _filePath = WotDossierSettings.SettingsPath;

        /// <summary>
        /// Reads this instance.
        /// </summary>
        /// <returns></returns>
        public static AppSettings Get()
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

        private static string GetFilePath()
        {
            return Environment.CurrentDirectory + _filePath;
        }

        /// <summary>
        /// Saves the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public static void Save(AppSettings settings)
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