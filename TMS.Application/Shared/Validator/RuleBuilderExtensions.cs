using FluentValidation;

namespace TMS.Application.Shared.Validator;

public static class RuleBuilderExtensions
{
    public static IRuleBuilderOptions<T, string> ValidCourseTitle<T>(this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("The Title of the course is required")
            .MaximumLength(150).WithMessage("Must be less than 150 digit");
    }

    public static IRuleBuilderOptions<T, int> ValidCourseDuration<T>(this IRuleBuilder<T, int> ruleBuilder)
    {
        return ruleBuilder
            .GreaterThan(0).WithMessage("The duration of the Course must be greater than zero.");
    }
}
