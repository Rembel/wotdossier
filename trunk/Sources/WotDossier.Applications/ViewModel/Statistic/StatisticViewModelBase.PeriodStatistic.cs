using WotDossier.Applications.Logic;

namespace WotDossier.Applications.ViewModel.Statistic
{
    public abstract partial class StatisticViewModelBase
    {
        #region Period statistic

        #region Delta

        public int BattlesCountDelta
        {
            get { return BattlesCount - TypedPrevStatistic.BattlesCount; }
        }

        public int WinsDelta
        {
            get { return Wins - TypedPrevStatistic.Wins; }
        }

        public double WinsPercentDelta
        {
            get { return WinsPercent - TypedPrevStatistic.WinsPercent; }
        }

        public int LossesDelta
        {
            get { return Losses - TypedPrevStatistic.Losses; }
        }

        public double LossesPercentDelta
        {
            get { return LossesPercent - TypedPrevStatistic.LossesPercent; }
        }

        public int SurvivedBattlesDelta
        {
            get { return SurvivedBattles - TypedPrevStatistic.SurvivedBattles; }
        }

        public double SurvivedBattlesPercentDelta
        {
            get { return SurvivedBattlesPercent - TypedPrevStatistic.SurvivedBattlesPercent; }
        }

        public int XpDelta
        {
            get { return Xp - TypedPrevStatistic.Xp; }
        }

        public int MaxXpDelta
        {
            get { return MaxXp - TypedPrevStatistic.MaxXp; }
        }

        public int FragsDelta
        {
            get { return Frags - TypedPrevStatistic.Frags; }
        }

        public int SpottedDelta
        {
            get { return Spotted - TypedPrevStatistic.Spotted; }
        }

        public double HitsPercentsDelta
        {
            get { return HitsPercents - TypedPrevStatistic.HitsPercents; }
        }

        public int DamageDealtDelta
        {
            get { return DamageDealt - TypedPrevStatistic.DamageDealt; }
        }

        public double KillDeathRatioDelta
        {
            get { return KillDeathRatio - TypedPrevStatistic.KillDeathRatio; }
        }

        public int CapturePointsDelta
        {
            get { return CapturePoints - TypedPrevStatistic.CapturePoints; }
        }

        public int DroppedCapturePointsDelta
        {
            get { return DroppedCapturePoints - TypedPrevStatistic.DroppedCapturePoints; }
        }

        #region Ratings

        public double WN7RatingDelta
        {
            get { return WN7Rating - TypedPrevStatistic.WN7Rating; }
        }

        public double WN8RatingDelta
        {
            get { return WN8Rating - TypedPrevStatistic.WN8Rating; }
        }

        public double PerformanceRatingDelta
        {
            get { return PerformanceRating - TypedPrevStatistic.PerformanceRating; }
        }

        public double RBRDelta
        {
            get { return RBR - TypedPrevStatistic.RBR; }
        }

        public double EffRatingDelta
        {
            get { return EffRating - TypedPrevStatistic.EffRating; }
        }

        public double KievArmorRatingDelta
        {
            get { return KievArmorRating - TypedPrevStatistic.KievArmorRating; }
        }

        public double XvmRatingDelta
        {
            get { return XEFF - TypedPrevStatistic.XEFF; }
        }

        public double NoobRatingDelta
        {
            get { return NoobRating - TypedPrevStatistic.NoobRating; }
        }

        #endregion

        #region Average values

        public double AvgXpDelta
        {
            get
            {
                return AvgXp - TypedPrevStatistic.AvgXp;
            }
        }

        public double AvgFragsDelta
        {
            get
            {
                return AvgFrags - TypedPrevStatistic.AvgFrags;
            }
        }

        public double AvgSpottedDelta
        {
            get
            {
                return AvgSpotted - TypedPrevStatistic.AvgSpotted;
            }
        }

        public double AvgDamageDealtDelta
        {
            get
            {
                return AvgDamageDealt - TypedPrevStatistic.AvgDamageDealt;
            }
        }

