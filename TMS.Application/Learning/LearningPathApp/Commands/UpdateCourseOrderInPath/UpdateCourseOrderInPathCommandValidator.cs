using FluentValidation;
using TMS.Application.Shared.Validator;

namespace TMS.Application.Learning.LearningPathApp.Commands.UpdateCourseOrderInPath;

public class UpdateCourseOrderInPathCommandValidator : AbstractValidator<UpdateCourseOrderInPathCommand>
{
    public UpdateCourseOrderInPathCommandValidator()
    {
        RuleFor(x => x.PathId).GreaterThan(0).WithMessage("PathId must be greater than zero.");
        RuleFor(x => x.CourseId).GreaterThan(0).WithMessage("CourseId must be greater than zero.");
        RuleFor(x => x.NewOrder).ValidLearningPathCourseOrder();
    }
}
