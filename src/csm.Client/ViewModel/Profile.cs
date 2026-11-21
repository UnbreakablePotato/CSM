using System;
using System.Collections.Generic;
using System.Text;

namespace csm.Client.ViewModel
{
    public class Profile
    {
        public string Gamename { get; set; } = String.Empty;

        public string Tag { get; set; } = String.Empty;

        public string Rank { get; set; } = String.Empty;

        public int Wins { get; set; }

        public int Losses { get; set; }

        public double Winrate { get;set; }

        public double KDA { get; set; }

        public List<MostPlayed> MostPlayeds { get; set; } = new();
    }
}
