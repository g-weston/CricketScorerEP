using System;
// using System.Web.UI;
using System.Collections.Generic;
using Android.App;
using Xamarin.Forms;

namespace CricketScorerEP
{
    public static class Scorer
    {
        public static int BallsFaced = 0;
        public static int DeliveryRuns = 1;
        public static int ByeRuns = 1;
        public static int LegByeRuns = 1;
        public static int validDeliveriesInThisOver = 0;
        public static bool maiden = true;
        public static int ByesOffWide;
        public static int ByesOffNoBall;
        public static int LegByesOffNoBall;

        public static void RecordFallenWicket()
        {
            Teams.teamOnePlayers[Teams.batsmanFacing].DeliveriesFaced++;
            Teams.teamOnePlayers[Teams.batsmanFacing].IsOut = true;
            //Teams.teamOnePlayers[batsmanFacing].HowOut = (int)Player.DismissalMethod.b;
            
            Innings.Wickets++;
            //Teams.teamTwoPlayers[currentBowler].NumberOfWicketsTaken++;

            switch (Teams.teamOnePlayers[Teams.batsmanFacing].DismissalMethod)
            {
                case ("b"):
                    Teams.teamTwoPlayers[Teams.currentBowler].NumberOfWicketsTaken++;
                    Teams.teamOnePlayers[Teams.batsmanFacing].DismissingBowler = Teams.currentBowler;
                    break;
                case ("c"):
                    Teams.teamTwoPlayers[Teams.currentBowler].NumberOfWicketsTaken++;
                    Teams.teamOnePlayers[Teams.batsmanFacing].DismissingBowler = Teams.currentBowler;
                    Teams.teamOnePlayers[Teams.batsmanFacing].DismissingFielder = Teams.wicketFielder;
                    break;

                case ("l"):
                    Teams.teamTwoPlayers[Teams.currentBowler].NumberOfWicketsTaken++;
                    Teams.teamOnePlayers[Teams.batsmanFacing].DismissingBowler = Teams.currentBowler;
                    break;
                case ("ro"):
                    Teams.teamOnePlayers[Teams.batsmanFacing].DismissingFielder = Teams.wicketFielder;
                    break;
                case ("s"):
                    Teams.teamTwoPlayers[Teams.currentBowler].NumberOfWicketsTaken++;
                    Teams.teamOnePlayers[Teams.batsmanFacing].DismissingBowler = Teams.currentBowler;
                    //Teams.teamOnePlayers[Teams.batsmanFacing].DismissingFielder = keeper;
                    // use the player listed as the keeper to take the dismissing fielder input
                    break;
                case ("hw"):
                    break;
                case ("hb"):
                    break;
                case ("o"):
                    break;
                case ("ht"):
                    break;
                case ("to"):
                    break;
            }
            Teams.batsmanFacing = Teams.nextBatsman;
            
        }

        public static void RecordRunsScored()
        {
            Innings.Runs += DeliveryRuns;
            Teams.teamTwoPlayers[Teams.currentBowler].RunsConceded += DeliveryRuns;
        }

        public static void RecordByesScored()
        {
            Innings.Byes += ByeRuns;
            Innings.Runs += ByeRuns;
        }

        public static void RecordLegByesScored()
        {
            Innings.LegByes += LegByeRuns;
            Innings.Runs += LegByeRuns;
        }

        public static void RecordNoBall()
        {
            Innings.Runs++;
            Innings.NoBalls++;
            Teams.teamTwoPlayers[Teams.currentBowler].RunsConceded++;
        }

        public static void RecordRunsOffNoBall()
        {
            Innings.Runs += DeliveryRuns;
            Teams.teamTwoPlayers[Teams.currentBowler].RunsConceded += DeliveryRuns;
        }

        public static void RecordByesOffNoBall()
        {
            
            Innings.Runs += ByesOffNoBall;
            Innings.NoBalls += ByesOffNoBall;
            Teams.teamTwoPlayers[Teams.currentBowler].RunsConceded += ByesOffNoBall;
            Teams.teamTwoPlayers[Teams.currentBowler].NoBallsDelivered += ByesOffNoBall;
        }

        public static void RecordLegByesOffNoBall()
        {
            
            Innings.Runs += LegByesOffNoBall;
            Innings.NoBalls += LegByesOffNoBall;
            Teams.teamTwoPlayers[Teams.currentBowler].RunsConceded += LegByesOffNoBall;
            Teams.teamTwoPlayers[Teams.currentBowler].NoBallsDelivered += LegByesOffNoBall;
        }

        public static void RecordWide()
        {
            Innings.Runs++;
            Teams.teamTwoPlayers[Teams.currentBowler].RunsConceded++;
            Teams.teamTwoPlayers[Teams.currentBowler].WidesConceded++;
            Innings.Wides++;
        }

        public static void RecordByesOffWide()
        {
            Innings.Runs += ByesOffWide;
            Innings.Wides += ByesOffWide;
            Teams.teamTwoPlayers[Teams.currentBowler].RunsConceded += ByesOffWide;
            Teams.teamTwoPlayers[Teams.currentBowler].WidesConceded += ByesOffWide;
        }

