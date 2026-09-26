using MediatR;
using TMS.Application.Shared.Interfaces;
using TMS.Domain.LearningDomain.Course;
using TMS.Domain.Shared.Results;

namespace TMS.Application.Learning.CourseApp.Commands.ArchiveCourse;

public class ArchiveCourseCommandHandler : IRequestHandler<ArchiveCourseCommand, ResultBase>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ArchiveCourseCommandHandler(ICourseRepository courseRepository, IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _courseRepository = courseRepository;
    }

    public async Task<ResultBase> Handle(ArchiveCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken);

        if (course is null)
            return CourseError.CourseNotFound;

        var result = course.MarkAsArchived();
        if (!result.IsSuccess)
            return result;

        _courseRepository.Update(course);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ResultBase.Success();
    }
}
