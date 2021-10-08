using System;
using System.Globalization;

using FuelTracker.Services;
using FuelTracker.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FuelTracker
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            CultureInfo myCulture = new("fi-fi");
            CultureInfo.DefaultThreadCurrentCulture = myCulture;

            DependencyService.Register<JsonDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
