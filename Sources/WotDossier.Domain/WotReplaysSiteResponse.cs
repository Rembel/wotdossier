namespace WotDossier.Domain
{
    public class WotReplaysSiteResponse
    {
        public string Error { get; set; }

        public int Code { get; set; }

        public string DirectLink { get; set; }

        public bool? IsSecret { get; set; }

        public string Md5 { get; set; }

        public bool? Result { get; set; }

        public string Url { get; set; }
    }
}
