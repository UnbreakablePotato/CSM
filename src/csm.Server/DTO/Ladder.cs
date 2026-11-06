namespace csm.Server.DTO
{
    public class Entries
    {
        public required string SummonerName { get; set; }
        public required string Puuid { get; set; }
        public required int LeaguePoints { get; set; }
        public required int Wins { get; set; }
        public required int Losses { get; set; }
        public required bool HotStreak { get; set; }
        public required bool Veteran { get; set; }
        public required bool FreshBlood { get; set; }
        public required bool Inactive { get; set; }
    }
    public class Ladder
    {
        public string? Tier { get; set; }
        public string? Queue { get; set; }
        public List<Entries>? Entries { get; set; } 
    }
}
