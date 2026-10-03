using Dapper;
using System.Data;
using TMS.Domain.LearningDomain.LearningPath;

namespace TMS.Infrastructure.Dapper.Handlers;

public class PathStatusTypeHandler : SqlMapper.TypeHandler<PathStatus>
{
    public override PathStatus? Parse(object value)
    {
        var statusName = value?.ToString();

        return PathStatus.FromName(statusName);
    }

    public override void SetValue(IDbDataParameter parameter, PathStatus status)
    {
        parameter.Value = status?.Name ?? (object)DBNull.Value;
    }
}
