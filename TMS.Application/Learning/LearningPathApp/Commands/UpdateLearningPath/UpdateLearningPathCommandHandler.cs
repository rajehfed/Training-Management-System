using MediatR;
using TMS.Application.Shared.Interfaces;
using TMS.Domain.LearningDomain.LearningPath;
using TMS.Domain.Shared.Results;

namespace TMS.Application.Learning.LearningPathApp.Commands.UpdateLearningPath;

public class UpdateLearningPathCommandHandler : IRequestHandler<UpdateLearningPathCommand, ResultBase>
{
    private readonly ILearningPathRepository _learningPathRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateLearningPathCommandHandler(ILearningPathRepository learningPathRepository, IUnitOfWork unitOfWork)
    {
        _learningPathRepository = learningPathRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<ResultBase> Handle(UpdateLearningPathCommand request, CancellationToken cancellationToken)
    {
        var learningPath = await _learningPathRepository.GetByIdAsync(request.PathId, cancellationToken);

        if (learningPath is null)
            return LearningPathError.LearningPathNotFound;

        learningPath.UpdateDetails(request.PathName, request.Outcomes, request.Audience);
        _learningPathRepository.Update(learningPath);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ResultBase.Success();
    }
}
