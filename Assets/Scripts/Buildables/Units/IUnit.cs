using BattleCruisers.Drones;

namespace BattleCruisers.Buildables.Units
{
    public interface IUnit : IBuildable
    {
		UnitCategory Category { get; }
        IDroneConsumerProvider DroneConsumerProvider { set; }
	}
}
