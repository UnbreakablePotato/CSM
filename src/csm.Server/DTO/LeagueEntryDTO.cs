namespace csm.Server.DTO
{
    public class LeagueEntryDTO
    {
        public string LeagueId { get; set; } = string.Empty;

        public string Puuid { get; set; } = string.Empty;

        public string QueueType { get; set; } = string.Empty;

        public string Tier { get; set; } = string.Empty;

        public string Rank { get; set; } = string.Empty;

        public int LeaguePoints { get; set; }

        public int Wins { get; set; }

        public int Losses { get; set; }

        public bool HotStreak { get; set; }

        public bool Veteran { get; set; }

        public bool FreshBlood { get; set; }

        public bool Inactive { get; set; }

    }
}
