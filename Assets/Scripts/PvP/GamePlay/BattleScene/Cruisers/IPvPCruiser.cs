using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Repairables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.LoadoutScreen.Comparisons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions;
using UnityEngine;
using System.Threading.Tasks;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.BuildableOutline;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    public interface IPvPCruiser : IPvPCruiserController, IPvPTarget, IPvPComparableItem, IPvPClickableEmitter
    {

        IPvPBuildableWrapper<IPvPBuilding> SelectedBuildingPrefab { get; set; }
        PvPBuildableOutlineController SelectedBuildableOutlinePrefab { get; set; }
        IPvPDroneConsumerProvider DroneConsumerProvider { get; }
        PvPDirection Direction { get; }
        float YAdjustmentInM { get; }
        Vector2 TrashTalkScreenPosition { get; }
        IPvPGameObject Fog { get; }
        IPvPRepairManager RepairManager { get; }
        int NumOfDrones { get; }
        IPvPBuildProgressCalculator BuildProgressCalculator { get; }
        IPvPFactoryProvider FactoryProvider { get; }
        IPvPCruiserSpecificFactories CruiserSpecificFactories { get; }
        bool IsPlayerCruiser { get; }
        PvPCruiserDeathExplosion DeathPrefab { get; }

        Task<IPvPBuilding> ConstructSelectedBuilding(IPvPSlot slot);
        void MakeInvincible();
        void AdjustStatsByDifficulty(Difficulty AIDifficulty);
        bool IsPvPCruiser();
        PvPSlotWrapperController GetSlotWrapperController();
    }
}

