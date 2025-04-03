using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Data.Settings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Effects.Explosions;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.BuildableOutline;
using BattleCruisers.UI;
using BattleCruisers.UI.ScreensScene.LoadoutScreen.Comparisons;
using BattleCruisers.Cruisers.Drones;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    public interface IPvPCruiser : IPvPCruiserController, ITarget, IComparableItem, IClickableEmitter
    {

        IPvPBuildableWrapper<IPvPBuilding> SelectedBuildingPrefab { get; set; }
        PvPBuildableOutlineController SelectedBuildableOutlinePrefab { get; set; }
        IDroneConsumerProvider DroneConsumerProvider { get; }
        Direction Direction { get; }
        float YAdjustmentInM { get; }
        Vector2 TrashTalkScreenPosition { get; }
        IGameObject Fog { get; }
        IRepairManager RepairManager { get; }
        int NumOfDrones { get; }
        IPvPBuildProgressCalculator BuildProgressCalculator { get; }
        PvPFactoryProvider FactoryProvider { get; }
        IPvPCruiserSpecificFactories CruiserSpecificFactories { get; }
        bool IsPlayerCruiser { get; }
        PvPCruiserDeathExplosion DeathPrefab { get; }

        IPvPBuilding ConstructSelectedBuilding(IPvPSlot slot);
        void MakeInvincible();
        void AdjustStatsByDifficulty(Difficulty AIDifficulty);
        bool IsPvPCruiser();
        bool IsAIBot();
        PvPSlotWrapperController GetSlotWrapperController();
    }
}

