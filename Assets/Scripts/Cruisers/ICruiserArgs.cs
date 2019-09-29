using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Fog;
using BattleCruisers.Cruisers.Helpers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Common.Click;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using UnityCommon.Properties;

namespace BattleCruisers.Cruisers
{
    public interface ICruiserArgs
    {
        Faction Faction { get; }
        ICruiser EnemyCruiser { get; }
        IUIManager UiManager { get; }
        IDroneManager DroneManager { get; }
        IDroneFocuser DroneFocuser { get; }
        IDroneConsumerProvider DroneConsumerProvider { get; }
        IFactoryProvider FactoryProvider { get; }
        ICruiserSpecificFactories CruiserSpecificFactories { get; }
        Direction FacingDirection { get; }
        IRepairManager RepairManager { get; }
        FogStrength FogStrength { get; }
        ICruiserHelper Helper { get; }
        ISlotFilter HighlightableFilter { get; }
        IBuildProgressCalculator BuildProgressCalculator { get; }
        IDoubleClickHandler<IBuilding> BuildingDoubleClickHandler { get; }
        IDoubleClickHandler<ICruiser> CruiserDoubleClickHandler { get; }
        IManagedDisposable FogOfWarManager { get; }
        IBroadcastingProperty<bool> HasActiveDrones { get; }
    }
}
