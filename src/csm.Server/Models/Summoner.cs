namespace csm.Server.Models
{
    public class Summoner
    {
        public int Id { get; set; }

        public required string Puuid { get; set; }

        public string Server { get; set; } = string.Empty;

        public string GameName { get; set; } = string.Empty;

        public string TagLine { get; set; } = string.Empty;

        public int ProfileIcon { get; set; }

        public int SummonerLevel { get; set; }

        public string Tier { get; set; } = "UNRANKED";

        public string Rank { get; set; } = string.Empty;

        public int LeaguePoints { get; set; }

        public int Wins { get; set; }

        public int Losses { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}
