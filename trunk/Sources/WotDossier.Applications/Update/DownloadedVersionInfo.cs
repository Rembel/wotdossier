using System;

namespace WotDossier.Applications.Update
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
        public Version LatestVersion { get; set; }

        /// <summary>
        /// Gets or sets the installer URL.
        /// </summary>
        /// <value>
        /// The installer URL.
        /// </value>
        public string InstallerUrl { get; set; }
    }
}