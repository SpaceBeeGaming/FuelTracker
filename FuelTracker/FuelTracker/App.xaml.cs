using System;
using System.Globalization;

using FuelTracker.Models;
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

            DependencyService.Register<IDataStore<RefuelingLogItem>, JsonDataStore<RefuelingLogItem>>();
            DependencyService.Register<IDataStore<DrivingLogItem>, JsonDataStore<DrivingLogItem>>();
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
