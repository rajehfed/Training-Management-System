using FluentValidation;
using TMS.Application.Shared.Validator;

namespace TMS.Application.Learning.LearningPathApp.Commands.UpdateLearningPathAudience;

public class UpdateLearningPathAudienceCommandValidator : AbstractValidator<UpdateLearningPathAudienceCommand>
{
    public UpdateLearningPathAudienceCommandValidator()
    {
        RuleFor(x => x.PathId).GreaterThan(0).WithMessage("PathId must be greater than zero.");
        RuleFor(x => x.NewAudience).ValidLearningPathAudience();
    }
}
