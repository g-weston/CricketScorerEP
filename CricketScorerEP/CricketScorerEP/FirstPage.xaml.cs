using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CricketScorerEP
{
    public partial class FirstPage : ContentPage, INotifyPropertyChanged
    {
        public FirstPage()
        {
            InitializeComponent();
            BindingContext = this;
            // TODO check that Match and Teams are not null - bail out if so
        }

        private async void NavigateMainPage()
        {
            await Navigation.PushAsync(new MainPage());
        }

        private async void NavigateSettingsPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewPage());
        }

        // TODO Need to extract this block of display strings to a separate file probably, as part of the partial class, in order to improve readability
        private string teamNameHeader = Match.HomeTeam;
        public string TeamNameHeader
        {
            get => teamNameHeader;
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
            get => oversHeader;
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
            get => scoreHeader;
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
            get => batsmanOne;
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
            get => batsmanOneRuns;
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
            get => batsmanTwo;
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
            get => batsmanTwoRuns;
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
            get => currentBowler;
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
            get => bowlerFigures;
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

        int GetPlayerIndexFromName(string playerName, List<Player> playerList)
        {
            int playerIndex = 0;
            for (int i = 0; i < playerList.Count; i++)
            {
                if (playerList[i].Name == playerName)
                {
                    playerIndex = i;
                }
            }
            return playerIndex;
        }

        async Task<string> PickNewBowler()
        {
            // TODO Need to invoke this picker for the opening bowler too.
            // TODO Perhaps there should be a "has there been a bowling change" question first - if not, go back to whoever was bowling the previous over from the other end
            string nextBowlerName = await DisplayActionSheet("Next Bowler", null, null, Teams.teamTwoPlayers[0].Name,
                                    Teams.teamTwoPlayers[1].Name, Teams.teamTwoPlayers[2].Name, Teams.teamTwoPlayers[3].Name,
                                    Teams.teamTwoPlayers[4].Name, Teams.teamTwoPlayers[5].Name, Teams.teamTwoPlayers[6].Name,
                                    Teams.teamTwoPlayers[7].Name, Teams.teamTwoPlayers[8].Name, Teams.teamTwoPlayers[9].Name,
                                    Teams.teamTwoPlayers[10].Name);

            Teams.currentBowler = GetPlayerIndexFromName(nextBowlerName, Teams.teamTwoPlayers);
            CurrentBowler = Teams.teamTwoPlayers[Teams.currentBowler].Name;
            UpdateDisplay();

            return nextBowlerName;
        }

        async Task<string> SelectNextBatsman()
        {
            // TODO Perhaps need this at the start of the innings, too, to select alternative openers.
            List<string> yetToBat = new List<string>();
            for (int i = 0; i < Teams.teamOnePlayers.Count; i++)
            {
                if (!Teams.teamOnePlayers[i].IsOut && (i != Teams.currentBatsmanOne) & (i != Teams.currentBatsmanTwo))
                {
                    yetToBat.Add(Teams.teamOnePlayers[i].Name);
                }
            }
            string nextBatsmanName = await DisplayActionSheet("Which is the next batsman?", null, null, yetToBat.ToArray());
            Teams.nextBatsman = GetPlayerIndexFromName(nextBatsmanName, Teams.teamOnePlayers);

            return nextBatsmanName;
        }

        async Task<string> GetDismissingFielder()
        {
            // Use a picker to display all fielders on the pitch
            string dismissingFielderName = await DisplayActionSheet("Which fielder?", null, null,
                                            Teams.teamTwoPlayers[0].Name, Teams.teamTwoPlayers[1].Name, Teams.teamTwoPlayers[2].Name,
                                            Teams.teamTwoPlayers[3].Name, Teams.teamTwoPlayers[4].Name, Teams.teamTwoPlayers[5].Name,
                                            Teams.teamTwoPlayers[6].Name, Teams.teamTwoPlayers[7].Name, Teams.teamTwoPlayers[8].Name,
                                            Teams.teamTwoPlayers[9].Name, Teams.teamTwoPlayers[10].Name);

            Teams.dismissingFielder = GetPlayerIndexFromName(dismissingFielderName, Teams.teamTwoPlayers);
            return dismissingFielderName;
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

        async void DotClicked(object sender, EventArgs e)
        {
            Teams.UpdateBowlerOversBowled();
            if (Innings.validDeliveriesInThisOver == 6)
            {
                await PickNewBowler();
            }
            UpdateDisplay();
        }

        async void RunsClicked(object sender, EventArgs e)
        {
            string runsScored = await DisplayActionSheet("How many runs did the batsman score?", "Cancel", null, "1", "2", "3", "other");
            if (runsScored != "Cancel")
            {
                if (runsScored == "other")
                {
                    runsScored = await DisplayPromptAsync("How many runs were scored?", null, "Enter", "Cancel");
                    while (int.TryParse(runsScored, out int numberRunsScored) == false)
                    {
                        await DisplayActionSheet("Please enter a number", null, "Ok");
                        runsScored = await DisplayPromptAsync("How many runs were scored?", null, "Enter", "Cancel");
                    }
                }

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
                {
                    await PickNewBowler();
                }
                Innings.maidenBowled = false;
                UpdateDisplay();
            }
            return;
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
                {
                    await PickNewBowler();
                }
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
                            Teams.teamOnePlayers[Teams.batsmanFacing].RunsScored += noBallRunsThisDelivery;
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
                Teams.teamTwoPlayers[Teams.currentBowler].NoBallsDelivered++;
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
                        Innings.Runs += legByesThisDelivery;
                        if (legByesThisDelivery % 2 == 1) //short runs?
                        {
                            Teams.SwapFacingBatsmen(ref Teams.batsmanFacing, ref Teams.batsmanNotFacing);
                        }
                        break;
                }
                Teams.UpdateBowlerOversBowled();
                if (Innings.validDeliveriesInThisOver == 6)
                {
                    await PickNewBowler();
                }
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

        async Task<string> DetermineWhichBatsmanIsOut()
        {
            string batsmanOut = await DisplayActionSheet("Which batsman was out?", null, null,
                                                        Teams.teamOnePlayers[Teams.batsmanFacing].Name,
                                                        Teams.teamOnePlayers[Teams.batsmanNotFacing].Name);
            if (batsmanOut == Teams.teamOnePlayers[Teams.batsmanFacing].Name)
            {
                Teams.teamOnePlayers[Teams.batsmanFacing].IsOut = true;
            }
            else if (batsmanOut == Teams.teamOnePlayers[Teams.batsmanNotFacing].Name)
            {
                Teams.teamOnePlayers[Teams.batsmanNotFacing].IsOut = true;
            }
            return batsmanOut;
        }

        public static bool batsmenCrossedBeforeWicket = false;
        public static int runsScoredBeforeWicket = 0;
        async Task<string> GetCompletedRunsBeforeWicket()
        {
            var completedRuns = await DisplayActionSheet("How many runs were completed before the wicket?", null, null,
                                                         "0", "1", "2", "3");
            int.TryParse(completedRuns, out runsScoredBeforeWicket);
            var batsmenCrossed = await DisplayActionSheet("Did the batsman cross on run " + (runsScoredBeforeWicket + 1),
                                                           null, null, "Yes", "No");
            if (batsmenCrossed == "Yes")
            {
                batsmenCrossedBeforeWicket = true;
            }
            return completedRuns;
        }

        void SetBatsmanAsDismissed()
        {
            Teams.teamOnePlayers[Teams.batsmanFacing].IsOut = true;
            Teams.teamOnePlayers[Teams.batsmanFacing].DeliveriesFaced++;
            Teams.teamOnePlayers[Teams.batsmanFacing].DismissalTime = DateTime.Now.TimeOfDay;
            Teams.teamOnePlayers[Teams.batsmanFacing].DismissingBowler = Teams.currentBowler;
            Teams.teamTwoPlayers[Teams.currentBowler].NumberOfWicketsTaken++;
        }

        void SwapInNewBatsman()
        {
            if ((Teams.teamOnePlayers[Teams.batsmanFacing].IsOut) && (Teams.batsmanFacing == Teams.currentBatsmanOne))
            {
                Teams.currentBatsmanOne = Teams.nextBatsman;
                Teams.batsmanFacing = Teams.currentBatsmanOne;
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
            Teams.teamOnePlayers[Teams.nextBatsman].StartTime = DateTime.Now.TimeOfDay;
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
                        SetBatsmanAsDismissed();
                        break;
                    case "Caught":
                        Teams.teamOnePlayers[Teams.batsmanFacing].DismissalMethod = "ct";
                        // TODO This block of code is common to caught and stumped
                        await GetDismissingFielder();
                        Teams.teamOnePlayers[Teams.batsmanFacing].DismissingFielder = Teams.dismissingFielder;
                        await GetCompletedRunsBeforeWicket();
                        Teams.teamOnePlayers[Teams.batsmanFacing].RunsScored += runsScoredBeforeWicket;
                        SetBatsmanAsDismissed();
                        // TODO Could DisplayActionSheet("Which batsman is facing now?") if we can't work out this from crossed.
                        if (batsmenCrossedBeforeWicket)
                            Teams.SwapFacingBatsmen(ref Teams.batsmanFacing, ref Teams.batsmanNotFacing);
                        break;
                    case "LBW":
                        Teams.teamOnePlayers[Teams.batsmanFacing].DismissalMethod = "lbw";
                        SetBatsmanAsDismissed();
                        break;
                    case "Run Out":
                        await DetermineWhichBatsmanIsOut();
                        await GetDismissingFielder();
                        Teams.teamOnePlayers[Teams.batsmanFacing].DismissingFielder = Teams.dismissingFielder;
                        if (Teams.teamOnePlayers[Teams.batsmanFacing].IsOut)
                        {
                            Teams.teamOnePlayers[Teams.batsmanFacing].DismissalMethod = "ro";
                            Teams.teamOnePlayers[Teams.batsmanFacing].DismissalTime = DateTime.Now.TimeOfDay;
                        }
                        else if (Teams.teamOnePlayers[Teams.batsmanNotFacing].IsOut)
                        {
                            Teams.teamOnePlayers[Teams.batsmanNotFacing].DismissalMethod = "ro";
                            Teams.teamOnePlayers[Teams.batsmanNotFacing].DismissalTime = DateTime.Now.TimeOfDay;
                        }
                        Teams.teamOnePlayers[Teams.batsmanFacing].DeliveriesFaced++;
                        await GetCompletedRunsBeforeWicket();
                        Teams.teamOnePlayers[Teams.batsmanFacing].RunsScored += runsScoredBeforeWicket;
                        break;
                    case "Stumped":
                        Teams.teamOnePlayers[Teams.batsmanFacing].DismissalMethod = "st";
                        await GetDismissingFielder();   // TODO or we could directly record wicket keeper here - use isKeeper variable (but what if not set?)
                        Teams.teamOnePlayers[Teams.batsmanFacing].DismissingFielder = Teams.dismissingFielder;
                        SetBatsmanAsDismissed();
                        break;
                    case "Other": // https://www.lords.org/mcc/laws-of-cricket/laws/
                        var otherWaysOut = await DisplayActionSheet("How was the batsman out?", "Cancel", null, "Hit Wicket", "Handled Ball", "Obstruction", "Hit Twice", "Timed Out");
                        switch (otherWaysOut)
                        {
                            case "Hit Wicket":
                                Teams.teamOnePlayers[Teams.batsmanFacing].DismissalMethod = "hw";
                                SetBatsmanAsDismissed();
                                break;
                            case "Handled Ball": // This is now classed as obstruction.
                            case "Obstruction": // no credit to the bowler
                                await DetermineWhichBatsmanIsOut();
                                if (Teams.teamOnePlayers[Teams.batsmanFacing].IsOut)
                                {
                                    Teams.teamOnePlayers[Teams.batsmanFacing].DismissalMethod = "ob";
                                    Teams.teamOnePlayers[Teams.batsmanFacing].DismissalTime = DateTime.Now.TimeOfDay;
                                }
                                else if (Teams.teamOnePlayers[Teams.batsmanNotFacing].IsOut)
                                {
                                    Teams.teamOnePlayers[Teams.batsmanNotFacing].DismissalMethod = "ob";
                                    Teams.teamOnePlayers[Teams.batsmanNotFacing].DismissalTime = DateTime.Now.TimeOfDay;
                                }
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
                await SelectNextBatsman();
                SwapInNewBatsman();  // Move to Teams
                Teams.UpdateBowlerOversBowled();
                if (Innings.validDeliveriesInThisOver == 6)
                {
                    await PickNewBowler();
                }
                UpdateDisplay();
            }

        }
    }
}
