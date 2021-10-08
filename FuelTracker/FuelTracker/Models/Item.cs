using System;

namespace FuelTracker.Models
{
    public class Item
    {
        public Guid Id { get; set; }
        public decimal Cost { get; set; }
        public double Amount { get; set; }
        public DateTime Date { get; set; }
        public double CarKm { get; set; }
    }
}