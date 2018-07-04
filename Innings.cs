using System;

public class Innings
{
    public int Runs { get; set; }
    public int Byes { get; set; }
    public int LegByes {get; set; }
    public int Wides {get; set; }
    public int NoBalls {get; set; }
    public int Overs {get; set; }
    public int Wickets {get; set; }
    public int ScheduledOvers { get; set; }

    public Innings()
	{
        Runs = 0;
        Byes = 0;
        LegByes = 0;
        Wides = 0;
        NoBalls = 0;
        Overs = 0;
        Wickets = 0;
        ScheduledOvers = 0;
    }
}
