using System.Text.RegularExpressions;
using TMS.Domain.Shared.Results;

namespace TMS.Domain.Shared.Person;

public class PhoneNumber : ValueObject
{
    private static readonly Regex _phoneRegex = new(@"^\+?[1-9]\d{1,14}$", RegexOptions.Compiled);
    public string Number { get; } = null!;
    protected PhoneNumber(string number) { Number = number; }
    public static Result<PhoneNumber> Create(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
            return PersonError.PhoneNumberEmpty;

        var cleanedNumber = number.Replace(" ", "").Replace("-", "");

        if (!_phoneRegex.IsMatch(cleanedNumber))
            return PersonError.InvalidPhoneNumber;

        return new PhoneNumber(cleanedNumber);
    }

    protected override IEnumerable<object> GetEqualityComponents() { yield return Number; }

    public override string ToString() => Number;
}
