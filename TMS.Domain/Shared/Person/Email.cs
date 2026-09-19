using System.Text.RegularExpressions;
using TMS.Domain.Shared.Results;

namespace TMS.Domain.Shared.Person;

public class Email : ValueObject
{
    private static readonly Regex _emailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
    public string Address { get; } = null!;

    protected Email(string address) { Address = address; }
    public static Result<Email> Create(string address)
    {
        if (string.IsNullOrEmpty(address))
            return PersonError.EmailAddressEmpty;

        if (_emailRegex.IsMatch(address))
            return PersonError.InvalidEmailAddress;

        return new Email(address);
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Address;
    }
}
