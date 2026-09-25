using MediatR;
using TMS.Application.Shared.Interfaces;
using TMS.Domain.LearningDomain.Course;
using TMS.Domain.Shared.Results;

namespace TMS.Application.Learning.CourseApp.Commands.CreateCourse;

public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, ResultBase>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateCourseCommandHandler(
        ICourseRepository courseRepository,
        IUnitOfWork unitOfWork)
    {
        _courseRepository = courseRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<ResultBase> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = new Course(
            request.Id,
            request.SellableItemId,
            request.CourseName,
            request.Description,
            request.Duration);

        await _courseRepository.AddAsync(course, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ResultBase.Success();
    }
}
