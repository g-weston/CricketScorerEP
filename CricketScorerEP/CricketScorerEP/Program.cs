using System;
using System.Collections.Generic;

namespace CricketScorerEP
{
 /*   public class ActiveBatsmen
    {
        public int Facing { get; set; }
        public int NonFacing { get; set; }

        public ActiveBatsmen(int Facing, int nonFacing)
        {
            Facing = 0;
            NonFacing = 0;
        }
    }*/

    public static class Teams
    {
        public static List<Player> teamOnePlayers;
        public static List<Player> teamTwoPlayers;
        public static int currentBatsmanOne = 0, currentBatsmanTwo = 1;
        public static int batsmanFacing = currentBatsmanOne;
        public static int batsmanNotFacing = currentBatsmanTwo;
        public static int nextBatsman = currentBatsmanTwo + 1;
    }

    public static class Scorer
    {
        static void PopulateTeamFromFile(int teamNumber, string filename)
        {
            List<string> fileContents = new List<string>();
            System.IO.StreamReader file = new System.IO.StreamReader(filename);
            string line;
            while ((line = file.ReadLine()) != null)
            {
                fileContents.Add(line);
            }
            string clubName = fileContents[0];
            int.TryParse(fileContents[1], out int numberOfPlayers);
            string[] lineContents = { };
            string forename, surname, playerName;
            for (int i = 2; i < numberOfPlayers + 2; i++)
            {
                lineContents = fileContents[i].Split(' ');
                int.TryParse(lineContents[0], out int playerId);
                forename = lineContents[1];
                surname = lineContents[2];
                playerName = forename + ' ' + surname;
                if (teamNumber == 1)
                    Teams.teamOnePlayers.Add(new Player(clubName, playerId, playerName));
                else if (teamNumber == 2)
                    Teams.teamTwoPlayers.Add(new Player(clubName, playerId, playerName));
            }
            file.Close();
        }

        static void PopulateTeamFromConsole(int teamNumber)
        {
            string teamName= "one";
            if (teamNumber == 2)
                teamName = "two";

            Console.WriteLine("Please enter the name of team {0}", teamName);
            string clubName = Console.ReadLine();

            Console.WriteLine("Enter the number of players in {0}", clubName);
            string input = Console.ReadLine();
            int.TryParse(input, out int numberOfPlayers);

            string playerName;
            int playerId;
            for (int i = 0; i < numberOfPlayers; i++)
            {
                Console.WriteLine("Please enter the name of player {0} from {1}", i + 1, clubName);
                playerName = Console.ReadLine();
                Console.WriteLine("Please enter the id of {0} from {1}", playerName, clubName);
                playerId = int.Parse(Console.ReadLine());
                if (teamNumber == 1)
                    Teams.teamOnePlayers.Add(new Player(clubName, playerId, playerName));
                else if (teamNumber == 2)
                    Teams.teamTwoPlayers.Add(new Player(clubName, playerId, playerName));
            }
        }

        static void SwapFacingBatsmen(ref int facingBatsman, ref int nonFacingBatsman)
        {
            int temp = facingBatsman;
            facingBatsman = nonFacingBatsman;
            nonFacingBatsman = temp;
            return;
        }

        static void ReadMatchDetails(string filename)
        {
            List<string> fileContents = new List<string>();
            System.IO.StreamReader file = new System.IO.StreamReader(filename);

            string line;
            while ((line = file.ReadLine()) != null)
            {
                fileContents.Add(line);
            }
            Match.Date = DateTime.ParseExact(fileContents[0], "d", System.Globalization.CultureInfo.CurrentCulture);
            Match.Competition = fileContents[1];
            Match.Venue = fileContents[2];
            Match.Format = fileContents[3];
            Match.AgeGroup = fileContents[4];
            int.TryParse(fileContents[5], out int input);
            Match.ScheduledOvers = input;
            int.TryParse(fileContents[6], out input);
            Match.RunsPerWideOrNoBall = input;
            int.TryParse(fileContents[7], out input);
            Match.RebowlDeliveriesFromOver = input;

            file.Close();
        }

