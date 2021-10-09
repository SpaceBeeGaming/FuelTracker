using System;
using System.Collections.Generic;
using System.ComponentModel;

using FuelTracker.Models;
using FuelTracker.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FuelTracker.Views
{
    public partial class RefuelingLogNewItemPage : ContentPage
    {
        public RefuelingLogItem? Item { get; set; }

        public RefuelingLogNewItemPage()
        {
            InitializeComponent();
            BindingContext = new RefuelingLogNewItemViewModel();
        }
    }
}