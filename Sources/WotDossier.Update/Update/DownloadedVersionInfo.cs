using System;

namespace WotDossier.Update.Update
{
    /// <summary>
    /// 
    /// </summary>
    public class DownloadedVersionInfo
    {
        /// <summary>
        /// Gets or sets the latest version.
        /// </summary>
        /// <value>
        /// The latest version.
        /// </value>
        public Version InstallerVersion { get; set; }

        /// <summary>
        /// Gets or sets the installer URL.
        /// </summary>
        /// <value>
        /// The installer URL.
        /// </value>
        public string InstallerUrl { get; set; }

        /// <summary>
        /// Gets or sets the data version.
        /// </summary>
        public Version DataVersion { get; set; }

        /// <summary>
        /// Gets or sets the data URL.
        /// </summary>
        public string DataUrl { get; set; }
    }
}