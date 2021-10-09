using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using FuelTracker.Models;
using FuelTracker.Services;

using Xamarin.Forms;

namespace FuelTracker.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public static IDataStore<RefuelingLogItem> RefuelingDataStore => DependencyService.Get<IDataStore<RefuelingLogItem>>();
        public static IDataStore<DrivingLogItem> DrivingDataStore => DependencyService.Get<IDataStore<DrivingLogItem>>();

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        private string title = String.Empty;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action? onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
            {
                return false;
            }

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
