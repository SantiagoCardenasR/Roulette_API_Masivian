using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette_API_Masivian.Models
{
    [Serializable]
    public class Player
    {
        public long playerId { get; set; }
        public String screenName { get; set; }
        public int playerCredits { get; set; }
    }
}
