using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using WotDossier.Dal;

namespace WotDossier.Applications
{
    public class ReplayFile
    {
        //20121201_1636_ussr-IS_42_north_america
        private const string REPLAY_DATETIME_FORMAT = @"(\d+_\d+)";
        private const string MAPNAME_FILENAME = @"(\d+_[a-zA-Z_]+)(\.wotreplay)";
        private const string TANKNAME_FORMAT = @"([a-zA-Z]+)-(.+)";

        public string MapName { get; set; }
        public string Tank { get; set; }
        public int CountryId { get; set; }
        public DateTime PlayTime { get; set; }
        public FileInfo FileInfo { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ReplayFile(FileInfo replayFileInfo)
        {
            FileInfo = replayFileInfo;
            string fileName = replayFileInfo.Name;

            Regex dateTimeRegexp = new Regex(REPLAY_DATETIME_FORMAT);
            Match dateTimeMatch = dateTimeRegexp.Match(fileName);
            CultureInfo provider = CultureInfo.InvariantCulture;
            PlayTime = DateTime.ParseExact(dateTimeMatch.Groups[1].Value, "yyyyMMdd_HHmm", provider);
            
            Regex mapNameRegexp = new Regex(MAPNAME_FILENAME);
            Match mapNameMatch = mapNameRegexp.Match(fileName);
            MapName = mapNameMatch.Groups[1].Value;

            Regex tankNameRegexp = new Regex(TANKNAME_FORMAT);
            string tankName = fileName.Replace(dateTimeMatch.Groups[1].Value, "").Replace(mapNameMatch.Groups[0].Value, "");
            tankName = tankName.Substring(1, tankName.Length - 2);
            Match tankNameMatch = tankNameRegexp.Match(tankName);
            CountryId = WotApiHelper.GetCountryId(tankNameMatch.Groups[1].Value);
            Tank = tankNameMatch.Groups[2].Value;
        }
    }
}
