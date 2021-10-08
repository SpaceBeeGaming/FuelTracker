using System;
using System.Collections.Generic;
using System.Text;

using FuelTracker.Models;

using Xamarin.Forms;

namespace FuelTracker.Services
{
    public class AlternateColorsDataTemplateSelector : DataTemplateSelector
    {
        private static int index = 0;

        public DataTemplate? EvenTemplate { get; set; }
        public DataTemplate? OddTemplate { get; set; }

        protected override DataTemplate? OnSelectTemplate(object item, BindableObject container) 
            => index++ % 2 is 0 ? EvenTemplate : OddTemplate;
    }
}
