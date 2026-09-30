using MediatR;
using TMS.Application.Shared.Interfaces;
using TMS.Domain.LearningDomain.LearningPath;
using TMS.Domain.Shared.Results;

namespace TMS.Application.Learning.LearningPathApp.Commands.UpdateLearningPathAudience;

public class UpdateLearningPathAudienceCommandHandler : IRequestHandler<UpdateLearningPathAudienceCommand, ResultBase>
{
    private readonly ILearningPathRepository _learningPathRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateLearningPathAudienceCommandHandler(
        ILearningPathRepository learningPathRepository,
        IUnitOfWork unitOfWork)
    {
        _learningPathRepository = learningPathRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<ResultBase> Handle(
        UpdateLearningPathAudienceCommand request,
        CancellationToken cancellationToken)
    {
        var learningPath = await _learningPathRepository.GetByIdAsync(request.PathId, cancellationToken);
        if (learningPath is null)
            return LearningPathError.LearningPathNotFound;

        var result = learningPath.ChangeAudience(request.NewAudience);
        if (!result.IsSuccess)
            return result;

        _learningPathRepository.Update(learningPath);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ResultBase.Success();
    }
}
