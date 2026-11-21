using System;
using System.Collections.Generic;
using System.Text;

namespace csm.Client.ViewModel
{
    public class TierlistViewModel
    {
        public List<TierlistRow> Champions { get; set; } = new()
        {
        new TierlistRow
        {
            Rank = 1,
            Name = "Ahri",
            WinRate = 0.524,
            PickRate = 0.15,
            GamesPlayed = 10000
        },
        new TierlistRow
        {
            Rank = 2,
            Name = "Yasuo",
            WinRate = 0.512,
            PickRate = 0.20,
            GamesPlayed = 15000
        }
    };

}
}
