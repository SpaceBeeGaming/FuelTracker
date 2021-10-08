using System;
using System.Collections.Generic;

using FuelTracker.ViewModels;
using FuelTracker.Views;

using Xamarin.Forms;

namespace FuelTracker
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

        private async void OnMenuItemClicked(object sender, EventArgs e) 
            => await Current.GoToAsync("//LoginPage");
    }
}
