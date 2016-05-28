using Microsoft.EntityFrameworkCore;

namespace WotDossier.Web
{
    public partial class dossierContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<dbversion>(entity =>
            {
                entity.Property(e => e.id).ValueGeneratedNever();

                entity.Property(e => e.schemaversion)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnType("varchar");
            });

            modelBuilder.Entity<historicalbattlesachievements>(entity =>
            {
                entity.HasKey(e => e.uid);

                entity.Property(e => e.uid).ValueGeneratedNever();

                entity.Property(e => e.rev).HasDefaultValueSql("2015122300");
            });

            modelBuilder.Entity<historicalbattlesstatistic>(entity =>
            {
                entity.HasKey(e => e.uid);

                entity.Property(e => e.uid).ValueGeneratedNever();

                entity.Property(e => e.damagetaken).HasDefaultValueSql("0");

                entity.Property(e => e.markofmastery).HasDefaultValueSql("0");

                entity.Property(e => e.maxdamage).HasDefaultValueSql("0");

                entity.Property(e => e.maxfrags).HasDefaultValueSql("0");

                entity.Property(e => e.performancerating).HasDefaultValueSql("0");

                entity.Property(e => e.rbr).HasDefaultValueSql("0");

                entity.Property(e => e.rev).HasDefaultValueSql("2015122300");

                entity.Property(e => e.wn8rating).HasDefaultValueSql("0");

                entity.HasOne(d => d.achievementsuidNavigation).WithMany(p => p.historicalbattlesstatistic).HasForeignKey(d => d.achievementsuid);

                entity.HasOne(d => d.playeruidNavigation).WithMany(p => p.historicalbattlesstatistic).HasForeignKey(d => d.playeruid);
            });

