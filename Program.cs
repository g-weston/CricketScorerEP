using System;
using System.Collections.Generic;

namespace Scorer
{
    public class ActiveBatsmen
    {
        public int Facing { get; set; }
        public int NonFacing { get; set; }

        public ActiveBatsmen(int Facing, int nonFacing)
        {
            Facing = 0;
            NonFacing = 0;
        }
    }

    class Program
    {
        static void PopulateTeamFromFile(string filename, List<Player> playerList)
        {
            List<string> fileContents = new List<string>();
            System.IO.StreamReader file = new System.IO.StreamReader(filename);
            string line;
            while ((line = file.ReadLine()) != null)
            {
                fileContents.Add(line);
            }
            string clubName = fileContents[0];
            int numberOfPlayers;
            int.TryParse(fileContents[1], out numberOfPlayers);
            string[] lineContents = { };
            string forename, surname, playerName;
            int playerId, startingScore = 0;
            bool isFacing = false;

            for (int i = 2; i < numberOfPlayers + 2; i++)
            {
                lineContents = fileContents[i].Split(' ');
                int.TryParse(lineContents[0], out playerId);
                forename = lineContents[1];
                surname = lineContents[2];
                playerName = forename + ' ' + surname;
                playerList.Add(new Player(clubName, playerId, playerName, startingScore, isFacing));
            }
        }

        static void PopulateTeamFromConsole(string teamName, List<Player> playerList)
        {
            Console.WriteLine("Please enter the name of team {0}", teamName);
            string clubName = Console.ReadLine();

            Console.WriteLine("Enter the number of players in {0}", clubName);
            int numberOfPlayers;
            string input = Console.ReadLine();
            int.TryParse(input, out numberOfPlayers);

            string playerName;
            int playerId, startingScore = 0;
            bool isFacing = false;
            for (int i = 0; i < numberOfPlayers; i++)
            {
                Console.WriteLine("Please enter the name of player {0} from {1}", i + 1, clubName);
                playerName = Console.ReadLine();
                Console.WriteLine("Please enter the id of {0} from {1}", playerName, clubName);
                playerId = int.Parse(Console.ReadLine());
                startingScore = 0;
                isFacing = false;
                playerList.Add(new Player(clubName, playerId, playerName, startingScore, isFacing)); 
            }
        }

        static void SwapFacingBatsmen(ref int facingBatsman, ref int nonFacingBatsman)
        {
            int temp = facingBatsman;
            facingBatsman = nonFacingBatsman;
            nonFacingBatsman = temp;
            return;
        }

