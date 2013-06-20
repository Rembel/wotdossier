using System;
using System.Globalization;
using System.IO;
using System.Text;
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

        /*
        public void wotReplay(FileInfo replayFileInfo)
        {
            string path = replayFileInfo.FullName;
            string str = "";
            string str2 = "";
            if (File.Exists(path))
            {
                FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read);
                int count = 0;
                byte[] buffer = new byte[4];
                stream.Read(buffer, 0, 4);
                if (buffer[0] != 0x21)
                {
                    stream.Read(buffer, 0, 4);
                    stream.Read(buffer, 0, 4);
                    count = ((buffer[0] + (0x100 * buffer[1])) + (0x10000 * buffer[2])) + (0x1000000 * buffer[3]);
                }
                byte[] buffer2 = new byte[count];
                stream.Read(buffer2, 0, count);
                ASCIIEncoding encoding = new ASCIIEncoding();
                str = encoding.GetString(buffer2);
                if (count > 0)
                {
                    stream.Read(buffer, 0, 4);
                    count = ((buffer[0] + (0x100 * buffer[1])) + (0x10000 * buffer[2])) + (0x1000000 * buffer[3]);
                    buffer2 = new byte[count];
                    stream.Read(buffer2, 0, count);
                    str2 = encoding.GetString(buffer2);
                }
                stream.Close();
                if (str.Length > 0)
                {
                    this.fb = JsonConvert.DeserializeObject<firstBlock>(str);
                }
                else
                {
                    this.fb = new firstBlock();
                    this.fb.playerVehicle = "no data";
                }
                try
                {
                    this.sb = JsonConvert.DeserializeObject<secondBlock[]>(str2)[0];
                }
                catch
                {
                    this.sb = new secondBlock();
                    this.sb.spotted = new int[0];
                    this.sb.killed = new int[0];
                    this.sb.factors = new factors();
                }
                str = "";
                str2 = "";
                foreach (int num2 in this.fb.vehicles.Keys)
                {
                    if (this.fb.vehicles[num2].name == this.fb.playerName)
                    {
                        this.fb.team = this.fb.vehicles[num2].team;
                        break;
                    }
                }
            }
        }
        */
    }
}
