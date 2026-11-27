using Microsoft.EntityFrameworkCore;

namespace csm.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        //public DbSet<Models.Build> Builds { get;set; }

        public DbSet<Models.Participant> Participants { get; set; }

        public DbSet<Models.Core> Cores { get; set; }

        public DbSet<Models.Build> Builds { get; set; }

        public DbSet<Models.Champion> Champions { get; set; }

        public DbSet<Models.Summoner> Summoners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Models.Summoner>(entity =>
            {
                entity.Property(x => x.Puuid).HasMaxLength(100);
                entity.Property(x => x.Server).HasMaxLength(16);
                entity.Property(x => x.GameName).HasMaxLength(64);
                entity.Property(x => x.TagLine).HasMaxLength(16);

                entity.HasIndex(x => x.Puuid).IsUnique();
                entity.HasIndex(x => new { x.Server, x.GameName, x.TagLine });
            });

            modelBuilder.Entity<Models.Participant>(entity =>
            {
                entity.Property(x => x.Puuid).HasMaxLength(100);
                entity.Property(x => x.MatchId).HasMaxLength(32);
                entity.HasIndex(x => x.Puuid);
                entity.HasIndex(x => x.MatchId);
            });
        }
    }
}
