using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Fog;
using BattleCruisers.Cruisers.Helpers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Data;
using BattleCruisers.Data.Models;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Common.Click;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using UnityCommon.Properties;
using UnityEngine;

namespace BattleCruisers.Cruisers
{
    public class CruiserFactory : ICruiserFactory
	{
        private readonly IFactoryProvider _factoryProvider;
        private readonly IBattleSceneHelper _helper;
        private readonly IApplicationModel _applicationModel;
        private readonly ISlotFilter _highlightableSlotFilter;
        private readonly IUIManager _uiManager;
        private readonly IFogVisibilityDecider _fogVisibilityDecider;

        private const int CRUISER_OFFSET_IN_M = 35;

        public CruiserFactory(
            IFactoryProvider factoryProvider,
            IBattleSceneHelper helper,
            IApplicationModel applicationModel,
            IUIManager uiManager)
        {
            Helper.AssertIsNotNull(factoryProvider, helper, applicationModel, uiManager);

            _factoryProvider = factoryProvider;
            _helper = helper;
            _applicationModel = applicationModel;
            _highlightableSlotFilter = helper.CreateHighlightableSlotFilter();
            _uiManager = uiManager;
            _fogVisibilityDecider = new FogVisibilityDecider();
        }

        public Cruiser CreatePlayerCruiser()
        {
            ILoadout playerLoadout = _helper.GetPlayerLoadout();
            Cruiser playerCruiserPrefab = _factoryProvider.PrefabFactory.GetCruiserPrefab(playerLoadout.Hull);
            Cruiser playerCruiser = _factoryProvider.PrefabFactory.CreateCruiser(playerCruiserPrefab);
            playerCruiser.Position = new Vector3(-CRUISER_OFFSET_IN_M, playerCruiser.YAdjustmentInM, 0);

            return playerCruiser;
        }

        public Cruiser CreateAICruiser()
        {
            ILevel currentLevel = _applicationModel.DataProvider.GetLevel(_applicationModel.SelectedLevel);
            Cruiser aiCruiserPrefab = _factoryProvider.PrefabFactory.GetCruiserPrefab(currentLevel.Hull);
            Cruiser aiCruiser = _factoryProvider.PrefabFactory.CreateCruiser(aiCruiserPrefab);

            aiCruiser.Position = new Vector3(CRUISER_OFFSET_IN_M, aiCruiser.YAdjustmentInM, 0);
            Quaternion rotation = aiCruiser.Rotation;
            rotation.eulerAngles = new Vector3(0, 180, 0);
            aiCruiser.Rotation = rotation;

            return aiCruiser;
        }

        public void InitialisePlayerCruiser(
            Cruiser playerCruiser, 
            Cruiser aiCruiser, 
            ICameraFocuser cameraFocuser,
            IRankedTargetTracker userChosenTargetTracker)
        {
            Helper.AssertIsNotNull(playerCruiser, aiCruiser, cameraFocuser, userChosenTargetTracker);

            ICruiserHelper helper = CreatePlayerHelper(_uiManager, cameraFocuser);
            Faction faction = Faction.Blues;
            Direction facingDirection = Direction.Right;
            FogStrength fogStrength = FogStrength.Weak;
            IDoubleClickHandler<IBuilding> buildingDoubleClickHandler = new PlayerBuildingDoubleClickHandler();
            IDoubleClickHandler<ICruiser> cruiserDoubleClickHandler = new PlayerCruiserDoubleClickHandler();

            InitialiseCruiser(
                playerCruiser,
                aiCruiser,
                _uiManager,
                helper,
                faction,
                facingDirection,
                fogStrength,
                _highlightableSlotFilter,
                _helper.PlayerCruiserBuildProgressCalculator,
                userChosenTargetTracker,
                buildingDoubleClickHandler,
                cruiserDoubleClickHandler,
                _factoryProvider.DroneMonitor.PlayerCruiserHasActiveDrones,
                isPlayerCruiser: true);
        }

