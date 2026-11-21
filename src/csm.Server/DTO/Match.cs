using csm.Server.Models;

namespace csm.Server.DTO
{
    public class Match
    {
        public class MatchResponse
        {
            public Metadata Metadata { get; set; } = new();
            public MatchInfo Info { get; set; } = new();
        }

        public class Metadata
        {
            public string DataVersion { get; set; } = string.Empty;
            public string MatchId { get; set; } = string.Empty;
            public List<string> Participants { get; set; } = new();
        }

        public class MatchInfo
        {
            public string GameMode { get; set; } = string.Empty;
            public long GameCreation { get; set; }
            public int GameDuration { get; set; }
            public long GameId { get; set; }
            public string GameVersion { get; set; } = string.Empty;
            public int MapId { get; set; }

            public List<Participant> Participants { get; set; } = new();
        }

        public class Participant
        {
            public string Puuid { get; set; } = string.Empty;
            public string RiotIdGameName { get; set; } = string.Empty;
            public string RiotIdTagline { get; set; } = string.Empty;

            public string ChampionName { get; set; } = string.Empty;
            public int ChampionId { get; set; }

            public int ChampionExperience { get; set; }

            public int ChampionLevel { get; set; }

            public int ProfileIcon { get; set; }

            public int TeamId { get; set; }

            public string Lane { get; set; } = string.Empty;

            public string Role { get; set; } = string.Empty;

            public string IndividualPosition { get; set; } = string.Empty;

            public int Kills { get; set; }
            public int Deaths { get; set; }
            public int Assists { get; set; }

            public int GoldEarned { get; set; }
            public int GoldSpent { get; set; }
            public int TotalMinionsKilled { get; set; }
            public int TotalDamageDealt { get; set; }

            public int TotalDamageDealtToChampions { get; set; }

            public int TotalDamageTaken { get; set; }

            public int DamageSelfMitigated { get; set; }

            public int TotalHeal { get; set; }

            public int VisionScore { get; set; }

            public int WardsPlaced { get; set; }

            public int WardsKilled { get; set; }
            public bool Win { get; set; }

            public int Item0 { get; set; }

            public int Item1 { get; set; }

            public int Item2 { get; set; }

            public int Item3 { get; set; }

            public int Item4 { get; set; }

            public int Item5 { get; set; }

            public int Item6 { get; set; }

            public Perks Perks { get; set; } = new();

            public int Summoner1Id { get; set; }

            public int Summoner2Id { get; set; }

            public int SummonerLevel { get; set; }
        }

        public class Perks
        {
            public StatPerks StatPerks { get; set; } = new();
            public List<PerkStyle> Styles { get; set; } = new();
        }

        public class StatPerks
        {
            public int Defense { get; set; }
            public int Flex { get; set; }
            public int Offense { get; set; }
        }

        public class PerkStyle
        {
            public string Description { get; set; } = string.Empty;
            public int Style { get; set; }
            public List<PerkSelection> Selections { get; set; } = new();
        }

        public class PerkSelection
        {
            public int Perk { get; set; }
            public int Var1 { get; set; }
            public int Var2 { get; set; }
            public int Var3 { get; set; }
        }
    }
}
