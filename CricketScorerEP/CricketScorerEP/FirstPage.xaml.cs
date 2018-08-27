using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace CricketScorerEP
{
	public partial class FirstPage : ContentPage, INotifyPropertyChanged
    {
        public FirstPage()
		{
			InitializeComponent();
		    BindingContext = this;
            // TODO check that Match and Teams are not null - bale out if so
        }

        private async void NavigateMainPage()
        {
            await Navigation.PushAsync(new MainPage());
        }

        // TODO Need to extract this block of display strings to a separate file probably, as part of the partial class, in order to improve readability
        private string teamNameHeader = Match.HomeTeam;
        public string TeamNameHeader
       {
            get
            {
                return teamNameHeader;
            }
            set
            {
                if (teamNameHeader != value)
                {
                    teamNameHeader = value;
                    this.OnPropertyChanged("TeamNameHeader");
                }
            }
        }

        private string oversHeader = Innings.Overs.ToString();
        public string OversHeader
        {
            get
            {
                return oversHeader;
            }
            set
            {
                if (oversHeader != value)
                {
                    oversHeader = value;
                    this.OnPropertyChanged("OversHeader");
                }
            }
        }

        private string scoreHeader = Innings.Runs.ToString() + "-" + Innings.Wickets.ToString();
        public string ScoreHeader
        {
            get
            {
                return scoreHeader;
            }
            set
            {
                if (scoreHeader != value)
                {
                    scoreHeader = value;
                    this.OnPropertyChanged("ScoreHeader");
                }
            }
        }

        private string batsmanOne = Teams.teamOnePlayers[Teams.currentBatsmanOne].Name + "*";
        public string BatsmanOne
        {
            get
            {
                return batsmanOne;
            }
            set
            {
                if (batsmanOne != value)
                {
                    batsmanOne = value;
                    this.OnPropertyChanged("BatsmanOne");
                }
            }
        }

        private string batsmanOneRuns = Teams.teamOnePlayers[Teams.currentBatsmanOne].RunsScored.ToString();
        public string BatsmanOneRuns
        {
            get
            {
                return batsmanOneRuns;
            }
            set
            {
                if (batsmanOneRuns != value)
                {
                    batsmanOneRuns = value;
                    this.OnPropertyChanged("BatsmanOneRuns");
                }
            }
        }

        private string batsmanTwo = Teams.teamOnePlayers[Teams.currentBatsmanTwo].Name;
        public string BatsmanTwo
        {
            get
            {
                return batsmanTwo;
            }
            set
            {
                if (batsmanTwo != value)
                {
                    batsmanTwo = value;
                    this.OnPropertyChanged("BatsmanTwo");
                }
            }
        }

        private string batsmanTwoRuns = Teams.teamOnePlayers[Teams.currentBatsmanTwo].RunsScored.ToString();
        public string BatsmanTwoRuns
        {
            get
            {
                return batsmanTwoRuns;
            }
            set
            {
                if (batsmanTwoRuns != value)
                {
                    batsmanTwoRuns = value;
                    this.OnPropertyChanged("BatsmanTwoRuns");
                }
            }
        }

        private string currentBowler = Teams.teamTwoPlayers[Teams.currentBowler].Name;
        public string CurrentBowler
        {
            get
            {
                return currentBowler;
            }
            set
            {
                if (currentBowler != value)
                {
                    currentBowler = value;
                    this.OnPropertyChanged("CurrentBowler");
                }
            }
        }

        private string bowlerFigures = Teams.teamTwoPlayers[Teams.currentBowler].NumberOfOversBowled.ToString() + "-" +
                                       Teams.teamTwoPlayers[Teams.currentBowler].NumberOfMaidensBowled.ToString() + "-" +
                                       Teams.teamTwoPlayers[Teams.currentBowler].RunsConceded.ToString() + "-" +
                                       Teams.teamTwoPlayers[Teams.currentBowler].NumberOfWicketsTaken.ToString();
        public string BowlerFigures
        {
            get
            {
                return bowlerFigures;
            }
            set
            {
                if (bowlerFigures != value)
                {
                    bowlerFigures = value;
                    this.OnPropertyChanged("BowlerFigures");
                }
            }
        }

        public void SwapFacingAsteriskInDisplay()
        {
            if (Teams.currentBatsmanOne == Teams.batsmanFacing)
            {
                BatsmanOne = Teams.teamOnePlayers[Teams.currentBatsmanOne].Name + "*";
                BatsmanTwo = Teams.teamOnePlayers[Teams.currentBatsmanTwo].Name;
            }
            else if (Teams.currentBatsmanTwo == Teams.batsmanFacing)
            {
                BatsmanOne = Teams.teamOnePlayers[Teams.currentBatsmanOne].Name;
                BatsmanTwo = Teams.teamOnePlayers[Teams.currentBatsmanTwo].Name + "*";
            }
        }

        void CheckForEndOfTheOver()
        {
            if (Innings.validDeliveriesInThisOver == 6)
            {
                Teams.SwapFacingBatsmen(ref Teams.batsmanFacing, ref Teams.batsmanNotFacing); // change of ends
                if (Innings.maidenBowled)
                {
                    Teams.teamTwoPlayers[Teams.currentBowler].NumberOfMaidensBowled++;
                }
                Innings.validDeliveriesInThisOver = 0;
                Innings.maidenBowled = true;
            }
            CheckForEndOfInnings();
        }

        void CheckForEndOfInnings()
        {
            if (Innings.CompleteOvers == Innings.ScheduledOvers)
            {
                ScorerIO.WriteScorecard();
                NavigateMainPage();
            }
        }

        async void PickNewBowler()
        {
            // TODO Need to invoke this picker for the opening bowler too.
            // TODO Perhaps there should be a "has there been a bowling change" question first - if not, go back to whoever was bowling the previous over from the other end
            string nextBowlerName = await DisplayActionSheet("Next Bowler", null, null, Teams.teamTwoPlayers[0].Name,
                                    Teams.teamTwoPlayers[1].Name, Teams.teamTwoPlayers[2].Name, Teams.teamTwoPlayers[3].Name,
                                    Teams.teamTwoPlayers[4].Name, Teams.teamTwoPlayers[5].Name, Teams.teamTwoPlayers[6].Name,
                                    Teams.teamTwoPlayers[7].Name, Teams.teamTwoPlayers[8].Name, Teams.teamTwoPlayers[9].Name,
                                    Teams.teamTwoPlayers[10].Name);

            for (int i = Teams.teamTwoPlayers.Count - 1; i >= 0; i--)
            {
                if (Teams.teamTwoPlayers[i].Name == nextBowlerName)
                    Teams.currentBowler = i;
            }
            CurrentBowler = Teams.teamTwoPlayers[Teams.currentBowler].Name;
            UpdateDisplay();
        }

        async void SelectNextBatsman()
        {
            // TODO Perhaps need this at the start of the innings, too, to select the openers.
            string nextBatsmanName = await DisplayActionSheet("Which is the next batsman?", null, null,
                                    Teams.teamOnePlayers[0].Name, Teams.teamOnePlayers[1].Name, Teams.teamOnePlayers[2].Name,
                                    Teams.teamOnePlayers[3].Name, Teams.teamOnePlayers[4].Name, Teams.teamOnePlayers[5].Name,
                                    Teams.teamOnePlayers[6].Name, Teams.teamOnePlayers[7].Name, Teams.teamOnePlayers[8].Name,
                                    Teams.teamOnePlayers[9].Name, Teams.teamOnePlayers[10].Name);

            for (int i = 0; i < Teams.teamOnePlayers.Count; i++)
            {
                if (Teams.teamOnePlayers[i].Name == nextBatsmanName)
                    Teams.nextBatsman = i;
            }
        }

        async void GetDismissingFielder()
        {
            // use picker to display all fielders on the pitch
            string dismissingFielderName = await DisplayActionSheet("Which fielder?", null, null,
                                            Teams.teamTwoPlayers[0].Name, Teams.teamTwoPlayers[1].Name, Teams.teamTwoPlayers[2].Name,
                                            Teams.teamTwoPlayers[3].Name, Teams.teamTwoPlayers[4].Name, Teams.teamTwoPlayers[5].Name,
                                            Teams.teamTwoPlayers[6].Name, Teams.teamTwoPlayers[7].Name, Teams.teamTwoPlayers[8].Name,
                                            Teams.teamTwoPlayers[9].Name, Teams.teamTwoPlayers[10].Name);
            for (int i = Teams.teamTwoPlayers.Count - 1; i >= 0; i--)
            {
                if (Teams.teamTwoPlayers[i].Name == dismissingFielderName)
                    Teams.dismissingFielder = i;
            }
        }


        void UpdateDisplay()
        {
            CheckForEndOfTheOver();
            SwapFacingAsteriskInDisplay();
            BatsmanOneRuns = Teams.teamOnePlayers[Teams.currentBatsmanOne].RunsScored.ToString();
            BatsmanTwoRuns = Teams.teamOnePlayers[Teams.currentBatsmanTwo].RunsScored.ToString();
            ScoreHeader = Innings.Runs.ToString() + "-" + Innings.Wickets.ToString();
            BowlerFigures = Teams.teamTwoPlayers[Teams.currentBowler].NumberOfOversBowled.ToString() + "-" +
                            Teams.teamTwoPlayers[Teams.currentBowler].NumberOfMaidensBowled.ToString() + "-" +
                            Teams.teamTwoPlayers[Teams.currentBowler].RunsConceded.ToString() + "-" +
                            Teams.teamTwoPlayers[Teams.currentBowler].NumberOfWicketsTaken.ToString();
            OversHeader = Innings.Overs.ToString();
        }

        void DotClicked(object sender, EventArgs e)
        {
            Teams.UpdateBowlerOversBowled();
            if (Innings.validDeliveriesInThisOver == 6)
                PickNewBowler();
            UpdateDisplay();
        }

        async void RunsClicked(object sender, EventArgs e)
        {
            string runsScored = await DisplayActionSheet("How many runs did the batsman score?", "Cancel", null, "1", "2", "3", "other");
            if (runsScored != "Cancel")
            {
                int.TryParse(runsScored, out int runsScoredThisDelivery);
                Innings.Runs += runsScoredThisDelivery;
                Teams.teamOnePlayers[Teams.batsmanFacing].RunsScored += runsScoredThisDelivery;
                Teams.teamOnePlayers[Teams.batsmanFacing].DeliveriesFaced++;
                if (runsScoredThisDelivery % 2 == 1)  // TODO - short runs?
                {
                    Teams.SwapFacingBatsmen(ref Teams.batsmanFacing, ref Teams.batsmanNotFacing);
                }
                Teams.teamTwoPlayers[Teams.currentBowler].RunsConceded += runsScoredThisDelivery;
                Teams.UpdateBowlerOversBowled();
                if (Innings.validDeliveriesInThisOver == 6)
                    PickNewBowler();
                Innings.maidenBowled = false;
                UpdateDisplay();
            }
        }

        async void BoundaryClicked(object sender, EventArgs e)
        {
            var boundaryRuns = await DisplayActionSheet("Was it a 4 or a 6?", "Cancel", null, "4", "6");
            if (boundaryRuns != "Cancel")
            {
                int.TryParse(boundaryRuns, out int boundaryRunsThisDelivery);
                Innings.Runs += boundaryRunsThisDelivery;
                Teams.teamOnePlayers[Teams.batsmanFacing].RunsScored += boundaryRunsThisDelivery;
                Teams.teamOnePlayers[Teams.batsmanFacing].DeliveriesFaced++;

                if (boundaryRunsThisDelivery == 4)
                {
                    Teams.teamOnePlayers[Teams.batsmanFacing].NumberOfFoursScored++;
                }
                if (boundaryRunsThisDelivery == 6)
                {
                    Teams.teamOnePlayers[Teams.batsmanFacing].NumberOfSixesScored++;
                }
                Teams.teamTwoPlayers[Teams.currentBowler].RunsConceded += boundaryRunsThisDelivery;
                Innings.maidenBowled = false;
                Teams.UpdateBowlerOversBowled();
                if (Innings.validDeliveriesInThisOver == 6)
                    PickNewBowler();
                UpdateDisplay();
            }
        }

        async void NoBallClicked(object sender, EventArgs e)
        {
            var anyRunsScoredOffNoBall = await DisplayActionSheet("Were there any runs scored off this no ball?", "Cancel", null, "Yes", "No");
            if (anyRunsScoredOffNoBall != "Cancel")
            {
                if (anyRunsScoredOffNoBall == "Yes")
                {
                    var runsOffNoBall = await DisplayActionSheet("How many runs were scored off the no ball?",
                                                                 "Cancel", null, "1", "2", "3", "4", "6", "other");

                    int.TryParse(runsOffNoBall, out int noBallRunsThisDelivery);
                    var typeRunsOffNoBall = await DisplayActionSheet("How were the runs scored?", "Cancel", null,
                                                                     "Off the bat", "Byes", "Leg byes");
                    switch (typeRunsOffNoBall)
                    {
                        case ("Off the bat"):
                            Innings.Runs += noBallRunsThisDelivery;
                            Teams.teamOnePlayers[Teams.batsmanFacing].RunsScored   += noBallRunsThisDelivery;
                            Teams.teamTwoPlayers[Teams.currentBowler].RunsConceded += noBallRunsThisDelivery;
                            if (noBallRunsThisDelivery == 4)
                            {
                                var boundaryOrNotFour = await DisplayActionSheet("Was the 4 a boundary?", "Cancel",
                                                                                  null, "Yes", "No");
                                if (boundaryOrNotFour == "Yes")
                                    Teams.teamOnePlayers[Teams.batsmanFacing].NumberOfFoursScored++;
                            }

                            if (noBallRunsThisDelivery == 6)
                            {
                                var boundaryOrNotSix = await DisplayActionSheet("Was the 6 a boundary?", "Cancel",
                                                                                null, "Yes", "No");
                                if (boundaryOrNotSix == "Yes")
                                    Teams.teamOnePlayers[Teams.batsmanFacing].NumberOfSixesScored++;
                            }
                            break;
                        case ("Byes"):
                            Innings.ByesOffNoBall = noBallRunsThisDelivery;
                            Innings.RecordByesOffNoBall();
                            break;
                        case ("Leg byes"):
                            Innings.LegByesOffNoBall = noBallRunsThisDelivery;
                            Innings.RecordLegByesOffNoBall();
                            break;
                    }

                    if (noBallRunsThisDelivery % 2 == 1)  //short runs?
                    {
                        Teams.SwapFacingBatsmen(ref Teams.batsmanFacing, ref Teams.batsmanNotFacing);
                    };
                }
                Teams.teamOnePlayers[Teams.batsmanFacing].DeliveriesFaced++;
                Innings.Runs++; // += RunsPerNoBall (=2 in junior cricket)
                Innings.NoBalls++;
                Teams.teamTwoPlayers[Teams.currentBowler].RunsConceded++;
                UpdateDisplay();
            }
        }

        async void ByesClicked(object sender, EventArgs e)
        {
            var typeOfByes = await DisplayActionSheet("Were they byes or leg byes?", "Cancel", null, "Byes", "Leg Byes");
            if (typeOfByes != "Cancel")
            {
                switch (typeOfByes)
                {
                    case "Byes":
                        var howManyByes = await DisplayActionSheet("How many byes were scored?", "Cancel", null, "1",
                            "2", "3", "4", "other");
                        int.TryParse(howManyByes, out int byesThisDelivery);
                        Innings.Byes += byesThisDelivery;
                        Innings.Runs += byesThisDelivery;
                        if (byesThisDelivery % 2 == 1) //short runs?
                        {
                            Teams.SwapFacingBatsmen(ref Teams.batsmanFacing, ref Teams.batsmanNotFacing);
                        }

                        break;
                    case "Leg Byes":
                        var howManyLegByes = await DisplayActionSheet("How many leg byes were scored?", "Cancel", null,
                            "1", "2", "3", "4", "other");
                        int.TryParse(howManyLegByes, out int legByesThisDelivery);
                        Innings.LegByes += legByesThisDelivery;
                        Innings.Runs    += legByesThisDelivery;
                        if (legByesThisDelivery % 2 == 1) //short runs?
                        {
                            Teams.SwapFacingBatsmen(ref Teams.batsmanFacing, ref Teams.batsmanNotFacing);
                        }
                        break;
                }
                Teams.UpdateBowlerOversBowled();
                if (Innings.validDeliveriesInThisOver == 6)
                    PickNewBowler();
                UpdateDisplay();
            }
        }

        async void WideClicked(object sender, EventArgs e)
        {
            var byesOffWide = await DisplayActionSheet("Were there any byes taken off the wide?", "Cancel", null, "Yes", "No");
            if (byesOffWide != "Cancel")
            {
                if (byesOffWide == "Yes")
                {
                    var howManyByesOffWide = await DisplayActionSheet("How many byes came off the Wide?", "Cancel",
                        null, "1", "2", "3", "4", "other");
                    int.TryParse(howManyByesOffWide, out int byesOffWideThisDelivery);
                    Innings.ByesOffWide = byesOffWideThisDelivery;
                    Innings.RecordByesOffWide();
                    if (byesOffWideThisDelivery % 2 == 1)  //short runs?
                    {
                        Teams.SwapFacingBatsmen(ref Teams.batsmanFacing, ref Teams.batsmanNotFacing);
                    }
                }
                Innings.RecordWide();
                UpdateDisplay();
            }
        }

        async void DetermineWhichBatsmanIsOut()
        {
            string batsmanOut = await DisplayActionSheet("Which batsman was out?", null, null,
                Teams.teamOnePlayers[Teams.batsmanFacing].Name, Teams.teamOnePlayers[Teams.batsmanNotFacing].Name);
            if (batsmanOut == Teams.teamOnePlayers[Teams.batsmanFacing].Name)
                Teams.teamOnePlayers[Teams.batsmanFacing].IsOut = true;
            else if (batsmanOut == Teams.teamOnePlayers[Teams.batsmanNotFacing].Name)
                Teams.teamOnePlayers[Teams.batsmanNotFacing].IsOut = true;
        }

        public static bool batsmenCrossedBeforeWicket = false;
        public static int runsScoredBeforeWicket = 0;
        async void GetCompletedRunsBeforeWicket()
        {
            var completedRuns = await DisplayActionSheet("How many runs were completed before the wicket?", null, null,
                                                         "0", "1", "2", "3");
            int.TryParse(completedRuns, out runsScoredBeforeWicket);
            var batsmenCrossed = await DisplayActionSheet("Did the batsman cross on the " + (runsScoredBeforeWicket + 1) + " run",
                null, null, "Yes", "No");
            if (batsmenCrossed == "Yes")
                batsmenCrossedBeforeWicket = true;
        }
        
        async void WicketClicked(object sender, EventArgs e)
        {
            var howOut = await DisplayActionSheet("How was the batsman out?", "Cancel", null, "Bowled", "Caught", "LBW", "Run Out", "Stumped", "Other");
            if (howOut != "Cancel")
            { // TODO Consider a method Innings.RecordFallenWicket() here - logic does not need to be in this file
                switch (howOut)
                {
                    case "Bowled":
                        Teams.teamOnePlayers[Teams.batsmanFacing].DismissalMethod = "b";
                        // TODO This block of code is common to Bowled, LBW and HitWicket
                        Teams.teamOnePlayers[Teams.batsmanFacing].IsOut = true;
                        Teams.teamOnePlayers[Teams.batsmanFacing].DeliveriesFaced++;
                        Teams.teamOnePlayers[Teams.batsmanFacing].DismissalTime = DateTime.Now.TimeOfDay;
                        Teams.teamOnePlayers[Teams.batsmanFacing].DismissingBowler = Teams.currentBowler;
                        Teams.teamTwoPlayers[Teams.currentBowler].NumberOfWicketsTaken++;
                        break;
                    case "Caught":
                        Teams.teamOnePlayers[Teams.batsmanFacing].DismissalMethod = "ct";
                        GetDismissingFielder();
                        Teams.teamOnePlayers[Teams.batsmanFacing].DismissingFielder = Teams.dismissingFielder;
                        GetCompletedRunsBeforeWicket();
                        Teams.teamOnePlayers[Teams.batsmanFacing].RunsScored += runsScoredBeforeWicket;
                        // TODO This block of code is common to caught and stumped
                        Teams.teamOnePlayers[Teams.batsmanFacing].IsOut = true;
                        Teams.teamOnePlayers[Teams.batsmanFacing].DeliveriesFaced++;
                        Teams.teamOnePlayers[Teams.batsmanFacing].DismissalTime = DateTime.Now.TimeOfDay;
                        Teams.teamOnePlayers[Teams.batsmanFacing].DismissingBowler = Teams.currentBowler;
                        Teams.teamTwoPlayers[Teams.currentBowler].NumberOfWicketsTaken++;
                        // TODO Could DisplayActionSheet("Which batsman is facing now?") if we can't work out this from crossed.
                        if (batsmenCrossedBeforeWicket)
                            Teams.SwapFacingBatsmen(ref Teams.batsmanFacing, ref Teams.batsmanNotFacing);
                        break;
                    case "LBW":
                        Teams.teamOnePlayers[Teams.batsmanFacing].DismissalMethod = "lbw";
                        Teams.teamOnePlayers[Teams.batsmanFacing].IsOut = true;
                        Teams.teamOnePlayers[Teams.batsmanFacing].DeliveriesFaced++;
                        Teams.teamOnePlayers[Teams.batsmanFacing].DismissalTime = DateTime.Now.TimeOfDay;
                        Teams.teamOnePlayers[Teams.batsmanFacing].DismissingBowler = Teams.currentBowler;
                        Teams.teamTwoPlayers[Teams.currentBowler].NumberOfWicketsTaken++;
                        break;
                    case "Run Out":
                        DetermineWhichBatsmanIsOut();
                        GetDismissingFielder();
                        Teams.teamOnePlayers[Teams.batsmanFacing].DismissingFielder = Teams.dismissingFielder;
                        if (Teams.teamOnePlayers[Teams.batsmanFacing].IsOut)
                            Teams.teamOnePlayers[Teams.batsmanFacing].DismissalMethod = "ro";
                        else if (Teams.teamOnePlayers[Teams.batsmanNotFacing].IsOut)
                            Teams.teamOnePlayers[Teams.batsmanNotFacing].DismissalMethod = "ro";
                        Teams.teamOnePlayers[Teams.batsmanFacing].DeliveriesFaced++;
                        GetCompletedRunsBeforeWicket();
                        Teams.teamOnePlayers[Teams.batsmanFacing].RunsScored += runsScoredBeforeWicket;
                        break;
                    case "Stumped":
                        Teams.teamOnePlayers[Teams.batsmanFacing].DismissalMethod = "st";
                        GetDismissingFielder();   // TODO or we could directly record wicket keeper here - use isKeeper variable (but what if not set?)
                        Teams.teamOnePlayers[Teams.batsmanFacing].DismissingFielder = Teams.dismissingFielder;
                        Teams.teamOnePlayers[Teams.batsmanFacing].IsOut = true;
                        Teams.teamOnePlayers[Teams.batsmanFacing].DeliveriesFaced++;
                        Teams.teamOnePlayers[Teams.batsmanFacing].DismissalTime = DateTime.Now.TimeOfDay;
                        Teams.teamOnePlayers[Teams.batsmanFacing].DismissingBowler = Teams.currentBowler;
                        Teams.teamTwoPlayers[Teams.currentBowler].NumberOfWicketsTaken++;
                        break;
                    case "Other": // https://www.lords.org/mcc/laws-of-cricket/laws/
                        var otherWaysOut = await DisplayActionSheet("How was the batsman out?", "Cancel", null, "Hit Wicket", "Handled Ball", "Obstruction", "Hit Twice", "Timed Out");
                        switch (otherWaysOut)
                        {
                            case "Hit Wicket":
                                Teams.teamOnePlayers[Teams.batsmanFacing].DismissalMethod = "hw";
                                Teams.teamOnePlayers[Teams.batsmanFacing].IsOut = true;
                                Teams.teamOnePlayers[Teams.batsmanFacing].DeliveriesFaced++;
                                Teams.teamOnePlayers[Teams.batsmanFacing].DismissalTime = DateTime.Now.TimeOfDay;
                                Teams.teamOnePlayers[Teams.batsmanFacing].DismissingBowler = Teams.currentBowler;
                                Teams.teamTwoPlayers[Teams.currentBowler].NumberOfWicketsTaken++;
                                break;
                            case "Handled Ball": // This is now classed as obstruction.
                            case "Obstruction": // no credit to the bowler
                                DetermineWhichBatsmanIsOut();
                                if (Teams.teamOnePlayers[Teams.batsmanFacing].IsOut)
                                    Teams.teamOnePlayers[Teams.batsmanFacing].DismissalMethod = "ob";
                                else if (Teams.teamOnePlayers[Teams.batsmanNotFacing].IsOut)
                                    Teams.teamOnePlayers[Teams.batsmanNotFacing].DismissalMethod = "ob";
                                Teams.teamOnePlayers[Teams.batsmanFacing].DeliveriesFaced++;
                                break;
                            case "Hit Twice": // no credit to the bowler
                                Teams.teamOnePlayers[Teams.batsmanFacing].DismissalMethod = "ht";
                                Teams.teamOnePlayers[Teams.batsmanFacing].IsOut = true;
                                Teams.teamOnePlayers[Teams.batsmanFacing].DeliveriesFaced++;
                                Teams.teamOnePlayers[Teams.batsmanFacing].DismissalTime = DateTime.Now.TimeOfDay;
                                break;
                            case "Timed Out": // no credit to the bowler
                                Teams.teamOnePlayers[Teams.batsmanFacing].DismissalMethod = "to";
                                Teams.teamOnePlayers[Teams.batsmanFacing].IsOut = true;
                                Teams.teamOnePlayers[Teams.batsmanFacing].DismissalTime = DateTime.Now.TimeOfDay;
                                break;
                        }
                        break;
                }
                Innings.Wickets++;
                SelectNextBatsman();

                // TODO Get time of new batsman and store in Player.StartTime, also get finishing time of dismissed batsman, store in Player.DismissalTime

                // TODO Extract this 4-way logic into a method NewBatsman()
                if ((Teams.teamOnePlayers[Teams.batsmanFacing].IsOut) && (Teams.batsmanFacing == Teams.currentBatsmanOne))
                {
                    Teams.currentBatsmanOne = Teams.nextBatsman;
                    Teams.batsmanFacing = Teams.currentBatsmanOne;
                    Teams.teamOnePlayers[Teams.batsmanFacing].StartTime = DateTime.Now.TimeOfDay;
                    BatsmanOne = Teams.teamOnePlayers[Teams.currentBatsmanOne].Name + "*";
                }
                else if ((Teams.teamOnePlayers[Teams.batsmanFacing].IsOut) && (Teams.batsmanFacing == Teams.currentBatsmanTwo))
                {
                    Teams.currentBatsmanTwo = Teams.nextBatsman;
                    Teams.batsmanFacing = Teams.currentBatsmanTwo;
                    BatsmanTwo = Teams.teamOnePlayers[Teams.currentBatsmanTwo].Name + "*";
                }
                else if ((Teams.teamOnePlayers[Teams.batsmanNotFacing].IsOut) && (Teams.batsmanNotFacing == Teams.currentBatsmanOne))
                {
                    Teams.currentBatsmanOne = Teams.nextBatsman;
                    Teams.batsmanNotFacing = Teams.currentBatsmanOne;
                    BatsmanOne = Teams.teamOnePlayers[Teams.currentBatsmanOne].Name;
                }
                else if ((Teams.teamOnePlayers[Teams.batsmanNotFacing].IsOut) && (Teams.batsmanNotFacing == Teams.currentBatsmanTwo))
                {
                    Teams.currentBatsmanTwo = Teams.nextBatsman;
                    Teams.batsmanNotFacing = Teams.currentBatsmanTwo;
                    BatsmanTwo = Teams.teamOnePlayers[Teams.currentBatsmanTwo].Name;
                };

                Teams.UpdateBowlerOversBowled();
                UpdateDisplay();
            }
        }
    }
}
