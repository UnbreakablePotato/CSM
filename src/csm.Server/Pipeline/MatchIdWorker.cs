using csm.Server.Data;
using csm.Server.Riot;
using Microsoft.EntityFrameworkCore;

namespace csm.Server.Pipeline
{
    /*
     MatchIdWorker pops from PlayerChannel, fetches Ranked Solo/Duo match ids from the last 24 hours
     (paginated), deduplicates against memory and SQL Server, and pushes unseen match ids to MatchIdsChannel.
     */
    public class MatchIdWorker : BackgroundService
    {
        private readonly SharedQueue _sharedQueue;
        private readonly RiotService _riotService;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public MatchIdWorker(SharedQueue sharedQueue, RiotService riotService, IServiceScopeFactory serviceScopeFactory)
        {
            _sharedQueue = sharedQueue;
            _riotService = riotService;
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var player = await _sharedQueue.PlayerChannel.Reader.ReadAsync(stoppingToken);
                    long startTime = _sharedQueue.CycleStartTimeUnixSeconds;
                    int start = 0;
                    const int pageSize = 100;

                    while (!stoppingToken.IsCancellationRequested)
                    {
                        var matchIds = await _riotService.GetMatchIds(player.region, player.puuid, startTime, start, pageSize);

                        if (matchIds == null || matchIds.Count == 0)
                        {
                            break;
                        }

                        var candidateIds = new List<string>();

                        foreach (var matchId in matchIds)
                        {
                            if (_sharedQueue.VisitedMatches.TryAdd(matchId, true))
                            {
                                candidateIds.Add(matchId);
                            }
                        }

                        if (candidateIds.Count > 0)
                        {
                            using var scope = _serviceScopeFactory.CreateScope();
                            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                            var existingMatchIds = await dbContext.Participants
                                .AsNoTracking()
                                .Where(x => candidateIds.Contains(x.MatchId))
                                .Select(x => x.MatchId)
                                .Distinct()
                                .ToListAsync(stoppingToken);

                            var existingSet = new HashSet<string>(existingMatchIds);

                            foreach (var matchId in candidateIds)
                            {
                                if (existingSet.Contains(matchId))
                                {
                                    continue;
                                }

                                await _sharedQueue.MatchIdsChannel.Writer.WriteAsync(new Match(player.region, matchId), stoppingToken);
                            }
                        }

                        if (matchIds.Count < pageSize)
                        {
                            break;
                        }

                        start += pageSize;
                    }
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error occurred while executing MatchIdWorker service: {ex.Message}");
                }
            }
        }
    }
}
