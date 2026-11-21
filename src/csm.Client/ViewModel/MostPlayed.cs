using System;
using System.Collections.Generic;
using System.Text;

namespace csm.Client.ViewModel
{
    public class MostPlayed
    {
        public string ChampionName { get; set; } = string.Empty;

        public int Wins { get; set; }

        public int Losses { get; set; }

        public double Winrate { get; set; }
    }
}
