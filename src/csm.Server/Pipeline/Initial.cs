using csm.Server.Riot;

namespace csm.Server.Pipeline
{
    /*
     Initial will call RiotService to call ladder. It needs to add every puuid to a channel(mby singleton type thing)
     
    have a sleep timer that will call ladder only once every 24 hours. The rest of the time it will just be popping from the channel and calling MatchIdWorker
     */
    public class Initial : BackgroundService
    {
        private readonly SharedQueue _sharedQueue;
        private readonly RiotService _riotService;

        private readonly IConfiguration _configuration;

        public Initial(SharedQueue sharedQueue, RiotService riotService, IConfiguration configuration)
        {
            _sharedQueue = sharedQueue;
            _riotService = riotService;
            _configuration = configuration;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var regions = _configuration
                    .GetSection("Pipeline:Regions")
                    .Get<string[]>() ?? [];

            while (!stoppingToken.IsCancellationRequested)
            {
                _sharedQueue.ResetForNewCycle();

                foreach (var region in regions)
                {
                    try
                    {
                        var ladder = await _riotService.GetLadder(region);

                        if (ladder == null || ladder.Entries == null)
                        {
                            continue;
                        }

                        foreach (var entry in ladder.Entries)
                        {
                            if (!string.IsNullOrEmpty(entry.Puuid) && _sharedQueue.VisitedPuuids.TryAdd(entry.Puuid, true))
                            {
                                await _sharedQueue.PlayerChannel.Writer.WriteAsync(new Player(region, entry.Puuid), stoppingToken);
                            }
                        }
                    }
                    catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                    {
                        return;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error occurred while executing Initial service: {ex.Message}");
                    }
                }
                
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}