        public static void BowledWicket()
        {

        }
        /*
        async void ChangeBowler(object sender, EventArgs e)
        {

            
            Picker bowlerPicker = new Picker
            {
                Title = "Next Bowler",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            for (int i = 0; i < Teams.teamTwoPlayers.Count; i++)
            {
                bowlerPicker.Items.Add(Teams.teamTwoPlayers[i].Name);
            }

            bowlerPicker.SelectedIndexChanged += (sender, args) =>
            {
                if (bowlerPicker.SelectedIndex == -1)
                {
                    //boxView.Color = Color.Default;
                }
                else
                {
                    string newBowler = bowlerPicker.Items[bowlerPicker.SelectedIndex];
                    //boxView.Color = nameToColor[colorName];
                    
                }
            };

        }
        */

        public static void ChangeBowler()
        {
            int nextBowlerNumber = 0;
            for (int i = Teams.teamTwoPlayers.Count - 1; i > 0; i--)
            {
                if (Teams.teamTwoPlayers[i].Name == FirstPage.nextBowler)
                {
                    nextBowlerNumber = i;
                }
            }
            Teams.currentBowler = nextBowlerNumber;
        }

        public static void ChangeBatsman()
        {
            int nextBatsmanNumber = 0;
            for (int i = Teams.teamTwoPlayers.Count - 1; i > 0; i--)
            {
                if (Teams.teamTwoPlayers[i].Name == FirstPage.nextBatsmanName)
                {
                    nextBatsmanNumber = i;
                }
            }
            Teams.nextBatsman = nextBatsmanNumber;
        }

        public static void UpdateBowlerBalls()
        {
            Teams.teamTwoPlayers[Teams.currentBowler].NumberOfOversBowled += 0.1;
            validDeliveriesInThisOver++;
            Innings.Overs += 0.1;
            if (validDeliveriesInThisOver == 6)
            {
                Teams.teamTwoPlayers[Teams.currentBowler].NumberOfOversBowled += 0.4;
                Innings.Overs += 0.4;
                Innings.CompleteOvers++;
                ChangeBowler();
            }
        }

        public static char DeliveryWayOut;

        /*
        public static void RecordWicketTaken()
        {
            Innings.Wickets++;
            // needs to only add it to the bowler if its the bowlers wicket
            Teams.teamTwoPlayers[Teams.currentBowler].NumberOfWicketsTaken++;

        }*/



        /*static void Main(string[] args)
        {
            /*         Console.WriteLine("Read from file (F) or console (C)?");
                     string inputChoice = Console.ReadLine();
                     int team1 = 1, team2 = 2;
                     if (inputChoice == "F")
                     {
                         ScorerIO.WriteScorecard();
                         ScorerIO.PopulateTeamFromFile(team2);
                         ScorerIO.ReadMatchDetails("MatchDefinition.txt");
                         Innings.ScheduledOvers = Match.ScheduledOvers;
                     }
                     else
                     {
                         ScorerIO.PopulateTeamFromConsole(team1);
                         ScorerIO.PopulateTeamFromConsole(team2);
                         Console.WriteLine("Please enter the number of overs in the Match");
                         Innings.ScheduledOvers = int.Parse(Console.ReadLine());
                     }*/

        // This is the "game play" section, should be a master "game play" method, with individual sub methods describing falls of wicket, runs etc.
        // think we should have a "Match" text file - with type of Match, number of overs, rules for wides & noballs
        // including number of runs per wide, and also which overs to rebowl from (=1 for senior cricket, =19 for junior cricket)
        // Presumably we want an "innings" method, which we can use twice, once for each team?

        //     int runsScoredThisDelivery = 0;
        /*         const int runsPerWide = 1;
                 const int runsPerNoBall = 1;*/

        //          bool endOfOver = false; // perhaps an over struct, with members validDeliveries, end of Over?
        //       int validDeliveriesInThisOver;
        //         int runsScoredThisOver;
        //       string howOut = "";

        /*           int currentBatsmanOne = 0, currentBatsmanTwo = 1;
                   int batsmanFacing    = currentBatsmanOne;
                   int batsmanNotFacing = currentBatsmanTwo;
                   int nextBatsman = currentBatsmanTwo + 1;*/

        /*          Console.WriteLine("Which player number is to bowl first?");
                  string input = Console.ReadLine();
                  int dismissingFielder = 0;
                  int.TryParse(input, out int currentBowler);
                  if (currentBowler > Teams.teamTwoPlayers.Count) // example error checking
                  {
                      Console.WriteLine("Not a valid player number");
                  }*/
        /*
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
                            nextBatsman++;
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
                                Teams.SwapFacingBatsmen(ref Teams.batsmanFacing, ref Teams.batsmanNotFacing);
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
                                Teams.SwapFacingBatsmen(ref Teams.batsmanFacing, ref Teams.batsmanNotFacing);
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
                    Teams.SwapFacingBatsmen(ref Teams.batsmanFacing, ref Teams.batsmanNotFacing);
                }
                runsScoredThisOver += runsScoredThisDelivery;
                // TO DO perhaps consolidate the swap batsmen ends here, to avoid having the logic on both runs and wickets?

                // Static void EndOfOver()
                if (validDeliveriesInThisOver == 6)
                {
                    Teams.SwapFacingBatsmen(ref Teams.batsmanFacing, ref Teams.batsmanNotFacing); // change of ends
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
        //  
        //   ScorerIO.WriteScorecard(); 

        // Testing - 
        // (1) Run through all options in the program (wickets, runs, byes, noballs + runs etc. etc.)
        // (2) Take a paper scorebook from this season, and follow it through the game
        // (3) Score a real match!
    }*/

    }
}