            modelBuilder.Entity<player>(entity =>
            {
                entity.HasKey(e => e.uid);

                entity.Property(e => e.uid).ValueGeneratedNever();

                entity.Property(e => e.name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("varchar");

                entity.Property(e => e.rev).HasDefaultValueSql("2015122300");

                entity.Property(e => e.server)
                    .HasMaxLength(5)
                    .HasColumnType("varchar");
            });

            modelBuilder.Entity<randombattlesachievements>(entity =>
            {
                entity.HasKey(e => e.uid);

                entity.Property(e => e.playeruid).IsRequired();

                entity.Property(e => e.uid).ValueGeneratedNever();

                entity.Property(e => e.arsonist).HasDefaultValueSql("0");

                entity.Property(e => e.bonecrusher).HasDefaultValueSql("0");

                entity.Property(e => e.charmed).HasDefaultValueSql("0");

                entity.Property(e => e.demolition).HasDefaultValueSql("0");

                entity.Property(e => e.duelist).HasDefaultValueSql("0");

                entity.Property(e => e.even).HasDefaultValueSql("0");

                entity.Property(e => e.fighter).HasDefaultValueSql("0");

                entity.Property(e => e.impenetrable).HasDefaultValueSql("0");

                entity.Property(e => e.maingun).HasDefaultValueSql("0");

                entity.Property(e => e.marksongun).HasDefaultValueSql("0");

                entity.Property(e => e.maxaimerseries).HasDefaultValueSql("0");

                entity.Property(e => e.medalantispgfire).HasDefaultValueSql("0");

                entity.Property(e => e.medalcoolblood).HasDefaultValueSql("0");

                entity.Property(e => e.medalgore).HasDefaultValueSql("0");

                entity.Property(e => e.medalmonolith).HasDefaultValueSql("0");

                entity.Property(e => e.medalstark).HasDefaultValueSql("0");

                entity.Property(e => e.movingavgdamage).HasDefaultValueSql("0");

                entity.Property(e => e.rev).HasDefaultValueSql("2015122300");

                entity.Property(e => e.shoottokill).HasDefaultValueSql("0");

                entity.Property(e => e.sniper2).HasDefaultValueSql("0");
            });

            modelBuilder.Entity<randombattlesstatistic>(entity =>
            {
                entity.HasKey(e => e.uid);

                entity.Property(e => e.uid).ValueGeneratedNever();

                entity.Property(e => e.damagetaken).HasDefaultValueSql("0");

                entity.Property(e => e.markofmastery).HasDefaultValueSql("0");

                entity.Property(e => e.maxdamage).HasDefaultValueSql("0");

                entity.Property(e => e.maxfrags).HasDefaultValueSql("0");

                entity.Property(e => e.performancerating).HasDefaultValueSql("0");

                entity.Property(e => e.rbr).HasDefaultValueSql("0");

                entity.Property(e => e.rev).HasDefaultValueSql("2015122300");

                entity.Property(e => e.wn8rating).HasDefaultValueSql("0");

                entity.HasOne(d => d.achievementsuidNavigation).WithMany(p => p.randombattlesstatistic).HasForeignKey(d => d.achievementsuid);

                entity.HasOne(d => d.playeruidNavigation).WithMany(p => p.randombattlesstatistic).HasForeignKey(d => d.playeruid);
            });

            modelBuilder.Entity<replay>(entity =>
            {
                entity.HasKey(e => e.uid);

                entity.Property(e => e.uid).ValueGeneratedNever();

                entity.Property(e => e.link)
                    .HasMaxLength(256)
                    .HasColumnType("varchar");

                entity.Property(e => e.rev).HasDefaultValueSql("2015122300");
            });

            modelBuilder.Entity<tank>(entity =>
            {
                entity.HasKey(e => e.uid);

                entity.Property(e => e.uid).ValueGeneratedNever();

                entity.Property(e => e.icon)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnType("varchar");

                entity.Property(e => e.name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("varchar");

                entity.Property(e => e.rev).HasDefaultValueSql("2015122300");

                entity.HasOne(d => d.playeruidNavigation).WithMany(p => p.tank).HasForeignKey(d => d.playeruid);
            });

            modelBuilder.Entity<tankhistoricalbattlestatistic>(entity =>
            {
                entity.HasKey(e => e.uid);

                entity.Property(e => e.uid).ValueGeneratedNever();

                entity.Property(e => e.battlescount).HasDefaultValueSql("0");

                entity.Property(e => e.raw).IsRequired();

                entity.Property(e => e.rev).HasDefaultValueSql("2015122300");

                entity.HasOne(d => d.tankuidNavigation).WithMany(p => p.tankhistoricalbattlestatistic).HasForeignKey(d => d.tankuid);
            });

            modelBuilder.Entity<tankrandombattlesstatistic>(entity =>
            {
                entity.HasKey(e => e.uid);

                entity.Property(e => e.uid).ValueGeneratedNever();

                entity.Property(e => e.battlescount).HasDefaultValueSql("0");

                entity.Property(e => e.raw).IsRequired();

                entity.Property(e => e.playeruid).IsRequired();

                entity.Property(e => e.rev).HasDefaultValueSql("2015122300");

                entity.HasOne(d => d.tankuidNavigation).WithMany(p => p.tankrandombattlesstatistic).HasForeignKey(d => d.tankuid);
            });

            modelBuilder.Entity<tankteambattlestatistic>(entity =>
            {
                entity.HasKey(e => e.uid);

                entity.Property(e => e.uid).ValueGeneratedNever();

                entity.Property(e => e.battlescount).HasDefaultValueSql("0");

                entity.Property(e => e.raw).IsRequired();

                entity.Property(e => e.rev).HasDefaultValueSql("2015122300");

                entity.HasOne(d => d.tankuidNavigation).WithMany(p => p.tankteambattlestatistic).HasForeignKey(d => d.tankuid);
            });

            modelBuilder.Entity<teambattlesachievements>(entity =>
            {
                entity.HasKey(e => e.uid);

                entity.Property(e => e.uid).ValueGeneratedNever();

                entity.Property(e => e.crucialshot).HasDefaultValueSql("0");

                entity.Property(e => e.crucialshotmedal).HasDefaultValueSql("0");

                entity.Property(e => e.fightingreconnaissance).HasDefaultValueSql("0");

                entity.Property(e => e.fightingreconnaissancemedal).HasDefaultValueSql("0");

                entity.Property(e => e.fireandsteel).HasDefaultValueSql("0");

                entity.Property(e => e.fireandsteelmedal).HasDefaultValueSql("0");

                entity.Property(e => e.fortacticaloperations).HasDefaultValueSql("0");

                entity.Property(e => e.godofwar).HasDefaultValueSql("0");

                entity.Property(e => e.heavyfire).HasDefaultValueSql("0");

                entity.Property(e => e.heavyfiremedal).HasDefaultValueSql("0");

                entity.Property(e => e.nomansland).HasDefaultValueSql("0");

                entity.Property(e => e.promisingfighter).HasDefaultValueSql("0");

                entity.Property(e => e.promisingfightermedal).HasDefaultValueSql("0");

                entity.Property(e => e.pyromaniac).HasDefaultValueSql("0");

                entity.Property(e => e.pyromaniacmedal).HasDefaultValueSql("0");

                entity.Property(e => e.ranger).HasDefaultValueSql("0");

                entity.Property(e => e.rangermedal).HasDefaultValueSql("0");

                entity.Property(e => e.rev).HasDefaultValueSql("2015122300");

                entity.Property(e => e.willtowinspirit).HasDefaultValueSql("0");
            });

            modelBuilder.Entity<teambattlesstatistic>(entity =>
            {
                entity.HasKey(e => e.uid);

                entity.Property(e => e.uid).ValueGeneratedNever();

                entity.Property(e => e.damagetaken).HasDefaultValueSql("0");

                entity.Property(e => e.markofmastery).HasDefaultValueSql("0");

                entity.Property(e => e.maxdamage).HasDefaultValueSql("0");

                entity.Property(e => e.maxfrags).HasDefaultValueSql("0");

                entity.Property(e => e.performancerating).HasDefaultValueSql("0");

                entity.Property(e => e.rbr).HasDefaultValueSql("0");

                entity.Property(e => e.rev).HasDefaultValueSql("2015122300");

                entity.Property(e => e.wn8rating).HasDefaultValueSql("0");

                entity.HasOne(d => d.achievementsuidNavigation).WithMany(p => p.teambattlesstatistic).HasForeignKey(d => d.achievementsuid);

                entity.HasOne(d => d.playeruidNavigation).WithMany(p => p.teambattlesstatistic).HasForeignKey(d => d.playeruid);
            });
        }

        public virtual DbSet<dbversion> dbversion { get; set; }
        public virtual DbSet<historicalbattlesachievements> historicalbattlesachievements { get; set; }
        public virtual DbSet<historicalbattlesstatistic> historicalbattlesstatistic { get; set; }
        public virtual DbSet<player> player { get; set; }
        public virtual DbSet<randombattlesachievements> randombattlesachievements { get; set; }
        public virtual DbSet<randombattlesstatistic> randombattlesstatistic { get; set; }
        public virtual DbSet<replay> replay { get; set; }
        public virtual DbSet<tank> tank { get; set; }
        public virtual DbSet<tankhistoricalbattlestatistic> tankhistoricalbattlestatistic { get; set; }
        public virtual DbSet<tankrandombattlesstatistic> tankrandombattlesstatistic { get; set; }
        public virtual DbSet<tankteambattlestatistic> tankteambattlestatistic { get; set; }
        public virtual DbSet<teambattlesachievements> teambattlesachievements { get; set; }
        public virtual DbSet<teambattlesstatistic> teambattlesstatistic { get; set; }
    }
}