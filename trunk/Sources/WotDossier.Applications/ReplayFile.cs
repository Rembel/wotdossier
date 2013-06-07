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
        private const string REPLAY_FILENAME_FORMAT = @"(\d+_\d+)_([0-9a-zA-Z.-]+)_(\d+)_(.+)";
        private const string TANKNAME_FORMAT = @"([a-zA-Z]+)-([0-9a-zA-Z.-]+)";

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
            Regex fileNameRegexp = new Regex(REPLAY_FILENAME_FORMAT);
            Match match = fileNameRegexp.Match(replayFileInfo.Name);
            if (match.Success)
            {
                CultureInfo provider = CultureInfo.InvariantCulture;

                PlayTime = DateTime.ParseExact(match.Groups[1].Value, "yyyyMMdd_HHmm", provider);
                Regex tankNameRegexp = new Regex(TANKNAME_FORMAT);
                Match nameMatch = tankNameRegexp.Match(replayFileInfo.Name);
                CountryId = WotApiHelper.GetCountryId(nameMatch.Groups[1].Value);
                Tank = nameMatch.Groups[2].Value;
                MapName = match.Groups[4].Value.Replace(".wotreplay", "");
            }
        }
    }
}
