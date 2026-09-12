using TMS.Domain.Shared;

namespace TMS.Domain.Learning.Course;

public class CourseDuration : ValueObject
{
    public int Minutes { get; }
    public CourseDuration(int minutes)
    {
        Minutes = minutes;
    }
    public static CourseDuration FromMinutes(int minutes)
    {
        if(minutes < 0)
            throw new ArgumentException("Course duration cannot be negative.", nameof(minutes));
        
        return new CourseDuration(minutes);
    }

    public static CourseDuration FromHours(int hours) => FromMinutes(hours * 60);
    public int TotalHours => Minutes / 60;
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Minutes;
    }
}
