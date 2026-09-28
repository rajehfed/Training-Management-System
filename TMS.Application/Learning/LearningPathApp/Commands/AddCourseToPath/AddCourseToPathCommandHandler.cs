using MediatR;
using TMS.Application.Shared.Interfaces;
using TMS.Domain.LearningDomain.Course;
using TMS.Domain.LearningDomain.LearningPath;
using TMS.Domain.Shared.Results;

namespace TMS.Application.Learning.LearningPathApp.Commands.AddCourseToPath;

public class AddCourseToPathCommandHandler : IRequestHandler<AddCourseToPathCommand, ResultBase>
{
    private readonly ILearningPathRepository _learningPathRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddCourseToPathCommandHandler(
        ILearningPathRepository learningPathRepository,
        ICourseRepository courseRepository,
        IUnitOfWork unitOfWork)
    {
        _learningPathRepository = learningPathRepository;
        _courseRepository = courseRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<ResultBase> Handle(AddCourseToPathCommand request, CancellationToken cancellationToken)
    {
        var courseExists = await _courseRepository.ExistsAsync(request.CourseId, cancellationToken);
        if (!courseExists)
            return CourseError.CourseNotFound;

        var learningPath = await _learningPathRepository.GetByIdAsync(request.LearningPathId, cancellationToken);
        if (learningPath is null)
            return LearningPathError.LearningPathNotFound;

        var result = learningPath.AddCourseToPath(request.CourseId, request.OrderInPath);
        if (!result.IsSuccess)
            return result;

        _learningPathRepository.Update(learningPath);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ResultBase.Success();
    }
}
