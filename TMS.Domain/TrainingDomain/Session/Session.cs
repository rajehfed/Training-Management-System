using TMS.Domain.Shared;

namespace TMS.Domain.TrainingDomain.Session;

public class Session : Entity<int>
{
    public Session(int id) : base(id) { }
}
