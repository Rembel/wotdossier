using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using Newtonsoft.Json.Linq;
using WotDossier.Domain;

namespace WotDossier.Dal
{
    internal class MapConfigConverter : JsonCreationConverter<MapConfig>
    {
        private static readonly CultureInfo _invariantCulture = new CultureInfo("ru-RU");

        protected override MapConfig Create(Type objectType, JObject jObject)
        {
            MapConfig mapConfig = new MapConfig();
            if (FieldExists("boundingBox", jObject))
            {
                var values = jObject["boundingBox"]["bottomLeft"].Value<string>().Split(' ');

                var x = ConvertToDouble(values[0]);
                var y = ConvertToDouble(values[1]);

                values = jObject["boundingBox"]["upperRight"].Value<string>().Split(' ');

                var x1 = ConvertToDouble(values[0]);
                var y1 = ConvertToDouble(values[1]);

                mapConfig.BoundingBox = new Rect(x, y, x1 - x, y1 - y);
            }
            if (FieldExists("gameplayTypes", jObject))
            {
                foreach (var gameplayType in jObject["gameplayTypes"].Children())
                {
                    GameplayDescription gameplayDescription = new GameplayDescription();
                    JToken jToken = ((JProperty)gameplayType).Value;
                    if (jToken.HasValues)
                    {
                        gameplayDescription.ControlPoint = GetControlPoint(jToken);
                        gameplayDescription.TeamBasePositions = GetTeamBasePositions(jToken);
                        gameplayDescription.TeamSpawnPoints = GetTeamSpawnPoints(jToken);
                    }

                    mapConfig.GameplayTypes.Add(((JProperty)gameplayType).Name, gameplayDescription);
                }
            }
            return mapConfig;
        }

        private Dictionary<int, List<Point>> GetTeamBasePositions(JToken jToken)
        {
            Dictionary<int, List<Point>> teamBasePositions = new Dictionary<int, List<Point>>();
            if (FieldExists("teamBasePositions", jToken))
            {
                foreach (JProperty team in jToken["teamBasePositions"].Children())
                {
                    List<Point> positions = new List<Point>();
                    
                    foreach (JProperty position in team.Value.Children())
                    {
                        var values = position.Value.Value<string>().Split(' ');
                        var x = ConvertToDouble(values[0]);
                        var y = ConvertToDouble(values[1]);
                        Point point = new Point(x, y);
                        positions.Add(point);
                    }

                    teamBasePositions.Add(GetTeamIndex(team.Name), positions);
                }
            }
            return teamBasePositions;
        }

        private static int GetTeamIndex(string team)
        {
            return Convert.ToInt32(team.Replace("team", string.Empty));
        }

        private Dictionary<int, List<Point>> GetTeamSpawnPoints(JToken jToken)
        {
            Dictionary<int, List<Point>> teamBasePositions = new Dictionary<int, List<Point>>();
            if (FieldExists("teamSpawnPoints", jToken))
            {
                foreach (JProperty team in jToken["teamSpawnPoints"].Children())
                {
                    List<Point> positions = new List<Point>();

                    foreach (JProperty position in team.Value.Children())
                    {
                        JToken value = position.Value;
                        List<Point> points = new List<Point>();
                        if (value is JArray)
                        {
                            JArray jArray = (JArray) value;
                            List<string> values = jArray.ToObject<List<string>>();
                            foreach (var pointString in values)
                            {
                                var point = ConvertToPoint(pointString);
                                points.Add(point);
                            }
                        }
                        else
                        {
                            string pointString = value.Value<string>();
                            var point = ConvertToPoint(pointString);
                            points.Add(point);
                        }

                        positions.AddRange(points);
                    }

                    teamBasePositions.Add(GetTeamIndex(team.Name), positions);
                }
            }
            return teamBasePositions;
        }

        private static Point ConvertToPoint(string pointString)
        {
            var values = pointString.Split(' ');
            var x = ConvertToDouble(values[0]);
            var y = ConvertToDouble(values[1]);
            Point point = new Point(x, y);
            return point;
        }

        private Point? GetControlPoint(JToken jToken)
        {
            if (FieldExists("controlPoint", jToken))
            {
                var values = jToken["controlPoint"].Value<string>().Split(' ');
                var x = ConvertToDouble(values[0]);
                var y = ConvertToDouble(values[1]);
                Point controlPoint = new Point(x, y);
                return controlPoint;
            }
            return null;
        }

        private static double ConvertToDouble(string value)
        {
            return Double.Parse(value.Replace(".", ",").Trim(), CultureInfo.CurrentCulture);

        }
    }
}