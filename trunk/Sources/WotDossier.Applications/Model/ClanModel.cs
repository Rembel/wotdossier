﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using WotDossier.Common;
using WotDossier.Domain.Server;

namespace WotDossier.Applications.Model
{
    public class ClanModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ClanModel(ClanData clan)
        {
            Id = clan.clan_id;
            Name = clan.name;
            Abbreviation = string.Format("[{0}]", clan.abbreviation);
            Description = clan.description;
            Motto = clan.motto;
            MembersCount = clan.members_count;
            Color = (Color)ColorConverter.ConvertFromString(clan.clan_color ?? "#BD3838");

            FullName = string.Format("[{0}] {1}", clan.abbreviation, clan.name);


            Updated = Utils.UnixDateToDateTime((long)clan.updated_at);
            Created = Utils.UnixDateToDateTime(clan.created_at);
            OwnerId = clan.owner_id;
            IsClanDisbanded = clan.is_clan_disbanded;
            RequestAvailability = clan.request_availability;

            Emblems = clan.emblems;
            if (clan.members != null)
            {
                Members = clan.members.Values.Select(x => new ClanMemberModel(x)).OrderBy(x => x.Name).ToList();
            }
        }

        public ClanModel(ClanData clan, string memberRole, long memberSince): this(clan)
        {
            Role = Resources.Resources.ResourceManager.GetString("Role_" + memberRole);
            Since = Utils.UnixDateToDateTime(memberSince);
            Days = (DateTime.Now - Since).Days;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }
        public string Description { get; set; }
        public string Motto { get; set; }
        public string FullName { get; set; }
        public int MembersCount { get; set; }

        public Color Color { get; set; }
        public DateTime Updated { get; set; }
        public DateTime Created { get; set; }

        public int OwnerId { get; set; }
        public bool IsClanDisbanded { get; set; }
        public bool RequestAvailability { get; set; }

        public string Role { get; set; }
        public DateTime Since { get; set; }
        public int Days { get; set; }

        public ClanEmblems Emblems { get; set; }
        public List<ClanMemberModel> Members { get; set; }
    }
}
