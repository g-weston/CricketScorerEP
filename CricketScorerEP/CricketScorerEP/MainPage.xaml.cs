﻿using System;
using Xamarin.Forms;


namespace CricketScorerEP
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            var innings = new Innings();
        }

        private async void NavigateFirstPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FirstPage());
        }

        async void TeamLoadOption(object sender, EventArgs e)
        {

            var loadOption = await DisplayActionSheet("Do you want to load the teams from a file or input now?", "Cancel", null, "File", "Input Now");
            switch (loadOption)
            {
                case "File":
                    break;
                case "Input Now":
                    break;
            }
        }

        /*protected void OnStart()
        {
            StartUp();
        }*/


        // ask further question if answer is yes to the pop up above to see how many byes
    }
}
