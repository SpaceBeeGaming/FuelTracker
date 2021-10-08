using System.ComponentModel;

using FuelTracker.ViewModels;

using Xamarin.Forms;

namespace FuelTracker.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}