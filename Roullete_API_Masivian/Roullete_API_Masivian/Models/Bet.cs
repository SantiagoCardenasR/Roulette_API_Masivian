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
        private long betId;
        private long rouletteId;
        private long playerId;
        private int quantityToBet;
        private String colorToBet;

        public Bet(long pBetId, long pRouletteId, long pPlayerId, int pQuantityToBet, String pColorToBet)
        {
            betId = pBetId;
            playerId = pPlayerId;
            quantityToBet = pQuantityToBet;
            colorToBet = pColorToBet;
        }
        public long getBetId()
        {
            return betId;
        }
        public long getRouletteId()
        {
            return rouletteId;
        }
        public long getPlayerId()
        {
            return playerId;
        }
        public int getQuantityToBet()
        {
            return quantityToBet;
        }
        public String getColorToBet()
        {
            return colorToBet;
        }
        public void setQuantityToBet(int pQuantityToBet)
        {
            quantityToBet = pQuantityToBet;
        }
        public void setColorToBet(String pColorToBet)
        {
            colorToBet = pColorToBet;
        }
    }
}
