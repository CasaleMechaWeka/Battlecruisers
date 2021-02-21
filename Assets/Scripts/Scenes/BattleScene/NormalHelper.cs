using BattleCruisers.AI;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Settings;
using BattleCruisers.Data.Static.Strategies.Helper;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Common.BuildableDetails.Buttons;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions.Time;
using BattleCruisers.Utils.Threading;
using BattleCruisers.Utils.Timers;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.BattleScene
{
    public class NormalHelper : BattleSceneHelper
    {
        private readonly IPrefabFactory _prefabFactory;
        private readonly IDeferrer _deferrer;
        private readonly IBuildProgressCalculatorFactory _calculatorFactory;

        private UIManager _uiManager;
        private const int IN_GAME_HINTS_CUTOFF = 3;

        public override bool ShowInGameHints { get; }

        private readonly BuildingCategoryFilter _buildingCategoryFilter;
        public override IBuildingCategoryPermitter BuildingCategoryPermitter => _buildingCategoryFilter;

        public NormalHelper(
            IApplicationModel appModel,
            IPrefabFetcher prefabFetcher,
            IPrefabFactory prefabFactory, 
            IDeferrer deferrer)
            : base(appModel, prefabFetcher)
        {
            Helper.AssertIsNotNull(prefabFactory, deferrer);

            _prefabFactory = prefabFactory;
            _deferrer = deferrer;

            ShowInGameHints =
                DataProvider.SettingsManager.ShowInGameHints
                && _appModel.SelectedLevel <= IN_GAME_HINTS_CUTOFF;

            _calculatorFactory = new BuildProgressCalculatorFactory();
            
            // For the real game want to enable all building categories :)
            _buildingCategoryFilter = new BuildingCategoryFilter();
            _buildingCategoryFilter.AllowAllCategories();
        }

        public override ILoadout GetPlayerLoadout()
        {
            return DataProvider.GameModel.PlayerLoadout;
        }
		
        public override IArtificialIntelligence CreateAI(ICruiserController aiCruiser, ICruiserController playerCruiser, int currentLevelNum)
		{
            ILevelInfo levelInfo = new LevelInfo(aiCruiser, playerCruiser, DataProvider.GameModel, _prefabFactory);
            IStrategyFactory strategyFactory = CreateStrategyFactory(currentLevelNum);
            IAIManager aiManager = new AIManager(_prefabFactory, DataProvider, _deferrer, playerCruiser, strategyFactory);
            return aiManager.CreateAI(levelInfo, FindDifficulty());
		}

        public override IBuildProgressCalculator CreatePlayerCruiserBuildProgressCalculator()
        {
            return _calculatorFactory.CreatePlayerCruiserCalculator();
        }

        public override IBuildProgressCalculator CreateAICruiserBuildProgressCalculator()
        {
            return _calculatorFactory.CreateAICruiserCalculator(FindDifficulty());
        }

        protected virtual Difficulty FindDifficulty()
        {
            return DataProvider.SettingsManager.AIDifficulty;
        }

        protected virtual IStrategyFactory CreateStrategyFactory(int currentLevelNum)
        {
            return new DefaultStrategyFactory(DataProvider.StaticData.Strategies, currentLevelNum);
        }
		
		public override ISlotFilter CreateHighlightableSlotFilter()
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
                    new StaticBroadcastingFilter(isMatch: true),
                    new BroadcastingFilter(_appModel.DataProvider.GameModel.ShowHelpLabels));
        }

        public override IManagedDisposable CreateDroneEventSoundPlayer(ICruiser playerCruiser, IDeferrer deferrer)
        {
            return
                new DroneEventSoundPlayer(
                    new DroneManagerMonitor(playerCruiser.DroneManager, deferrer),
                    playerCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer,
                    new Debouncer(TimeBC.Instance.RealTimeSinceGameStartProvider, debounceTimeInS: 20));
        }

        public override IPrioritisedSoundPlayer GetBuildableButtonSoundPlayer(ICruiser playerCruiser)
        {
            return playerCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer;
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
