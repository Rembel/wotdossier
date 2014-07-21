using System.Runtime.Serialization;
using WotDossier.Domain.Interfaces;

namespace WotDossier.Domain.Tank
{
    [DataContract]
    public class AchievementsJson : IRandomBattlesAchievements
    {
        [DataMember(Name = "alaric")]
        public int Alaric { get; set; }

        [DataMember(Name = "armorPiercer")]
        public int MasterGunner { get; set; }

        [DataMember(Name = "battleHeroes")]
        public int BattleHero { get; set; }

        [DataMember(Name = "beasthunter")]
        public int Hunter { get; set; }

        [DataMember(Name = "bombardier")]
        public int Bombardier { get; set; }

        [DataMember(Name = "defender")]
        public int Defender { get; set; }

        [DataMember(Name = "diehard")]
        public int Survivor { get; set; }

        [DataMember(Name = "diehardSeries")]
        public int SurvivorProgress { get; set; }

        [DataMember(Name = "evileye")]
        public int PatrolDuty { get; set; }

        [DataMember(Name = "fragsBeast")]
        public int FragsBeast { get; set; }

        [DataMember(Name = "fragsPatton")]
        public int FragsPatton { get; set; }

        [DataMember(Name = "fragsSinai")]
        public int FragsSinai { get; set; }

        [DataMember(Name = "handOfDeath")]
        public int Reaper { get; set; }

        [DataMember(Name = "heroesOfRassenay")]
        public int HeroesOfRassenay { get; set; }

        [DataMember(Name = "huntsman")]
        public int Huntsman { get; set; }

        [DataMember(Name = "invader")]
        public int Invader { get; set; }

        [DataMember(Name = "invincible")]
        public int Invincible { get; set; }

        [DataMember(Name = "invincibleSeries")]
        public int InvincibleProgress { get; set; }

        [DataMember(Name = "ironMan")]
        public int IronMan { get; set; }

        [DataMember(Name = "kamikaze")]
        public int Kamikaze { get; set; }

        [DataMember(Name = "killingSeries")]
        public int ReaperProgress { get; set; }

        [DataMember(Name = "luckyDevil")]
        public int LuckyDevil { get; set; }

        [DataMember(Name = "lumberjack")]
        public int Lumberjack { get; set; }

        [DataMember(Name = "maxDiehardSeries")]
        public int SurvivorLongest { get; set; }

        [DataMember(Name = "maxInvincibleSeries")]
        public int InvincibleLongest { get; set; }

        [DataMember(Name = "maxKillingSeries")]
        public int ReaperLongest { get; set; }

        [DataMember(Name = "maxPiercingSeries")]
        public int MasterGunnerLongest { get; set; }

        [DataMember(Name = "maxSniperSeries")]
        public int SharpshooterLongest { get; set; }

        [DataMember(Name = "medalAbrams")]
        public int Abrams { get; set; }

        [DataMember(Name = "medalBillotte")]
        public int Billotte { get; set; }

        [DataMember(Name = "medalBrothersInArms")]
        public int BrothersInArms { get; set; }

        [DataMember(Name = "medalBrunoPietro")]
        public int BrunoPietro { get; set; }

        [DataMember(Name = "medalBurda")]
        public int Burda { get; set; }

        [DataMember(Name = "medalCarius")]
        public int Carius { get; set; }

        [DataMember(Name = "medalCrucialContribution")]
        public int CrucialContribution { get; set; }

        [DataMember(Name = "medalDeLanglade")]
        public int DeLanglade { get; set; }

        [DataMember(Name = "medalDumitru")]
        public int Dumitru { get; set; }

        [DataMember(Name = "medalEkins")]
        public int Ekins { get; set; }

        [DataMember(Name = "medalFadin")]
        public int Fadin { get; set; }

        [DataMember(Name = "medalHalonen")]
        public int Halonen { get; set; }

        [DataMember(Name = "medalKay")]
        public int Kay { get; set; }

        [DataMember(Name = "medalKnispel")]
        public int Knispel { get; set; }

        [DataMember(Name = "medalKolobanov")]
        public int Kolobanov { get; set; }

        [DataMember(Name = "medalLafayettePool")]
        public int LafayettePool { get; set; }

        [DataMember(Name = "medalLavrinenko")]
        public int Lavrinenko { get; set; }

        [DataMember(Name = "medalLeClerc")]
        public int Leclerk { get; set; }

        [DataMember(Name = "medalLehvaslaiho")]
        public int Lehvaslaiho { get; set; }

        [DataMember(Name = "medalNikolas")]
        public int Nikolas { get; set; }

        [DataMember(Name = "medalOrlik")]
        public int Orlik { get; set; }

        [DataMember(Name = "medalOskin")]
        public int Oskin { get; set; }

        [DataMember(Name = "medalPascucci")]
        public int Pascucci { get; set; }

        [DataMember(Name = "medalPoppel")]
        public int Poppel { get; set; }

        [DataMember(Name = "medalRadleyWalters")]
        public int RadleyWalters { get; set; }

        [DataMember(Name = "medalTamadaYoshio")]
        public int TamadaYoshio { get; set; }

        [DataMember(Name = "medalTarczay")]
        public int Tarczay { get; set; }

        [DataMember(Name = "medalWittmann")]
        public int Boelter { get; set; }

        [DataMember(Name = "mousebane")]
        public int MouseTrap { get; set; }

        [DataMember(Name = "pattonValley")]
        public int PattonValley { get; set; }

        [DataMember(Name = "piercingSeries")]
        public int MasterGunnerProgress { get; set; }

        [DataMember(Name = "raider")]
        public int Raider { get; set; }

        [DataMember(Name = "scout")]
        public int Scout { get; set; }

        [DataMember(Name = "sinai")]
        public int Sinai { get; set; }

        [DataMember(Name = "sniper")]
        public int Sniper { get; set; }

        [DataMember(Name = "sniper2")]
        public int Sniper2 { get; set; }

        [DataMember(Name = "mainGun")]
        public int MainGun { get; set; }

        [DataMember(Name = "sniperSeries")]
        public int SharpshooterProgress { get; set; }

        [DataMember(Name = "steelWall")]
        public int SteelWall { get; set; }

        [DataMember(Name = "sturdy")]
        public int Sturdy { get; set; }

        [DataMember(Name = "supporter")]
        public int Confederate { get; set; }

        [DataMember(Name = "tankExpertStrg")]
        public int TankExpertStrg { get; set; }

        [DataMember(Name = "titleSniper")]
        public int Sharpshooter { get; set; }

        [DataMember(Name = "warrior")]
        public int Warrior { get; set; }

        [DataMember(Name = "markOfMastery")]
        public int MarkOfMastery { get; set; }

        //0.9.1
        [DataMember(Name = "marksOnGun")]
        public int MarksOnGun { get; set; }

        [DataMember(Name = "movingAvgDamage")]
        public int MovingAvgDamage { get; set; }

        [DataMember(Name = "medalMonolith")]
        public int MedalMonolith { get; set; }

        [DataMember(Name = "medalAntiSpgFire")]
        public int MedalAntiSpgFire { get; set; }

        [DataMember(Name = "medalGore")]
        public int MedalGore { get; set; }

        [DataMember(Name = "medalCoolBlood")]
        public int MedalCoolBlood { get; set; }

        [DataMember(Name = "medalStark")]
        public int MedalStark { get; set; }

        [DataMember(Name = "damageRating")]
        public int DamageRating { get; set; }
    }
}