using MediatR;
using TMS.Application.Learning.CourseApp.Commands.UpdateCourseDescription;
using TMS.Application.Shared.Interfaces;
using TMS.Domain.LearningDomain.Course;
using TMS.Domain.Shared.Results;

namespace TMS.Application.Learning.CourseApp.Commands.UpdateCourseDuration;

public class UpdateCourseDurationCommandHandler : IRequestHandler<UpdateCourseDurationCommand, ResultBase>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCourseDurationCommandHandler(ICourseRepository courseRepository, IUnitOfWork unitOfWork)
    {
        _courseRepository = courseRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<ResultBase> Handle(UpdateCourseDurationCommand request, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken);
        if (course is null)
            return CourseError.CourseNotFound;

        course.ChangeDuration(request.DurationInMinutes);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ResultBase.Success();
    }
}
