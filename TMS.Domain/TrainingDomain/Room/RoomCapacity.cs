using TMS.Domain.Shared;

namespace TMS.Domain.Training.Room;

public class RoomCapacity : ValueObject
{
    public int Capacity { get; }
    protected RoomCapacity(int capacity)
    {
        this.Capacity = capacity;
    }
    public static RoomCapacity FromCapacity(int value)
    {
        if (value <= 0)
            throw new ArgumentOutOfRangeException(nameof(value), "Room capacity cannot be negative or zero.");

        return new RoomCapacity(value);
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Capacity;
    }
}
