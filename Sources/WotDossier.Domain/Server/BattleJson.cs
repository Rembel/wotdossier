using System.Collections.Generic;

namespace WotDossier.Domain.Server
{
    public class BattleJson
    {
        /// <summary>
        /// Идентификаторы провинций
        /// </summary>
        public string[] provinces { get; set; }
        public List<ProvinceSearchJson> provinceDescriptions { get; set; }
        /// <summary>
        /// Бой начался
        /// </summary>
        public bool started { get; set; }
        /// <summary>
        /// Время начала боя
        /// </summary>
        public int time { get; set; }
        /// <summary>
        /// Тип боя
        /// </summary>
        public ClanBattleType type { get; set; }
        /// <summary>
        /// Информация о карте
        /// </summary>
        public Arena[] arenas { get; set; }
    }

    public class Arena
    {
        /// <summary>
        /// ID карты
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// Название карты
        /// </summary>
        public string name_i18n { get; set; }
    }
}
