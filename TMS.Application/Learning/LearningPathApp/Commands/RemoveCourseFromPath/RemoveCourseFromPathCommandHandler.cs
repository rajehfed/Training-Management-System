using MediatR;
using TMS.Application.Shared.Interfaces;
using TMS.Domain.LearningDomain.Course;
using TMS.Domain.LearningDomain.LearningPath;
using TMS.Domain.Shared.Results;

namespace TMS.Application.Learning.LearningPathApp.Commands.RemoveCourseFromPath;

public class RemoveCourseFromPathCommandHandler : IRequestHandler<RemoveCourseFromPathCommand, ResultBase>
{
    private readonly ILearningPathRepository _learningPathRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RemoveCourseFromPathCommandHandler(
        ILearningPathRepository learningPathRepository, 
        ICourseRepository courseRepository, 
        IUnitOfWork unitOfWork)
    {
        _learningPathRepository = learningPathRepository;
        _courseRepository = courseRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<ResultBase> Handle(RemoveCourseFromPathCommand request, CancellationToken cancellationToken)
    {
        var courseExists = await _courseRepository.ExistsAsync(request.CourseId, cancellationToken);
        if (!courseExists)
            return CourseError.CourseNotFound;

        var learningPath = await _learningPathRepository.GetByIdAsync(request.PathId, cancellationToken);
        if (learningPath is null)
            return LearningPathError.LearningPathNotFound;

        var result = learningPath.RemoveCourseFromPath(request.CourseId);
        if (!result.IsSuccess)
            return result;

        await _learningPathRepository.RemoveCourseFromLearningPathAsync(request.PathId, request.CourseId, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ResultBase.Success();
    }
}
