using Microsoft.EntityFrameworkCore;
using SmurfGame.BL.Entities;

namespace SmurfGame.DAL
{
    public class SmurfGameContext : DbContext
    {
        // Constructeur
        public SmurfGameContext(DbContextOptions<SmurfGameContext> options) : base(options) { }

        // ── Tables ──────────────────────────────
        public DbSet<Smurf> Smurfs { get; set; }

        // ── Connexion ───────────────────────────
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"server=(LocalDB)\MSSQLLocalDB;Initial Catalog=SmurfGameDB;Integrated Security=true;TrustServerCertificate=True",
                builder => builder.EnableRetryOnFailure()
            );
        }

        // ── Fluent API ──────────────────────────
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Smurf table with Table Per Hierarchy inheritance
            modelBuilder.Entity<Smurf>(entity =>
            {
                entity.ToTable("Smurfs");
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Id).ValueGeneratedOnAdd();

                // Map inherited properties from Creature
                entity.Property(s => s.X);
                entity.Property(s => s.Y);
                entity.Property(s => s.Speed);
                entity.Property(s => s.Health);
                entity.Property(s => s.MaxHealth);
                entity.Property(s => s.Size);

                // Map Smurf-specific properties
                entity.Property(s => s.Level);
                entity.Property(s => s.IsInForest);

                // Configure TPH discriminator
                entity.HasDiscriminator<string>("SmurfType")
                    .HasValue<PapaSmurf>("Papa")
                    .HasValue<LadySmurf>("Lady")
                    .HasValue<StrongSmurf>("Strong");
            });

            // Configure StrongSmurf-specific properties
            modelBuilder.Entity<StrongSmurf>(entity =>
            {
                entity.Property(s => s.CounterDamage);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
