using MediatR;
using TMS.Application.Shared.Interfaces;
using TMS.Domain.LearningDomain.Course;
using TMS.Domain.Shared.Results;

namespace TMS.Application.Learning.CourseApp.Commands.UpdateCourseDescription;

public class UpdateCourseDescriptionCommandHandler : IRequestHandler<UpdateCourseDescriptionCommand, ResultBase>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCourseDescriptionCommandHandler(ICourseRepository courseRepository, IUnitOfWork unitOfWork)
    {
        _courseRepository = courseRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<ResultBase> Handle(UpdateCourseDescriptionCommand request, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken);
        if (course is null)
            return CourseError.CourseNotFound;

        course.ChangeDescription(request.Description);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return ResultBase.Success();
    }
}
