using System;
using System.Collections.Generic;

public class Player
{
    public int RunsScored { get; set; }
    public int DeliveriesFaced { get; set; }
    public int NumberOfFoursScored { get; set; }
    public int NumberOfSixesScored { get; set; }
    public int TimeAtTheCrease { get; set; } // in minutes
    public int NumberOfWicketsTaken { get; set; }
    public int NumberOfMaidensBowled { get; set; }
    public int WidesConceded { get; set; }
    public int NoBallsDelivered { get; set; }
    public int RunsConceded { get; set; }
    public int PositionBowling { get; set; }
    public int UniqueId { get; set; }

    public double NumberOfOversBowled { get; set; }

    public string ClubName { get; set; }
    public string Name { get; set; }

    public int DismissingBowler { get; set; }
    public int DismissingFielder { get; set; }
    public bool IsOut { get; set; }

    public Player(string clubName, int uniquePlayerId, string playerName) 
    {
        ClubName = clubName;
        UniqueId = uniquePlayerId;
        Name = playerName;

        // starting Score makes sense if a previously retired batsman returns to the crease, but in that instance, we'd need to pass in values for runs, balls faced, 4s, 6s and Time.
        RunsScored = 0;
        DeliveriesFaced = 0;
        NumberOfFoursScored = 0; // ??
        NumberOfSixesScored = 0;
        TimeAtTheCrease = 0;

        NumberOfOversBowled = 0;
        NumberOfWicketsTaken = 0;
        NumberOfMaidensBowled = 0;
        WidesConceded = 0;
        NoBallsDelivered = 0;
        RunsConceded = 0;
        PositionBowling = 0;

        IsOut     = false;
        DismissingBowler = 0;
        DismissingFielder = 0;
    }
}

