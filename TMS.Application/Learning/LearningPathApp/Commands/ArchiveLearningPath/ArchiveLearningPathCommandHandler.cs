using MediatR;
using TMS.Application.Shared.Interfaces;
using TMS.Domain.LearningDomain.LearningPath;
using TMS.Domain.Shared.Results;

namespace TMS.Application.Learning.LearningPathApp.Commands.ArchiveLearningPath;

public class ArchiveLearningPathCommandHandler : IRequestHandler<ArchiveLearningPathCommand, ResultBase>
{
    private readonly ILearningPathRepository _learningPathRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ArchiveLearningPathCommandHandler(
        ILearningPathRepository learningPathRepository,
        IUnitOfWork unitOfWork)
    {
        _learningPathRepository = learningPathRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<ResultBase> Handle(ArchiveLearningPathCommand request, CancellationToken cancellationToken)
    {
        var learningPath = await _learningPathRepository.GetByIdAsync(request.PathId, cancellationToken);
        if (learningPath is null)
            return LearningPathError.LearningPathNotFound;

        var result = learningPath.MarkAsArchived();
        if (!result.IsSuccess)
            return result;

        _learningPathRepository.Update(learningPath);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ResultBase.Success();
    }
}
