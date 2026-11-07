using csm.Server.DTO;
using DotNetEnv;
using System.Web;
using System.Net;
using System.Threading;
namespace csm.Server.Riot
{
    public class RiotService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        readonly string? api_key = Environment.GetEnvironmentVariable("RIOT_API_KEY");

        public RiotService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Puuid?> GetPuuidAsync(string region,string gamename, string tag)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            

            //using var client = _httpClientFactory.CreateClient();

            try
            {
                var response = await client.GetAsync($"https://{region}.api.riotgames.com/riot/account/v1/accounts/by-riot-id/{gamename}/{tag}?api_key={api_key}");
                
                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    if (response.Headers.TryGetValues("Retry-After", out var retryAfterValues))
                    {
                        if (int.TryParse(retryAfterValues.FirstOrDefault(), out int retryAfterSeconds))
                        {
                            Console.WriteLine($"Rate limit exceeded. Retrying after {retryAfterSeconds} seconds...");
                            await Task.Delay(retryAfterSeconds * 1000);
                            return await GetPuuidAsync(region, gamename, tag);
                        }
                    }

                }

                Puuid? puuid = await response.Content.ReadFromJsonAsync<Puuid>();

                return puuid;

            }
            catch (HttpRequestException ex)
            {
                
                Console.WriteLine(ex.Message);
                throw new Exception("Error occurred while making HTTP request to by-riot-id endpoint.", ex);
                
            }
        }

        public async Task<List<string>?> GetMatchIds(string region, string puuid)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            try
            {
                var response = await client.GetAsync($"https://{region}.api.riotgames.com/lol/match/v5/matches/by-puuid/{puuid}/ids?start=0&count=10&api_key={api_key}");

                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    if (response.Headers.TryGetValues("Retry-After", out var retryAfterValues))
                    {
                        if (int.TryParse(retryAfterValues.FirstOrDefault(), out int retryAfterSeconds))
                        {
                            Console.WriteLine($"Rate limit exceeded. Retrying after {retryAfterSeconds} seconds...");
                            await Task.Delay(retryAfterSeconds * 1000);
                            return await GetMatchIds(region, puuid);
                        }
                    }
                }

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
            HttpClient client = _httpClientFactory.CreateClient();
            try
            {
                var response = await client.GetAsync($"https://{region}.api.riotgames.com/lol/league/v4/challengerleagues/by-queue/RANKED_SOLO_5x5?api_key={api_key}");

                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    if (response.Headers.TryGetValues("Retry-After", out var retryAfterValues))
                    {
                        if (int.TryParse(retryAfterValues.FirstOrDefault(), out int retryAfterSeconds))
                        {
                            Console.WriteLine($"Rate limit exceeded. Retrying after {retryAfterSeconds} seconds...");
                            await Task.Delay(retryAfterSeconds * 1000);
                            return await GetLadder(region);
                        }
                    }
                }

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
            HttpClient client = _httpClientFactory.CreateClient();
            try
            {
                var response = await client.GetAsync($"https://{region}.api.riotgames.com/lol/match/v5/matches/{matchId}?api_key={api_key}");

                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    if (response.Headers.TryGetValues("Retry-After", out var retryAfterValues))
                    {
                        if (int.TryParse(retryAfterValues.FirstOrDefault(), out int retryAfterSeconds))
                        {
                            Console.WriteLine($"Rate limit exceeded. Retrying after {retryAfterSeconds} seconds...");
                            await Task.Delay(retryAfterSeconds * 1000);
                            return await GetMatchData(region, matchId);
                        }
                    }
                }

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
