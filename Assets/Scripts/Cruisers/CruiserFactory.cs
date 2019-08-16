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
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions;
using UnityEngine;

namespace BattleCruisers.Cruisers
{
    public class CruiserFactory : ICruiserFactory
	{
        private readonly IPrefabFactory _prefabFactory;
        private readonly IBattleSceneGodComponents _components;
        private readonly ISpriteProvider _spriteProvider;
        private readonly ICamera _soleCamera;
        private readonly IBattleSceneHelper _helper;
        private readonly IApplicationModel _applicationModel;
        private readonly ISlotFilter _highlightableSlotFilter;
        private readonly IUIManager _uiManager;
        private readonly IFogVisibilityDecider _fogVisibilityDecider;

        private const int CRUISER_OFFSET_IN_M = 35;

        public CruiserFactory(
            IPrefabFactory prefabFactory,
            IBattleSceneGodComponents components,
            ISpriteProvider spriteProvider,
            ICamera soleCamera,
            IBattleSceneHelper helper,
            IApplicationModel applicationModel,
            IUIManager uiManager)
        {
            Helper.AssertIsNotNull(prefabFactory, components, spriteProvider, soleCamera, helper, applicationModel, uiManager);
            
            _prefabFactory = prefabFactory;
            _components = components;
            _spriteProvider = spriteProvider;
            _soleCamera = soleCamera;
            _helper = helper;
            _applicationModel = applicationModel;
            _highlightableSlotFilter = helper.CreateHighlightableSlotFilter();
            _uiManager = uiManager;
            _fogVisibilityDecider = new FogVisibilityDecider();
        }

        public Cruiser CreatePlayerCruiser()
        {
            ILoadout playerLoadout = _helper.GetPlayerLoadout();
            Cruiser playerCruiserPrefab = _prefabFactory.GetCruiserPrefab(playerLoadout.Hull);
            Cruiser playerCruiser = _prefabFactory.CreateCruiser(playerCruiserPrefab);
            playerCruiser.Position = new Vector3(-CRUISER_OFFSET_IN_M, playerCruiser.YAdjustmentInM, 0);

            return playerCruiser;
        }

        public Cruiser CreateAICruiser()
        {
            ILevel currentLevel = _applicationModel.DataProvider.GetLevel(_applicationModel.SelectedLevel);
            Cruiser aiCruiserPrefab = _prefabFactory.GetCruiserPrefab(currentLevel.Hull);
            Cruiser aiCruiser = _prefabFactory.CreateCruiser(aiCruiserPrefab);

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
            IDroneNumFeedbackFactory feedbackFactory = new DroneNumFeedbackFactory();
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
                feedbackFactory,
                buildingDoubleClickHandler,
                cruiserDoubleClickHandler,
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

            IDroneNumFeedbackFactory feedbackFactory = new DummyDroneNumFeedbackFactory();
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
                feedbackFactory,
                buildingDoubleClickHandler,
                cruiserDoubleClickHandler,
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
            IDroneNumFeedbackFactory feedbackFactory,
            IDoubleClickHandler<IBuilding> buildingDoubleClickHandler,
            IDoubleClickHandler<ICruiser> cruiserDoubleClickHandler,
            bool isPlayerCruiser)
        {
            IFactoryProvider factoryProvider 
                = new FactoryProvider(
                    _prefabFactory, 
                    cruiser, 
                    enemyCruiser, 
                    _spriteProvider, 
                    _components.Deferrer,
                    userChosenTargetTracker, 
                    _soleCamera, 
                    isPlayerCruiser, 
                    _components.AudioSource,
                    _components.UpdaterProvider);

            IDroneManager droneManager = new DroneManager();
            IDroneFocuser droneFocuser = CreateDroneFocuser(isPlayerCruiser, droneManager, factoryProvider.Sound.PrioritisedSoundPlayer);
            IDroneConsumerProvider droneConsumerProvider = new DroneConsumerProvider(droneManager);
            RepairManager repairManager = new RepairManager(feedbackFactory, droneConsumerProvider, cruiser);
            FogOfWarManager fogOfWarManager = new FogOfWarManager(cruiser.Fog, _fogVisibilityDecider, cruiser.BuildingMonitor, enemyCruiser.BuildingMonitor);

            ICruiserArgs cruiserArgs
                = new CruiserArgs(
                    faction,
                    enemyCruiser,
                    uiManager,
                    droneManager,
                    droneFocuser,
                    droneConsumerProvider,
                    factoryProvider,
                    facingDirection,
                    repairManager,
                    fogStrength,
                    helper,
                    highlightableFilter,
                    buildProgressCalculator,
                    buildingDoubleClickHandler,
                    cruiserDoubleClickHandler,
                    fogOfWarManager);

            cruiser.Initialise(cruiserArgs);
        }

        private IDroneFocuser CreateDroneFocuser(bool isPlayerCruiser, IDroneManager droneManager, IPrioritisedSoundPlayer soundPlayer)
        {
            if (isPlayerCruiser)
            {
                return new DroneFocuser(droneManager, new DroneFocusSoundPicker(), soundPlayer);
            }
            else
            {
                return new SimpleDroneFocuser(droneManager);
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