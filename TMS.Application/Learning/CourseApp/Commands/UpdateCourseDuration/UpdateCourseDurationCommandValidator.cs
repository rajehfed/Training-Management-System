using FluentValidation;
using TMS.Application.Shared.Validator;

namespace TMS.Application.Learning.CourseApp.Commands.UpdateCourseDuration;

public class UpdateCourseDurationCommandValidator : AbstractValidator<UpdateCourseDurationCommand>
{
    public UpdateCourseDurationCommandValidator()
    {
        RuleFor(x => x.CourseId).NotEmpty();

        RuleFor(x => x.DurationInMinutes).ValidCourseDuration();
    }
}
