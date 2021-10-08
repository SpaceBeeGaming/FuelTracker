using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FuelTracker.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FuelTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DrivingLogItemsPage : ContentPage
    {
        private readonly DrivingLogItemsViewModel viewModel;

        public DrivingLogItemsPage()
        {
            BindingContext = viewModel = new();
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.OnAppearing();
        }
    }
}