namespace WotDossier.Domain.Server
{
    public class ClanMember
    {
        public int account_id { get; set; }
        public long created_at { get; set; }
        public double updated_at { get; set; }
        public string account_name { get; set; }
        public double since { get; set; }
        public string role { get; set; }
    }
}
