using TMS.Domain.Shared;

namespace TMS.Domain.LearningDomain.LearningPath;

public class Audience : ValueObject
{
    public string Value { get; }

    public Audience(string value) : base()
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Audience text cannot be empty.", nameof(value));

        if (value.Length > 500)
            throw new ArgumentException("Audience text cannot exceed 500 characters.", nameof(value));

        Value = value.Trim();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
