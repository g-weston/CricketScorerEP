using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CricketScorerEP
{
	public partial class FirstPage : ContentPage, INotifyPropertyChanged
    {
        public FirstPage()
		{
			InitializeComponent();
		    BindingContext = this;
        }

        private async void NavigateMainPage()
        {
            await Navigation.PushAsync(new MainPage());
        }

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

        private string overHeader = Innings.Overs.ToString();
        public string OverHeader
        {
            get
            {
                return overHeader;
            }
            set
            {
                if (overHeader != value)
                {
                    overHeader = value;
                    this.OnPropertyChanged("OverHeader");
                }
            }
        }

        private string scoreHeader = Innings.Runs.ToString() + "-" + Innings.Wickets.ToString();
        public string  ScoreHeader
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

        private string batsmanOne = Teams.teamOnePlayers[Teams.currentBatsmanOne].Name;
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

        public void CheckEndOver()
        {
            if (Scorer.validDeliveriesInThisOver == 6)
            {
                Scorer.SwapFacingBatsmen(ref Teams.batsmanFacing, ref Teams.batsmanNotFacing); // change of ends
                
                /*
                Console.WriteLine("Which number player is the next bowler?"); // TO DO - perhaps store currentBowler1 and currentBowler2; avoids clicks if they bowl for a spell
                input = Console.ReadLine();
                int.TryParse(input, out currentBowler);
                if (currentBowler > Teams.teamTwoPlayers.Count)
                {
                    Console.WriteLine("Not a valid player number");
                }
                */
                if (Scorer.maiden)
                {
                    Teams.teamTwoPlayers[Teams.currentBowler].NumberOfMaidensBowled++;
                }
                PickNewBowler();
                Scorer.validDeliveriesInThisOver = 0;
                Scorer.maiden = true;
            }

            if (Innings.CompleteOvers == Innings.ScheduledOvers)
            {
                Scorer.WriteScorecard("Scorecard.html");
                NavigateMainPage();
            }
        }

        public static string nextBowler;
        public async void PickNewBowler()
        {
            //Scorer.ChangeBowler();
            nextBowler = await DisplayActionSheet("Next Bowler", null, null, Teams.teamTwoPlayers[9].Name, Teams.teamTwoPlayers[8].Name, Teams.teamTwoPlayers[7].Name, Teams.teamTwoPlayers[6].Name);
            Scorer.ChangeBowler();
            CurrentBowler = Teams.teamTwoPlayers[Teams.currentBowler].Name;
        }

        public void UpdateDisplay()
        {
            CheckEndOver();
            BatsmanOneRuns = Teams.teamOnePlayers[Teams.currentBatsmanOne].RunsScored.ToString();
            BatsmanTwoRuns = Teams.teamOnePlayers[Teams.currentBatsmanTwo].RunsScored.ToString();
            ScoreHeader = Innings.Runs.ToString() + "-" + Innings.Wickets.ToString();
            BowlerFigures = Teams.teamTwoPlayers[Teams.currentBowler].NumberOfOversBowled.ToString() + "-" +
                            Teams.teamTwoPlayers[Teams.currentBowler].NumberOfMaidensBowled.ToString() + "-" +
                            Teams.teamTwoPlayers[Teams.currentBowler].RunsConceded.ToString() + "-" +
                            Teams.teamTwoPlayers[Teams.currentBowler].NumberOfWicketsTaken.ToString();
            OverHeader = Innings.Overs.ToString();
        }

        void DotClicked(object sender, EventArgs e)
        {
            Scorer.UpdateBowlerBalls();
            UpdateDisplay();
        }

        async void RunsClicked(object sender, EventArgs e)
        {
            string runsScored = await DisplayActionSheet("How many runs did the batsman score?", "Cancel", null, "1", "2", "3", "other");
            int.TryParse(runsScored, out int runsScoredThisDelivery);
            Scorer.DeliveryRuns = runsScoredThisDelivery;
            Scorer.RecordRunsScored();
            Scorer.UpdateBowlerBalls();
            Teams.teamOnePlayers[Teams.batsmanFacing].RunsScored += runsScoredThisDelivery;
            Teams.teamOnePlayers[Teams.batsmanFacing].DeliveriesFaced++;
            UpdateDisplay();
            if (runsScoredThisDelivery % 2 == 1)
            {
                Scorer.SwapFacingBatsmen(ref Teams.batsmanFacing, ref Teams.batsmanNotFacing);
            }
            Scorer.maiden = false;
        }
        async void WicketClicked(object sender, EventArgs e)
        {
            var wayOut = await DisplayActionSheet("How was the batsman out?", "Cancel", null, "Bowled", "Caught", "LBW", "Run Out", "Stumped", "Other");
            switch (wayOut)
            {
                case "Bowled":
                    //RecordFallenWicket();
                    break;
                case "Caught":
                    break;
                case "LBW":
                    break;
                case "Run Out":
                    break;
                case "Stumped":
                    break;
                case "Other":
                    var otherWaysOut = await DisplayActionSheet("How was the batsman out?", "Cancel", null, "Hit Wicket", "Handled Ball", "Obstruction", "Hit Twice", "Timed Out");
                    switch (otherWaysOut)
                    {
                        case "Hit Wicket":
                            break;
                        case "Handled Ball":
                            break;
                        case "Obstruction":
                            break;
                        case "Hit Twice":
                            break;
                        case "Timed Out":
                            break;
                    }
                    break;
            }
            Scorer.RecordFallenWicket(Teams.batsmanFacing,Teams.teamOnePlayers, Teams.nextBatsman, Teams.currentBowler, Teams.dismissingFielder, Teams.teamTwoPlayers);
            Scorer.RecordWicketTaken();
            Scorer.UpdateBowlerBalls();
            UpdateDisplay();
        }
        async void NoBallClicked(object sender, EventArgs e)
        {

            var anyRunsNoBall = await DisplayActionSheet("Were there any runs scored off the no ball?", "Cancel", null, "Yes", "No");

            switch (anyRunsNoBall)
            {
                case "Yes":
                    var runsOffNoBall = await DisplayActionSheet("How many runs were scored off the no ball?", "Cancel",
                        null, "1", "2", "3", "4", "6", "other");
                    int.TryParse(runsOffNoBall, out int noBallRunsThisDelivery);
                    Scorer.DeliveryRuns = noBallRunsThisDelivery;
                    Scorer.RecordRunsScored();
                    Teams.teamOnePlayers[Teams.batsmanFacing].RunsScored += noBallRunsThisDelivery;
                    if (noBallRunsThisDelivery == 4)
                    {
                        var boundaryOrNotFour = await DisplayActionSheet("Was the 4 a boundary?", "Cancel", null, "Yes", "No");
                        switch (boundaryOrNotFour)
                        {
                            case "Yes":
                                Teams.teamOnePlayers[Teams.batsmanFacing].NumberOfFoursScored++;
                                break;
                            case "No":
                                break;
                        }
                    }

                    if (noBallRunsThisDelivery == 6)
                    {
                        var boundaryOrNotSix = await DisplayActionSheet("Was the 6 a boundary?", "Cancel", null, "Yes", "No");
                            switch (boundaryOrNotSix)
                                {
                                    case "Yes":
                                        Teams.teamOnePlayers[Teams.batsmanFacing].NumberOfSixesScored++;
                                        break;
                                    case "No":
                                        break;
                                }
                    }
                    if (noBallRunsThisDelivery % 2 == 1)
                    {
                        Scorer.SwapFacingBatsmen(ref Teams.batsmanFacing, ref Teams.batsmanNotFacing);
                    }
                    break;
                case "NO":
                    break;
            }
            Teams.teamOnePlayers[Teams.batsmanFacing].DeliveriesFaced++;
            Scorer.RecordNoBall();
            UpdateDisplay();
        }

        async void BoundaryClicked(object sender, EventArgs e)
        {
            var boundaryRuns = await DisplayActionSheet("Was it a 4 or a 6?", "Cancel", null, "4", "6");
            int.TryParse(boundaryRuns, out int boundaryRunsThisDelivery);
            Scorer.DeliveryRuns = boundaryRunsThisDelivery;
            Scorer.RecordRunsScored();
            Scorer.UpdateBowlerBalls();
            Teams.teamOnePlayers[Teams.batsmanFacing].RunsScored += boundaryRunsThisDelivery;
            Teams.teamOnePlayers[Teams.batsmanFacing].DeliveriesFaced++;
            UpdateDisplay();
            if (boundaryRunsThisDelivery == 4)
            {
                Teams.teamOnePlayers[Teams.batsmanFacing].NumberOfFoursScored++;
            }
            if (boundaryRunsThisDelivery == 6)
            {
                Teams.teamOnePlayers[Teams.batsmanFacing].NumberOfSixesScored++;
            }
        }
        async void ByesClicked(object sender, EventArgs e)
        {

            var typeByes = await DisplayActionSheet("Were they byes or leg byes?", "Cancel", null, "Byes", "Leg Byes");
            switch (typeByes)
            {
                case "Byes":
                    var howManyByes = await DisplayActionSheet("How many byes were scored?", "Cancel", null, "1", "2", "3", "4", "other");
                    int.TryParse(howManyByes, out int byesThisDelivery);
                    Scorer.ByeRuns = byesThisDelivery;
                    Scorer.RecordByesScored();
                    if (byesThisDelivery % 2 == 1)
                    {
                        Scorer.SwapFacingBatsmen(ref Teams.batsmanFacing, ref Teams.batsmanNotFacing);
                    }
                    break;
                case "Leg Byes":
                    var howManyLegByes = await DisplayActionSheet("How many leg byes were scored?", "Cancel", null, "1", "2", "3", "4", "other");
                    int.TryParse(howManyLegByes, out int legByesThisDelivery);
                    Scorer.LegByeRuns = legByesThisDelivery;
                    Scorer.RecordLegByesScored();
                    if (legByesThisDelivery % 2 == 1)
                    {
                        Scorer.SwapFacingBatsmen(ref Teams.batsmanFacing, ref Teams.batsmanNotFacing);
                    }
                    break;
            }
            Scorer.UpdateBowlerBalls();
            UpdateDisplay();
        }
        async void WideClicked(object sender, EventArgs e)
        {

            var byesOffWide = await DisplayActionSheet("Were there any byes ran off the wide?", "Cancel", null, "Yes", "No");
            switch (byesOffWide)
            {
                case "Yes":
                    var howManyByesOffWide = await DisplayActionSheet("How many byes came off the Wide?", "Cancel", null, "1", "2", "3", "4", "other");
                    switch (howManyByesOffWide)
                    {
                        case "1":
                            break;
                        case "2":
                            break;
                        case "3":
                            break;
                        case "4":
                            break;
                        case "other":
                            break;
                    }
                    break;
                case "No":
                    break;
            }
        }
    }
}
