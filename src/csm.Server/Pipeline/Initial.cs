namespace csm.Server.Pipeline
{
    /*
     Initial will call RiotService to call ladder. It needs to add every puuid to a channel(mby singleton type thing)
     
    have a sleep timer that will call ladder only once every 24 hours. The rest of the time it will just be popping from the channel and calling MatchIdWorker
     */
    public class Initial : BackgroundService
    {
        private readonly SharedQueue _sharedQueue;

        public Initial(SharedQueue sharedQueue)
        {
            _sharedQueue = sharedQueue;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
        }
    }
}
