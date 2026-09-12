using System.Reflection;

namespace TMS.Domain.Shared;

public abstract class Enumeration<TEnum> : IEquatable<Enumeration<TEnum>>
    where TEnum : Enumeration<TEnum>
{
    public int Id { get; }
    public string Name { get; }
    protected Enumeration(int id, string name)
    {
        Id = id;
        Name = name;
    }
    public static IReadOnlyCollection<TEnum> GetAll() =>
        typeof(TEnum)
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Select(f => f.GetValue(null))
            .OfType<TEnum>()
            .ToList();
    public static TEnum? FromName(string name) => 
        GetAll().FirstOrDefault(item => string.Equals(item.Name, name, StringComparison.OrdinalIgnoreCase));
    public bool Equals(Enumeration<TEnum>? other) => other is not null && Id == other.Id;
    public override bool Equals(object? obj) => obj is Enumeration<TEnum> other && Equals(other);
    public override int GetHashCode() => Id.GetHashCode();
    public override string ToString() => Name;
}
