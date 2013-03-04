using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WotDossier.Domain;

namespace WotDossier.Applications
{
    public class Read
    {
        public List<Tank> Start(string json)
        {
            List<Tank> tanks = new List<Tank>();

            using (StreamReader re = new StreamReader(json))
            {
                JsonTextReader reader = new JsonTextReader(re);
                JsonSerializer se = new JsonSerializer();
                JObject parsedData = (JObject)se.Deserialize(reader);
                //JToken headerData = parsedData["header"];
                JToken tanksData = parsedData["tanks"];

                foreach (JToken jToken in tanksData)
                {
                    JProperty property = (JProperty)jToken;
                    Tank tank = JsonConvert.DeserializeObject<Tank>(property.Value.ToString());
                    tank.Name = tank.Common.tanktitle;
                    tanks.Add(tank);
                }
            }
            return tanks;
        }
    }
}
