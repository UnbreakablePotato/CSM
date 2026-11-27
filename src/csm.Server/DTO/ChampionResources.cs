namespace csm.Server.DTO
{
    public class ChampionResources
    {
        public string ChampionName { get; set; } = string.Empty;

        public byte[] Icon { get; set; } = [];

        public byte[] Passive { get; set; } = [];

        public byte[] Q { get; set; } = [];

        public byte[] W { get; set; } = [];

        public byte[] E { get; set; } = [];

        public byte[] R { get; set; } = [];
    }
}
