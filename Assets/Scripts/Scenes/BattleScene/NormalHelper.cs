using BattleCruisers.AI;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Cruisers;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Data.Settings;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene.Buttons.Filters;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Common.BuildableDetails.Buttons;
using BattleCruisers.UI.Filters;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;
using UnityEngine.Assertions;

namespace BattleCruisers.Scenes.BattleScene
{
    public class NormalHelper : IBattleSceneHelper
    {
        private readonly IDataProvider _dataProvider;
        private readonly IPrefabFactory _prefabFactory;
        private readonly IVariableDelayDeferrer _variableDelayDeferrer;

        private UIManager _uiManager;

        public IBuildProgressCalculator PlayerCruiserBuildProgressCalculator { get; private set; }
        public IBuildProgressCalculator AICruiserBuildProgressCalculator { get; private set; }

        public NormalHelper(IDataProvider dataProvider, IPrefabFactory prefabFactory, IVariableDelayDeferrer variableDelayDeferrer)
        {
            Helper.AssertIsNotNull(dataProvider, prefabFactory, variableDelayDeferrer);

            _dataProvider = dataProvider;
            _prefabFactory = prefabFactory;
            _variableDelayDeferrer = variableDelayDeferrer;

            IBuildProgressCalculator normalCalculator = new LinearCalculator(FindBuildSpeedMultiplier(_dataProvider.SettingsManager));
            PlayerCruiserBuildProgressCalculator = normalCalculator;
            AICruiserBuildProgressCalculator = normalCalculator;
        }

        private float FindBuildSpeedMultiplier(ISettingsManager settingsManager)
        {
            switch (settingsManager.AIDifficulty)
            {
                case Difficulty.Easy:
                    return BuildSpeedMultipliers.POINT_3_DEFAULT;

                case Difficulty.Normal:
                    return BuildSpeedMultipliers.POINT_7_DEFAULT;

                case Difficulty.Insane:
                    return BuildSpeedMultipliers.ONE_AND_A_HALF_DEFAULT;

                default:
                    return BuildSpeedMultipliers.DEFAULT;
            }
        }

        public ILoadout GetPlayerLoadout()
        {
            return _dataProvider.GameModel.PlayerLoadout;
        }
		
        public IArtificialIntelligence CreateAI(ICruiserController aiCruiser, ICruiserController playerCruiser, int currentLevelNum)
		{
            ILevelInfo levelInfo = new LevelInfo(aiCruiser, playerCruiser, _dataProvider.StaticData, _prefabFactory, currentLevelNum);
            IAIManager aiManager = new AIManager(_prefabFactory, _dataProvider, _variableDelayDeferrer, playerCruiser);
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
                    // For the real game want to enable all building categories :)
                    new StaticBroadcastingFilter<BuildingCategory>(isMatch: true),
                    new ChooseTargetButtonVisibilityFilter(),
                    new DeleteButtonVisibilityFilter(),
                    new BroadcastingFilter(isMatch: true),
                    new StaticBroadcastingFilter(isMatch: true),
                    new BroadcastingFilter(isMatch: false));
        }

        public IManagedDisposable CreateDroneEventSoundPlayer(ICruiser playerCruiser, IVariableDelayDeferrer deferrer)
        {
            return
                new DroneEventSoundPlayer(
                    new DroneManagerMonitor(playerCruiser.DroneManager, deferrer),
                    playerCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer);
        }

        public IPrioritisedSoundPlayer GetBuildableButtonSoundPlayer(ICruiser playerCruiser)
        {
            return playerCruiser.FactoryProvider.Sound.PrioritisedSoundPlayer;
        }

        public IBroadcastingFilter CreateNavigationWheelEnabledFilter()
        {
            return new StaticBroadcastingFilter(isMatch: true);
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
            IPrioritisedSoundPlayer soundPlayer)
        {
            Helper.AssertIsNotNull(playerCruiserUserChosenTargetManager, soundPlayer);

            return new UserChosenTargetHelper(playerCruiserUserChosenTargetManager, soundPlayer);
        }
    }
}
