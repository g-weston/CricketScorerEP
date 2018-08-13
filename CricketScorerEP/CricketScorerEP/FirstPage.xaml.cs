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

        async void RunsClicked(object sender, EventArgs e)
        {
            Scorer.RecordRunsScored();
            var runsScored = await DisplayActionSheet("How many runs did the batsman score?", "Cancel", null, "1", "2", "3", "other");
            switch (runsScored)
            {
                case "1":
                    Scorer.DeliveryRuns = 1;
                    Scorer.RecordRunsHit();
                    ScoreHeader = Innings.Runs.ToString() + "-" + Innings.Wickets.ToString();
   //                 await DisplayAlert("DeliveryRuns", ScoreHeader, "Cancel");
                    //TotalScore
                    break;
                case "2":
                    Scorer.DeliveryRuns = 2;
                    Scorer.RecordRunsHit();
                    ScoreHeader = Innings.Runs.ToString() + "-" + Innings.Wickets.ToString();
  //                  await DisplayAlert("DeliveryRuns", ScoreHeader, "Cancel");
                    break;
                case "3":
                    Scorer.DeliveryRuns = 3;
                    Scorer.RecordRunsHit();
                    ScoreHeader = Innings.Runs.ToString() + "-" + Innings.Wickets.ToString();
   //                 await DisplayAlert("DeliveryRuns", ScoreHeader, "Cancel");
                    break;
                case "other":
                    break;
            }
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
        }
        async void NoBallClicked(object sender, EventArgs e)
        {

            var anyRunsNoBall = await DisplayActionSheet("Were there any runs scored off the no ball?", "Cancel", null, "Yes", "No");

            switch (anyRunsNoBall)
            {
                case "Yes":
                    var runsOffNoBall = await DisplayActionSheet("How many runs were scored off the no ball?", "Cancel", null, "1", "2", "4", "6", "other");
                    switch (runsOffNoBall)
                    {
                        case "1":
                            //link to method about the noballs
                            break;
                        case "2":
                            break;
                        case "4":
                            var boundaryOrNotFour = await DisplayActionSheet("Was the 4 a boundary?", "Cancel", null, "Yes", "No");
                            switch (boundaryOrNotFour)
                            {
                                case "Yes":
                                    break;
                                case "No":
                                    break;
                            }
                            break;
                        case "6":
                            var boundaryOrNotSix = await DisplayActionSheet("Was the 6 a boundary?", "Cancel", null, "Yes", "No");
                            switch (boundaryOrNotSix)
                            {
                                case "Yes":
                                    break;
                                case "No":
                                    break;
                            }
                            break;
                        case "other":
                            break;
                    }
                    break;
                case "NO":

                    break;
            }
        }

        // use result of the pop up above to ask how many runs and if it is a 4 or 6, was it a boundary
        async void BoundaryClicked(object sender, EventArgs e)
        {

            var boundaryRuns = await DisplayActionSheet("Was it a 4 or a 6?", "Cancel", null, "4", "6");
            switch (boundaryRuns)
            {
                case "4":
                    break;
                case "6":
                    break;
            }

        }
        async void ByesClicked(object sender, EventArgs e)
        {

            var typeByes = await DisplayActionSheet("Were they byes or leg byes?", "Cancel", null, "Byes", "Leg Byes");
            switch (typeByes)
            {
                case "Byes":
                    var howManyByes = await DisplayActionSheet("How many byes were scored?", "Cancel", null, "1", "2", "3", "4", "other");
                    switch (howManyByes)
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
                case "Leg Byes":
                    var howManyLegByes = await DisplayActionSheet("How many leg byes were scored?", "Cancel", null, "1", "2", "3", "4", "other");
                    switch (howManyLegByes)
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
            }
        }
        async void WideClicked(object sender, EventArgs e)
        {

            var byesOffWide = await DisplayActionSheet("Were there any byes ran off the wide?", "Cancel", null, "Yes", "No");
            switch (byesOffWide)
            {
                case "Yes":
                    var howManyByesOffWide = await DisplayActionSheet("How man byes came off the Wide?", "Cancel", null, "1", "2", "3", "4", "other");
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