        static void RecordFallenWicket(int batsmanFacing, List<Player> teamOnePlayers, int nextBatsman, int currentBowler, int dismissingFielder, List<Player> teamTwoPlayers)
        {
            Teams.teamOnePlayers[batsmanFacing].DeliveriesFaced++;
            Teams.teamOnePlayers[batsmanFacing].IsOut = true;
            Teams.teamOnePlayers[batsmanFacing].HowOut = (int)Player.DismissalMethod.b;
            Teams.teamOnePlayers[batsmanFacing].DismissingBowler = currentBowler;
            Teams.teamTwoPlayers[currentBowler].NumberOfWicketsTaken++;
            batsmanFacing = nextBatsman;
            nextBatsman++;
        }

        public static void RecordRunsScored()
        {
            Innings.Runs += DeliveryRuns;
            // BallsFaced
        }

        public static char DeliveryWayOut;

        public static void RecordWicketTaken()
        {
            Innings.Wickets++;
        }

        static void WriteScorecard(string filename)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(filename);
            file.WriteLine("<html>");
            file.WriteLine("<body>");

            file.WriteLine("<pre>");
            file.WriteLine(Match.Venue);
            file.WriteLine("Name", "Runs", "4s", "6s", "Balls");
            for (int i = 0; i < Teams.teamOnePlayers.Count; i++)
            {
                if (Teams.teamOnePlayers[i].IsOut)
                {
                    file.WriteLine("{0}, Bowled {1}, {2}, {3}, {4}, {5}", Teams.teamOnePlayers[i].Name, Teams.teamTwoPlayers[Teams.teamOnePlayers[i].DismissingBowler].Name,
                                                                          Teams.teamOnePlayers[i].RunsScored, Teams.teamOnePlayers[i].NumberOfFoursScored,
                                                                          Teams.teamOnePlayers[i].NumberOfSixesScored, Teams.teamOnePlayers[i].DeliveriesFaced);
                }
                else
                {
                    file.WriteLine("{0}, Not Out {1}, {2}, {3}, {4}", Teams.teamOnePlayers[i].Name,
                                                                      Teams.teamOnePlayers[i].RunsScored, Teams.teamOnePlayers[i].NumberOfFoursScored,
                                                                      Teams.teamOnePlayers[i].NumberOfSixesScored, Teams.teamOnePlayers[i].DeliveriesFaced);
                }
            }
            file.WriteLine("Extras b({0}),lb ({1}),w ({2}),nb ({3}),   {4}", Innings.Byes, Innings.LegByes, Innings.Wides, Innings.NoBalls, 
                                                                            (Innings.Byes + Innings.LegByes + Innings.Wides + Innings.NoBalls));
            file.WriteLine("Total  {0}", Innings.Runs);
            file.WriteLine("</pre>");

            file.WriteLine("</body>");
            file.WriteLine("</html>");

            file.Close();

            return;
        }

        public static int BallsFaced = 0;
        public static int DeliveryRuns = 1;


