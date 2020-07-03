using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Roulette_API_Masivian.Models
{
    [Serializable]
    public class Player
    {
        private long playerId;
        private String screenName;
        private int playerCredits;

        public Player(long pPlayerId, String pScreenName, int pPlayerCredits)
        {
            playerId = pPlayerId;
            screenName = pScreenName;
            playerCredits = pPlayerCredits;
        }
        public long getPlayerId()
        {
            return playerId;
        }
        public String getPlayerScreenName()
        {
            return screenName;
        }
        public int getPlayerCredits()
        {
            return playerCredits;
        }
    }
}
