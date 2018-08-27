using System.Collections.Generic;

namespace CricketScorerEP
{
    public static class Teams
    {
        public static List<Player> teamOnePlayers = new List<Player>();
        public static List<Player> teamTwoPlayers = new List<Player>();
        public static int currentBatsmanOne = 0, currentBatsmanTwo = 1; // Perhaps use a picker instead in case it isn't the first 2 in the list
        public static int batsmanFacing = currentBatsmanOne;
        public static int batsmanNotFacing = currentBatsmanTwo;
        public static int nextBatsman;
        public static int currentBowler = 10; // Perhaps use a picker instead in case it isn't the #11 who opens the bowling
        public static int dismissingFielder;

        public static void SwapFacingBatsmen(ref int facingBatsman, ref int nonFacingBatsman)
        {
            int temp = facingBatsman;
            facingBatsman = nonFacingBatsman;
            nonFacingBatsman = temp;
            return;
        }

        public static void UpdateBowlerOversBowled() 
        {
            Teams.teamTwoPlayers[Teams.currentBowler].NumberOfOversBowled += 0.1;
            Innings.validDeliveriesInThisOver++;
            Innings.Overs += 0.1;

            if (Innings.validDeliveriesInThisOver == 6)
            {
                Teams.teamTwoPlayers[Teams.currentBowler].NumberOfOversBowled += 0.4;
                Innings.Overs += 0.4;
                Innings.CompleteOvers++;
            }
        }
    } 
}