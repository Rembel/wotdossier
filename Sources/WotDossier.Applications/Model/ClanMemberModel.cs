using System;
using WotDossier.Common;
using WotDossier.Domain.Server;

namespace WotDossier.Applications.Model
{
    public class ClanMemberModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public DateTime Since { get; set; }
        public int Days { get; set; }

        public ClanMemberModel(ClanMember clanMember)
        {
            Id = clanMember.account_id;
            Name = clanMember.nickname;
            Role = Resources.Resources.ResourceManager.GetString("Role_" + clanMember.role) ?? clanMember.role;
            Since = Utils.UnixDateToDateTime(clanMember.joined_at);
            Days = (DateTime.Now - Since).Days;
        }
    }
}
