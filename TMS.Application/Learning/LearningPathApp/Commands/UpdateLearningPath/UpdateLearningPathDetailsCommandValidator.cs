using FluentValidation;

namespace TMS.Application.Learning.LearningPathApp.Commands.UpdateLearningPath;

public class UpdateLearningPathDetailsCommandValidator : AbstractValidator<UpdateLearningPathCommand>
{
    public UpdateLearningPathDetailsCommandValidator()
    {
        RuleFor(x => x.PathId)
            .GreaterThan(0)
            .WithMessage("The Identifier must be grater than zero.");

        RuleFor(x => x.SellableItemId)
            .GreaterThan(0)
            .WithMessage("the Identifier of the Sellable item must be positive.");

        RuleFor(x => x.PathName)
            .NotEmpty()
            .WithMessage("The Name of the path is mandatory.")
            .MaximumLength(200)
            .WithMessage("The name of the path must be less than 200 digits");

        RuleFor(x => x.Outcomes)
            .NotEmpty()
            .WithMessage("The outcomes are mandatory.");

        RuleFor(x => x.Audience)
            .MaximumLength(500)
            .WithMessage("the audience must be less than 500 digits")
            .When(x => !string.IsNullOrWhiteSpace(x.Audience));
    }
}
