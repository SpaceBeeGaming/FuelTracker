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
    public class ItemsViewModel : BaseViewModel
    {
        private Item? _selectedItem;
        private readonly string sharePath = Path.Combine(FileSystem.CacheDirectory, "fuelTrackerData.json");
        private readonly string filePath = Path.Combine(FileSystem.AppDataDirectory, "store.json");


        public ObservableCollection<Item> Items { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command<Item> ItemTapped { get; }

        public Command ExportCommand { get; }

        public Item? SelectedItem
        {
            get => _selectedItem;
            set
            {
                _ = SetProperty(ref _selectedItem, value);
                OnItemSelected(value);
            }
        }

        public ItemsViewModel()
        {
            Title = "Browse";
            Items = new ObservableCollection<Item>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ItemTapped = new Command<Item>(OnItemSelected);
            ExportCommand = new Command(OnExport);

            AddItemCommand = new Command(OnAddItem);
        }

        private async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Items.Clear();
                IEnumerable<Item> items = await DataStore.GetItemsAsync(true);
                foreach (Item item in items)
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

        private async void OnItemSelected(Item? item)
        {
            if (item is not null)
            {
                // This will push the ItemDetailPage onto the navigation stack
                await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={item.Id}");
            }
        }
    }
}