using BattleCruisers.AI;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Common.BuildableDetails.Buttons;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;
using BattleCruisers.Utils.Timers;
using UnityCommon.PlatformAbstractions.Time;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.BattleScene
{
    public class NormalHelper : IBattleSceneHelper
    {
        private readonly IDataProvider _dataProvider;
        private readonly IPrefabFactory _prefabFactory;
        private readonly IDeferrer _deferrer;

        private UIManager _uiManager;

        public IBuildProgressCalculator PlayerCruiserBuildProgressCalculator { get; }
        public IBuildProgressCalculator AICruiserBuildProgressCalculator { get; }

        private readonly BuildingCategoryFilter _buildingCategoryFilter;
        public IBuildingCategoryPermitter BuildingCategoryPermitter { get; }

        public NormalHelper(IDataProvider dataProvider, IPrefabFactory prefabFactory, IDeferrer deferrer)
        {
            Helper.AssertIsNotNull(dataProvider, prefabFactory, deferrer);

            _dataProvider = dataProvider;
            _prefabFactory = prefabFactory;
            _deferrer = deferrer;

            IBuildProgressCalculatorFactory calculatorFactory = new BuildProgressCalculatorFactory(_dataProvider.SettingsManager);
            PlayerCruiserBuildProgressCalculator = calculatorFactory.CreatePlayerCruiserCalculator(); ;
            AICruiserBuildProgressCalculator = calculatorFactory.CreateAICruiserCalculator();
            
            // For the real game want to enable all building categories :)
            _buildingCategoryFilter = new BuildingCategoryFilter();
            _buildingCategoryFilter.AllowAllCategories();
        }

        public ILoadout GetPlayerLoadout()
        {
            return _dataProvider.GameModel.PlayerLoadout;
        }
		
        public IArtificialIntelligence CreateAI(ICruiserController aiCruiser, ICruiserController playerCruiser, int currentLevelNum)
		{
            ILevelInfo levelInfo = new LevelInfo(aiCruiser, playerCruiser, _dataProvider.StaticData, _prefabFactory, currentLevelNum);
            IAIManager aiManager = new AIManager(_prefabFactory, _dataProvider, _deferrer, playerCruiser);
            return aiManager.CreateAI(levelInfo);
		}
		
		public ISlotFilter CreateHighlightableSlotFilter()
		{
            return new FreeSlotFilter();
		}

        public IButtonVisibilityFilters CreateButtonVisibilityFilters(IDroneManager droneManager)
        {
            return
                new ButtonVisibilityFilters(
                    new AffordableBuildableFilter(droneManager),
                    _buildingCategoryFilter,
                    new ChooseTargetButtonVisibilityFilter(),
                    new DeleteButtonVisibilityFilter(),
                    new BroadcastingFilter(isMatch: true),
                    new StaticBroadcastingFilter(isMatch: true),
                    new BroadcastingFilter(isMatch: false));
        }

        public IManagedDisposable CreateDroneEventSoundPlayer(ICruiser playerCruiser, IDeferrer deferrer)
        {
            return
                new DroneEventSoundPlayer(
                    new DroneManagerMonitor(playerCruiser.DroneManager, deferrer),
                    playerCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer,
                    new Debouncer(TimeBC.Instance.RealTimeSinceGameStartProvider, debounceTimeInS: 20));
        }

        public IPrioritisedSoundPlayer GetBuildableButtonSoundPlayer(ICruiser playerCruiser)
        {
            return playerCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer;
        }

        public IUIManager CreateUIManager()
        {
            Assert.IsNull(_uiManager, "Should only call CreateUIManager() once");
            _uiManager = new UIManager();
            return _uiManager;
        }

        public void InitialiseUIManager(ManagerArgs args)
        {
            Assert.IsNotNull(_uiManager, "Should only call after CreateUIManager()");
            _uiManager.Initialise(args);
        }

        public IUserChosenTargetHelper CreateUserChosenTargetHelper(
            IUserChosenTargetManager playerCruiserUserChosenTargetManager,
            IPrioritisedSoundPlayer soundPlayer,
            ITargetIndicator targetIndicator)
        {
            Helper.AssertIsNotNull(playerCruiserUserChosenTargetManager, soundPlayer, targetIndicator);

            return new UserChosenTargetHelper(playerCruiserUserChosenTargetManager, soundPlayer, targetIndicator);
        }
    }
}