        static void Main(string[] args)
        {
            // struct opening and continuing as a struct out of a list not in one. creating errors in toher parth of the code
            List<Player> teamOnePlayers = new List<Player>();
            List<Player> teamTwoPlayers = new List<Player>();

            Console.WriteLine("Read from file (F) or console (C)?");
            string inputChoice = Console.ReadLine();

            if (inputChoice == "F")
            {
                PopulateTeamFromFile("TeamOneDefinition.txt", teamOnePlayers);
                PopulateTeamFromFile("TeamTwoDefinition.txt", teamTwoPlayers);
            }
            else
            {
                PopulateTeamFromConsole("one", teamOnePlayers);
                PopulateTeamFromConsole("two", teamTwoPlayers);
            }
            // This is the "game play" section, should be a master "game play" method, with individual sub methods describing falls of wicket, runs etc.
            // think we should have a "match" text file - with type of match, number of overs, rules for wides & noballs
            // including number of runs per wide, and also which overs to rebowl from (=1 for senior cricket, =19 for junior cricket)
            // Presumably we want an "innings" method, which we can use twice, once for each team?

            var innings = new Innings();
            Console.WriteLine("Please enter the number of overs in the match");
            innings.ScheduledOvers = int.Parse(Console.ReadLine());
   
            int runsScoredThisDelivery = 0;
            const int runsPerWide = 1;
            const int runsPerNoBall = 1;

            bool endOfOver = false; // perhaps an over struct, with members validDeliveries, end of Over?
            int validDeliveriesInThisOver, runsScoredThisOver;
            bool wicketFallen = false;
            string howOut = "";

            int currentBatsmanOne = 0, currentBatsmanTwo = 1;
            int batsmanFacing    = currentBatsmanOne;
            int batsmanNotFacing = currentBatsmanTwo;
            int nextBatsman = currentBatsmanTwo + 1;

            Console.WriteLine("Which player number is to bowl first?");
            string input = Console.ReadLine();
            int currentBowler;
            int.TryParse(input, out currentBowler);
            if (currentBowler > teamTwoPlayers.Count) // example error checking
            {
                Console.WriteLine("Not a valid player number");
            }

            for (int numberOversBowled = 0; numberOversBowled < innings.ScheduledOvers; numberOversBowled++)
            {
                endOfOver = false;
                validDeliveriesInThisOver = 0;
                runsScoredThisOver = 0;
                while (endOfOver == false)
                {
                    Console.WriteLine("What happened on this ball? (Enter either R(UNS) D(OT) W(ICKET) N(OBALL) W(IDE) B(YES) or L(EGBYES))");
                    string deliveryOutcome = Console.ReadLine();
                    switch (deliveryOutcome)
                    {
                        case "DOT":
                            runsScoredThisDelivery = 0;
                            teamOnePlayers[batsmanFacing].DeliveriesFaced++;
                            validDeliveriesInThisOver++;
                            break;
                        case "RUNS":
                            Console.WriteLine("How many runs were scored on this delivery?");
                            input = Console.ReadLine();
                            int.TryParse(input, out runsScoredThisDelivery);

                            teamOnePlayers[batsmanFacing].RunsScored += runsScoredThisDelivery;
                            teamOnePlayers[batsmanFacing].DeliveriesFaced++;
                            teamTwoPlayers[currentBowler].RunsConceded += runsScoredThisDelivery;
                            innings.Runs += runsScoredThisDelivery;

                            Console.WriteLine("Total runs {0}, wickets {1}, overs {2}", innings.Runs, innings.Wickets, innings.Overs);

                            switch (runsScoredThisDelivery)
                            {
                            case 4: // what about the rare case of a run four, or overthrows?  This case needs to mean a boundary four.  Special app button?
                                    teamOnePlayers[batsmanFacing].NumberOfFoursScored++;
                                    break;
                            case 6:
                                    teamOnePlayers[batsmanFacing].NumberOfSixesScored++;
                                    break;
                            }

                            if (runsScoredThisDelivery % 2 == 1) // TO DO This is fine, except for when we have a short run
                            {
                                SwapFacingBatsmen(ref batsmanFacing, ref batsmanNotFacing);
                            }
                            validDeliveriesInThisOver++;
                            break;
                        case "BYES":
                            Console.WriteLine("How many byes were scored?");
                            runsScoredThisDelivery = int.Parse(Console.ReadLine());
                            innings.Runs += runsScoredThisDelivery;
                            innings.Byes += runsScoredThisDelivery;
                            validDeliveriesInThisOver++;
                            break;
                        case "LEGBYES":
                            Console.WriteLine("How many leg byes were scored?");
                            runsScoredThisDelivery = int.Parse(Console.ReadLine());
                            innings.Runs    += runsScoredThisDelivery;
                            innings.LegByes += runsScoredThisDelivery;
                            validDeliveriesInThisOver++;
                            break;
                        case "WICKET":
                            wicketFallen = true;
                            // Lots of logic which is associated with a wicket - will need several methods (think about all there is to do in a scorebook when a wicket falls)
                            // TO DO Have a wicketFallen method, with sub-methods for each dismissal type
                            Console.WriteLine("How was the batsman out? (Enter BOWLED CAUGHT RUNOUT LBW STUMPED HITWICKET OBSTRUCTION TIMEDOUT HITTWICE HANDLEDBALL)");
                            howOut = Console.ReadLine();
                            validDeliveriesInThisOver++;
                            break;
                        case "NOBALL":
                            Console.WriteLine("How many extra runs were scored?");  // TO DO distinguish between runs off the bat and "byes"
                            runsScoredThisDelivery = runsPerNoBall + int.Parse(Console.ReadLine());
                            innings.NoBalls += runsScoredThisDelivery;
                            innings.Runs    += runsScoredThisDelivery;
                            teamTwoPlayers[currentBowler].NoBallsDelivered++;
                            break;
                        case "WIDE":
                            Console.WriteLine("How many extra wides were conceded?");
                            runsScoredThisDelivery = runsPerWide + int.Parse(Console.ReadLine());
                            innings.Wides += runsScoredThisDelivery;
                            innings.Runs  += runsScoredThisDelivery;
                            teamTwoPlayers[currentBowler].WidesConceded++;
                            break;
                    }

                    if (wicketFallen)
                    {
                        if ((howOut == "BOWLED") || (howOut == "TIMEDOUT") || (howOut == "STUMPED") || (howOut == "LBW") || (howOut == "HITTWICE")) // and handledball?
                        {
                            // TO DO Need a generic "WicketFallen" method here
                            teamOnePlayers[batsmanFacing].DeliveriesFaced++;
                            teamOnePlayers[batsmanFacing].IsOut = true;
                            batsmanFacing = nextBatsman;
                            teamTwoPlayers[currentBowler].NumberOfWicketsTaken++;
                            nextBatsman++;
                            innings.Wickets++;
                        }
                        if (howOut == "CAUGHT")
                        {
                            // TO DO method "DismissalCaught" - could reuse the "wicketfallen" method for much of the syntax
                            teamOnePlayers[batsmanFacing].DeliveriesFaced++;
                            teamOnePlayers[batsmanFacing].IsOut = true;
                            batsmanFacing = nextBatsman;
                            Console.WriteLine("Is the next batsman facing? (YES or NO)"); // how do we work out if the batsmen crossed while the ball was in the air?
                            string newBatFace = Console.ReadLine();
                            if (newBatFace == "NO")
                            {
                                SwapFacingBatsmen(ref batsmanFacing, ref batsmanNotFacing);
                            }
                            teamTwoPlayers[currentBowler].NumberOfWicketsTaken++;
                            nextBatsman++;
                            innings.Wickets++; 
                        }
                        if (howOut == "RUNOUT" || howOut == "OBSTRUCTION" || howOut == "HANDLEDBALL") // TO DO Why is handledball the same as runout?  They wouldn't change ends?  Handledball same as caught?
                        {
                            teamOnePlayers[batsmanFacing].DeliveriesFaced++;
                            Console.WriteLine("Was the FACING or NOTFACING batsman out?");
                            string runOutBatsman = Console.ReadLine();
                            if (runOutBatsman == "FACING") // TO DO how to count runs, for example when they are run out on the second run?
                            {
                                teamOnePlayers[batsmanFacing].IsOut = true;
                            }
                            else if (runOutBatsman == "NOTFACING")
                            {
                                teamOnePlayers[batsmanNotFacing].IsOut = true;
                            }

                            Console.WriteLine("Will the new batsman be facing? (Enter FACING or NOTFACING");
                            batsmanFacing = nextBatsman;
                            string newFacing = Console.ReadLine();
                            if (newFacing == "NOTFACING")
                            {
                                SwapFacingBatsmen(ref batsmanFacing, ref batsmanNotFacing);
                            }
                            teamTwoPlayers[currentBowler].NumberOfWicketsTaken++;
                            nextBatsman++;
                            innings.Wickets++;  
                        }
                        wicketFallen = false;
                    }
                    runsScoredThisOver += runsScoredThisDelivery;
                    if (validDeliveriesInThisOver == 6)
                    {
                        SwapFacingBatsmen(ref batsmanFacing, ref batsmanNotFacing); // change of ends
                        teamTwoPlayers[currentBowler].NumberOfOversBowled++;
                        if (runsScoredThisOver == 0)
                        {
                            teamTwoPlayers[currentBowler].NumberOfMaidensBowled++;
                        }

                        Console.WriteLine("Which number player is the next bowler?");
                        input = Console.ReadLine();
                        int.TryParse(input, out currentBowler);
                        if (currentBowler > teamTwoPlayers.Count)
                        {
                            Console.WriteLine("Not a valid player number");
                        }
                        endOfOver = true;
                    }
                }
            }
            Console.WriteLine("Total byes {0}, legbyes {1}, wides {2}, noballs {3}", innings.Byes, innings.LegByes, innings.Wides, innings.NoBalls);
        }
    }
}
