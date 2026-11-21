using System;
using System.Collections.Generic;
using System.Text;

namespace csm.Client.ViewModel
{
    public class TierlistRow
    {
        public int Rank { get; set; } = 1;

        public string Name { get; set; } = "Ahri";

        public int Lane { get; set; } = 3;

        public double WinRate { get; set; } = 59.00;

        public double PickRate { get; set; } = 10.3;

        public int GamesPlayed { get; set; } = 4230;

    }
}
