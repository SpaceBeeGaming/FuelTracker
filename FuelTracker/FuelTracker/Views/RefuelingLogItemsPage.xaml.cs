using FuelTracker.ViewModels;

using Xamarin.Forms;

namespace FuelTracker.Views
{
    public partial class RefuelingLogItemsPage : ContentPage
    {
        private readonly RefuelingItemsViewModel _viewModel;

        public RefuelingLogItemsPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new RefuelingItemsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}