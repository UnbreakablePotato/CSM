using csm.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace csm.Server.Services
{
    public class TierlistService
    {
        private readonly AppDbContext _context;

        public TierlistService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Models.Build>> GetTierlistAsync()
        {
            var tierlist = await _context.Builds
                .AsNoTracking()
                .GroupBy(x => x.ChampionName)
                .Select(y => y.OrderByDescending(x => x.Winrate).First())
                .OrderByDescending(x => x.Winrate)
                .ToListAsync();

            return tierlist;
        }

        public async Task<List<Models.Build>> GetSpecificTierlistAsync(string server)
        {
            var tierlist = await _context.Builds
                .AsNoTracking()
                .ToListAsync();

            return tierlist;
        }
    }
}
