using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette_API_Masivian.Models
{
    [Serializable]
    public class Roulette
    {
        public long rouletteId { get; set; }
        public bool isActive { get; set; }
        public List<Bet> bets { get; set; }

        /*public Roulette(long pRouletteId, bool pIsActive, List<Bet> pBets)
        {
            rouletteId = pRouletteId;
            isActive = pIsActive;
            bets = pBets;
        }*/
    }
}
