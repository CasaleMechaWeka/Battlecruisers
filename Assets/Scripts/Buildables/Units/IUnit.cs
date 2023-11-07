using BattleCruisers.Buildables.Pools;
using BattleCruisers.Buildables.Boost;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Utils.BattleScene;
using BattleCruisers.Utils.BattleScene.Pools;
using System.Collections.ObjectModel;
using BattleCruisers.UI.ScreensScene.ProfileScreen;

namespace BattleCruisers.Buildables.Units
{
    public enum UnitCategory
	{
		Naval, Aircraft//, Untouchable
	}

	public enum Direction
	{
		Left, Right, Up, Down
	}

    public interface IUnit : IBuildable, IRemovable, IPoolable<BuildableActivationArgs>
    {
		UnitCategory Category { get; }
        IDroneConsumerProvider DroneConsumerProvider { set; }
        Direction FacingDirection { get; }
        float MaxVelocityInMPerS { get; }
        bool IsUltra { get; }

        void AddBuildRateBoostProviders(ObservableCollection<IBoostProvider> boostProviders);
        void OverwriteComparableItem(string name, string description);
        int variantIndex { get; set; }
        void ApplyVariantStats(StatVariant statVariant);
    }
}
