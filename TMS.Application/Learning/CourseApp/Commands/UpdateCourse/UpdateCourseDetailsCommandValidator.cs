using FluentValidation;

namespace TMS.Application.Learning.CourseApp.Commands.UpdateCourse;

public class UpdateCourseDetailsCommandValidator : AbstractValidator<UpdateCourseCommand>
{
    public UpdateCourseDetailsCommandValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty().WithMessage("The Identifier of the the course is required");

        RuleFor(x => x.CourseTitle)
            .NotEmpty().WithMessage("The course title is required")
            .MaximumLength(150).WithMessage("Must be less than 150 digits");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("The description is mandatory.")
            .MaximumLength(1000).WithMessage("The description must be no more than 1000 digits");

        RuleFor(x => x.Duration)
            .GreaterThan(0).WithMessage("The duration must be greater than zero");
    }
}
