using System;

namespace CricketScorerEP
{
    public class Player
    {
        public int UniqueId { get; set; }
        public string ClubName { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }

        public bool IsCaptain { get; set; }
        public bool IsKeeper { get; set; }

        public int RunsScored { get; set; }
        public int DeliveriesFaced { get; set; }
        public int NumberOfFoursScored { get; set; }
        public int NumberOfSixesScored { get; set; }

        public TimeSpan StartTime { get; set; }
        public TimeSpan DismissalTime { get; set; }
        public int TimeAtTheCrease { get; set; } //  TODO in minutes (Dismissal Time - Start Time) - need a method to do this

        public int PositionBowling { get; set; }
        public double NumberOfOversBowled { get; set; }
        public int NumberOfWicketsTaken { get; set; }
        public int NumberOfMaidensBowled { get; set; }
        public int WidesConceded { get; set; }
        public int NoBallsDelivered { get; set; }
        public int RunsConceded { get; set; }

        public bool IsOut { get; set; }
        public int HowOut { get; set; }
        public int DismissingBowler { get; set; }
        public int DismissingFielder { get; set; }
        public string DismissalMethod { get; set; }

        public Player(string clubName, int uniquePlayerId, string playerName, string fullName)
        {
            ClubName = clubName;
            UniqueId = uniquePlayerId;
            Name     = playerName;
            FullName = fullName;

            IsCaptain = false;
            IsKeeper = false;

            // starting Score makes sense if a previously retired batsman returns to the crease, but in that instance, we'd need to pass in values for runs, balls faced, 4s, 6s and Time.
            RunsScored = 0;
            DeliveriesFaced = 0;
            NumberOfFoursScored = 0;
            NumberOfSixesScored = 0;
            TimeAtTheCrease = 0;

            PositionBowling = 0;
            NumberOfOversBowled = 0;
            NumberOfWicketsTaken = 0;
            NumberOfMaidensBowled = 0;
            WidesConceded = 0;
            NoBallsDelivered = 0;
            RunsConceded = 0;
            
            IsOut = false;
            HowOut = 0;
            DismissingBowler = 0;
            DismissingFielder = 0; // TODO this is always a real value - need the concept of "unset"
            DismissalMethod = "";
        }
    }
 }
