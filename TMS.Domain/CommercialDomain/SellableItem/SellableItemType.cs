using TMS.Domain.Shared;

namespace TMS.Domain.Commercial.SellableItem;

public class SellableItemType : Enumeration<SellableItemType>
{
    public static readonly SellableItemType Course = new CourseType();
    public static readonly SellableItemType LearningPath = new LearningPathType();

    public SellableItemType(int id, string name) : base(id, name) { }

    private sealed class CourseType() : SellableItemType(1, "Course") { }
    private sealed class LearningPathType() : SellableItemType(2, "LearningPath") { }
}
