using MediatR;
using TMS.Application.Shared.Interfaces;
using TMS.Domain.LearningDomain.Course;
using TMS.Domain.LearningDomain.LearningPath;
using TMS.Domain.Shared.Results;

namespace TMS.Application.Learning.LearningPathApp.Commands.UpdateCourseOrderInPath;

public class UpdateCourseOrderInPathCommandHandler : IRequestHandler<UpdateCourseOrderInPathCommand, ResultBase>
{
    private readonly ILearningPathRepository _learningPathRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateCourseOrderInPathCommandHandler(
        ILearningPathRepository learningPathRepository,
        ICourseRepository courseRepository,
        IUnitOfWork unitOfWork)
    {
        _learningPathRepository = learningPathRepository;
        _courseRepository = courseRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<ResultBase> Handle(UpdateCourseOrderInPathCommand request, CancellationToken cancellationToken)
    {
        var courseExists = await _courseRepository.ExistsAsync(request.CourseId, cancellationToken);
        if (!courseExists)
            return CourseError.CourseNotFound;

        var courseExistsInPath = await _learningPathRepository.CourseExistsInPath(request.PathId, request.CourseId);
        if (!courseExistsInPath)
            return LearningPathError.CourseNotFoundInThePath;

        var learningPath = await _learningPathRepository.GetByIdAsync(request.PathId, cancellationToken);
        if (learningPath is null)
            return LearningPathError.LearningPathNotFound;

        var result = learningPath.ChangeCourseOrderInPath(request.CourseId, request.NewOrder);
        if (!result.IsSuccess)
            return result;

        _learningPathRepository.Update(learningPath);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ResultBase.Success();
    }
}
