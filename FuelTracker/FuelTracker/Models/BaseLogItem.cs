using System;

namespace FuelTracker.Models
{
    public class BaseLogItem
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public double CarKm { get; set; }

    }
}
