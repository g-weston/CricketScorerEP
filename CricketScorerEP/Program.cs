using System;
using System.Collections.Generic;

namespace GeorgeConsole1
{
    public class Player
    {
        public int RunsScored { get; set; }
        public int BallsFaced { get; set; }
        public int Fours { get; set; }
        public int Sixes { get; set; }
        public int Time { get; set; }
        public int PositionBatting { get; set; }
        public bool Out { get; set; }


        public double OversBowled { get; set; }
        public int WicketsTaken { get; set; }
        public int Maidens { get; set; }
        public int Wides { get; set; }
        public int NoBalls { get; set; }
        public int RunsConceded { get; set; }
        public int PositionBowling { get; set; }


        public string Club { get; set; }
        public string UniqueId { get; set; }
        public string Name { get; set; }

        public bool Batting { get; set; }
        public bool Facing { get; set; }

        public Player(string club, string uniqueId, string name, int start, bool bat)
        {
            Club = club;
            UniqueId = uniqueId;
            Name = name;

            RunsScored = start;
            BallsFaced = start;
            Fours = start;
            Sixes = start;
            Time = start;
            PositionBatting = start;
            Out = bat;

            OversBowled = start;
            WicketsTaken = start;
            Maidens = start;
            Wides = start;
            NoBalls = start;
            RunsConceded = start;
            PositionBowling = start;

            Batting = bat;
            Facing = bat;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            List<string> teamOne = new List<string>();
            //List<struct> teamOneFigures = new List<struct>();
            List<string> teamTwo = new List<string>();
            //List<struct> teamTwoFigures = new List<struct>();
            // struct opening and continuing as a struct out of a list not in one. creating errors in toher parth of the code
            List<Player> onePlayers = new List<Player>();
            List<Player> twoPlayers = new List<Player>();
            bool lastPlayerOne = false;
            int count = 0;
            Console.WriteLine("Please enter the name of the first club");
            string clubOne = Console.ReadLine();

            Console.WriteLine("Please enter the name of the first player from the first team");
            string name = Console.ReadLine();
            teamOne.Add(name);

            while (lastPlayerOne == false)
            {
                Console.WriteLine("Please enter the name of next player from the first team");
                name = Console.ReadLine();
                teamOne.Add(name);
                count++;
                if (count > 9)
                {
                    Console.WriteLine("Is this the last player from the team? (Enter YES or NO)");
                    if (Console.ReadLine() == "YES")
                    {
                        lastPlayerOne = true;
                    }
                    else
                    {
                        lastPlayerOne = false;
                    }
                }
            }

            bool lastPlayerTwo = false;
            Console.WriteLine("Please enter the name of the second club");
            string clubTwo = Console.ReadLine();
            count = 0;

            Console.WriteLine("Please enter the name of the first player from the second team");
            name = Console.ReadLine();
            teamTwo.Add(name);

            while (lastPlayerTwo == false)
            {
                Console.WriteLine("Please enter the name of next player from the second team");
                name = Console.ReadLine();
                teamTwo.Add(name);
                count++;
                if (count > 9)
                {
                    Console.WriteLine("Is this the last player from the team? (Enter YES or NO)");
                    if (Console.ReadLine() == "YES")
                    {
                        lastPlayerTwo = true;
                    }
                    else
                    {
                        lastPlayerTwo = false;
                    }
                }
            }

            for (int i = 0; i < teamOne.Count; i++)
            {
                Console.WriteLine("Please enter the id of " + teamOne[i] + " from " + clubOne);
                string playerId = Console.ReadLine();
                name = teamOne[i];
                onePlayers.Add(new Player(clubOne, playerId, name, 0, false));
            }

            for (int i = 0; i < teamTwo.Count; i++)
            {
                Console.WriteLine("Please enter the id of " + teamTwo[i] + " from " + clubTwo);
                string playerId = Console.ReadLine();
                name = teamTwo[i];
                twoPlayers.Add(new Player(clubTwo, playerId, name, 0, false));
            }

            Console.WriteLine("Please enter the number of overs in the match");
            int totalOvers = int.Parse(Console.ReadLine());
            //System.Diagnostic.Debug.WriteLine("Some text");
            int runsScored = 0;
            int byesScored = 0;
            int totalByes = 0;
            int legByesScored = 0;
            int totalLegByes = 0;
            int totalRuns = 0;
            int totalWickets = 0;
            bool endOfOver = false;
            int overBalls = 0;
            int face = 0;
            int nonFace = 0;
            bool wicket = false;
            int nextBatsman = 2;
            string wayOut = "";
            bool wide = false;
            int totalWides = 0;
            bool noBall = false;
            int totalNoBall = 0;
            bool maiden = true;
            bool dotBall = false;

            onePlayers[1].Batting = true;
            onePlayers[2].Batting = false;
            onePlayers[1].Facing = true;

            Console.WriteLine("Which number batsman is the first bowler");
            int currentBowler = int.Parse(Console.ReadLine());

            for (int a = 0; a < totalOvers; a++)
            {
                endOfOver = false;
                overBalls = 0;

                while (endOfOver == false)
                {
                    Console.WriteLine("What happened on this ball? (Enter either RUNS DOT WICKET NOBALL WIDE BYES or LEGBYES)");
                    string outcome = Console.ReadLine();
                    if (outcome == "RUNS")
                    {
                        Console.WriteLine("How many runs were scored?");
                        runsScored = int.Parse(Console.ReadLine());
                    }
                    else if (outcome == "DOT")
                    {
                        dotBall = true;
                        runsScored = 0;
                    }
                    else if (outcome == "WICKET")
                    {
                        wicket = true;
                        Console.WriteLine("How was the batsman out? (Enter BOWLED CAUGHT RUNOUT LBW STUMPED HITWICKET OBSTRUCTION TIMEDOUT HITTWICE HANDLEDBALL)");
                        wayOut = Console.ReadLine();
                    }
                    else if (outcome == "NOBALL")
                    {
                        noBall = true;
                    }
                    else if (outcome == "WIDE")
                    {
                        wide = true;
                    }
                    else if (outcome == "BYES")
                    {
                        Console.WriteLine("How many byes were scored?");
                        byesScored = int.Parse(Console.ReadLine());
                    }
                    else if (outcome == "LEGBYES")
                    {
                        Console.WriteLine("How many leg byes were scored?");
                        legByesScored = int.Parse(Console.ReadLine());
                    }

                    overBalls++;
                    if (runsScored != 0)
                    {
                        maiden = false;


                        //ensure the current bowler is less than 11 as the lists start from 0;
                        twoPlayers[currentBowler].RunsConceded = twoPlayers[currentBowler].RunsConceded + runsScored;

                        totalRuns = totalRuns + runsScored;

                        Console.WriteLine(totalRuns);

                        for (int i = 0; i < onePlayers.Count; i++)
                        {
                            if (onePlayers[i].Batting == true && onePlayers[i].Facing == true)
                            {
                                onePlayers[i].BallsFaced++;
                                onePlayers[i].RunsScored = onePlayers[i].RunsScored + runsScored;
                                if (runsScored == 4)
                                {
                                    onePlayers[i].Fours++;
                                }
                                if (runsScored == 6)
                                {
                                    onePlayers[i].Sixes++;
                                }
                                face = i;
                            }
                            if (onePlayers[i].Batting == true && onePlayers[i].Facing == false)
                            {
                                nonFace = i;
                            }
                            if (runsScored % 2 == 1)
                            {
                                onePlayers[face].Facing = false;
                                onePlayers[nonFace].Facing = true;
                            }

                        }
                        //runsScored = 0;
                    }

                    if (dotBall == true)
                    {
                        int dotFacing = 0;
                        int dotNonFacing = 0;
                        // set initially to 0 to avoid unassigned variable error
                        for (int i = 0; i < onePlayers.Count; i++)
                        {
                            if (onePlayers[i].Batting == true && onePlayers[i].Facing == true)
                            {
                                onePlayers[i].BallsFaced++;
                                dotFacing = i;
                            }
                            if (onePlayers[i].Batting == true && onePlayers[i].Facing == false)
                            {
                                dotNonFacing = i;
                            }
                        }
                        if (overBalls == 5)
                        {
                            onePlayers[dotFacing].Facing = false;
                            onePlayers[dotNonFacing].Facing = true;
                        }
                        dotBall = false;
                    }

                    if (byesScored != 0)
                    {
                        totalRuns = totalRuns + byesScored;
                        byesScored = 0;
                        totalByes = totalByes + byesScored;
                    }

                    if (legByesScored != 0)
                    {
                        totalRuns = totalRuns + legByesScored;
                        legByesScored = 0;
                        totalLegByes = totalLegByes + legByesScored;
                    }

                    if (wide == true)
                    {
                        overBalls = overBalls - 1;
                        totalWides++;
                        totalRuns++;
                        twoPlayers[currentBowler].Wides++;
                    }

                    if (noBall == true)
                    {
                        overBalls = overBalls - 1;
                        totalNoBall++;
                        totalRuns++;
                        twoPlayers[currentBowler].NoBalls++;
                    }

                    if (wicket == true)
                    {
                        if ((wayOut == "BOWLED") || (wayOut == "TIMEDOUT") || (wayOut == "STUMPED") || (wayOut == "LBW") || (wayOut == "HITTWICE"))
                        {
                            for (int i = 0; i < onePlayers.Count; i++)
                            {
                                if (onePlayers[i].Batting == true && onePlayers[i].Facing == true)
                                {
                                    onePlayers[i].Out = true;
                                    onePlayers[i].Batting = false;
                                    onePlayers[i].Facing = false;
                                }

                            }
                            onePlayers[nextBatsman].Batting = true;
                            onePlayers[nextBatsman].Facing = true;
                            nextBatsman++;
                            totalWickets++;
                        }
                        if (wayOut == "CAUGHT")
                        {
                            Console.WriteLine("Is the next batsman facing? (YES or NO)");
                            for (int i = 0; i < onePlayers.Count; i++)
                            {
                                if (onePlayers[i].Batting == true && onePlayers[i].Facing == true)
                                {
                                    onePlayers[i].Out = true;
                                    onePlayers[i].Batting = false;
                                    onePlayers[i].Facing = false;
                                }
                            }
                            onePlayers[nextBatsman].Batting = true;
                            if (Console.ReadLine() == "YES")
                            {
                                onePlayers[nextBatsman].Facing = true;

                            }
                            if (Console.ReadLine() == "NO")
                            {
                                onePlayers[nextBatsman].Facing = false;
                            }
                            nextBatsman++;
                            totalWickets++;
                        }
                        if (wayOut == "RUNOUT")
                        {
                            Console.WriteLine("Was the FACING or NOTFACING batsman run out?");
                            bool runOutFacing;
                            if (Console.ReadLine() == "FACING")
                            {
                                runOutFacing = true;
                            }
                            else if (Console.ReadLine() == "NOTFACING")
                            {
                                runOutFacing = false;
                            }
                            else
                            {
                                runOutFacing = false;
                                //avoids error of runOutFacing not being assigned for later use in the code to determine other things. 
                                // could use error checking to only accept "FACING" and "NOTFACING" or ask which one again later on?
                            }

                            Console.WriteLine("Will the new batsman be facing? (ENTER FACING or NOTFACING");
                            bool runOutNext;
                            if (Console.ReadLine() == "FACING")
                            {
                                runOutNext = true;
                            }
                            else if (Console.ReadLine() == "NOTFACING")
                            {
                                runOutNext = false;
                            }
                            else
                            {
                                runOutNext = false;
                                // avoids the error like the one above
                            }
                            int runOutOtherBat = 0;
                            if (runOutFacing == true)
                            {
                                for (int i = 0; i < onePlayers.Count; i++)
                                {
                                    if (onePlayers[i].Batting == true && onePlayers[i].Facing == true)
                                    {
                                        onePlayers[i].Out = true;
                                        onePlayers[i].Batting = false;
                                        onePlayers[i].Facing = false;
                                    }
                                    if (onePlayers[i].Batting == true && onePlayers[i].Facing == false)
                                    {
                                        runOutOtherBat = i;
                                    }
                                }
                                onePlayers[nextBatsman].Batting = true;
                                if (runOutNext == true)
                                {
                                    onePlayers[nextBatsman].Facing = true;
                                }
                                else if (runOutNext == false)
                                {
                                    onePlayers[nextBatsman].Facing = false;
                                    onePlayers[runOutOtherBat].Facing = true;
                                }

                                nextBatsman++;
                            }
                            if (runOutFacing == false)
                            {
                                for (int i = 0; i < onePlayers.Count; i++)
                                {
                                    if (onePlayers[i].Batting == true && onePlayers[i].Facing == false)
                                    {
                                        onePlayers[i].Out = true;
                                        onePlayers[i].Batting = false;
                                        onePlayers[i].Facing = false;
                                    }
                                    if (onePlayers[i].Batting == true && onePlayers[i].Facing == true)
                                    {
                                        runOutOtherBat = i;
                                    }
                                }
                                onePlayers[nextBatsman].Batting = true;
                                if (runOutNext == true)
                                {
                                    onePlayers[nextBatsman].Facing = true;
                                }
                                else if (runOutNext == false)
                                {
                                    onePlayers[nextBatsman].Facing = false;
                                }
                                nextBatsman++;
                            }
                            totalWickets++;
                        }


                        if (wayOut == "OBSTRUCTION")
                        {
                            Console.WriteLine("Was the FACING or NOTFACING batsman out for obstruction?");
                            bool obstructionFacing = false;
                            // set to false initially to avoid use of unassigned variable. 
                            if (Console.ReadLine() == "FACING")
                            {
                                obstructionFacing = true;
                            }
                            else if (Console.ReadLine() == "NOTFACING")
                            {
                                obstructionFacing = false;
                            }

                            Console.WriteLine("Will the new batsman be facing? (ENTER FACING or NOTFACING");
                            bool obstructionNext = false;
                            // set to false initially to avoid use of unassigned variable error
                            if (Console.ReadLine() == "FACING")
                            {
                                obstructionNext = true;
                            }
                            else if (Console.ReadLine() == "NOTFACING")
                            {
                                obstructionNext = false;
                            }
                            int obstructionOtherBat = 0;
                            // set to 0 initially to avoid use of unassigned variable error
                            if (obstructionFacing == true)
                            {
                                for (int i = 0; i < onePlayers.Count; i++)
                                {
                                    if (onePlayers[i].Batting == true && onePlayers[i].Facing == true)
                                    {
                                        onePlayers[i].Out = true;
                                        onePlayers[i].Batting = false;
                                        onePlayers[i].Facing = false;
                                    }
                                    if (onePlayers[i].Batting == true && onePlayers[i].Facing == false)
                                    {
                                        obstructionOtherBat = i;
                                    }
                                }
                                onePlayers[nextBatsman].Batting = true;
                                if (obstructionNext == true)
                                {
                                    onePlayers[nextBatsman].Facing = true;
                                }
                                else if (obstructionNext == false)
                                {
                                    onePlayers[nextBatsman].Facing = false;
                                    onePlayers[obstructionOtherBat].Facing = true;
                                }

                                nextBatsman++;

                            }
                            if (obstructionFacing == false)
                            {
                                for (int i = 0; i < onePlayers.Count; i++)
                                {
                                    if (onePlayers[i].Batting == true && onePlayers[i].Facing == false)
                                    {
                                        onePlayers[i].Out = true;
                                        onePlayers[i].Batting = false;
                                        onePlayers[i].Facing = false;
                                    }
                                    if (onePlayers[i].Batting == true && onePlayers[i].Facing == true)
                                    {
                                        obstructionOtherBat = i;
                                    }
                                }
                                onePlayers[nextBatsman].Batting = true;
                                if (obstructionNext == true)
                                {
                                    onePlayers[nextBatsman].Facing = true;
                                }
                                else if (obstructionNext == false)
                                {
                                    onePlayers[nextBatsman].Facing = false;
                                }
                                nextBatsman++;
                            }
                            totalWickets++;
                        }
                    }
                    if (overBalls == 6)
                    {
                        endOfOver = true;
                    }
                    //runsScored = 0;
                }
                onePlayers[face].Facing = false;
                onePlayers[nonFace].Facing = true;
                // end of the over changes defined here
                twoPlayers[currentBowler].OversBowled++;
                if (maiden == true)
                {
                    twoPlayers[currentBowler].Maidens++;
                }
                else if (maiden == false)
                {
                    maiden = true;
                }
                Console.WriteLine("Which number batsman is the next bowler?");
                currentBowler = int.Parse(Console.ReadLine());
            }
        }
    }
}
