namespace WotDossier.Domain.Server
{
    public class ClanMember
    {
        public int account_id { get; set; }
        public int joined_at { get; set; }
        public string nickname { get; set; }
        public string role { get; set; }
        public string role_i18n { get; set; }
    }
}
