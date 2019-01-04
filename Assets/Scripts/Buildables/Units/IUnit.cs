using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.Factories;

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

    public interface IUnit : IBuildable, IDestructable
    {
		UnitCategory Category { get; }
        IDroneConsumerProvider DroneConsumerProvider { set; }
        Direction FacingDirection { get; }
        float MaxVelocityInMPerS { get; }
        bool IsUltra { get; }

		void Initialise(ICruiser parentCruiser, ICruiser enemyCruiser, IUIManager uiManager, IFactoryProvider factoryProvider);
	}
}
