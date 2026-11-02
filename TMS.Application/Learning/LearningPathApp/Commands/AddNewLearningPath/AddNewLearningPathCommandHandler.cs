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
        IUnitOfWork unitOfWork)
    {
        _learningPathRepository = learningPathRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<ResultBase> Handle(AddNewLearningPathCommand request, CancellationToken cancellationToken)
    {
        var outcomes = request.Outcomes
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToList();
        var learningPath = new LearningPath(
           0,
           0,
            request.PathName,
            new Outcomes(outcomes),
            request.Audience,
            PathStatus.Draft,
            DateTime.UtcNow,
            null,
            []);

        await _learningPathRepository.AddAsync(learningPath, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ResultBase.Success();
    }
}
