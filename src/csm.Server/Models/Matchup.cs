using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace csm.Server.Models
{
    public class Matchup
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(Build))]
        public int BuildId { get; set; }

        public int OpponentChampionId { get; set; }

        public double WinRate { get; set; }

        public int GamesPlayed { get; set; }
    }
}
