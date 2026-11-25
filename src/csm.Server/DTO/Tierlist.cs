namespace csm.Server.DTO
{
    public class Tierlist
    {
        public List<TierListRow> TierlistCollection { get; set; } = new();
    }

    public class TierListRow
    {
        public int Rank { get; set; }

        public string ChampionName { get; set; } = string.Empty;

        public double Winrate { get; set; }

        public double Pickrate { get; set; }
    }
}
