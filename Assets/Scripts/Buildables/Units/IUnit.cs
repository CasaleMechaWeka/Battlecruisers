using BattleCruisers.Buildables.Pools;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.BattleScene.Pools;
using System.Collections.ObjectModel;

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

    public interface IUnit : IBuildable, IRemovable, IPoolable<UnitActivationArgs>
    {
		UnitCategory Category { get; }
        IDroneConsumerProvider DroneConsumerProvider { set; }
        Direction FacingDirection { get; }
        float MaxVelocityInMPerS { get; }
        bool IsUltra { get; }

        void AddBuildRateBoostProviders(ObservableCollection<IBoostProvider> boostProviders);
	}
}
