namespace csm.Server.Models
{
    public class Build
    {
        public int Id { get; set; }
        public required int ChampionId { get; set; }

        public string? ChampionIcon { get; set; }

        public string? Position { get; set; }

        public int Item0 { get; set; }


        public int Item1 { get; set; }

        public int Item2 { get; set; }

        public int Item3 { get; set; }

        public int Item4 { get; set; }

        public int Item5 { get; set; }

        public int Item6 { get; set; }
        
        public List<Styles>? Styles { get; set; }

        public int Defense { get; set; }

        public int Flex { get; set; }

        public int Offense { get; set; }

        public int Spell1 { get; set; }

        public int Spell2 { get; set; }

        public List<Matchup>? Matchups { get; set; }

    }

    public class Styles
    {
        public string Description { get; set; } = string.Empty;
        public int Style { get; set; }

        public List<Selections> Selections { get; set; } = new();
    }

    public class Selections
    {
        public int Perk { get; set; }
        public int Var1 { get; set; }
        public int Var2 { get; set; }
        public int Var3 { get; set; }
    }
}
