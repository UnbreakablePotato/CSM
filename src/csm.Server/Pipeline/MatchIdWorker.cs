namespace csm.Server.Pipeline
{
    /*
     MatchIdWorker should pop from the channel with puuids in it, then call RiotService to get 10 match ids for that puuid.
     It should then add those match ids to a channel for the next worker to pop from.
     */
    public class MatchIdWorker : BackgroundService
    {
        private readonly SharedQueue _sharedQueue;

        public MatchIdWorker(SharedQueue sharedQueue)
        {
            _sharedQueue = sharedQueue;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
