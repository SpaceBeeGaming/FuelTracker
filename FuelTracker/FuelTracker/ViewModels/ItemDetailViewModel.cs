using System;
using System.Diagnostics;
using System.Threading.Tasks;

using FuelTracker.Models;

using Xamarin.Forms;

namespace FuelTracker.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public class ItemDetailViewModel : BaseViewModel
    {
        private string itemId;
        private decimal cost;
        private double amount;
        private DateTime date;
        private decimal costPreL;

        public Command DeleteItemCommand { get; }

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

        public string ItemId
        {
            get => itemId;
            set
            {
                itemId = value;
                LoadItemId(Guid.Parse(value));
            }
        }

        public decimal CostPerL
        {
            get => costPreL;
            set => SetProperty(ref costPreL, value);
        }

        private double carKm;

        public double CarKm
        {
            get => carKm;
            set => SetProperty(ref carKm, value);
        }


        public DateTime Date
        {
            get => date;
            set => SetProperty(ref date, value);
        }

        public ItemDetailViewModel()
            => DeleteItemCommand = new Command(OnDeleteItem);

        public async void OnDeleteItem(object obj)
        {
            await DataStore.DeleteItemAsync(Guid.Parse(itemId));

            await Shell.Current.GoToAsync("..");
        }

        public async void LoadItemId(Guid itemId)
        {
            try
            {
                Item? item = await DataStore.GetItemAsync(itemId);
                Cost = item.Cost;
                Amount = item.Amount;
                Date = item.Date;
                CostPerL = Cost / (decimal)Amount;
                CarKm = item.CarKm;
            }
            catch (Exception)
            {
                Debug.WriteLine("Failed to Load Item");
            }
        }
    }
}
