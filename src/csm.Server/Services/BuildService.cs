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

        public async Task<Models.Build> GetBuildAsync(string champion, string tier)
        {
            var build = await _context.Builds
                .AsNoTracking()
                .Where(x => x.Tier == tier && x.ChampionName == champion)
                .OrderByDescending(x => x.GamesPlayed)
                .ThenByDescending(x => x.Winrate)
                .ToListAsync();

            return build[0];
        }
    }
}
