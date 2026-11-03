namespace csm.Server.Models
{
    public class Build
    {
        int ChampionId { get; set; }

        string? ChampionIcon { get; set; }

        List<int>? RecommendedPerks { get; set; }

        List<int>? RecommendedSpells { get; set; }

        List<string>? RecommendedAbilityOrder { get; set; }

        List<int>? RecommendedStartItems { get; set; }

        List<int>? RecommendedItems { get; set; }

        List<int>? Matchups { get; set; }

    }
}
