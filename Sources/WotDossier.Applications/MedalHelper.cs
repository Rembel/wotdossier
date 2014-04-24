using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Newtonsoft.Json.Linq;
using WotDossier.Domain;

namespace WotDossier.Applications
{
    public static class MedalHelper
    {
        /// <summary>
        /// Gets or sets the medals dictionary.
        /// </summary>
        /// <value>
        /// The medals.
        /// </value>
        public static Dictionary<int, Medal> Medals { get; set; }

        /// <summary>
        /// Gets the medals by identifiers.
        /// </summary>
        /// <param name="achievements">The achievements.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Reads the medals.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the achiev medals.
        /// </summary>
        /// <param name="dossierPopUps">The dossier pop ups.</param>
        /// <returns></returns>
        public static List<Medal> GetAchievMedals(List<List<JValue>> dossierPopUps)
        {
            if (Medals == null)
            {
                Medals = ReadMedals();
            }

            List<Medal> list = new List<Medal>();

            foreach (List<JValue> achievement in dossierPopUps)
            {
                int id = achievement[0].Value<int>();
                int value = 0;
                if (achievement[1].Type == JTokenType.Integer)
                {
                    value = achievement[1].Value<int>();
                }
                else
                {
                    value = -1;
                }

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