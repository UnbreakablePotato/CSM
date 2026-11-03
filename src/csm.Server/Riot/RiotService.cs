namespace csm.Server.Riot
{
    public class RiotService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RiotService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetPuuidAsync()
        {
            using var client = _httpClientFactory.CreateClient();

            return await client.GetStringAsync("");
        }
    }
}
