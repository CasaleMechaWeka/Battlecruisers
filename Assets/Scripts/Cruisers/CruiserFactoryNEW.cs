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
using BattleCruisers.Scenes;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras;
using BattleCruisers.UI.Common.Click;
using BattleCruisers.UI.Sound;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.PlatformAbstractions;
using BattleCruisers.Utils.PlatformAbstractions.UI;
using BattleCruisers.Utils.Threading;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    public class CruiserFactoryNEW : ICruiserFactoryNEW
	{
        private readonly IPrefabFactory _prefabFactory;
		private readonly IDeferrer _deferrer;
        private readonly IVariableDelayDeferrer _variableDelayDeferrer;
        private readonly ISpriteProvider _spriteProvider;
        private readonly ICamera _soleCamera;
        private readonly IAudioSource _audioSource;
        private readonly IBattleSceneHelper _helper;
        private readonly IApplicationModel _applicationModel;
        private Cruiser _playerCruiser, _aiCruiser;

        private const int CRUISER_OFFSET_IN_M = 35;

        public CruiserFactoryNEW(
            IPrefabFactory prefabFactory, 
            IDeferrer deferrer, 
            IVariableDelayDeferrer variableDelayDeferrer, 
            ISpriteProvider spriteProvider,
            ICamera soleCamera,
            IAudioSource audioSource,
            IBattleSceneHelper helper,
            IApplicationModel applicationModel)
        {
            Helper.AssertIsNotNull(prefabFactory, deferrer, variableDelayDeferrer, spriteProvider, soleCamera, audioSource, helper, applicationModel);
            
            _prefabFactory = prefabFactory;
            _deferrer = deferrer;
            _variableDelayDeferrer = variableDelayDeferrer;
            _spriteProvider = spriteProvider;
            _soleCamera = soleCamera;
            _audioSource = audioSource;
            _helper = helper;
            _applicationModel = applicationModel;
        }

        public ICruiser CreatePlayerCruiser()
        {
            ILoadout playerLoadout = _helper.GetPlayerLoadout();
            Cruiser playerCruiserPrefab = _prefabFactory.GetCruiserPrefab(playerLoadout.Hull);
            _playerCruiser = _prefabFactory.CreateCruiser(playerCruiserPrefab);
            _playerCruiser.transform.position = new Vector3(-CRUISER_OFFSET_IN_M, _playerCruiser.YAdjustmentInM, 0);

            return _playerCruiser;
        }

        public ICruiser CreateAICruiser()
        {
            ILevel currentLevel = _applicationModel.DataProvider.GetLevel(_applicationModel.SelectedLevel);
            Cruiser aiCruiserPrefab = _prefabFactory.GetCruiserPrefab(currentLevel.Hull);
            _aiCruiser = _prefabFactory.CreateCruiser(aiCruiserPrefab);

            _aiCruiser.transform.position = new Vector3(CRUISER_OFFSET_IN_M, _aiCruiser.YAdjustmentInM, 0);
            Quaternion rotation = _aiCruiser.transform.rotation;
            rotation.eulerAngles = new Vector3(0, 180, 0);
            _aiCruiser.transform.rotation = rotation;

            return _aiCruiser;
        }

        public void InitialisePlayerCruiser(
            IUIManager uiManager,
            ICruiserHelper helper,
            ISlotFilter highlightableFilter,
            IBuildProgressCalculator buildProgressCalculator,
            IRankedTargetTracker userChosenTargetTracker)
        {
            Helper.AssertIsNotNull(
                uiManager,
                helper,
                highlightableFilter,
                buildProgressCalculator,
                userChosenTargetTracker);
            Assert.IsNotNull(_playerCruiser, "Must call CreatePlayerCruiser() before InitialisePlayerCruiser()");
            Assert.IsNotNull(_aiCruiser, "Must call CreateAICruiser() before InitialisePlayerCruiser()");

            Faction faction = Faction.Blues;
            Direction facingDirection = Direction.Right;
            bool shouldShowFog = false;
            IDroneNumFeedbackFactory feedbackFactory = new DroneNumFeedbackFactory();
            IDoubleClickHandler<IBuilding> buildingDoubleClickHandler = new PlayerBuildingDoubleClickHandler();
            IDoubleClickHandler<ICruiser> cruiserDoubleClickHandler = new PlayerCruiserDoubleClickHandler();

            InitialiseCruiser(
                _playerCruiser,
                _aiCruiser,
                uiManager,
                helper,
                faction,
                facingDirection,
                shouldShowFog,
                highlightableFilter,
                buildProgressCalculator,
                userChosenTargetTracker,
                feedbackFactory,
                buildingDoubleClickHandler,
                cruiserDoubleClickHandler,
                isPlayerCruiser: true);
        }

        public void InitialiseAICruiser(
            IUIManager uiManager,
            ICruiserHelper helper,
            ISlotFilter highlightableFilter,
            IBuildProgressCalculator buildProgressCalculator,
            IRankedTargetTracker userChosenTargetTracker,
            IUserChosenTargetHelper userChosenTargetHelper)
        {
            Helper.AssertIsNotNull(
                uiManager,
                helper,
                highlightableFilter,
                buildProgressCalculator,
                userChosenTargetTracker,
                userChosenTargetHelper);
            Assert.IsNotNull(_aiCruiser, "Must call CreateAICruiser() before InitialiseAICruiser()");
            Assert.IsNotNull(_playerCruiser, "Must call CreatePlayerCruiser() before InitialiseAICruiser()");

            Faction faction = Faction.Reds;
            Direction facingDirection = Direction.Left;
            bool shouldShowFog = true;

            IDroneNumFeedbackFactory feedbackFactory = new DroneNumFeedbackFactory();
            IDoubleClickHandler<IBuilding> buildingDoubleClickHandler = new AIBuildingDoubleClickHandler(userChosenTargetHelper);
            IDoubleClickHandler<ICruiser> cruiserDoubleClickHandler = new AICruiserDoubleClickHandler(userChosenTargetHelper);

            InitialiseCruiser(
                _aiCruiser,
                _playerCruiser,
                uiManager,
                helper,
                faction,
                facingDirection,
                shouldShowFog,
                highlightableFilter,
                buildProgressCalculator,
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
            bool shouldShowFog,
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
                    _variableDelayDeferrer, 
                    userChosenTargetTracker, 
                    _soleCamera, 
                    isPlayerCruiser, 
                    _audioSource);

            IDroneManager droneManager = new DroneManager();
            IDroneFocuser droneFocuser = CreateDroneFocuser(isPlayerCruiser, droneManager, factoryProvider.Sound.PrioritisedSoundPlayer);
            IDroneConsumerProvider droneConsumerProvider = new DroneConsumerProvider(droneManager);
            RepairManager repairManager = new RepairManager(_deferrer, feedbackFactory);
            FogOfWarManager fogOfWarManager = new FogOfWarManager(cruiser.Fog, cruiser, enemyCruiser);

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
                    shouldShowFog,
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

        public ICruiserHelper CreateAIHelper(IUIManager uiManager, ICameraController camera)
        {
            return new AICruiserHelper(uiManager, camera);
        }

        public ICruiserHelper CreatePlayerHelper(IUIManager uiManager, ICameraController camera)
        {
            return new PlayerCruiserHelper(uiManager, camera);
        }
    }
}