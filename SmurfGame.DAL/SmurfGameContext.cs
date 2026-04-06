using Microsoft.EntityFrameworkCore;
using SmurfGame.BL.Entities;

namespace SmurfGame.DAL
{
    public class SmurfGameContext : DbContext
    {
        // Constructeur
        public SmurfGameContext(DbContextOptions<SmurfGameContext> options) : base(options) { }

        // ── Tables ──────────────────────────────
        public DbSet<Creature> Creatures { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Smurf> Smurfs { get; set; }
        public DbSet<Bug> Bugs { get; set; }
        public DbSet<Spider> Spiders { get; set; }
        public DbSet<BzzFly> BzzFlies { get; set; }
        public DbSet<Berry> Berries { get; set; }
        public DbSet<RedPotion> RedPotions { get; set; }
        public DbSet<BluePotion> BluePotions { get; set; }
        public DbSet<SpeedBuff> SpeedBuffs { get; set; }
        public DbSet<Azrael> Azraels { get; set; }
        public DbSet<Coin> Coins { get; set; }
        // ── Connexion ───────────────────────────
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"server=(LocalDB)\MSSQLLocalDB;Initial Catalog=SmurfGameDB;Integrated Security=true"
            );
        }

        // ── Fluent API ──────────────────────────
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ── TPT : une table par classe ──────
            modelBuilder.Entity<Creature>().ToTable("Creatures");
            modelBuilder.Entity<Bug>().ToTable("Bugs");
            modelBuilder.Entity<Smurf>().ToTable("Smurfs");
            modelBuilder.Entity<Spider>().ToTable("Spiders");
            modelBuilder.Entity<BzzFly>().ToTable("BzzFlies");

            modelBuilder.Entity<Item>().ToTable("Items");
            modelBuilder.Entity<Berry>().ToTable("Berries");
            modelBuilder.Entity<RedPotion>().ToTable("RedPotions");
            modelBuilder.Entity<BluePotion>().ToTable("BluePotions");

            // ── Configuration Creature ──────────
            modelBuilder.Entity<Creature>(entity =>
            {
                entity.Property(c => c.Name)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(c => c.Health).IsRequired();
                entity.Property(c => c.MaxHealth).IsRequired();
            });

            // ── Configuration Smurf ─────────────
            modelBuilder.Entity<Smurf>(entity =>
            {
                entity.Property(s => s.Level).HasDefaultValue(1);
                entity.Property(s => s.IsInForest).HasDefaultValue(false);
            });

            // ── Configuration Item ──────────────
            modelBuilder.Entity<Item>(entity =>
            {
                entity.Property(i => i.HealthBoost).IsRequired();
                entity.Property(i => i.IsConsumed).HasDefaultValue(false);
            });

            // ── Configuration RedPotion ─────────
            modelBuilder.Entity<RedPotion>(entity =>
            {
                entity.Property(r => r.BoostMultiplier).HasDefaultValue(2);
            });

            // ── Index sur les coordonnées ────────
            modelBuilder.Entity<Creature>()
                .HasIndex(c => new { c.X, c.Y })
                .HasDatabaseName("IX_Creatures_Position");

            modelBuilder.Entity<Item>()
                .HasIndex(i => new { i.X, i.Y })
                .HasDatabaseName("IX_Items_Position");
        }
    }
}