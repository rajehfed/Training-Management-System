namespace TMS.Domain.Shared.Person;

public abstract class PersonStatus : Enumeration<PersonStatus>
{
    public static readonly PersonStatus Active = new ActiveStatus();
    public static readonly PersonStatus Archived = new ArchivedStatus();

    protected PersonStatus(int id, string name) : base(id, name) { }

    private sealed class ActiveStatus() : PersonStatus(1, "Active");
    private sealed class ArchivedStatus() : PersonStatus(2, "Archived");
}
