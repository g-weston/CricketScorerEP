using System;

namespace CricketScorerEP
{
    public static class Innings
    {
        public static int Runs { get; set; }
        public static int Byes { get; set; }
        public static int LegByes {get; set; }
        public static int Wides {get; set; }
        public static int NoBalls {get; set; }
        public static double Overs {get; set; }
        public static int CompleteOvers { get; set; }
        public static int Wickets {get; set; }
        public static int ScheduledOvers { get; set; }

 /*       public Innings()
	    {
            Runs = 0;
            Byes = 0;
            LegByes = 0;
            Wides = 0;
            NoBalls = 0;
            Overs = 0;
            Wickets = 0;
            ScheduledOvers = 0;
        }*/
    }
}
