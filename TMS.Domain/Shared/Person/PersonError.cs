namespace TMS.Domain.Shared.Person;

public static class PersonError
{
    public static readonly ErrorBase InvalidFirstName = ErrorBase.Validation(
        "Person.InvalidFirstName",
        "The first name is invalid.");
    public static readonly ErrorBase InvalidLastName = ErrorBase.Validation(
        "Person.InvalidLastName",
        "The last name is invalid.");
    public static readonly ErrorBase PersonNotFound = ErrorBase.NotFound(
        "Person.PersonNotFound",
        "The person was not found.");
    public static readonly ErrorBase InvalidPhoneNumber = ErrorBase.Validation(
        "Person.InvalidPhoneNumber",
        "The phone number is invalid.");
    public static readonly ErrorBase InvalidEmailAddress = ErrorBase.Validation(
        "Person.InvalidEmailAddress",
        "The email address is invalid.");
    public static readonly ErrorBase PhoneNumberEmpty = ErrorBase.Validation(
        "Person.PhoneNumberEmpty",
        "The phone number cannot be empty.");
    public static readonly ErrorBase EmailAddressEmpty = ErrorBase.Validation(
        "Person.EmailAddressEmpty",
        "The email address cannot be empty.");
    public static readonly ErrorBase AlreadyArchived = ErrorBase.Conflict(
        "Person.AlreadyArchived",
        "The status of the person is already archived");
    public static readonly ErrorBase PersonArchived = ErrorBase.Validation(
        "Person.PersonArchived",
        "The status of the person is Archived.");
}
