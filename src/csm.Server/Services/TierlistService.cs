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
                //.OrderByDescending(x => x.Winrate)
                .ToListAsync();

            return tierlist;
        }
    }
}
