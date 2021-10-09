using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using FuelTracker.Models;
using FuelTracker.Services;
using FuelTracker.Views;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace FuelTracker.ViewModels
{
    public class RefuelingLogItemsViewModel : BaseViewModel
    {
        private RefuelingLogItem? _selectedItem;
        private readonly string sharePath = Path.Combine(FileSystem.CacheDirectory, "fuelTrackerData.json");
        private readonly string filePath = Path.Combine(FileSystem.AppDataDirectory, "store.json");


        public ObservableCollection<RefuelingLogItem> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<RefuelingLogItem> ItemTapped { get; }

        public Command ExportCommand { get; }

        public RefuelingLogItem? SelectedItem
        {
            get => _selectedItem;
            set
            {
                _ = SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        public RefuelingLogItemsViewModel()
        {
            Title = "Refueling Log";
            Items = new ObservableCollection<RefuelingLogItem>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ItemTapped = new Command<RefuelingLogItem>(OnItemSelected);
            ExportCommand = new Command(OnExport);

            AddItemCommand = new Command(OnAddItem);
        }

        private async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                IEnumerable<RefuelingLogItem> items = await DataStore.GetItemsAsync(true);
                foreach (RefuelingLogItem item in items)
                {
                    Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing() => IsBusy = true;

        private async void OnAddItem(object obj)
            => await Shell.Current.GoToAsync(nameof(NewItemPage));

        private async void OnExport()
        {
            if (File.Exists(filePath))
            {
                File.Copy(filePath, sharePath, true);
                await ShareService.ShareFile(sharePath);
            }
        }

        private async void OnItemSelected(RefuelingLogItem? item)
        {
            if (item is not null)
            {
                // This will push the ItemDetailPage onto the navigation stack
                await Shell.Current.GoToAsync($"{nameof(RefuelingLogItemDetailPage)}?{nameof(RefuelingLogItemDetailViewModel.ItemId)}={item.Id}");
            }
        }
    }
}