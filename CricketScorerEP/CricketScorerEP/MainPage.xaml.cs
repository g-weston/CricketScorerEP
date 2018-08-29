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

        async void TeamLoadOption(object sender, EventArgs e)
        {
            // TODO Have a guard or try/catch in here in case file does not exist (or cannot be read) to avoid exceptions
            ScorerIO.ReadMatchDetails("MatchDefinition.txt");
            Innings.ScheduledOvers = Match.ScheduledOvers;
            string team =
                await DisplayActionSheet("Which team do you wish to input (1 or 2)?", "Cancel", null, "1", "2");
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
    }
}
