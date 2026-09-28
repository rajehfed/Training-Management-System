using TMS.Domain.Shared;

namespace TMS.Domain.LearningDomain.LearningPath;

public record Outcome
{
    public int Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public Outcome(int id, string name, string description)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("The outcome name cannot be empty.", nameof(name));
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("The outcome description cannot be empty.", nameof(description));
        Id = id;
        Name = name;
        Description = description;
    }
}
public class Outcomes : ValueObject
{
    private readonly List<Outcome> _outcomeList = [];
    public IReadOnlyList<Outcome> Items => _outcomeList.AsReadOnly();
    protected Outcomes() : base() { }
    public Outcomes(IEnumerable<Outcome> outcomeList)
    {
        if (outcomeList == null || !outcomeList.Any())
            throw new ArgumentException("The outcome list cannot be empty.", nameof(outcomeList));

        _outcomeList.AddRange(outcomeList);
    }
    public Outcomes AddOutcome(Outcome outcome)
    {
        if (outcome == null)
            throw new ArgumentNullException(nameof(outcome), "Outcome cannot be null.");

        var updatedList = new List<Outcome>(_outcomeList) { outcome };
        return new Outcomes(updatedList);
    }
    public Outcomes RemoveOutcome(Outcome outcome)
    {
        if (outcome == null)
            throw new ArgumentNullException(nameof(outcome), "Outcome cannot be null.");

        var updatedList = _outcomeList.Where(o => o.Id != outcome.Id).ToList();
        return new Outcomes(updatedList);
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        foreach (var outcome in _outcomeList.OrderBy(o => o.Id))
        {
            yield return outcome;
        }
    }
    public override string ToString()
    {
        return string.Join(',', Items.Select(i => i.Name));
    }
}
