using System;
using System.Collections.Generic;
using System.ComponentModel;

using FuelTracker.Models;
using FuelTracker.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FuelTracker.Views
{
    public partial class NewItemPage : ContentPage
    {
        public Item? Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();
            BindingContext = new NewItemViewModel();
        }
    }
}