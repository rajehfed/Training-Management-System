using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TMS.Application.Shared.Validator;

namespace TMS.Application.Learning.CourseApp.Commands.UpdateCourseTitle;

public class RenameCourseCommandValidator : AbstractValidator<UpdateCourseTitleCommand>
{
    public RenameCourseCommandValidator()
    {
        RuleFor(x => x.CourseId).NotEmpty();

        // استخدام القاعدة المشتركة
        RuleFor(x => x.CourseTitle).ValidCourseTitle();
    }
}
