using TMS.Domain.Shared.Person.Events;
using TMS.Domain.Shared.Results;

namespace TMS.Domain.Shared.Person;

public class Person : AggregateRoot<int>
{
    public string FirstName { get; private set; } = null!;
    public string LastName { get; private set; } = null!;
    public PhoneNumber PhoneNumber { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public PersonStatus Status { get; private set; } = PersonStatus.Active;
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    private Person() : base(default) { }
    public Person(
        int id,
        string firstName,
        string lastName,
        PhoneNumber phoneNumber,
        Email email) : base(id)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new ArgumentException("First name cannot be empty.", nameof(firstName));

        if (string.IsNullOrWhiteSpace(lastName))
            throw new ArgumentException("Last name cannot be empty.", nameof(lastName));

        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        Email = email;
        Status = PersonStatus.Active;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = null;
    }

    public ResultBase ChangeFirstName(string firstName)
    {
        if (Status == PersonStatus.Archived)
            return PersonError.PersonArchived;

        if (string.IsNullOrWhiteSpace(firstName))
            return PersonError.InvalidFirstName;

        this.FirstName = firstName;
        this.UpdatedAt = DateTime.UtcNow;

        this.AddDomainEvent(new PersonUpdatedDomainEvent(this.Id));

        return ResultBase.Success();
    }
    public ResultBase ChangeLastName(string lastName)
    {
        if (Status == PersonStatus.Archived)
            return PersonError.PersonArchived;

        if (string.IsNullOrWhiteSpace(lastName))
            return PersonError.InvalidLastName;

        this.LastName = lastName;
        this.UpdatedAt = DateTime.UtcNow;

        this.AddDomainEvent(new PersonUpdatedDomainEvent(this.Id));

        return ResultBase.Success();
    }
    public ResultBase ChangePhoneNumber(string phoneNumber)
    {
        if (Status == PersonStatus.Archived)
            return PersonError.PersonArchived;

        var phoneResult = PhoneNumber.Create(phoneNumber);

        if (!phoneResult.IsSuccess)
            return phoneResult.Error!;

        this.PhoneNumber = phoneResult.Value!;
        this.UpdatedAt = DateTime.UtcNow;

        this.AddDomainEvent(new PersonUpdatedDomainEvent(this.Id));

        return ResultBase.Success();
    }
    public ResultBase ChangeEmailAddress(string email)
    {
        if (Status == PersonStatus.Archived)
            return PersonError.PersonArchived;

        var emailResult = Email.Create(email);

        if (!emailResult.IsSuccess)
            return emailResult.Error!;

        this.Email = emailResult.Value!;
        this.UpdatedAt = DateTime.UtcNow;

        this.AddDomainEvent(new PersonUpdatedDomainEvent(this.Id));

        return ResultBase.Success();
    }

    public ResultBase Archive()
    {
        if (this.Status == PersonStatus.Archived)
            return PersonError.AlreadyArchived;

        this.Status = PersonStatus.Archived;
        this.UpdatedAt = DateTime.UtcNow;

        this.AddDomainEvent(new PersonArchivedDomainEvent(this.Id));

        return ResultBase.Success();
    }
    public ResultBase Activate()
    {
        this.Status = PersonStatus.Active;
        this.UpdatedAt = DateTime.UtcNow;

        this.AddDomainEvent(new PersonActivatedDomainEvent(this.Id));

        return ResultBase.Success();
    }
}
