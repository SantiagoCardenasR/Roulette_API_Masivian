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
        public List<Roulette> _roulettes { get; set; }

        public Casino()
        {
            players = new List<Player>();
            _roulettes = new List<Roulette>();
        }
        public Roulette getRouletteById(int pId)
        {
            Roulette rouletteToFind = null;
            for (int i=0;i<_roulettes.Count;i++)
            {
                Roulette actual = _roulettes[i];
                if (actual.rouletteId == pId)
                {
                    rouletteToFind = actual;
                }
            }
            return rouletteToFind;
        }
        public void addRoulette(Roulette newRoulette)
        {
            _roulettes.Add(newRoulette);
        }
    }
}
