using System;
using System.Collections.Generic;

namespace CricketScorerEP
{
    public static class Teams
    {
        public static List<Player> teamOnePlayers = new List<Player>();
        public static List<Player> teamTwoPlayers = new List<Player>();
        public static int currentBatsmanOne = 0, currentBatsmanTwo = 1;
        public static int batsmanFacing = currentBatsmanOne;
        public static int batsmanNotFacing = currentBatsmanTwo;
        public static int nextBatsman = currentBatsmanTwo + 1;
        public static int currentBowler = 10;
        public static int dismissingFielder;
        public static int wicketFielder;

        public static void SwapFacingBatsmen(ref int facingBatsman, ref int nonFacingBatsman)
        {
            int temp = facingBatsman;
            facingBatsman = nonFacingBatsman;
            nonFacingBatsman = temp;
            return;
        }
    }

    
}