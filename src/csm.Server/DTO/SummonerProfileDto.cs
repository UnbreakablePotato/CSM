namespace csm.Server.DTO
{
    public class SummonerProfileDto
    {
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

        public double Winrate { get; set; }

        public double Kda { get; set; }

        public List<MostPlayedChampionDto> MostPlayedChampions { get; set; } = new();

        public List<MatchSummaryDto> PreviousMatches { get; set; } = new();
    }

    public class MostPlayedChampionDto
    {
        public int ChampionId { get; set; }

        public string ChampionName { get; set; } = string.Empty;

        public int GamesPlayed { get; set; }

        public int Wins { get; set; }

        public int Losses { get; set; }

        public double Winrate { get; set; }

        public double Kda { get; set; }
    }

    public class MatchSummaryDto
    {
        public string MatchId { get; set; } = string.Empty;

        public int ChampionId { get; set; }

        public string ChampionName { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;

        public int Kills { get; set; }

        public int Deaths { get; set; }

        public int Assists { get; set; }

        public double Kda { get; set; }

        public bool Win { get; set; }

        public int Cs { get; set; }

        public int[] Items { get; set; } = new int[7];

        public int SummonerSpell1Id { get; set; }

        public int SummonerSpell2Id { get; set; }
    }
}
