using csm.Server.Riot;

namespace csm.Server.Pipeline
{
    /*
     Pops from the channel with match ids in it, then calls RiotService to get the match data for that match id.
     Then adds the match data object to a new channel that goes to the DBWorker
     */
    public class MatchDataWorker : BackgroundService
    {
        private readonly SharedQueue _sharedQueue;

        private readonly RiotService _riotService;

        public MatchDataWorker(SharedQueue sharedQueue, RiotService riotService)
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
                    var match = await _sharedQueue.MatchIdsChannel.Reader.ReadAsync(stoppingToken);
                    var matchData = await _riotService.GetMatchData(match.region, match.matchid);
                    if (matchData == null)
                    {
                        continue;
                    }

                    foreach (var puuid in matchData.Metadata.Participants)
                    {
                        if (!string.IsNullOrEmpty(puuid) && _sharedQueue.VisitedPuuids.TryAdd(puuid, true))
                        {
                            await _sharedQueue.PlayerChannel.Writer.WriteAsync(new Player(match.region, puuid), stoppingToken);
                        }
                    }

                    await _sharedQueue.MatchDataChannel.Writer.WriteAsync(new MatchData(match.region, matchData), stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error occurred while executing MatchDataWorker service: {ex.Message}");
                }
            }
        }
    }
}
