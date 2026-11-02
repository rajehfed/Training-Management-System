using TMS.Domain.Shared;

namespace TMS.Domain.LearningDomain.LearningPath;

public class Outcomes : ValueObject
{
    private readonly List<string> _outcomeList = [];
    public IReadOnlyList<string> Items => _outcomeList.AsReadOnly();
    public string Value => string.Join(',', _outcomeList);

    protected Outcomes() { }

    public Outcomes(IEnumerable<string> outcomeList)
    {
        if (outcomeList == null || !outcomeList.Any())
            throw new ArgumentException("The outcome list cannot be empty.", nameof(outcomeList));

        _outcomeList.AddRange(outcomeList);
    }

    public Outcomes AddOutcome(string outcome)
    {
        if (string.IsNullOrWhiteSpace(outcome))
            throw new ArgumentException("Outcome cannot be empty.", nameof(outcome));

        var updatedList = new List<string>(_outcomeList) { outcome };
        return new Outcomes(updatedList);
    }

    public Outcomes RemoveOutcome(string outcome)
    {
        if (string.IsNullOrWhiteSpace(outcome))
            throw new ArgumentException("Outcome cannot be empty.", nameof(outcome));

        var updatedList = _outcomeList.Where(o => !o.Equals(outcome, StringComparison.OrdinalIgnoreCase)).ToList();
        return new Outcomes(updatedList);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        foreach (var outcome in _outcomeList.OrderBy(o => o))
        {
            yield return outcome;
        }
    }

    public override string ToString()
    {
        return string.Join(',', Items);
    }
}