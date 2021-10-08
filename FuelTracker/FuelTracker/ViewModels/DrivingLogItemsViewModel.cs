using System;
using System.Collections.Generic;
using System.Text;

namespace FuelTracker.ViewModels
{
    public class DrivingLogItemsViewModel : BaseViewModel
    {
        public DrivingLogItemsViewModel()
        {
            Title = "Driving Log";
        }
        public void OnAppearing() => IsBusy = true;
    }
}
