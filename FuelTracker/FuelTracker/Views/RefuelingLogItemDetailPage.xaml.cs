using System.ComponentModel;

using FuelTracker.ViewModels;

using Xamarin.Forms;

namespace FuelTracker.Views
{
    public partial class RefuelingLogItemDetailPage : ContentPage
    {
        public RefuelingLogItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new RefuelingLogItemDetailViewModel();
        }
    }
}