namespace CricketScorerEP
{
    public static class Innings
    {
        public static int Runs { get; set; }
        public static int Byes { get; set; }
        public static int LegByes { get; set; }
        public static int Wides { get; set; }
        public static int NoBalls { get; set; }
        public static double Overs { get; set; }
        public static int CompleteOvers { get; set; }
        public static int Wickets { get; set; }
        public static int ScheduledOvers { get; set; }

        public static int validDeliveriesInThisOver = 0, runsScoredThisOver = 0;
        public static bool maidenBowled = true;
        public static int ByesOffWide, ByesOffNoBall, LegByesOffNoBall;

        public static void RecordNoBall()
        {
            Teams.teamOnePlayers[Teams.batsmanFacing].DeliveriesFaced++;
            Innings.Runs += Match.RunsPerWideOrNoBall;
            Innings.NoBalls++;
            Teams.teamTwoPlayers[Teams.currentBowler].NoBallsDelivered++;
            Teams.teamTwoPlayers[Teams.currentBowler].RunsConceded += Match.RunsPerWideOrNoBall;
        }

        public static void RecordByesOffNoBall()
        {
            Innings.Runs += ByesOffNoBall;
            Innings.NoBalls += ByesOffNoBall;
            Teams.teamTwoPlayers[Teams.currentBowler].RunsConceded += ByesOffNoBall;
            Teams.teamTwoPlayers[Teams.currentBowler].NoBallsDelivered += ByesOffNoBall;
        }

        public static void RecordLegByesOffNoBall()
        {
            Innings.Runs += LegByesOffNoBall;
            Innings.NoBalls += LegByesOffNoBall;
            Teams.teamTwoPlayers[Teams.currentBowler].RunsConceded += LegByesOffNoBall;
            Teams.teamTwoPlayers[Teams.currentBowler].NoBallsDelivered += LegByesOffNoBall;
        }

        public static void RecordWide()
        {
            Innings.Runs += Match.RunsPerWideOrNoBall;
            Teams.teamTwoPlayers[Teams.currentBowler].RunsConceded += Match.RunsPerWideOrNoBall;
            Teams.teamTwoPlayers[Teams.currentBowler].WidesConceded++;
            Innings.Wides++;
        }

        public static void RecordByesOffWide()
        {
            Innings.Runs += ByesOffWide;
            Innings.Wides += ByesOffWide;
            Teams.teamTwoPlayers[Teams.currentBowler].RunsConceded += ByesOffWide;
            Teams.teamTwoPlayers[Teams.currentBowler].WidesConceded += ByesOffWide;
        }
    }
}
