using TMS.Domain.Shared;
using TMS.Domain.Shared.Person;

namespace TMS.Domain.Training.Mentor;

public class Mentor : AggregateRoot<int>
{
    public int PersonId { get; private set; }
    protected Mentor() : base(default) { }
    public Mentor(int mentorId, int personId) : base(mentorId)
    {
        if (personId < 0)
            throw new ArgumentOutOfRangeException(nameof(personId), "The personId must be greater than zero.");

        this.PersonId = personId;
    }
}
