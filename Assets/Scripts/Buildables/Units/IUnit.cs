using BattleCruisers.Drones;

namespace BattleCruisers.Buildables.Units
{
	public enum UnitCategory
	{
		Naval, Aircraft, Untouchable
	}

	public enum Direction
	{
		Left, Right, Up, Down
	}

    public interface IUnit : IBuildable
    {
		UnitCategory Category { get; }
        IDroneConsumerProvider DroneConsumerProvider { set; }
	}
}
