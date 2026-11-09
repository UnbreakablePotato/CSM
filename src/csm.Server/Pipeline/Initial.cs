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

        public Initial(SharedQueue sharedQueue, RiotService riotService)
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
                    var ladder = await _riotService.GetLadder("euw1");

                    foreach (var entry in ladder.Entries)
                    {
                        await _sharedQueue.PuuidsChannel.Writer.WriteAsync(entry.Puuid, stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error occurred while executing Initial service: {ex.Message}");
                }
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}
