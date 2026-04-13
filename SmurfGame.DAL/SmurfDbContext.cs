using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using SmurfGame.DAL.Entities;

namespace SmurfGame.DAL
{
    /// <summary>
    /// Entity Framework Core DbContext for SQLite-based score persistence.
    /// </summary>
    public class SmurfDbContext : DbContext
    {
        public DbSet<Score> Scores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Use SQLite database file in the application directory
            string dbPath = Path.Combine(AppContext.BaseDirectory, "smurfgame.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Score entity
            modelBuilder.Entity<Score>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.PlayerName)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(s => s.SmurfType)
                    .IsRequired()
                    .HasMaxLength(50);
                entity.Property(s => s.Points)
                    .IsRequired();
                entity.Property(s => s.PlayedAt)
                    .IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
