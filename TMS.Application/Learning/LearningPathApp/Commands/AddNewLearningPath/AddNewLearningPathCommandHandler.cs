using MediatR;
using TMS.Application.Shared.Interfaces;
using TMS.Domain.LearningDomain.LearningPath;
using TMS.Domain.Shared.Results;

namespace TMS.Application.Learning.LearningPathApp.Commands.AddNewLearningPath;

public class AddNewLearningPathCommandHandler : IRequestHandler<AddNewLearningPathCommand, ResultBase>
{
    private readonly ILearningPathRepository _learningPathRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddNewLearningPathCommandHandler(
        ILearningPathRepository learningPathRepository,
        ICourseRepository courseRepository,
        IUnitOfWork unitOfWork)
    {
        _learningPathRepository = learningPathRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<ResultBase> Handle(AddNewLearningPathCommand request, CancellationToken cancellationToken)
    {
        var outcomes = request.Outcomes.Split([',']).ToList();
        var learningPath = new LearningPath(
            request.PathId,
            request.SellableItemId,
            request.PathName,
            new Outcomes(outcomes),
            request.Audience);

        await _learningPathRepository.AddAsync(learningPath, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ResultBase.Success();
    }
}
