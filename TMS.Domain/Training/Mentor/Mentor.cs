using TMS.Domain.Shared;

namespace TMS.Domain.Training.Mentor;

public class Mentor : AggregateRoot<int>
{
    protected Mentor() : base(default) { }
}
