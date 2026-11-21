using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace csm.Server.Models
{
    [Index(
    nameof(ChampionId),
    nameof(Position),
    nameof(Item0),
    nameof(Item1),
    nameof(Item2),
    nameof(Spell1),
    nameof(Spell2),
    nameof(PrimaryStyle),
    nameof(Perk1),
    nameof(Perk2),
    nameof(Perk3),
    nameof(Perk4),
    nameof(SecondaryStyle),
    nameof(SubPerk1),
    nameof(SubPerk2),
    nameof(Defense),
    nameof(Flex),
    nameof(Offense),
    IsUnique = true
    )]
    public class Core
    {
        [Key]
        public int Id { get; set; }
        public int ChampionId { get; set; }

        public string? ChampionName { get; set; }

        public string? Position { get; set; }

        public int Item0 { get; set; }
        public int Item1 { get; set; }
        public int Item2 { get; set; }

        public int Spell1 { get; set; }
        public int Spell2 { get; set; }

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

        public int GamesPlayed { get; set; }

        public int GamesWon { get; set; }
    }
}