        static void Main(string[] args)
        {
            Console.WriteLine("Read from file (F) or console (C)?");
            string inputChoice = Console.ReadLine();
            int team1 = 1, team2 = 2;
            if (inputChoice == "F")
            {
                PopulateTeamFromFile(team1, "TeamOneDefinition.txt");
                PopulateTeamFromFile(team2, "TeamTwoDefinition.txt");
                ReadMatchDetails("MatchDefinition.txt");
                Innings.ScheduledOvers = Match.ScheduledOvers;
            }
            else
            {
                PopulateTeamFromConsole(team1);
                PopulateTeamFromConsole(team2);
                Console.WriteLine("Please enter the number of overs in the Match");
                Innings.ScheduledOvers = int.Parse(Console.ReadLine());
            }
            // This is the "game play" section, should be a master "game play" method, with individual sub methods describing falls of wicket, runs etc.
            // think we should have a "Match" text file - with type of Match, number of overs, rules for wides & noballs
            // including number of runs per wide, and also which overs to rebowl from (=1 for senior cricket, =19 for junior cricket)
            // Presumably we want an "innings" method, which we can use twice, once for each team?

            int runsScoredThisDelivery = 0;
   /*         const int runsPerWide = 1;
            const int runsPerNoBall = 1;*/

            bool endOfOver = false; // perhaps an over struct, with members validDeliveries, end of Over?
            int validDeliveriesInThisOver, runsScoredThisOver;
            string howOut = "";

 /*           int currentBatsmanOne = 0, currentBatsmanTwo = 1;
            int batsmanFacing    = currentBatsmanOne;
            int batsmanNotFacing = currentBatsmanTwo;
            int nextBatsman = currentBatsmanTwo + 1;*/

            Console.WriteLine("Which player number is to bowl first?");
            string input = Console.ReadLine();
            int dismissingFielder = 0;
            int.TryParse(input, out int currentBowler);
            if (currentBowler > Teams.teamTwoPlayers.Count) // example error checking
            {
                Console.WriteLine("Not a valid player number");
            }

            for (int numberOversBowled = 0; numberOversBowled < Innings.ScheduledOvers; numberOversBowled++)
            {
                endOfOver = false;
                validDeliveriesInThisOver = 0;
                runsScoredThisOver = 0;
                while (endOfOver == false)
                {
                    Console.WriteLine("What happened on this ball? (Enter either R(UNS) D(OT) W(ICKET) N(OBALL) I (WIDE) B (BYES) or L (LEGBYES))");
                    string deliveryOutcome = Console.ReadLine();
                    switch (deliveryOutcome)
                    {
                        case "D": // DOT
                            runsScoredThisDelivery = 0;
 //                           recordRuns(batsmanFacing, teamOnePlayers, runsScoredThisDelivery);
                            
                            Teams.teamOnePlayers[Teams.batsmanFacing].DeliveriesFaced++;
                            validDeliveriesInThisOver++;
                            break;
                        case "R": // RUNS
                            Console.WriteLine("How many runs were scored on this delivery?");
                            input = Console.ReadLine();
                            int.TryParse(input, out runsScoredThisDelivery);

 //                           recordRuns(batsmanFacing, teamOnePlayers, teamTwoPlayers, runsScoredThisDelivery);
                            Teams.teamOnePlayers[Teams.batsmanFacing].RunsScored += runsScoredThisDelivery;
                            Teams.teamOnePlayers[Teams.batsmanFacing].DeliveriesFaced++;
                            Teams.teamTwoPlayers[currentBowler].RunsConceded += runsScoredThisDelivery;
                            Innings.Runs += runsScoredThisDelivery;

                            switch (runsScoredThisDelivery)
                            {
                            case 4: // what about the rare case of a run four, or overthrows?  This case needs to mean a boundary four.  Special app button?
                                    Teams.teamOnePlayers[Teams.batsmanFacing].NumberOfFoursScored++;
                                    break;
                            case 6:
                                    Teams.teamOnePlayers[Teams.batsmanFacing].NumberOfSixesScored++;
                                    break;
                            }
                            validDeliveriesInThisOver++;
                            break;
                        case "B": // BYES
                            Console.WriteLine("How many byes were scored?");
                            runsScoredThisDelivery = int.Parse(Console.ReadLine());
                           // recordByes(Innings, runsScoredThisDelivery);
                            Innings.Runs += runsScoredThisDelivery;
                            Innings.Byes += runsScoredThisDelivery;
                            validDeliveriesInThisOver++;
                            break;
                        case "L": // LEGBYES
                            Console.WriteLine("How many leg byes were scored?");
                            runsScoredThisDelivery = int.Parse(Console.ReadLine());
                            // recordLegByes(Innings, runsScoredThisDelivery);
                            Innings.Runs    += runsScoredThisDelivery;
                            Innings.LegByes += runsScoredThisDelivery;
                            validDeliveriesInThisOver++;
                            break;
                        case "N":  // NOBALL
                            Console.WriteLine("How many extra runs were scored?");  // TO DO distinguish between runs off the bat and "byes"
                            // TO DO keep track of "extra" runs to work out who is facing
                            runsScoredThisDelivery = Match.RunsPerWideOrNoBall + int.Parse(Console.ReadLine());
                            // recordNoBalls(Innings, runsScoredThisDelivery);
                            Innings.NoBalls += runsScoredThisDelivery;
                            Innings.Runs += runsScoredThisDelivery;
                            Teams.teamTwoPlayers[currentBowler].NoBallsDelivered++;
                            break;
                        case "I": // WIDE
                            Console.WriteLine("How many extra wides were conceded?"); // TO DO keep track of "extra" runs to work out who is facing
                            runsScoredThisDelivery = Match.RunsPerWideOrNoBall + int.Parse(Console.ReadLine());
                            Innings.Wides += runsScoredThisDelivery;
                            Innings.Runs += runsScoredThisDelivery;
                            Teams.teamTwoPlayers[currentBowler].WidesConceded++;
                            break;
                        case "W": // WICKET
                            // Lots of logic which is associated with a wicket - will need several methods (think about all there is to do in a scorebook when a wicket falls)
                            // TO DO Have a wicketFallen method, with sub-methods for each dismissal type
                            validDeliveriesInThisOver++;
                            Console.WriteLine("How was the batsman out? (Enter B(OWLED) C(AUGHT) R(UNOUT) L(BW) S(TUMPED) H(IT WICKET) O(BSTRUCTION) T(IMED OUT) I (HIT TWICE) A (HANDLED BALL))");
                            howOut = Console.ReadLine();
                            if ((howOut == "B") || (howOut == "L") || (howOut == "T") || (howOut == "S") || (howOut == "H")) // BOWLED, LBW, TIMED OUT, STUMPED, HIT WICKET and handledball?
                            {
                                // TO DO Need a generic "WicketFallen" method here
                                // recordFallenWicket(dismissalMethod, batsmanFacing, nextBatsman, teamOnePlayers, currentBowler, teamTwoPlayers)
                             /*   teamOnePlayers[batsmanFacing].DeliveriesFaced++;
                                teamOnePlayers[batsmanFacing].IsOut = true;
                                teamOnePlayers[batsmanFacing].HowOut = (int)Player.DismissalMethod.b;
                                teamOnePlayers[batsmanFacing].DismissingBowler = currentBowler;
                                teamTwoPlayers[currentBowler].NumberOfWicketsTaken++;
                                batsmanFacing = nextBatsman;
                                nextBatsman++;*/
                                Innings.Wickets++;
                            }
                            if (howOut == "C") // CAUGHT
                            {
                                // TO DO method "DismissalCaught" - could reuse the "wicketfallen" method for much of the syntax
                                Console.WriteLine("Which number player caught the batsman?");
                                input = Console.ReadLine();
                                int.TryParse(input, out dismissingFielder);
                                // recordFallenWicket(batsmanFacing, teamOnePlayers, currentBowler, dismissingFielder, teamTwoPlayers)
                                Teams.teamOnePlayers[Teams.batsmanFacing].DeliveriesFaced++;
                                Teams.teamOnePlayers[Teams.batsmanFacing].IsOut = true;
                                Teams.teamOnePlayers[Teams.batsmanFacing].DismissingBowler = currentBowler;
                                Teams.teamOnePlayers[Teams.batsmanFacing].DismissingFielder = dismissingFielder;
                                Teams.teamTwoPlayers[currentBowler].NumberOfWicketsTaken++;
                                Teams.batsmanFacing = Teams.nextBatsman;
                                Teams.nextBatsman++;
                                Innings.Wickets++;
                                
                                Console.WriteLine("Is the next batsman facing? (YES or NO)"); // how do we work out if the batsmen crossed while the ball was in the air?
                                string newBatFace = Console.ReadLine();
                                if (newBatFace == "NO")
                                {
                                    SwapFacingBatsmen(ref Teams.batsmanFacing, ref Teams.batsmanNotFacing);
                                }
                            }
                            if (howOut == "R" || howOut == "O" || howOut == "A") // RUN OUT, OBSTRUCTION, HANDLED BALL 
                                                                                    // TO DO Why is handledball the same as runout?  They wouldn't change ends?  Handledball same as caught?
                            {
                                Console.WriteLine("Which number player ran the batsman out?");
                                input = Console.ReadLine();
                                int.TryParse(input, out dismissingFielder);
                                Teams.teamOnePlayers[Teams.batsmanFacing].DeliveriesFaced++;
                                Console.WriteLine("Was the F(ACING) or N(OT FACING) batsman out?");
                                string runOutBatsman = Console.ReadLine();
                                if (runOutBatsman == "F") // FACING.  TO DO how to count runs, for example when they are run out on the second run?
                                {
                                    Teams.teamOnePlayers[Teams.batsmanFacing].IsOut = true;
                                    Teams.teamOnePlayers[Teams.batsmanFacing].DismissingFielder = dismissingFielder;
                                }
                                else if (runOutBatsman == "N") // NOT FACING
                                {
                                    Teams.teamOnePlayers[Teams.batsmanNotFacing].IsOut = true;
                                    Teams.teamOnePlayers[Teams.batsmanNotFacing].DismissingFielder = dismissingFielder;
                                }

                                Console.WriteLine("Will the new batsman be facing? (Enter F(ACING) or N(OT FACING)");
                                Teams.batsmanFacing = Teams.nextBatsman;
                                string newFacing = Console.ReadLine();
                                if (newFacing == "N") // NOT FACING
                                {
                                    SwapFacingBatsmen(ref Teams.batsmanFacing, ref Teams.batsmanNotFacing);
                                }
                                Teams.teamTwoPlayers[currentBowler].NumberOfWicketsTaken++;
                                Teams.nextBatsman++;
                                Innings.Wickets++;
                            }
                            break;
                    }
                    if (runsScoredThisDelivery % 2 == 1) // TO DO This is fine, except for when we have a short run.  
                    // When we have wide or noball, we need to ensure we are looking at the extra runs only for the purposes of changing ends.
                    {
                        SwapFacingBatsmen(ref Teams.batsmanFacing, ref Teams.batsmanNotFacing);
                    }
                    runsScoredThisOver += runsScoredThisDelivery;
                    // TO DO perhaps consolidate the swap batsmen ends here, to avoid having the logic on both runs and wickets?

                    if (validDeliveriesInThisOver == 6)
                    {
                        SwapFacingBatsmen(ref Teams.batsmanFacing, ref Teams.batsmanNotFacing); // change of ends
                        Teams.teamTwoPlayers[currentBowler].NumberOfOversBowled++;
                        if (runsScoredThisOver == 0)
                        {
                            Teams.teamTwoPlayers[currentBowler].NumberOfMaidensBowled++;
                        }

                        Console.WriteLine("Which number player is the next bowler?"); // TO DO - perhaps store currentBowler1 and currentBowler2; avoids clicks if they bowl for a spell
                        input = Console.ReadLine();
                        int.TryParse(input, out currentBowler);
                        if (currentBowler > Teams.teamTwoPlayers.Count)
                        {
                            Console.WriteLine("Not a valid player number");
                        }
                        endOfOver = true;
                    }
                }
            }
        // Here is the scorecard - base it on play-cricket
        // need a WriteScorecard() method, and perhaps WriteBatsman and WriteBowler methods which are invoked in a loop?
        //  Need a game object to cover venue, time of match, format of match, league/cup/friendly etc.  (see file GameDefinition.txt)
            WriteScorecard("Scorecard.html");

            // Testing - 
            // (1) Run through all options in the program (wickets, runs, byes, noballs + runs etc. etc.)
            // (2) Take a paper scorebook from this season, and follow it through the game
            // (3) Score a real match!
        }
  
    }
}
