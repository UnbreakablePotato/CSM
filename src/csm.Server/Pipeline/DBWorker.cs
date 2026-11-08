namespace csm.Server.Pipeline
{
    /*
     DBWorker should add build, core, and the remaining match data to the database.
     
     Might need to be a background service?
          
     */
    public class DBWorker : BackgroundService
    {
        private readonly SharedQueue _sharedQueue;

        public DBWorker(SharedQueue sharedQueue)
        {
            _sharedQueue = sharedQueue;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
