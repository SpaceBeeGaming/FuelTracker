using FuelTracker.ViewModels;

using Xamarin.Forms;

namespace FuelTracker.Views
{
    public partial class RefuelingLogItemsPage : ContentPage
    {
        private readonly RefuelingLogItemsViewModel _viewModel;

        public RefuelingLogItemsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new RefuelingLogItemsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}