        public void InitialiseAICruiser(
            Cruiser playerCruiser, 
            Cruiser aiCruiser, 
            ICameraFocuser cameraFocuser,
            IRankedTargetTracker userChosenTargetTracker,
            IUserChosenTargetHelper userChosenTargetHelper)
        {
            Helper.AssertIsNotNull(playerCruiser, aiCruiser, userChosenTargetTracker, userChosenTargetHelper);

            ICruiserHelper helper = CreateAIHelper(_uiManager, cameraFocuser);
            Faction faction = Faction.Reds;
            Direction facingDirection = Direction.Left;
            FogStrength fogStrength = FogStrength.Strong;

            IDoubleClickHandler<IBuilding> buildingDoubleClickHandler = new AIBuildingDoubleClickHandler(userChosenTargetHelper);
            IDoubleClickHandler<ICruiser> cruiserDoubleClickHandler = new AICruiserDoubleClickHandler(userChosenTargetHelper);

            InitialiseCruiser(
                aiCruiser,
                playerCruiser,
                _uiManager,
                helper,
                faction,
                facingDirection,
                fogStrength,
                _highlightableSlotFilter,
                _helper.AICruiserBuildProgressCalculator,
                userChosenTargetTracker,
                buildingDoubleClickHandler,
                cruiserDoubleClickHandler,
                _factoryProvider.DroneMonitor.AICruiserHasActiveDrones,
                isPlayerCruiser: false);
        }

        private void InitialiseCruiser(
            Cruiser cruiser, 
            ICruiser enemyCruiser,
            IUIManager uiManager,
            ICruiserHelper helper,
            Faction faction, 
            Direction facingDirection,
            FogStrength fogStrength,
            ISlotFilter highlightableFilter,
            IBuildProgressCalculator buildProgressCalculator,
            IRankedTargetTracker userChosenTargetTracker,
            IDoubleClickHandler<IBuilding> buildingDoubleClickHandler,
            IDoubleClickHandler<ICruiser> cruiserDoubleClickHandler,
            IBroadcastingProperty<bool> parentCruiserHasActiveDrones,
            bool isPlayerCruiser)
        {
            ICruiserSpecificFactories cruiserSpecificFactories
                = new CruiserSpecificFactories(
                    _factoryProvider,
                    cruiser,
                    enemyCruiser,
                    userChosenTargetTracker,
                    _factoryProvider.UpdaterProvider,
                    faction,
                    _applicationModel.IsTutorial);

            IDroneManager droneManager = new DroneManager();
            IDroneFocuser droneFocuser = CreateDroneFocuser(isPlayerCruiser, droneManager, _factoryProvider.Sound.PrioritisedSoundPlayer);
            IDroneConsumerProvider droneConsumerProvider = new DroneConsumerProvider(droneManager);
            RepairManager repairManager = new RepairManager(cruiserSpecificFactories.DroneFeedbackFactory, droneConsumerProvider, cruiser);
            FogOfWarManager fogOfWarManager = new FogOfWarManager(cruiser.Fog, _fogVisibilityDecider, cruiser.BuildingMonitor, enemyCruiser.BuildingMonitor);

            ICruiserArgs cruiserArgs
                = new CruiserArgs(
                    faction,
                    enemyCruiser,
                    uiManager,
                    droneManager,
                    droneFocuser,
                    droneConsumerProvider,
                    _factoryProvider,
                    cruiserSpecificFactories,
                    facingDirection,
                    repairManager,
                    fogStrength,
                    helper,
                    highlightableFilter,
                    buildProgressCalculator,
                    buildingDoubleClickHandler,
                    cruiserDoubleClickHandler,
                    fogOfWarManager,
                    parentCruiserHasActiveDrones);

            cruiser.Initialise(cruiserArgs);
        }

        private IDroneFocuser CreateDroneFocuser(bool isPlayerCruiser, IDroneManager droneManager, IPrioritisedSoundPlayer soundPlayer)
        {
            if (isPlayerCruiser)
            {
                return new PlayerCruiserDroneFocuser(droneManager, new DroneFocusSoundPicker(), soundPlayer);
            }
            else
            {
                return new AICruiserDroneFocuser(droneManager);
            }
        }

        private ICruiserHelper CreateAIHelper(IUIManager uiManager, ICameraFocuser cameraFocuser)
        {
            return new AICruiserHelper(uiManager, cameraFocuser);
        }

        private ICruiserHelper CreatePlayerHelper(IUIManager uiManager, ICameraFocuser cameraFocuser)
        {
            if (_applicationModel.IsTutorial)
            {
                return new TutorialPlayerCruiserHelper(uiManager, cameraFocuser);
            }
            else
            {
                return new PlayerCruiserHelper(uiManager, cameraFocuser);
            }
        }
    }
}