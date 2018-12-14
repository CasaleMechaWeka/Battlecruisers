using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Fog;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Rows;
using BattleCruisers.Utils.Factories;

namespace BattleCruisers.Cruisers
{
    public interface ICruiser : ICruiserController, ITarget, IComparableItem, IClickableEmitter
    {
        IBuildableWrapper<IBuilding> SelectedBuildingPrefab { get; set; }
        IDroneConsumerProvider DroneConsumerProvider { get; }
        Direction Direction { get; }
        float YAdjustmentInM { get; }
        IFogOfWar Fog { get; }
        IRepairManager RepairManager { get; }
        int NumOfDrones { get; }
        IBuildProgressCalculator BuildProgressCalculator { get; }
        IFactoryProvider FactoryProvider { get; }
        bool IsPlayerCruiser { get; }

        IBuilding ConstructSelectedBuilding(ISlot slot);
    }
}
