using csm.Server.Data;
using csm.Server.DTO;
using csm.Server.Riot;
using Microsoft.EntityFrameworkCore;

namespace csm.Server.Services
{
    public class SummonerService
    {
        private readonly AppDbContext _context;

        public SummonerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<SummonerProfileDto?> GetSummonerProfileAsync(string server, string gameName, string tagLine, int matchCount = 10)
        {
            string platformServer = NormalizeServer(server);
            int clampedCount = Math.Clamp(matchCount, 1, 50);

            var summoner = await _context.Summoners
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Server == platformServer && x.GameName == gameName && x.TagLine == tagLine);

            if (summoner == null)
            {
                return null;
            }

            double kda = await GetOverallKdaAsync(summoner.Puuid);
            var mostPlayed = await GetMostPlayedChampionsAsync(summoner.Puuid, 6);
            var previousMatches = await GetRecentMatchesAsync(summoner.Puuid, clampedCount);

            int totalGames = summoner.Wins + summoner.Losses;
            double winrate = totalGames > 0
                ? Math.Round((double)summoner.Wins / totalGames * 100, 1)
                : 0;

            return new SummonerProfileDto
            {
                Server = summoner.Server,
                GameName = summoner.GameName,
                TagLine = summoner.TagLine,
                ProfileIcon = summoner.ProfileIcon,
                SummonerLevel = summoner.SummonerLevel,
                Tier = summoner.Tier,
                Rank = summoner.Rank,
                LeaguePoints = summoner.LeaguePoints,
                Wins = summoner.Wins,
                Losses = summoner.Losses,
                Winrate = winrate,
                Kda = kda,
                MostPlayedChampions = mostPlayed,
                PreviousMatches = previousMatches
            };
        }

        public async Task<double> GetOverallKdaAsync(string puuid)
        {
            var totals = await _context.Participants
                .AsNoTracking()
                .Where(x => x.Puuid == puuid)
                .GroupBy(x => x.Puuid)
                .Select(g => new
                {
                    Kills = g.Sum(x => x.Kills),
                    Deaths = g.Sum(x => x.Deaths),
                    Assists = g.Sum(x => x.Assists)
                })
                .FirstOrDefaultAsync();

            if (totals == null)
            {
                return 0;
            }

            return CalculateKda(totals.Kills, totals.Deaths, totals.Assists);
        }

        public async Task<List<MostPlayedChampionDto>> GetMostPlayedChampionsAsync(string puuid, int count = 6)
        {
            var grouped = await _context.Participants
                .AsNoTracking()
                .Where(x => x.Puuid == puuid)
                .GroupBy(x => new { x.ChampionId, x.ChampionName })
                .Select(g => new
                {
                    g.Key.ChampionId,
                    ChampionName = g.Key.ChampionName ?? string.Empty,
                    GamesPlayed = g.Count(),
                    Wins = g.Count(x => x.Win),
                    Losses = g.Count(x => !x.Win),
                    Kills = g.Sum(x => x.Kills),
                    Deaths = g.Sum(x => x.Deaths),
                    Assists = g.Sum(x => x.Assists)
                })
                .OrderByDescending(x => x.GamesPlayed)
                .ThenByDescending(x => x.Wins)
                .Take(count)
                .ToListAsync();

            var result = new List<MostPlayedChampionDto>();

            foreach (var champ in grouped)
            {
                double winrate = champ.GamesPlayed > 0
                    ? Math.Round((double)champ.Wins / champ.GamesPlayed * 100, 1)
                    : 0;

                result.Add(new MostPlayedChampionDto
                {
                    ChampionId = champ.ChampionId,
                    ChampionName = champ.ChampionName,
                    GamesPlayed = champ.GamesPlayed,
                    Wins = champ.Wins,
                    Losses = champ.Losses,
                    Winrate = winrate,
                    Kda = CalculateKda(champ.Kills, champ.Deaths, champ.Assists)
                });
            }

            return result;
        }

        public async Task<List<MatchSummaryDto>> GetRecentMatchesAsync(string puuid, int count = 10)
        {
            var matches = await _context.Participants
                .AsNoTracking()
                .Where(x => x.Puuid == puuid)
                .OrderByDescending(x => x.Id)
                .Take(count)
                .Select(x => new
                {
                    x.MatchId,
                    x.ChampionId,
                    x.ChampionName,
                    x.IndividualPosition,
                    x.Kills,
                    x.Deaths,
                    x.Assists,
                    x.Win,
                    Cs = x.TotalMinionsKilled + x.neutralMinionsKilled,
                    x.Item0,
                    x.Item1,
                    x.Item2,
                    x.Item3,
                    x.Item4,
                    x.Item5,
                    x.Item6,
                    x.SummonerSpell1Id,
                    x.SummonerSpell2Id
                })
                .ToListAsync();

            var result = new List<MatchSummaryDto>();

            foreach (var match in matches)
            {
                result.Add(new MatchSummaryDto
                {
                    MatchId = match.MatchId,
                    ChampionId = match.ChampionId,
                    ChampionName = match.ChampionName ?? string.Empty,
                    Position = match.IndividualPosition ?? string.Empty,
                    Kills = match.Kills,
                    Deaths = match.Deaths,
                    Assists = match.Assists,
                    Kda = CalculateKda(match.Kills, match.Deaths, match.Assists),
                    Win = match.Win,
                    Cs = match.Cs,
                    Items = new int[]
                    {
                        match.Item0,
                        match.Item1,
                        match.Item2,
                        match.Item3,
                        match.Item4,
                        match.Item5,
                        match.Item6
                    },
                    SummonerSpell1Id = match.SummonerSpell1Id,
                    SummonerSpell2Id = match.SummonerSpell2Id
                });
            }

            return result;
        }

        private static double CalculateKda(int kills, int deaths, int assists)
        {
            if (deaths == 0)
            {
                return kills + assists;
            }

            return Math.Round((double)(kills + assists) / deaths, 2);
        }

        private static string NormalizeServer(string server)
        {
            try
            {
                var route = RegionRouter.Resolve(server);
                return route.Platform.ToLowerInvariant();
            }
            catch
            {
                return server.Trim().ToLowerInvariant();
            }
        }
    }
}
