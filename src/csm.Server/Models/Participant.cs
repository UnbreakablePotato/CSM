using System.ComponentModel.DataAnnotations;

namespace csm.Server.Models
{
    public class Participant
    {
        public required string Id { get; set; } //matchId

        public string? Puuid { get; set; }

        public string? SummonerName { get; set; }

        public int SummonerLevel { get; set; }

        public string? RiotIdGameName { get; set; }

        public string? RiotIdTagLine { get; set; }

        public int ProfileIcon { get; set; }

        public int ChampionId { get; set; }

        public string? ChampionName { get; set; }

        public int ChampionLevel { get; set; }

        public int ChampionExperience { get; set; }

        public int TeamId { get; set; }

        public string? IndividualPosition { get; set; }

        public string? Lane { get; set; }

        public string? Role { get; set; }

        public int Kills { get; set; }

        public int Deaths { get; set; }

        public int Assists { get; set; }

        public bool Win { get; set; }

        public int GoldEarned { get; set; }

        public int GoldSpent { get; set; }

        public int TotalMinionsKilled { get; set; }

        public int neutralMinionsKilled { get; set; }

        public int TotalDamageDealt { get; set; }

        public int TotalDamageDealtToChampions { get; set; }

        public int TotalDamageTaken { get; set; }

        public int DamageSelfMitigated { get; set; }

        public int TotalHeal { get; set; }

        public int VisionScore { get; set; }

        public int WardsPlaced { get; set; }

        public int WardsKilled { get; set; }

        public int Item0 { get; set; }
        public int Item1 { get; set; }

        public int Item2 { get; set; }
        public int Item3 { get; set; }

        public int Item4 { get; set; }

        public int Item5 { get; set; }
        public int Item6 { get; set; }

        public int SummonerSpell1Id { get; set; }
        public int SummonerSpell2Id { get; set; }

        public bool FirstBloodKill { get; set; }

        public bool FirstBloodAssist { get; set; }
        public bool FirstTowerKill { get; set; }
        public bool FirstTowerAssist { get; set; }

        public int DoubleKills { get; set; }
        public int TripleKills { get; set; }
        public int QuadraKills { get; set; }
        public int PentaKills { get; set; }
    }
}
