using BattleCruisers.Buildables;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Helpers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Common.Click;
using BattleCruisers.Utils.Factories;

namespace BattleCruisers.Cruisers
{
    public interface ICruiserArgs
    {
        Faction Faction { get; }
        ICruiser EnemyCruiser { get; }
        IUIManager UiManager { get; }
        IDroneManager DroneManager { get; }
        IDroneConsumerProvider DroneConsumerProvider { get; }
        IFactoryProvider FactoryProvider { get; }
        Direction FacingDirection { get; }
        RepairManager RepairManager { get; }
        bool ShouldShowFog { get; }
        ICruiserHelper Helper { get; }
        ISlotFilter HighlightableFilter { get; }
        IBuildProgressCalculator BuildProgressCalculator { get; }
        // FELIX  Also add (after creating :P) cruiser double click handler
        IBuildingDoubleClickHandler BuildingDoubleClickHandler { get; }
    }
}
