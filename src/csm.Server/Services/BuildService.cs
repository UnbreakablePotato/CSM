using csm.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace csm.Server.Services
{
    public class BuildService
    {
        private readonly AppDbContext _context;

        public BuildService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Models.Build> GetBuildAsync(string champion)
        {
            var build = await _context.Builds
                .Where(x => x.ChampionName == champion)
                .OrderByDescending(x => x.Winrate)
                .AsNoTracking()
                .ToListAsync();

            return build[0];
        }
    }
}
