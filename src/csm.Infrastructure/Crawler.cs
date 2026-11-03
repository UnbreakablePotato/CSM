using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace csm.Infrastructure
{


    public class Crawler : BackgroundService
    {
        private readonly ILogger<Crawler> _logger;

        public Crawler(ILogger<Crawler> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stopToken)
        {
            _logger.LogInformation("Crawler is starting");

            while (!stopToken.IsCancellationRequested)
            {
                _logger.LogInformation("Crawler is working...");

                //await SomeWork();
            }

            _logger.LogInformation("Crawler has stopped work...");
        }
