namespace csm.Server.Pipeline
{
    /*
     Pops from the channel with match ids in it, then calls RiotService to get the match data for that match id.
     Then adds the match data object to a new channel that goes to the DBWorker
     */
    public class MatchDataWorker : BackgroundService
    {
        private readonly SharedQueue _sharedQueue;

        public MatchDataWorker(SharedQueue sharedQueue)
        {
            _sharedQueue = sharedQueue;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
