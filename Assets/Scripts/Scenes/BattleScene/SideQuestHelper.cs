using BattleCruisers.AI;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static;
using BattleCruisers.Data.Static.Strategies.Helper;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Clouds.Stats;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Common.BuildableDetails.Buttons;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using BattleCruisers.Utils.Threading;
using BattleCruisers.Utils.Timers;
using System.Threading.Tasks;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.BattleScene
{
    public class SideQuestHelper : BattleSceneHelper
    {
        private readonly IDeferrer _deferrer;

        private UIManager _uiManager;
        private const int IN_GAME_HINTS_CUTOFF = 3;

        public override bool ShowInGameHints { get; }

        private readonly BuildingCategoryFilter _buildingCategoryFilter;
        public override IBuildingCategoryPermitter BuildingCategoryPermitter => _buildingCategoryFilter;

        public SideQuestHelper(IDeferrer deferrer)
            : base()
        {
            Helper.AssertIsNotNull(deferrer);

            _deferrer = deferrer;

            ShowInGameHints =
                DataProvider.SettingsManager.ShowInGameHints
                && ApplicationModel.SelectedLevel <= IN_GAME_HINTS_CUTOFF;

            // For the real game want to enable all building categories :)
            _buildingCategoryFilter = new BuildingCategoryFilter();
            _buildingCategoryFilter.AllowAllCategories();
        }

        public override ILoadout GetPlayerLoadout()
        {
            return DataProvider.GameModel.PlayerLoadout;
        }

        public override async Task<PrefabContainer<BackgroundImageStats>> GetBackgroundStatsAsync(int levelNum)
        {
            IPrefabKey key = new SideQuestBackgroundImageStatsKey(ApplicationModel.SelectedSideQuestID);
            return await PrefabFetcher.GetPrefabAsync<BackgroundImageStats>(key);
        }

        public override IManagedDisposable CreateAI(Cruiser aiCruiser, Cruiser playerCruiser, int currentLevelNum)
        {
            LevelInfo levelInfo = new LevelInfo(aiCruiser, playerCruiser);
            IStrategyFactory strategyFactory = CreateStrategyFactory(currentLevelNum);
            AIManager aiManager = new AIManager(_deferrer, playerCruiser, strategyFactory);
            return aiManager.CreateAI(levelInfo);
        }

        public override IBuildProgressCalculator CreatePlayerCruiserBuildProgressCalculator()
        {
            return _calculatorFactory.CreatePlayerCruiserCalculator();
        }

        public override IBuildProgressCalculator CreateAICruiserBuildProgressCalculator()
        {
            return _calculatorFactory.CreateIncrementalAICruiserCalculator(FindDifficulty(), ApplicationModel.SelectedSideQuestID, true);
        }

        protected virtual Difficulty FindDifficulty()
        {
            return DataProvider.SettingsManager.AIDifficulty;
        }

        protected virtual IStrategyFactory CreateStrategyFactory(int currentLevelNum)
        {
            return new DefaultStrategyFactory(StaticData.Strategies, StaticData.SideQuestStrategies, currentLevelNum, ApplicationModel.Mode == GameMode.SideQuest);
        }

        public override IFilter<ISlot> CreateHighlightableSlotFilter()
        {
            return new FreeSlotFilter();
        }

        public override IButtonVisibilityFilters CreateButtonVisibilityFilters(IDroneManager droneManager)
        {
            return
                new ButtonVisibilityFilters(
                    new AffordableBuildableFilter(droneManager),
                    _buildingCategoryFilter,
                    new ChooseTargetButtonVisibilityFilter(),
                    new DeleteButtonVisibilityFilter(),
                    new BroadcastingFilter(isMatch: true),
                    new StaticBroadcastingFilter(isMatch: true));
        }

        public override IManagedDisposable CreateDroneEventSoundPlayer(ICruiser playerCruiser, IDeferrer deferrer)
        {
            return
                new DroneEventSoundPlayer(
                    new DroneManagerMonitor(playerCruiser.DroneManager, deferrer),
                    FactoryProvider.Sound.PrioritisedSoundPlayer,
                    new Debouncer(TimeBC.Instance.RealTimeSinceGameStartProvider, debounceTimeInS: 20));
        }

        public override IPrioritisedSoundPlayer GetBuildableButtonSoundPlayer(ICruiser playerCruiser)
        {
            return FactoryProvider.Sound.PrioritisedSoundPlayer;
        }

        public override IUIManager CreateUIManager()
        {
            Assert.IsNull(_uiManager, "Should only call CreateUIManager() once");
            _uiManager = new UIManager();
            return _uiManager;
        }

        public override void InitialiseUIManager(ManagerArgs args)
        {
            Assert.IsNotNull(_uiManager, "Should only call after CreateUIManager()");
            _uiManager.Initialise(args);
        }

        /*
        public override async Task<PrefabContainer<BackgroundImageStats>> GetBackgroundStatsAsync(int levelNum)
        {
            return await _backgroundStatsProvider.GetStatsAsync(_skirmish.BackgroundLevelNum);
        }
        */


        public override IUserChosenTargetHelper CreateUserChosenTargetHelper(
            IUserChosenTargetManager playerCruiserUserChosenTargetManager,
            IPrioritisedSoundPlayer soundPlayer,
            ITargetIndicator targetIndicator)
        {
            Helper.AssertIsNotNull(playerCruiserUserChosenTargetManager, soundPlayer, targetIndicator);

            return new UserChosenTargetHelper(playerCruiserUserChosenTargetManager, soundPlayer, targetIndicator);
        }
    }
}
