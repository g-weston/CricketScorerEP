using System;
using System.Collections.Generic;

public class Player
{
    public int RunsScored { get; set; }
    public int DeliveriesFaced { get; set; }
    public int NumberOfFoursScored { get; set; }
    public int NumberOfSixesScored { get; set; }
    public int TimeAtTheCrease { get; set; } // in minutes
    public int PositionBatting { get; set; }
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

    public bool IsOut { get; set; }

    public Player(string clubName, int uniquePlayerId, string playerName, int startingScore, bool isBatting) // not convinced we need startingScore or isBatting
    {
        ClubName = clubName;
        UniqueId = uniquePlayerId;
        Name = playerName;

        // startingScore doesn't make any sense for TimeAtTheCrease etc.
        // starting Score makes sense if a previously retired batsman returns to the crease, but in that instance, we'd need to pass in values for runs, balls faced, 4s, 6s and Time.
        RunsScored = startingScore;

        DeliveriesFaced = 0;
        NumberOfFoursScored = 0; // ??
        NumberOfSixesScored = 0;
        TimeAtTheCrease = 0;

        PositionBatting = 0; // ?? is this just the index into the array

        // think we need to change the initialisation here - "startingScore" doesn't make much sense for "number of maidens" - it should be zero really.
        NumberOfOversBowled = 0;
        NumberOfWicketsTaken = 0;
        NumberOfMaidensBowled = 0;
        WidesConceded = 0;
        NoBallsDelivered = 0;
        RunsConceded = 0;
        PositionBowling = 0;

        IsOut     = false; 
    }
}

