using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette_API_Masivian.Models
{
    [Serializable]
    public class Bet
    {
        public long betId { get; set; }
        public long rouletteId { get; set; }
        public long playerId { get; set; }
        public int quantityToBet { get; set; }
        public int numberToBet { get; set; }
        public String colorToBet { get; set; }
    }
}
