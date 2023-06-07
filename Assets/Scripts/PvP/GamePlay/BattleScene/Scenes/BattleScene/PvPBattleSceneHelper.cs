using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BattleCruisers.Data;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.UserChosen;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Clouds.Stats;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Data.Models;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene
{
    public abstract class PvPBattleSceneHelper : IPvPBattleSceneHelper
    {
        protected readonly IApplicationModel _appModel;
        protected readonly IPvPBuildProgressCalculatorFactory _calculatorFactory;

        protected readonly IPvPBackgroundStatsProvider _backgroundStatsProvider;
        private readonly IPvPPrefabFetcher _prefabFetcher;
        private readonly ILocTable _storyStrings;
        public virtual IPvPPrefabKey PlayerACruiser => SynchedServerData.Instance == null ? new PvPHullKey("PvP" + _appModel.DataProvider.GameModel.PlayerLoadout.Hull.PrefabName) : new PvPHullKey("PvP" + SynchedServerData.Instance.playerAPrefabName.Value);
        public virtual IPvPPrefabKey PlayerBCruiser => SynchedServerData.Instance == null ? new PvPHullKey("PvP" + _appModel.DataProvider.GameModel.PlayerLoadout.Hull.PrefabName) : new PvPHullKey("PvP" + SynchedServerData.Instance.playerAPrefabName.Value);
        public abstract IPvPBuildingCategoryPermitter BuildingCategoryPermitter { get; }
        public abstract IPvPBuildProgressCalculator CreateAICruiserBuildProgressCalculator();
        public abstract IPvPSlotFilter CreateHighlightableSlotFilter();
        public abstract IPvPBuildProgressCalculator CreatePlayerACruiserBuildProgressCalculator();
        public abstract IPvPBuildProgressCalculator CreatePlayerBCruiserBuildProgressCalculator();
        public abstract IPvPUIManager CreateUIManager();
        public abstract void InitialiseUIManager(PvPManagerArgs args);
        public abstract ILoadout GetPlayerLoadout();
        public abstract IPvPButtonVisibilityFilters CreateButtonVisibilityFilters(IPvPDroneManager droneManager);
        public abstract IPvPButtonVisibilityFilters CreateButtonVisibilityFilters();
        public abstract IPvPUserChosenTargetHelper CreateUserChosenTargetHelper(IPvPUserChosenTargetManager playerCruiserUserChosenTargetManager /*, IPvPPrioritisedSoundPlayer soundPlayer, IPvPTargetIndicator targetIndicator*/);
        public abstract IPvPUserChosenTargetHelper CreateUserChosenTargetHelper(IPvPUserChosenTargetManager playerCruiserUserChosenTargetManager, IPvPPrioritisedSoundPlayer soundPlayer, IPvPTargetIndicator targetIndicator);
        public abstract IPvPPrioritisedSoundPlayer GetBuildableButtonSoundPlayer(IPvPCruiser playerCruiser);

        protected PvPBattleSceneHelper(
            IApplicationModel appModel,
            IPvPPrefabFetcher prefabFetcher,
            ILocTable storyString
            )
        {
            _appModel = appModel;
            _prefabFetcher = prefabFetcher;
            _storyStrings = storyString;
            _backgroundStatsProvider = new PvPBackgroundStatsProvider(_prefabFetcher);
            // _appModel.DataProvider.GameModel.PlayerLoadout.Hull.PrefabPat
            // PlayerACruiser = new PvPHullKey("PvPYeti");
            // PlayerBCruiser = new PvPHullKey("PvPRaptor");
            _calculatorFactory
                 = new PvPBuildProgressCalculatorFactory(
                   new PvPBuildSpeedCalculator());
        }

        public virtual IPvPLevel GetPvPLevel()
        {
            return _appModel.DataProvider.GetPvPLevel(Map.PracticeWreckyards);
            // return _appModel.DataProvider.GetPvPLevel();
        }

        public virtual async Task<IPvPPrefabContainer<PvPBackgroundImageStats>> GetBackgroundStatsAsync(int levelNum)
        {
            return await _backgroundStatsProvider.GetStatsAsync(levelNum);
        }
    }
}

