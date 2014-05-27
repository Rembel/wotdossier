namespace WotDossier.Applications.ViewModel
{
    public class CompareStatisticViewModelBase<T> where T : StatisticViewModelBase
    {
        public StatisticViewModelBase First { get; set; }
        public StatisticViewModelBase Second { get; set; }

        public int BattlesCount { get { return First.BattlesCount - Second.BattlesCount; } }
        public int Wins { get { return First.Wins - Second.Wins; } }
        public int Losses { get { return First.Losses - Second.Losses; } }
        public int SurvivedBattles { get { return First.SurvivedBattles - Second.SurvivedBattles; } }
        public int Xp { get { return First.Xp - Second.Xp; } }

        public int MaxXp { get { return First.MaxXp - Second.MaxXp; } }
        public int Frags { get { return First.Frags - Second.Frags; } }
        public int Spotted { get { return First.Spotted - Second.Spotted; } }

        public int DamageDealt { get { return First.DamageDealt - Second.DamageDealt; } }
        public int DamageTaken { get { return First.DamageTaken - Second.DamageTaken; } }
        public int CapturePoints { get { return First.CapturePoints - Second.CapturePoints; } }
        public int DroppedCapturePoints { get { return First.DroppedCapturePoints - Second.DroppedCapturePoints; } }

        public double Tier { get { return First.Tier - Second.Tier; } }

        public double KillDeathRatio { get { return First.KillDeathRatio - Second.KillDeathRatio; } }

        public double DamageRatio { get { return First.DamageRatio - Second.DamageRatio; } }

        #region Percents

        public double HitsPercents { get { return First.HitsPercents - Second.HitsPercents; } }
        public double WinsPercent { get { return First.WinsPercent - Second.WinsPercent; } }
        public double LossesPercent { get { return First.LossesPercent - Second.LossesPercent; } }
        public double SurvivedBattlesPercent { get { return First.SurvivedBattlesPercent - Second.SurvivedBattlesPercent; } }

        #endregion

        #region Average values

        public double AvgXp
        {
            get { return First.AvgXp - Second.AvgXp; }
        }

        public double AvgFrags
        {
            get { return First.AvgFrags - Second.AvgFrags; }
        }

        public double AvgSpotted
        {
            get { return First.AvgSpotted - Second.AvgSpotted; }
        }

        public double AvgDamageDealt
        {
            get { return First.AvgDamageDealt - Second.AvgDamageDealt; }
        }

        public double AvgCapturePoints
        {
            get { return First.AvgCapturePoints - Second.AvgCapturePoints; }
        }

        public double AvgDroppedCapturePoints
        {
            get { return First.AvgDroppedCapturePoints - Second.AvgDroppedCapturePoints; }
        }

        #endregion

        #region Unofficial ratings

        public double WN7Rating
        {
            get { return First.WN7Rating - Second.WN7Rating; }
        }

        public double NoobRating
        {
            get { return First.NoobRating - Second.NoobRating; }
        }

        public double EffRating
        {
            get { return First.EffRating - Second.EffRating; }
        }

        public double KievArmorRating
        {
            get { return First.KievArmorRating - Second.KievArmorRating; }
        }

        public double XEFF
        {
            get { return First.XEFF - Second.XEFF; }
        }

        public double XWN
        {
            get { return First.XWN - Second.XWN; }
        }

        #endregion

        #region Achievments

        #region [ ITankRowBattleAwards ]

        public int BattleHero { get { return First.BattleHero - Second.BattleHero; } }

        public int Warrior { get { return First.Warrior - Second.Warrior; } }

        public int Invader { get { return First.Invader - Second.Invader; } }

        public int Sniper { get { return First.Sniper - Second.Sniper; } }

        public int Sniper2 { get { return First.Sniper2 - Second.Sniper2; } }

        public int MainGun { get { return First.MainGun - Second.MainGun; } }

        public int Defender { get { return First.Defender - Second.Defender; } }

        public int SteelWall { get { return First.SteelWall - Second.SteelWall; } }

        public int Confederate { get { return First.Confederate - Second.Confederate; } }

        public int Scout { get { return First.Scout - Second.Scout; } }

        public int PatrolDuty { get { return First.PatrolDuty - Second.PatrolDuty; } }

        public int BrothersInArms { get { return First.BrothersInArms - Second.BrothersInArms; } }

        public int CrucialContribution { get { return First.CrucialContribution - Second.CrucialContribution; } }

        public int IronMan { get { return First.IronMan - Second.IronMan; } }

        public int LuckyDevil { get { return First.LuckyDevil - Second.LuckyDevil; } }

        public int Sturdy { get { return First.Sturdy - Second.Sturdy; } }

        #endregion

        #region [ ITankRowEpic ]

        public int Boelter { get { return First.Boelter - Second.Boelter; } }

        public int RadleyWalters { get { return First.RadleyWalters - Second.RadleyWalters; } }

        public int LafayettePool { get { return First.LafayettePool - Second.LafayettePool; } }

        public int Orlik { get { return First.Orlik - Second.Orlik; } }

        public int Oskin { get { return First.Oskin - Second.Oskin; } }

        public int Lehvaslaiho { get { return First.Lehvaslaiho - Second.Lehvaslaiho; } }

        public int Nikolas { get { return First.Nikolas - Second.Nikolas; } }

        public int Halonen { get { return First.Halonen - Second.Halonen; } }

        public int Burda { get { return First.Burda - Second.Burda; } }

        public int Pascucci { get { return First.Pascucci - Second.Pascucci; } }

        public int Dumitru { get { return First.Dumitru - Second.Dumitru; } }

        public int TamadaYoshio { get { return First.TamadaYoshio - Second.TamadaYoshio; } }

        public int Billotte { get { return First.Billotte - Second.Billotte; } }

        public int BrunoPietro { get { return First.BrunoPietro - Second.BrunoPietro; } }

        public int Tarczay { get { return First.Tarczay - Second.Tarczay; } }

        public int Kolobanov { get { return First.Kolobanov - Second.Kolobanov; } }

        public int Fadin { get { return First.Fadin - Second.Fadin; } }

        public int HeroesOfRassenay { get { return First.HeroesOfRassenay - Second.HeroesOfRassenay; } }

        public int DeLanglade { get { return First.DeLanglade - Second.DeLanglade; } }

        #endregion

        #region [ ITankRowSpecialAwards ]

        public int Kamikaze { get { return First.Kamikaze - Second.Kamikaze; } }

        public int Raider { get { return First.Raider - Second.Raider; } }

        public int Bombardier { get { return First.Bombardier - Second.Bombardier; } }

        public int Reaper { get { return First.Reaper - Second.Reaper; } }

        public int Sharpshooter { get { return First.Sharpshooter - Second.Sharpshooter; } }

        //public int Invincible { get { return First. - Second.; } }

        //public int Survivor { get { return First. - Second.; } }

        //public int MouseTrap { get { return First. - Second.; } }

        //public int Hunter { get { return First. - Second.; } }

        //public int Sinai { get { return First. - Second.; } }

        //public int PattonValley { get { return First. - Second.; } }

        //public int Huntsman { get { return First. - Second.; } }

        //#endregion

        //#region [ ITankRowMedals]

        //public int Kay { get { return First. - Second.; } }

        //public int Carius { get { return First. - Second.; } }

        //public int Knispel { get { return First. - Second.; } }

        //public int Poppel { get { return First. - Second.; } }

        //public int Abrams { get { return First. - Second.; } }

        //public int Leclerk { get { return First. - Second.; } }

        //public int Lavrinenko { get { return First. - Second.; } }

        //public int Ekins { get { return First. - Second.; } }

        //#endregion

        //#region [ ITankRowSeries ]

        //public int ReaperLongest { get { return First. - Second.; } }

        //public int ReaperProgress { get { return First. - Second.; } }

        //public int SharpshooterLongest { get { return First. - Second.; } }

        //public int SharpshooterProgress { get { return First. - Second.; } }

        //public int MasterGunnerLongest { get { return First. - Second.; } }

        //public int MasterGunnerProgress { get { return First. - Second.; } }

        //public int InvincibleLongest { get { return First. - Second.; } }

        //public int InvincibleProgress { get { return First. - Second.; } }

        //public int SurvivorLongest { get { return First. - Second.; } }

        //public int SurvivorProgress { get { return First. - Second.; } }

        #endregion

        #endregion

        //public int BattlesPerDay { get { return First. - Second.; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public CompareStatisticViewModelBase(StatisticViewModelBase first, StatisticViewModelBase second)
        {
            First = first;
            Second = second;
        }



        /*
Рейтинг Эффективности:
WN6 Рейтинг:
Рейтинг Эффективности XVM:
Средний уровень танков:

         * Общие результаты
Проведено боёв:
Побед: (%)
Проигрышей:
Проигрышей: (%)
GPL:
GPL: (%)
Среднее количество игр в день

         * Боевая эффективность
Уничтожено:
Уничтожено за бой:
Обнаружено врагов:
Обнаружено за бой:
Процент попадания:
Нанесенные повреждения:
Повреждений за бой:
Очки захвата базы:
Захват базы за бой:
Очки защиты базы:
Очков защиты базы за бой:

         * Боевой опыт
Суммарный опыт:
Средний опыт за бой:
Максимальный опыт за бой:

         * Достижения
Герой битвы 	Значение 	Бои* 	Значение 	Бои* 	Значение 	Бои*
Захватчик
Дозорный
Защитник
Поддержка
Стальная стена
Братья по оружию
Снайпер
Воин
Решающий вклад
Разведчик

         * Эпические достижения 						
Медаль Халонена
Медаль Паскуччи
Медаль Фадина
Медаль Бруно
Медаль героев Расейняя
Медаль Тамада Йошио
Медаль Орлика
Медаль Тарцая
Медаль де Лагланда
Медаль Бурды
Медаль Рэдли-Уолтерса
Медаль Николса
Медаль Оськина
Медаль Бийота
Медаль Колобанова
Медаль Бёльтера
Медаль Пула
Медаль Лехвеслайхо
Медаль Думитру

         * Этапные достижения 	степень 	степень 	степень
Медаль Кариуса
Медаль Экинса
Медаль Кея
Медаль Леклерка
Медаль Абрамса
Медаль Попеля
Медаль Лавриненко
Медаль Книспеля

         * Почетные звания 	количество 	количество 	количество
Бронебойщик
Гроза мышей
Бомбардир
Живучий
Коса смерти
Лев Синая
Стрелок
Неуязвимый
Зверобой
Камикадзе
Рейдер
Лесоруб
Эксперт
Егерь
Счастливчик
Невозмутимый
Спартанец
Долина Паттонов

         * Эксперт 	Значение 	Значение 	Значение
«Эксперт» США
«Эксперт» Франция
«Эксперт» СССР
«Эксперт» Великобритания
«Эксперт» Германия
«Инженер-механик» США
«Инженер-механик» Франция
«Инженер-механик» СССР
«Инженер-механик» Великобритания
«Инженер-механик» Германия
         */
    }
}