        public double AvgCapturePointsDelta
        {
            get
            {
                return AvgCapturePoints - TypedPrevStatistic.AvgCapturePoints;
            }
        }

        public double AvgDroppedCapturePointsDelta
        {
            get
            {
                return AvgDroppedCapturePoints - TypedPrevStatistic.AvgDroppedCapturePoints;
            }
        }

        #endregion

        #endregion

        #region Statistic For Period

        public double WinsPercentForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return WinsDelta / (double)BattlesCountDelta * 100.0;
                }
                return 0;
            }
        }

        public double LossesPercentForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return LossesDelta / (double)BattlesCountDelta * 100.0;
                }
                return 0;
            }
        }

        public double SurvivedBattlesPercentForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return SurvivedBattlesDelta / (double)BattlesCountDelta * 100.0;
                }
                return 0;
            }
        }

        public double HitsPercentForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    //TODO
                    return 0;
                }
                return 0;
            }
        }

        public double TierForInterval
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return (Tier * BattlesCount - TypedPrevStatistic.Tier * TypedPrevStatistic.BattlesCount) / BattlesCountDelta;
                }
                return 0;
            }
        }

        public double EffRatingForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return RatingHelper.EffectivityRating(AvgDamageDealtForPeriod, TierForInterval, AvgFragsForPeriod, AvgSpottedForPeriod,
                                               AvgCapturePointsForPeriod, AvgDroppedCapturePointsForPeriod);
                }
                return 0;
            }
        }

        public double WN6RatingForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return RatingHelper.Wn6(AvgDamageDealtForPeriod, TierForInterval, AvgFragsForPeriod, AvgSpottedForPeriod,
                                                AvgDroppedCapturePointsForPeriod, WinsPercentForPeriod);
                }
                return 0;
            }
        }

        public double WN7RatingForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return RatingHelper.Wn7(BattlesCountDelta, AvgDamageDealtForPeriod, TierForInterval, AvgFragsForPeriod, AvgSpottedForPeriod,
                                                AvgDroppedCapturePointsForPeriod, WinsPercentForPeriod);
                }
                return 0;
            }
        }

        private double _performanceRatingForPeriod;
        public virtual double PerformanceRatingForPeriod
        {
            get { return _performanceRatingForPeriod; }
            set
            {
                _performanceRatingForPeriod = value;
                OnPropertyChanged("PerformanceRatingForPeriod");
            }
        }

        private double _wn8RatingForPeriod;
        public virtual double WN8RatingForPeriod
        {
            get { return _wn8RatingForPeriod; }
            set
            {
                _wn8RatingForPeriod = value;
                OnPropertyChanged("WN8RatingForPeriod");
            }
        }

        public double KievArmorRatingForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    //Battle count used to calc rating for period on noobmeter.com
                    return RatingHelper.KievArmorRating(BattlesCount, AvgXpForPeriod, AvgDamageDealtForPeriod,
                                                            WinsPercentForPeriod / 100.0,
                                                            AvgFragsForPeriod, AvgSpottedForPeriod,
                                                            AvgCapturePointsForPeriod, AvgDroppedCapturePointsForPeriod);
                }
                return 0;
            }
        }

        public double AvgXpForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return XpDelta / (double)BattlesCountDelta;
                }
                return 0;
            }
        }

        public double AvgFragsForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return FragsDelta / (double)BattlesCountDelta;
                }
                return 0;
            }
        }

        public double AvgSpottedForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return SpottedDelta / (double)BattlesCountDelta;
                }
                return 0;
            }
        }

        public double AvgDamageDealtForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return DamageDealtDelta / (double)BattlesCountDelta;
                }
                return 0;
            }
        }

        public double AvgCapturePointsForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return CapturePointsDelta / (double)BattlesCountDelta;
                }
                return 0;
            }
        }

        public double AvgDroppedCapturePointsForPeriod
        {
            get
            {
                if (BattlesCountDelta > 0)
                {
                    return DroppedCapturePointsDelta / (double)BattlesCountDelta;
                }
                return 0;
            }
        }

        #endregion

        #region Achievments

        public int BattleHeroDelta
        {
            get { return BattleHero - TypedPrevStatistic.BattleHero; }
        }

        public int WarriorDelta
        {
            get { return Warrior - TypedPrevStatistic.Warrior; }
        }

        public int InvaderDelta
        {
            get { return Invader - TypedPrevStatistic.Invader; }
        }

        public int SniperDelta
        {
            get { return Sniper - TypedPrevStatistic.Sniper; }
        }

        public int Sniper2Delta
        {
            get { return Sniper2 - TypedPrevStatistic.Sniper2; }
        }

        public int MainGunDelta
        {
            get { return MainGun - TypedPrevStatistic.MainGun; }
        }

        public int DefenderDelta
        {
            get { return Defender - TypedPrevStatistic.Defender; }
        }

        public int SteelWallDelta
        {
            get { return SteelWall - TypedPrevStatistic.SteelWall; }
        }

        public int ConfederateDelta
        {
            get { return Confederate - TypedPrevStatistic.Confederate; }
        }

        public int ScoutDelta
        {
            get { return Scout - TypedPrevStatistic.Scout; }
        }

        public int PatrolDutyDelta
        {
            get { return PatrolDuty - TypedPrevStatistic.PatrolDuty; }
        }

        public int BrothersInArmsDelta
        {
            get { return BrothersInArms - TypedPrevStatistic.BrothersInArms; }
        }

        public int CrucialContributionDelta
        {
            get { return CrucialContribution - TypedPrevStatistic.CrucialContribution; }
        }

        public int IronManDelta
        {
            get { return IronMan - TypedPrevStatistic.IronMan; }
        }

        public int LuckyDevilDelta
        {
            get { return LuckyDevil - TypedPrevStatistic.LuckyDevil; }
        }

        public int SturdyDelta
        {
            get { return Sturdy - TypedPrevStatistic.Sturdy; }
        }

        public int BoelterDelta
        {
            get { return Boelter - TypedPrevStatistic.Boelter; }
        }

        public int RadleyWaltersDelta
        {
            get { return RadleyWalters - TypedPrevStatistic.RadleyWalters; }
        }

        public int LafayettePoolDelta
        {
            get { return LafayettePool - TypedPrevStatistic.LafayettePool; }
        }

        public int OrlikDelta
        {
            get { return Orlik - TypedPrevStatistic.Orlik; }
        }

        public int OskinDelta
        {
            get { return Oskin - TypedPrevStatistic.Oskin; }
        }

        public int LehvaslaihoDelta
        {
            get { return Lehvaslaiho - TypedPrevStatistic.Lehvaslaiho; }
        }

        public int NikolasDelta
        {
            get { return Nikolas - TypedPrevStatistic.Nikolas; }
        }

        public int HalonenDelta
        {
            get { return Halonen - TypedPrevStatistic.Halonen; }
        }

        public int BurdaDelta
        {
            get { return Burda - TypedPrevStatistic.Burda; }
        }

        public int PascucciDelta
        {
            get { return Pascucci - TypedPrevStatistic.Pascucci; }
        }

        public int DumitruDelta
        {
            get { return Dumitru - TypedPrevStatistic.Dumitru; }
        }

        public int TamadaYoshioDelta
        {
            get { return TamadaYoshio - TypedPrevStatistic.TamadaYoshio; }
        }

        public int BillotteDelta
        {
            get { return Billotte - TypedPrevStatistic.Billotte; }
        }

        public int BrunoPietroDelta
        {
            get { return BrunoPietro - TypedPrevStatistic.BrunoPietro; }
        }

        public int TarczayDelta
        {
            get { return Tarczay - TypedPrevStatistic.Tarczay; }
        }

        public int KolobanovDelta
        {
            get { return Kolobanov - TypedPrevStatistic.Kolobanov; }
        }

        public int FadinDelta
        {
            get { return Fadin - TypedPrevStatistic.Fadin; }
        }

        public int HeroesOfRassenayDelta
        {
            get { return HeroesOfRassenay - TypedPrevStatistic.HeroesOfRassenay; }
        }

        public int DeLangladeDelta
        {
            get { return DeLanglade - TypedPrevStatistic.DeLanglade; }
        }

        public int KamikazeDelta
        {
            get { return Kamikaze - TypedPrevStatistic.Kamikaze; }
        }

        public int RaiderDelta
        {
            get { return Raider - TypedPrevStatistic.Raider; }
        }

        public int BombardierDelta
        {
            get { return Bombardier - TypedPrevStatistic.Bombardier; }
        }

        public int ReaperDelta
        {
            get { return Reaper - TypedPrevStatistic.Reaper; }
        }

        public int SharpshooterDelta
        {
            get { return Sharpshooter - TypedPrevStatistic.Sharpshooter; }
        }

        public int InvincibleDelta
        {
            get { return Invincible - TypedPrevStatistic.Invincible; }
        }

        public int SurvivorDelta
        {
            get { return Survivor - TypedPrevStatistic.Survivor; }
        }

        public int MouseTrapDelta
        {
            get { return MouseTrap - TypedPrevStatistic.MouseTrap; }
        }

        public int HunterDelta
        {
            get { return Hunter - TypedPrevStatistic.Hunter; }
        }

        public int SinaiDelta
        {
            get { return Sinai - TypedPrevStatistic.Sinai; }
        }

        public int PattonValleyDelta
        {
            get { return PattonValley - TypedPrevStatistic.PattonValley; }
        }

        public int HuntsmanDelta
        {
            get { return Huntsman - TypedPrevStatistic.Huntsman; }
        }

        public int KayDelta
        {
            get { return Kay - TypedPrevStatistic.Kay; }
        }

        public int CariusDelta
        {
            get { return Carius - TypedPrevStatistic.Carius; }
        }

        public int KnispelDelta
        {
            get { return Knispel - TypedPrevStatistic.Knispel; }
        }

        public int PoppelDelta
        {
            get { return Poppel - TypedPrevStatistic.Poppel; }
        }

        public int AbramsDelta
        {
            get { return Abrams - TypedPrevStatistic.Abrams; }
        }

        public int LeclerkDelta
        {
            get { return Leclerk - TypedPrevStatistic.Leclerk; }
        }

        public int LavrinenkoDelta
        {
            get { return Lavrinenko - TypedPrevStatistic.Lavrinenko; }
        }

        public int EkinsDelta
        {
            get { return Ekins - TypedPrevStatistic.Ekins; }
        }

        public int ReaperLongestDelta
        {
            get { return ReaperLongest - TypedPrevStatistic.ReaperLongest; }
        }

        public int ReaperProgressDelta
        {
            get { return ReaperProgress - TypedPrevStatistic.ReaperProgress; }
        }

        public int SharpshooterLongestDelta
        {
            get { return SharpshooterLongest - TypedPrevStatistic.SharpshooterLongest; }
        }

        public int SharpshooterProgressDelta
        {
            get { return SharpshooterProgress - TypedPrevStatistic.SharpshooterProgress; }
        }

        public int MasterGunnerLongestDelta
        {
            get { return MasterGunnerLongest - TypedPrevStatistic.MasterGunnerLongest; }
        }

        public int MasterGunnerProgressDelta
        {
            get { return MasterGunnerProgress - TypedPrevStatistic.MasterGunnerProgress; }
        }

        public int InvincibleLongestDelta
        {
            get { return InvincibleLongest - TypedPrevStatistic.InvincibleLongest; }
        }

        public int InvincibleProgressDelta
        {
            get { return InvincibleProgress - TypedPrevStatistic.InvincibleProgress; }
        }

        public int SurvivorLongestDelta
        {
            get { return SurvivorLongest - TypedPrevStatistic.SurvivorLongest; }
        }

        public int SurvivorProgressDelta
        {
            get { return SurvivorProgress - TypedPrevStatistic.SurvivorProgress; }
        }

        public double DamageRatioDelta
        {
            get { return DamageRatio - TypedPrevStatistic.DamageRatio; }
        }

        public int MedalMonolithDelta
        {
            get { return MedalMonolith - TypedPrevStatistic.MedalMonolith; }
        }

        public int MedalAntiSpgFireDelta
        {
            get { return MedalAntiSpgFire - TypedPrevStatistic.MedalAntiSpgFire; }
        }

        public int MedalGoreDelta
        {
            get { return MedalGore - TypedPrevStatistic.MedalGore; }
        }

        public int MedalCoolBloodDelta
        {
            get { return MedalCoolBlood - TypedPrevStatistic.MedalCoolBlood; }
        }

        public int MedalStarkDelta
        {
            get { return MedalStark - TypedPrevStatistic.MedalStark; }
        }

        public int DamageRatingDelta
        {
            get { return DamageRating - TypedPrevStatistic.DamageRating; }
        }

        #endregion

        #region Achievments 7x7

        public int KingOfTheHillDelta
        {
            get { return KingOfTheHill - TypedPrevStatistic.KingOfTheHill; }
        }

        public int ArmoredFistDelta
        {
            get { return ArmoredFist - TypedPrevStatistic.ArmoredFist; }
        }

        public int TacticalBreakthroughDelta
        {
            get { return TacticalBreakthrough - TypedPrevStatistic.TacticalBreakthrough; }
        }

        public int CrucialShotDelta
        {
            get { return CrucialShot - TypedPrevStatistic.CrucialShot; }
        }

        public int CrucialShotMedalDelta
        {
            get { return CrucialShotMedal - TypedPrevStatistic.CrucialShotMedal; }
        }

        public int FightingReconnaissanceMedalDelta
        {
            get { return FightingReconnaissanceMedal - TypedPrevStatistic.FightingReconnaissanceMedal; }
        }

        public int ForTacticalOperationsDelta
        {
            get { return ForTacticalOperations - TypedPrevStatistic.ForTacticalOperations; }
        }

        public int GeniusForWarMedalDelta
        {
            get { return GeniusForWarMedal - TypedPrevStatistic.GeniusForWarMedal; }
        }

        public int GodOfWarDelta
        {
            get { return GodOfWar - TypedPrevStatistic.GodOfWar; }
        }

        public int WillToWinSpiritDelta
        {
            get { return WillToWinSpirit - TypedPrevStatistic.WillToWinSpirit; }
        }

        public int WolfAmongSheepMedalDelta
        {
            get { return WolfAmongSheepMedal - TypedPrevStatistic.WolfAmongSheepMedal; }
        }

        public int NoMansLandDelta
        {
            get { return NoMansLand - TypedPrevStatistic.NoMansLand; }
        }

        public int PyromaniacMedalDelta
        {
            get { return PyromaniacMedal - TypedPrevStatistic.PyromaniacMedal; }
        }

        public int FireAndSteelMedalDelta
        {
            get { return FireAndSteel - TypedPrevStatistic.FireAndSteel; }
        }

        public int RangerMedalDelta
        {
            get { return RangerMedal - TypedPrevStatistic.RangerMedal; }
        }

        public int HeavyFireMedalDelta
        {
            get { return HeavyFireMedal - TypedPrevStatistic.HeavyFireMedal; }
        }

        public int PromisingFighterMedalDelta
        {
            get { return PromisingFighterMedal - TypedPrevStatistic.PromisingFighterMedal; }
        }

        #endregion

        #region Achievments  Historical

        public int GuardsManDelta
        {
            get { return GuardsMan - TypedPrevStatistic.GuardsMan; }
        }

        public int MakerOfHistoryDelta
        {
            get { return MakerOfHistory - TypedPrevStatistic.MakerOfHistory; }
        }

        public int BothSidesWinsDelta
        {
            get { return BothSidesWins - TypedPrevStatistic.BothSidesWins; }
        }

        public int WeakVehiclesWinsDelta
        {
            get { return WeakVehiclesWins - TypedPrevStatistic.WeakVehiclesWins; }
        }

        #endregion

        #endregion
    }
}