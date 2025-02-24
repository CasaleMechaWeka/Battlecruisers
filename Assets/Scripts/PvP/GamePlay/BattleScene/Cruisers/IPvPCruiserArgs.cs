using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Fog;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Helpers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.Click;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    public interface IPvPCruiserArgs
    {
        Faction Faction { get; }
        IPvPCruiser EnemyCruiser { get; }
        IPvPUIManager UiManager { get; }
        IDroneManager DroneManager { get; }
        IPvPDroneFocuser DroneFocuser { get; }
        IPvPDroneConsumerProvider DroneConsumerProvider { get; }
        IPvPFactoryProvider FactoryProvider { get; }
        IPvPCruiserSpecificFactories CruiserSpecificFactories { get; }
        PvPDirection FacingDirection { get; }
        IRepairManager RepairManager { get; }
        PvPFogStrength FogStrength { get; }
        IPvPCruiserHelper Helper { get; }
        IPvPSlotFilter HighlightableFilter { get; }
        IPvPBuildProgressCalculator BuildProgressCalculator { get; }
        IPvPDoubleClickHandler<IPvPBuilding> BuildingDoubleClickHandler { get; }
        IPvPDoubleClickHandler<IPvPCruiser> CruiserDoubleClickHandler { get; }
        IManagedDisposable FogOfWarManager { get; }
        IBroadcastingProperty<bool> HasActiveDrones { get; }
    }
}
