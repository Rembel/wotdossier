using System;
using System.Collections.Generic;
using System.IO;
using System.Resources;
using System.Reflection;
using System.Globalization;
using System.Linq;

namespace WotDossier.Framework.Localization
{
    /// <summary>
    /// 
    /// </summary>
    public class ResxTranslationProvider : ITranslationProvider
    {
        #region Private Members

        private readonly ResourceManager _resourceManager;

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="ResxTranslationProvider"/> class.
        /// </summary>
        /// <param name="baseName">Name of the base.</param>
        /// <param name="assembly">The assembly.</param>
        public ResxTranslationProvider(string baseName, Assembly assembly)
        {
            _resourceManager = new ResourceManager(baseName, assembly);
        }

        #endregion

        #region ITranslationProvider Members

        /// <summary>
        /// See <see cref="ITranslationProvider.Translate" />
        /// </summary>
        public object Translate(string key)
        {
            return _resourceManager.GetString(key);
        }

        #endregion

        #region ITranslationProvider Members

        /// <summary>
        /// See <see cref="ITranslationProvider.AvailableLanguages" />
        /// </summary>
        public IEnumerable<CultureInfo> Languages
        {
            get
            {
                string[] directories = Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory, "*-*");
                List<string> list = directories.ToList();
                list.Add("en-US");

                foreach (string directory in list)
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(directory);
                    yield return CultureInfo.GetCultureInfo(directoryInfo.Name);
                }
            }
        }

        #endregion
    }
}
