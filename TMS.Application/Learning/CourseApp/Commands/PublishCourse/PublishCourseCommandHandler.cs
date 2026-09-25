using MediatR;
using TMS.Application.Shared.Interfaces;
using TMS.Domain.LearningDomain.Course;
using TMS.Domain.Shared.Results;

namespace TMS.Application.Learning.CourseApp.Commands.PublishCourse;

public class PublishCourseCommandHandler : IRequestHandler<PublishCourseCommand, ResultBase>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PublishCourseCommandHandler(ICourseRepository courseRepository, IUnitOfWork unitOfWork)
    {
        _courseRepository = courseRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<ResultBase> Handle(PublishCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken);
        if (course is null)
            return CourseError.CourseNotFound;

        var result = course.MarkAsPublished();
        if (!result.IsSuccess)
            return result;

        _courseRepository.Update(course);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ResultBase.Success();
    }
}
