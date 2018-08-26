using System;

namespace CricketScorerEP
{
    public static class Match
    {
        public static DateTime Date { get; set; }
        public static string Competition { get; set; } // League, cup, friendly
        public static string Venue { get; set; }
        public static string HomeTeam { get; set; }
        public static string AwayTeam { get; set; }
        public static string Format { get; set; } // Standard, Pairs
        public static string AgeGroup { get; set; } // Senior, Junior
        public static int ScheduledOvers { get; set; } // 20, 40, 50 etc.
        public static int RunsPerWideOrNoBall { get; set; } // 1 or 2
        public static int RebowlDeliveriesFromOver { get; set; } // From over 1 for Senior, from (e.g) 19 for junior
    }
}

