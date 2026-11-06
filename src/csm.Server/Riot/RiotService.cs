using csm.Server.DTO;
using DotNetEnv;
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
                Puuid? response = await client.GetFromJsonAsync<Puuid>($"https://{region}.api.riotgames.com/riot/account/v1/accounts/by-riot-id/{gamename}/{tag}?api_key={api_key}");

                return response;

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
                List<string>? response = await client.GetFromJsonAsync<List<string>>($"https://{region}.api.riotgames.com/lol/match/v5/matches/by-puuid/{puuid}/ids?start=0&count=10&api_key={api_key}");
                return response;
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
                Ladder? response = await client.GetFromJsonAsync<Ladder>($"https://{region}.api.riotgames.com/lol/league/v4/challengerleagues/by-queue/RANKED_SOLO_5x5?api_key={api_key}");
                return response;
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
                Match.MatchResponse? response = await client.GetFromJsonAsync<Match.MatchResponse>($"https://{region}.api.riotgames.com/lol/match/v5/matches/{matchId}?api_key={api_key}");
                return response;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception("Error occurred while making HTTP request to /matches/{matchId} endpoint.", ex);
            }
        }
    }
}
