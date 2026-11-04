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

    }
}
