using BattleCruisers.AI;
using BattleCruisers.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Utils.Localisation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.UserChosen;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.BuildableDetails.Buttons;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Filters;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Models;
using UnityEngine.Assertions;
using BattleCruisers.Utils.Timers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.PlatformAbstractions.Time;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.AI;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Static.Strategies.Helper;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Threading;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.Cruisers.Drones;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene
{
    public class PvPBattleHelper : PvPBattleSceneHelper
    {

        private readonly IPvPPrefabFactory _prefabFactory;
        private readonly IDeferrer _deferrer;

        private PvPUIManager _uiManager;

        protected IDataProvider DataProvider => _appModel.DataProvider;


        private readonly PvPBuildingCategoryFilter _buildingCategoryFilter;
        public override IBuildingCategoryPermitter BuildingCategoryPermitter => _buildingCategoryFilter;

        public PvPBattleHelper(
            IApplicationModel appModel,
            IPvPPrefabFetcher prefabFetcher,
            ILocTable storyStrings,
            IPvPPrefabFactory prefabFactory,
            IDeferrer deferrer
        ) : base(appModel, prefabFetcher, storyStrings)
        {
            // _appModel = appModel;
            _prefabFactory = prefabFactory;
            _deferrer = deferrer;


            // For the real game want to enable all building categories :)
            _buildingCategoryFilter = new PvPBuildingCategoryFilter();
            _buildingCategoryFilter.AllowAllCategories();
        }

        public override IPvPUIManager CreateUIManager()
        {
            Assert.IsNull(_uiManager, "Should only call CreateUIManager() once");
            _uiManager = new PvPUIManager();
            return _uiManager;
        }

        public override void InitialiseUIManager(PvPManagerArgs args)
        {
            Assert.IsNotNull(_uiManager, "Should only call after CreateUIManager()");
            _uiManager.Initialise(args);
        }

        public override IPvPSlotFilter CreateHighlightableSlotFilter()
        {
            return new PvPFreeSlotFilter();
        }


        public override ILoadout GetPlayerLoadout()
        {
            return DataProvider.GameModel.PlayerLoadout;
        }

        public override IPvPBuildProgressCalculator CreateAICruiserBuildProgressCalculator()
        {
            return _calculatorFactory.CreateIncrementalAICruiserCalculator(FindDifficulty(), _appModel.SelectedLevel);
        }

        public override IPvPBuildProgressCalculator CreatePlayerACruiserBuildProgressCalculator()
        {
            return _calculatorFactory.CreatePlayerACruiserCalculator();
        }

        public override IPvPBuildProgressCalculator CreatePlayerBCruiserBuildProgressCalculator()
        {
            return _calculatorFactory.CreatePlayerBCruiserCalculator();
        }

        protected virtual Difficulty FindDifficulty()
        {
            return Difficulty.Harder;
        }

        public override IPvPButtonVisibilityFilters CreateButtonVisibilityFilters(IPvPDroneManager droneManager)
        {
            return
                new PvPButtonVisibilityFilters(
                    new PvPAffordableBuildableFilter(droneManager),
                    _buildingCategoryFilter,
                    new PvPChooseTargetButtonVisibilityFilter(),
                    new PvPDeleteButtonVisibilityFilter(),
                    new PvPBroadcastingFilter(isMatch: true),
                    new PvPStaticBroadcastingFilter(isMatch: true));
        }

        public override IPvPButtonVisibilityFilters CreateButtonVisibilityFilters(PvPCruiser playerCruiser)
        {
            return
                new PvPButtonVisibilityFilters(
                    new PvPAffordableBuildableFilter(playerCruiser),
                    _buildingCategoryFilter,
                    new PvPChooseTargetButtonVisibilityFilter(),
                    new PvPDeleteButtonVisibilityFilter(),
                    new PvPBroadcastingFilter(isMatch: true),
                    new PvPStaticBroadcastingFilter(isMatch: true));
        }

        public override IPrioritisedSoundPlayer GetBuildableButtonSoundPlayer(IPvPCruiser playerCruiser)
        {
            return playerCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer;
        }

        public override IPvPUserChosenTargetHelper CreateUserChosenTargetHelper(
            IPvPUserChosenTargetManager playerCruiserUserChosenTargetManager
            )
        {
            PvPHelper.AssertIsNotNull(playerCruiserUserChosenTargetManager);
            return new PvPUserChosenTargetHelper(playerCruiserUserChosenTargetManager);
        }

        public override IArtificialIntelligence CreateAI(PvPCruiser aiCruiser, PvPCruiser playerCruiser, int currentLevelNum)
        {
            IPvPLevelInfo levelInfo = new PvPLevelInfo(aiCruiser, playerCruiser, PvPBattleSceneGodServer.Instance._battleSceneGodTunnel, _prefabFactory);
            IPvPStrategyFactory strategyFactory = CreateStrategyFactory(currentLevelNum);
            IPvPAIManager aiManager = new PvPAIManager(_prefabFactory, DataProvider, PvPBattleSceneGodServer.Instance._battleSceneGodTunnel, _deferrer, playerCruiser, strategyFactory);
            return aiManager.CreateAI(levelInfo, FindDifficulty() /* should be modified in production*/);
        }

        public override IPvPUserChosenTargetHelper CreateUserChosenTargetHelper(
            IPvPUserChosenTargetManager playerCruiserUserChosenTargetManager,
            IPrioritisedSoundPlayer soundPlayer,
            IPvPTargetIndicator targetIndicator)
        {
            PvPHelper.AssertIsNotNull(playerCruiserUserChosenTargetManager);

            return new PvPUserChosenTargetHelper(playerCruiserUserChosenTargetManager, soundPlayer, targetIndicator);
        }


        public override IManagedDisposable CreateDroneEventSoundPlayer(IPvPCruiser playerCruiser, IDeferrer deferrer)
        {
            return
                new DroneEventSoundPlayer(
                    new PvPDroneManagerMonitor(playerCruiser.DroneManager, deferrer),
                    playerCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer,
                    new Debouncer(PvPTimeBC.Instance.RealTimeSinceGameStartProvider, debounceTimeInS: 20));
        }

        protected virtual IPvPStrategyFactory CreateStrategyFactory(int currentLevelNum)
        {
            return new PvPDefaultStrategyFactory(PvPBattleSceneGodServer.Instance.dataProvider.StaticData.PvPStrategies, currentLevelNum);
        }

    }
}

