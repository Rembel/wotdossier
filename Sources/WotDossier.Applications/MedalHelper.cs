﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using WotDossier.Domain;

namespace WotDossier.Applications
{
    public static class MedalHelper
    {
        public static Dictionary<int, Medal> Medals { get; set; }

        public static List<Medal> GetMedals(List<int> achievements)
        {
            if (Medals == null)
            {
                Medals = ReadMedals();
            }

            List<Medal> list = new List<Medal>();

            foreach (int achievement in achievements)
            {
                if (Medals.ContainsKey(achievement))
                {
                    list.Add(Medals[achievement]);
                }
            }
            return list;
        }

        public static Dictionary<int, Medal> ReadMedals()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(File.OpenRead(Path.Combine(Environment.CurrentDirectory, @"Data\Medals.xml")));

            XmlNodeList nodes = doc.SelectNodes("Medals/node()/medal");

            Dictionary<int, Medal> medals = new Dictionary<int, Medal>();

            foreach (XmlNode node in nodes)
            {
                Medal medal = new Medal();
                medal.Id = Convert.ToInt32(node.Attributes["id"].Value);
                medal.Name = node.Attributes["name"].Value;
                medal.Icon = node.Attributes["icon"].Value;
                medal.Type = int.Parse(node.Attributes["type"].Value);
                medals.Add(medal.Id, medal);
            }

            return medals;
        }

        public static List<Medal> GetAchievMedals(List<List<int>> dossierPopUps)
        {
            if (Medals == null)
            {
                Medals = ReadMedals();
            }

            List<Medal> list = new List<Medal>();

            foreach (List<int> achievement in dossierPopUps)
            {
                int id = achievement[0];
                int value = achievement[1];
                int exId = id * 10 + value;

                if (Medals.ContainsKey(id))
                {
                    list.Add(Medals[id]);
                }
                else if (Medals.ContainsKey(exId))
                {
                    list.Add(Medals[exId]);
                }
            }
            return list;
        }
    }
}