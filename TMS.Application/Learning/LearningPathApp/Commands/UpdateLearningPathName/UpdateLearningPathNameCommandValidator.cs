using FluentValidation;
using TMS.Application.Shared.Validator;

namespace TMS.Application.Learning.LearningPathApp.Commands.UpdateLearningPathName;

public class UpdateLearningPathNameCommandValidator : AbstractValidator<UpdateLearningPathNameCommand>
{
    public UpdateLearningPathNameCommandValidator()
    {
        RuleFor(x => x.PathId).GreaterThan(0).WithMessage("PathId must be greater than zero.");
        RuleFor(x => x.NewPathName).ValidLearningPathName();
    }
}
