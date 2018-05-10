using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Fog;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
using BattleCruisers.Buildables.BuildProgress;

namespace BattleCruisers.Cruisers
{
    public interface ICruiser : ICruiserController, ITarget, IComparableItem, IClickable
    {
        IBuildableWrapper<IBuilding> SelectedBuildingPrefab { get; set; }
        IDroneConsumerProvider DroneConsumerProvider { get; }
        Direction Direction { get; }
        float YAdjustmentInM { get; }
        IFogOfWar Fog { get; }
        IRepairManager RepairManager { get; }
        int NumOfDrones { get; }
        IBuildProgressCalculator BuildProgressCalculator { get; }

        IBuilding ConstructSelectedBuilding(ISlot slot);
    }
}
