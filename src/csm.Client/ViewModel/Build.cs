using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace csm.Client.ViewModel
{
    public class Build
    {
        public int ChampionId { get; set; }

        public string ChampionName { get; set; } = string.Empty;

        public string ChampionIcon { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;

        public int Rank { get; set; }

        public double Winrate { get; set; }

        public double Pickrate { get; set; }

        public int GamesPlayed { get; set; }

        public int Item0 { get; set; }

        public int Item1 { get; set; }

        public int Item2 { get; set; }

        public int Item3 { get; set; }

        public int Item4 { get; set; }

        public int Item5 { get; set; }

        public int PrimaryStyle { get; set; }

        public int Perk1 { get; set; }

        public int Perk2 { get; set; }

        public int Perk3 { get; set; }

        public int Perk4 { get; set; }

        public int SecondaryStyle { get; set; }

        public int SubPerk1 { get; set; }

        public int SubPerk2 { get; set; }

        public int Defense { get; set; }

        public int Flex { get; set; }

        public int Offense { get; set; }

        public int Spell1 { get; set; }

        public int Spell2 { get; set; }

        public List<Matchup>? Matchups { get; set; }
    }
}
