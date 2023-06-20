using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Repairables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Fog;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Helpers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.Click;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    public interface IPvPCruiserArgs
    {
        PvPFaction Faction { get; }
        IPvPCruiser EnemyCruiser { get; }
        IPvPUIManager UiManager { get; }
        IPvPDroneManager DroneManager { get; }
        IPvPDroneFocuser DroneFocuser { get; }
        IPvPDroneConsumerProvider DroneConsumerProvider { get; }
        IPvPFactoryProvider FactoryProvider { get; }
        IPvPCruiserSpecificFactories CruiserSpecificFactories { get; }
        PvPDirection FacingDirection { get; }
        IPvPRepairManager RepairManager { get; }
        PvPFogStrength FogStrength { get; }
        IPvPCruiserHelper Helper { get; }
        IPvPSlotFilter HighlightableFilter { get; }
        IPvPBuildProgressCalculator BuildProgressCalculator { get; }
        IPvPDoubleClickHandler<IPvPBuilding> BuildingDoubleClickHandler { get; }
        IPvPDoubleClickHandler<IPvPCruiser> CruiserDoubleClickHandler { get; }
        IPvPManagedDisposable FogOfWarManager { get; }
        IPvPBroadcastingProperty<bool> HasActiveDrones { get; }
    }
}
