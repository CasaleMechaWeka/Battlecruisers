using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.Utils;

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
        Direction FacingDirection { get; }
        float MaxVelocityInMPerS { get; }

		void Initialise(ICruiser parentCruiser, ICruiser enemyCruiser, IUIManager uiManager, IFactoryProvider factoryProvider);
	}
}
