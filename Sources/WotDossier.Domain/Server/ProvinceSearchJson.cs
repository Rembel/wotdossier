namespace WotDossier.Domain.Server
{
    public class ProvinceSearchJson
    {
        /// <summary>
        /// Игровая карта
        /// </summary>
        public string arena_id { get; set; }
        /// <summary>
        /// Владелец провинции
        /// </summary>
        public int clan_id { get; set; }
        /// <summary>
        /// Соседние провинции
        /// </summary>
        public string[] neighbors { get; set; }
        /// <summary>
        /// Основной регион
        /// </summary>
        public string primary_region { get; set; }
        /// <summary>
        /// Прайм-тайм
        /// </summary>
        public int prime_time { get; set; }
        /// <summary>
        /// Название провинции
        /// </summary>
        public string province_i18n { get; set; }
        /// <summary>
        /// Идентификатор провинции
        /// </summary>
        public string province_id { get; set; }
        /// <summary>
        /// Суточный доход с провинции
        /// </summary>
        public int revenue { get; set; }
        /// <summary>
        /// Вид провинции: обычная, стартовая, ключевая, мятежная, с отложенным мятежом
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// Максимальный уровень техники
        /// </summary>
        public int vehicle_max_level { get; set; }
    }
}
