using MediatR;
using TMS.Application.Shared.Interfaces;
using TMS.Domain.LearningDomain.LearningPath;
using TMS.Domain.Shared.Results;

namespace TMS.Application.Learning.LearningPathApp.Commands.UpdateLearningPathOutcomes;

public class UpdateLearningPathOutcomesCommandHandler : IRequestHandler<UpdateLearningPathOutcomesCommand, ResultBase>
{
    private readonly ILearningPathRepository _learningPathRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateLearningPathOutcomesCommandHandler(
        ILearningPathRepository learningPathRepository,
        IUnitOfWork unitOfWork)
    {
        _learningPathRepository = learningPathRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<ResultBase> Handle(
        UpdateLearningPathOutcomesCommand request,
        CancellationToken cancellationToken)
    {
        var learningPath = await _learningPathRepository.GetByIdAsync(request.PathId, cancellationToken);
        if (learningPath is null)
            return LearningPathError.LearningPathNotFound;

        var result = learningPath.ChangeOutcomes(new Outcomes(request.Outcomes));
        if (!result.IsSuccess)
            return result;

        _learningPathRepository.Update(learningPath);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ResultBase.Success();
    }
}
