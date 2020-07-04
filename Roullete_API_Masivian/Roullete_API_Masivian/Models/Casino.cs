using Microsoft.AspNetCore.Routing;
using Roullete_API_Masivian.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette_API_Masivian.Models
{
    public class Casino
    {
        public List<Player> players { get; set; }
        public List<Roulette> roulettes { get; set; }
        public Casino(List<Player> pPlayers, List<Roulette> pRoulettes)
        {
            players = pPlayers;
            roulettes = pRoulettes;
        }
    }
}
