namespace csm.Server.Models
{
    public class Champion
    {
        public int Id { get; set; }
        public string ChampionName { get; set; } = string.Empty;

        public int ChampionId { get; set; }

        public double Winrate { get; set; }

        public double Pickrate { get; set; }

        public int GamesPlayed { get; set; }
    }
}
