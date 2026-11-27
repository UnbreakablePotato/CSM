using csm.Server.DTO;
using System.Text.Json;

namespace csm.Server.Services
{
    public class DragontailService
    {
        private readonly string _basePath;
        readonly string version = "16.23.1";

        public DragontailService(IWebHostEnvironment environment)
        {
            _basePath = Path.Combine(environment.ContentRootPath, "Resources", version);
        }

        public async Task<ChampionResources?> GetChampionResourcesAsync(int championId)
        {
            try
            {
                string championListPath = Path.Combine(_basePath, "data", "en_US", "champion.json");
                using var championList = JsonDocument.Parse(await File.ReadAllTextAsync(championListPath));

                string? championName = null;

                foreach (var champion in championList.RootElement.GetProperty("data").EnumerateObject())
                {
                    if (champion.Value.GetProperty("key").GetString() == championId.ToString())
                    {
                        championName = champion.Value.GetProperty("id").GetString();
                        break;
                    }
                }

                if (championName == null)
                {
                    return null;
                }

                string championPath = Path.Combine(_basePath, "data", "en_US", "champion", $"{championName}.json");
                using var championData = JsonDocument.Parse(await File.ReadAllTextAsync(championPath));

                var championElement = championData.RootElement.GetProperty("data").GetProperty(championName);
                string passiveFile = championElement.GetProperty("passive").GetProperty("image").GetProperty("full").GetString()!;
                var spells = championElement.GetProperty("spells");

                string imgPath = Path.Combine(_basePath, "img");

                ChampionResources resources = new ChampionResources();
                resources.ChampionName = championName;
                resources.Icon = await File.ReadAllBytesAsync(Path.Combine(imgPath, "champion", $"{championName}.png"));
                resources.Passive = await File.ReadAllBytesAsync(Path.Combine(imgPath, "passive", passiveFile));
                resources.Q = await File.ReadAllBytesAsync(Path.Combine(imgPath, "spell", GetSpellFile(spells, 0)));
                resources.W = await File.ReadAllBytesAsync(Path.Combine(imgPath, "spell", GetSpellFile(spells, 1)));
                resources.E = await File.ReadAllBytesAsync(Path.Combine(imgPath, "spell", GetSpellFile(spells, 2)));
                resources.R = await File.ReadAllBytesAsync(Path.Combine(imgPath, "spell", GetSpellFile(spells, 3)));

                return resources;
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception($"Error occurred while reading dragontail resources for champion id {championId}.", ex);
            }
        }

        private string GetSpellFile(JsonElement spells, int index)
        {
            return spells[index].GetProperty("image").GetProperty("full").GetString()!;
        }
    }
}
