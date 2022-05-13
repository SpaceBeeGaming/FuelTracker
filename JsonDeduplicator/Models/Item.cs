using System.Text.Json.Serialization;

namespace JsonDeduplicator.Models;

internal class Item : IEquatable<Item>
{
    public Guid Id { get; set; }
    public decimal Cost { get; set; }
    public double Amount { get; set; }
    public DateTime Date { get; set; }
    public double CarKm { get; set; }

    public bool Equals(Item? other) =>
        ReferenceEquals(this, other)
        || (Id == other?.Id
            && Cost == other.Cost
            && Amount == other.Amount
            && Date == other.Date
            && CarKm == other.CarKm);

    public override bool Equals(object? obj) => Equals(obj as Item);
    public override int GetHashCode() => HashCode.Combine(Id, Cost, Amount, Date, CarKm);
    public static bool Equals(Item? x, Item? y) => x is null ? y is null : x.Equals(y);
    public static bool operator ==(Item? item1, Item? item2) => Equals(item1, item2);
    public static bool operator !=(Item? item1, Item? item2) => !(item1 == item2);
}

[JsonSerializable(typeof(List<Item>))]
[JsonSourceGenerationOptions(WriteIndented = true)]
internal partial class ItemJsonContext : JsonSerializerContext { }
