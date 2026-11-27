using csm.Server.DTO;

namespace csm.Server.Riot
{
    public class RiotService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly RiotRateLimiter _rateLimiter;
        readonly string? api_key = Environment.GetEnvironmentVariable("RIOT_API_KEY");

        public RiotService(IHttpClientFactory httpClientFactory, RiotRateLimiter rateLimiter)
        {
            _httpClientFactory = httpClientFactory;
            _rateLimiter = rateLimiter;
        }

        private async Task<HttpResponseMessage> SendGetAsync(string url)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            int maxRetries = 5;

            for (int attempt = 0; attempt < maxRetries; attempt++)
            {
                await _rateLimiter.WaitIfNeededAsync();

                var response = await client.GetAsync(url);

                if (_rateLimiter.HandleRateLimitHeaders(response))
                {
                    response.Dispose();
                    continue;
                }

                response.EnsureSuccessStatusCode();
                return response;
            }

            throw new HttpRequestException($"Riot API request exceeded max rate-limit retries for URL.");
        }

        public async Task<Puuid?> GetPuuidAsync(string region, string gamename, string tag)
        {
            var route = RegionRouter.Resolve(region);
            var regionalServer = route.Regional;

            try
            {
                using var response = await SendGetAsync($"https://{regionalServer}.api.riotgames.com/riot/account/v1/accounts/by-riot-id/{gamename}/{tag}?api_key={api_key}");
                Puuid? puuid = await response.Content.ReadFromJsonAsync<Puuid>();
                return puuid;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Error occurred while making HTTP request to by-riot-id endpoint.", ex);
            }
        }

        public async Task<LeagueEntryDTO?> GetLEDTO(string region, string puuid)
        {
            var route = RegionRouter.Resolve(region);
            var platformServer = route.Platform;

            try
            {
                using var response = await SendGetAsync($"https://{platformServer}.api.riotgames.com/lol/league/v4/entries/by-puuid/{puuid}?api_key={api_key}");
                var entries = await response.Content.ReadFromJsonAsync<List<LeagueEntryDTO>>();
                return entries?.FirstOrDefault(x => x.QueueType == "RANKED_SOLO_5x5");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Error occurred while making HTTP request to /entries/by-puuid endpoint.", ex);
            }
        }

        public async Task<List<string>?> GetMatchIds(string region, string puuid, long startTime, int start = 0, int count = 100)
        {
            var route = RegionRouter.Resolve(region);
            var regionalServer = route.Regional;

            try
            {
                using var response = await SendGetAsync($"https://{regionalServer}.api.riotgames.com/lol/match/v5/matches/by-puuid/{puuid}/ids?startTime={startTime}&queue=420&start={start}&count={count}&api_key={api_key}");
                List<string>? matchIds = await response.Content.ReadFromJsonAsync<List<string>>();
                return matchIds;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Error occurred while making HTTP request to /matches/by-puuid endpoint.", ex);
            }
        }

        public async Task<Ladder?> GetLadder(string region)
        {
            var route = RegionRouter.Resolve(region);
            var platformServer = route.Platform;

            try
            {
                using var response = await SendGetAsync($"https://{platformServer}.api.riotgames.com/lol/league/v4/challengerleagues/by-queue/RANKED_SOLO_5x5?api_key={api_key}");
                Ladder? ladder = await response.Content.ReadFromJsonAsync<Ladder>();
                return ladder;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Error occurred while making HTTP request to /challengerleagues/by-queue/RANKED_SOLO_5x5 endpoint.", ex);
            }
        }

        public async Task<Match.MatchResponse?> GetMatchData(string region, string matchId)
        {
            var route = RegionRouter.Resolve(region);
            var regionalServer = route.Regional;

            try
            {
                using var response = await SendGetAsync($"https://{regionalServer}.api.riotgames.com/lol/match/v5/matches/{matchId}?api_key={api_key}");
                Match.MatchResponse? matchData = await response.Content.ReadFromJsonAsync<Match.MatchResponse>();
                return matchData;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Error occurred while making HTTP request to /matches/{matchId} endpoint.", ex);
            }
        }
    }
}
