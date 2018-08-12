using System;

namespace CricketScorerEP
{
    public class Match
    {
        public DateTime Date { get; set; }
        public string Competition { get; set; } // League, cup, friendly
        public string Venue { get; set; }
        public string Format { get; set; } // Standard, Pairs
        public string AgeGroup { get; set; } // Senior, Junior
        public int ScheduledOvers { get; set; } // 20, 40, 50 etc.
        public int RunsPerWideOrNoBall { get; set; } // 1 or 2
        public int RebowlDeliveriesFromOver { get; set; } // From over 1 for Senior, from (e.g) 19 for junior

        public Match()
        {
            Date = new DateTime(2018, 06, 30);
            Competition     = "League";
            Venue           = "Home";
            Format          = "Standard";
            AgeGroup        = "Senior";
            ScheduledOvers              = 20;
            RunsPerWideOrNoBall         = 1;
            RebowlDeliveriesFromOver    = 1;
        }
    }
}

