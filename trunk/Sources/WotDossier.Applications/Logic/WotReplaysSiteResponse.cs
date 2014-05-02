namespace WotDossier.Applications.Logic
{
    public class WotReplaysSiteResponse
    {
        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        public string Error { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets the direct link.
        /// </summary>
        /// <value>
        /// The direct link.
        /// </value>
        public string DirectLink { get; set; }

        /// <summary>
        /// Gets or sets the is secret.
        /// </summary>
        /// <value>
        /// The is secret.
        /// </value>
        public bool? IsSecret { get; set; }

        /// <summary>
        /// Gets or sets the MD5.
        /// </summary>
        /// <value>
        /// The MD5.
        /// </value>
        public string Md5 { get; set; }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        public bool? Result { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url { get; set; }
    }
}
