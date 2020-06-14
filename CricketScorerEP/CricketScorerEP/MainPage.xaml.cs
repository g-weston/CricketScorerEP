using System;
using Xamarin.Forms;

namespace CricketScorerEP
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private async void NavigateFirstPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FirstPage());
        }

        /*private async void NavigateSettingsPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NewPage());
        }
        */
        async void TeamLoadOption(object sender, EventArgs e)
        {
            // TODO Have a guard or try/catch in here in case file does not exist (or cannot be read) to avoid exceptions
            ScorerIO.ReadMatchDetails("MatchDefinition.txt");
            Innings.ScheduledOvers = Match.ScheduledOvers;
            string team = await DisplayActionSheet("Which team do you wish to input (1 or 2)?", "Cancel", null, "1", "2");
            int.TryParse(team, out int teamNumber);

            var loadOption = await DisplayActionSheet("Do you want to load the teams from a file or input now?",
                "Cancel", null, "File", "Input Now");
            switch (loadOption)
            {
                case "File":
                    try
                    {
                        ScorerIO.PopulateTeamFromFile(teamNumber);
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Unhandled exception", ex.Message, "cancel");
                    }

                    break;
                case "Input Now":
                    ScorerIO.PopulateTeamFromConsole(teamNumber);
                    break;
            }
        }

        
        async void EditMatchDetails(object sender, EventArgs e)
        {
            string changeDetail = await DisplayActionSheet(
                "Which part of the match definition would you like to change?",
                "Cancel", null, "Update Date", "Competition Type", "Venue", "Format",
                "Seniors/Juniors", "Scheduled Overs", "Runs per no ball/wide", "Over wides/no balls rebowled from");
            // if juniors provide action sheet for which age group
            switch (changeDetail)
            {
                case "Update Date":
                    break;
                case "Competition Type":
                    string competitionType = await DisplayActionSheet("What competition type is it?", "Cancel", null,
                        "League", "Cup", "Other");
                    Match.Competition = competitionType;
                    break;
                case "Venue":
                    string venue = await DisplayPromptAsync("Where is the venue?", null, "Enter", "Cancel");
                    Match.Venue = venue;
                    break;
                case "Format":
                    string formatType = await DisplayActionSheet("What format is it?", "Cancel", null,
                        "Standard", "Pairs");
                    Match.Format = formatType;
                    break;
                case "Seniors/Juniors":
                    string ageType = await DisplayActionSheet("What format is it?", "Cancel", null,
                        "Seniors", "Juniors");
                    Match.AgeGroup = ageType;
                    break;
                case "Scheduled Overs":
                    string scheduledOvers = await DisplayPromptAsync("How many overs are there?", null, "Enter", "Cancel");
                    while (int.TryParse(scheduledOvers, out int scheduledNumberOvers) == false)
                    {
                        await DisplayActionSheet("Please enter a number", null, "Ok");
                        scheduledOvers = await DisplayPromptAsync("How many overs are there?", null, "Enter", "Cancel");
                    }
                    Match.ScheduledOvers = int.Parse(scheduledOvers);
                    break;
                case "Runs per no ball/wide":
                    string extraRuns = await DisplayActionSheet("How many runs are there for a wide or no ball?", "Cancel", null,
                        "1", "2");
                    Match.RunsPerWideOrNoBall = int.Parse(extraRuns);
                    break;
                case "Over wides/no balls rebowled from":
                    string overRebowls = await DisplayPromptAsync("What over are runs rebowled from?", null, "Enter", "Cancel");
                    while (int.TryParse(overRebowls, out int overNumerRebowls) == false)
                    {
                        await DisplayActionSheet("Please enter a number", null, "Ok");
                        overRebowls = await DisplayPromptAsync("What over are runs rebowled from?", null, "Enter", "Cancel");
                    }
                    Match.RebowlDeliveriesFromOver = int.Parse(overRebowls);
                    break;
            }

            /*await DisplayActionSheet("Test", null, "ok", Match.Date.ToString(), Match.Competition, Match.Venue,
                Match.Format, Match.AgeGroup, Match.ScheduledOvers.ToString(), Match.RunsPerWideOrNoBall.ToString(),
                Match.RebowlDeliveriesFromOver.ToString());*/
            ScorerIO.WriteMatchDetails("MatchDefinition.txt");
        }
    }
}
