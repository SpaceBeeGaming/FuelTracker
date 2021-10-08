using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

using FuelTracker.Models;

using Xamarin.Forms;

namespace FuelTracker.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        private decimal cost;
        private double amount;

        public NewItemViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            PropertyChanged += (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
            => cost != 0 && amount != 0 && carKm != 0;

        public decimal Cost
        {
            get => cost;
            set => SetProperty(ref cost, value);
        }

        public double Amount
        {
            get => amount;
            set => SetProperty(ref amount, value);
        }

        private double carKm;

        public double CarKm
        {
            get => carKm;
            set => SetProperty(ref carKm, value);
        }

        private bool errorVisible;

        public bool ErrorVisible
        {
            get => errorVisible;
            set => SetProperty(ref errorVisible, value);
        }



        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
            => await Shell.Current.GoToAsync("..");

        private async void OnSave()
        {
            RefuelingLogItem newItem = new()
            {
                Id = Guid.NewGuid(),
                Cost = Cost,
                Amount = Amount,
                CarKm = CarKm,
                Date = DateTime.Now
            };

            bool result = await DataStore.AddItemAsync(newItem);
            if (result is false)
            {
                ErrorVisible = true;
            }

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}
