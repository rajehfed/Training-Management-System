using Dapper;
using System.Data;
using TMS.Domain.LearningDomain.Course;

namespace TMS.Infrastructure.Dapper.Handlers;

public class CourseStatusTypeHandler : SqlMapper.TypeHandler<CourseStatus>
{
    public override CourseStatus? Parse(object value)
    {
        var statusName = value?.ToString();

        return CourseStatus.FromName(statusName);
    }

    public override void SetValue(IDbDataParameter parameter, CourseStatus? value)
    {
        parameter.Value = value?.Name ?? (object)DBNull.Value;
    }
}
