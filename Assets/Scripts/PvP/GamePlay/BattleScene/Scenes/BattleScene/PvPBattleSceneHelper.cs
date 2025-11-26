using BattleCruisers.Data;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Data.Models;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;
using System.Threading.Tasks;
using BattleCruisers.UI.BattleScene.Clouds.Stats;
using BattleCruisers.Data.Static;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.BuildMenus;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static;
using BattleCruisers.UI.ScreensScene.ProfileScreen;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene
{
    public abstract class PvPBattleSceneHelper
    {
        protected readonly PvPBuildProgressCalculatorFactory _calculatorFactory;

        public virtual IPrefabKey PlayerACruiser => SynchedServerData.Instance == null ? new PvPHullKey("PvP" + DataProvider.GameModel.PlayerLoadout.Hull.PrefabName) : PvPStaticPrefabKeys.PvPHulls.HullKeyFromType((HullType)SynchedServerData.Instance.playerACruiserID.Value);
        public virtual IPrefabKey PlayerBCruiser => SynchedServerData.Instance == null ? new PvPHullKey("PvP" + DataProvider.GameModel.PlayerLoadout.Hull.PrefabName) : PvPStaticPrefabKeys.PvPHulls.HullKeyFromType((HullType)SynchedServerData.Instance.playerBCruiserID.Value);
        public abstract BuildingCategoryFilter BuildingCategoryPermitter { get; }
        public abstract IFilter<PvPSlot> CreateHighlightableSlotFilter();
        public abstract IPvPBuildProgressCalculator CreatePlayerACruiserBuildProgressCalculator();
        public abstract IPvPBuildProgressCalculator CreatePlayerBCruiserBuildProgressCalculator();
        public abstract PvPUIManager CreateUIManager();
        public abstract void InitialiseUIManager(
            PvPCruiser playerCruiser,
            PvPCruiser enemyCruiser,
            PvPBuildMenu buildMenu,
            PvPItemDetailsManager detailsManager,
            IPrioritisedSoundPlayer soundPlayer,
            SingleSoundPlayer uiSoundPlayer);
        public abstract Loadout GetPlayerLoadout();
        public abstract PvPButtonVisibilityFilters CreateButtonVisibilityFilters(DroneManager droneManager);
        public abstract PvPButtonVisibilityFilters CreateButtonVisibilityFilters(PvPCruiser playerCruiser);
        public abstract IUserChosenTargetHelper CreateUserChosenTargetHelper(IUserChosenTargetManager playerCruiserUserChosenTargetManager /*, IPrioritisedSoundPlayer soundPlayer, ITargetIndicator targetIndicator*/);
        public abstract IUserChosenTargetHelper CreateUserChosenTargetHelper(IUserChosenTargetManager playerCruiserUserChosenTargetManager, IPrioritisedSoundPlayer soundPlayer, TargetIndicatorController targetIndicator);
        public abstract IPrioritisedSoundPlayer GetBuildableButtonSoundPlayer(IPvPCruiser playerCruiser);
        public abstract IManagedDisposable CreateDroneEventSoundPlayer(IPvPCruiser playerCruiser, IDeferrer deferrer);

        protected PvPBattleSceneHelper()
        {
            // DataProvider.GameModel.PlayerLoadout.Hull.PrefabPat
            // PlayerACruiser = new PvPHullKey("PvPYeti");
            // PlayerBCruiser = new PvPHullKey("PvPRaptor");
            _calculatorFactory
                 = new PvPBuildProgressCalculatorFactory(
                   new PvPBuildSpeedCalculator());
        }
        public virtual PvPLevel GetPvPLevel()
        {
            /*#if UNITY_EDITOR
            return StaticData.PvPLevels[(Map)DataProvider.GameModel.GameMap];
            #else
            return DataProvider.GetPvPLevel(SynchedServerData.Instance.map.Value);
            #endif*/
            return StaticData.PvPLevels[SynchedServerData.Instance.map.Value];
        }
        public virtual async Task<PrefabContainer<BackgroundImageStats>> GetBackgroundStatsAsync(int levelNum)
        {
            return await PrefabFetcher.GetPrefabAsync<BackgroundImageStats>(new LevelBackgroundImageStatsKey(levelNum));
        }
    }
}

