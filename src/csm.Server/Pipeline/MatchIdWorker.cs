using csm.Server.Riot;

namespace csm.Server.Pipeline
{
    /*
     MatchIdWorker should pop from the channel with puuids in it, then call RiotService to get 10 match ids for that puuid.
     It should then add those match ids to a channel for the next worker to pop from.
     */
    public class MatchIdWorker : BackgroundService
    {
        private readonly SharedQueue _sharedQueue;
        private readonly RiotService _riotService;

        public MatchIdWorker(SharedQueue sharedQueue, RiotService riotService)
        {
            _sharedQueue = sharedQueue;
            _riotService = riotService;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var puuid = await _sharedQueue.PuuidsChannel.Reader.ReadAsync(stoppingToken);
                    var matchIds = await _riotService.GetMatchIds("euw1",puuid);

                    if (matchIds == null || matchIds.Count == 0)
                    {
                        Console.WriteLine($"No match ids found for puuid: {puuid}");
                        continue;
                    }

                    foreach (var matchId in matchIds)
                    {
                        if (_sharedQueue.VisitedMatches.ContainsKey(matchId))
                        {
                            Console.WriteLine($"MatchId: {matchId} already exists in visited matches. Skipping.");
                            continue;
                        }

                        await _sharedQueue.MatchIdsChannel.Writer.WriteAsync(matchId, stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error occurred while executing MatchIdWorker service: {ex.Message}");
                }
            }
        }
    }
}
