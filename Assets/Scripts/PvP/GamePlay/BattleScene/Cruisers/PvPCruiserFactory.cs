using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Repairables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Units;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Fog;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Helpers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Data.Models.PrefabKeys;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers.UserChosen;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.Click;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Sound.Players;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Properties;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    public class PvPCruiserFactory : IPvPCruiserFactory
    {
        private readonly IPvPFactoryProvider _factoryProvider;
        private readonly IPvPBattleSceneHelper _helper;
        private readonly IApplicationModel _applicationModel;
        private readonly IPvPSlotFilter _highlightableSlotFilter;
        private readonly IPvPUIManager _uiManager;
        private readonly IPvPFogVisibilityDecider _fogVisibilityDecider;

        private const int CRUISER_OFFSET_IN_M = 35;

        public PvPCruiserFactory(
            IPvPFactoryProvider factoryProvider,
            IPvPBattleSceneHelper helper,
            IApplicationModel applicationModel
         /*   IPvPUIManager uiManager*/)
        {
            PvPHelper.AssertIsNotNull(factoryProvider, helper, applicationModel/*, uiManager*/);

            _factoryProvider = factoryProvider;
            _helper = helper;
            _applicationModel = applicationModel;
            _highlightableSlotFilter = helper.CreateHighlightableSlotFilter();
            // _uiManager = uiManager;
            _fogVisibilityDecider = new PvPFogVisibilityDecider();
        }

        public PvPCruiser CreatePlayerCruiser()
        {
            PvPCruiser playerCruiserPrefab = _factoryProvider.PrefabFactory.GetCruiserPrefab(_helper.PlayerCruiser);
            PvPCruiser playerCruiser = _factoryProvider.PrefabFactory.CreateCruiser(playerCruiserPrefab);
            playerCruiser.Position = new Vector3(-CRUISER_OFFSET_IN_M, playerCruiser.YAdjustmentInM, 0);

            return playerCruiser;
        }

        public PvPCruiser CreateAICruiser(IPvPPrefabKey aiCruiserKey)
        {
            Assert.IsNotNull(aiCruiserKey);

            PvPCruiser aiCruiserPrefab = _factoryProvider.PrefabFactory.GetCruiserPrefab(aiCruiserKey);
            PvPCruiser aiCruiser = _factoryProvider.PrefabFactory.CreateCruiser(aiCruiserPrefab);

            aiCruiser.Position = new Vector3(CRUISER_OFFSET_IN_M, aiCruiser.YAdjustmentInM, 0);
            Quaternion rotation = aiCruiser.Rotation;
            rotation.eulerAngles = new Vector3(0, 180, 0);
            aiCruiser.Rotation = rotation;

            return aiCruiser;
        }

        public void InitialisePlayerCruiser(
            PvPCruiser playerCruiser,
            PvPCruiser aiCruiser,
            IPvPCameraFocuser cameraFocuser,
            IPvPRankedTargetTracker userChosenTargetTracker)
        {
            PvPHelper.AssertIsNotNull(playerCruiser, aiCruiser, cameraFocuser, userChosenTargetTracker);

            IPvPCruiserHelper helper = CreatePlayerHelper(_uiManager, cameraFocuser);
            PvPFaction faction = PvPFaction.Blues;
            PvPDirection facingDirection = PvPDirection.Right;
            PvPFogStrength fogStrength = PvPFogStrength.Weak;
            IPvPDoubleClickHandler<IPvPBuilding> buildingDoubleClickHandler = new PvPPlayerBuildingDoubleClickHandler();
            IPvPDoubleClickHandler<IPvPCruiser> cruiserDoubleClickHandler = new PvPPlayerCruiserDoubleClickHandler();

            InitialiseCruiser(
                playerCruiser,
                aiCruiser,
                _uiManager,
                helper,
                faction,
                facingDirection,
                fogStrength,
                _highlightableSlotFilter,
                _helper.CreatePlayerCruiserBuildProgressCalculator(),
                userChosenTargetTracker,
                buildingDoubleClickHandler,
                cruiserDoubleClickHandler,
                _factoryProvider.DroneMonitor.PlayerCruiserHasActiveDrones,
                isPlayerCruiser: true);
        }

        public void InitialiseAICruiser(
            PvPCruiser playerCruiser,
            PvPCruiser aiCruiser,
            IPvPCameraFocuser cameraFocuser,
            IPvPRankedTargetTracker userChosenTargetTracker,
            IPvPUserChosenTargetHelper userChosenTargetHelper)
        {
            PvPHelper.AssertIsNotNull(playerCruiser, aiCruiser, userChosenTargetTracker, userChosenTargetHelper);

            IPvPCruiserHelper helper = CreateAIHelper(_uiManager, cameraFocuser);
            PvPFaction faction = PvPFaction.Reds;
            PvPDirection facingDirection = PvPDirection.Left;
            PvPFogStrength fogStrength = PvPFogStrength.Strong;

            IPvPDoubleClickHandler<IPvPBuilding> buildingDoubleClickHandler = new PvPAIBuildingDoubleClickHandler(userChosenTargetHelper);
            IPvPDoubleClickHandler<IPvPCruiser> cruiserDoubleClickHandler = new PvPAICruiserDoubleClickHandler(userChosenTargetHelper);

            InitialiseCruiser(
                aiCruiser,
                playerCruiser,
                _uiManager,
                helper,
                faction,
                facingDirection,
                fogStrength,
                _highlightableSlotFilter,
                _helper.CreateAICruiserBuildProgressCalculator(),
                userChosenTargetTracker,
                buildingDoubleClickHandler,
                cruiserDoubleClickHandler,
                _factoryProvider.DroneMonitor.AICruiserHasActiveDrones,
                isPlayerCruiser: false);
        }

        private void InitialiseCruiser(
            PvPCruiser cruiser,
            IPvPCruiser enemyCruiser,
            IPvPUIManager uiManager,
            IPvPCruiserHelper helper,
            PvPFaction faction,
            PvPDirection facingDirection,
            PvPFogStrength fogStrength,
            IPvPSlotFilter highlightableFilter,
            IPvPBuildProgressCalculator buildProgressCalculator,
            IPvPRankedTargetTracker userChosenTargetTracker,
            IPvPDoubleClickHandler<IPvPBuilding> buildingDoubleClickHandler,
            IPvPDoubleClickHandler<IPvPCruiser> cruiserDoubleClickHandler,
            IPvPBroadcastingProperty<bool> parentCruiserHasActiveDrones,
            bool isPlayerCruiser)
        {
            IPvPCruiserSpecificFactories cruiserSpecificFactories
                = new PvPCruiserSpecificFactories(
                    _factoryProvider,
                    cruiser,
                    enemyCruiser,
                    userChosenTargetTracker,
                    _factoryProvider.UpdaterProvider,
                    faction,
                    _applicationModel.IsTutorial);

            IPvPDroneManager droneManager = new PvPDroneManager();
            IPvPDroneFocuser droneFocuser = CreateDroneFocuser(isPlayerCruiser, droneManager /*, _factoryProvider.Sound.PrioritisedSoundPlayer*/);
            IPvPDroneConsumerProvider droneConsumerProvider = new PvPDroneConsumerProvider(droneManager);
            PvPFogOfWarManager fogOfWarManager = new PvPFogOfWarManager(cruiser.Fog, _fogVisibilityDecider, cruiser.BuildingMonitor, enemyCruiser.BuildingMonitor);

            PvPRepairManager repairManager = new PvPRepairManager(cruiserSpecificFactories.DroneFeedbackFactory, droneConsumerProvider, cruiser);
            if (!isPlayerCruiser)
            {
                repairManager.RemoveCruiser();
            }

            IPvPCruiserArgs cruiserArgs
                = new PvPCruiserArgs(
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

        private IPvPDroneFocuser CreateDroneFocuser(bool isPlayerCruiser, IPvPDroneManager droneManager /*, IPvPPrioritisedSoundPlayer soundPlayer */)
        {
            if (isPlayerCruiser)
            {
                return new PvPPlayerCruiserDroneFocuser(droneManager, new PvPDroneFocusSoundPicker() /*, soundPlayer*/);
            }
            else
            {
                return new PvPAICruiserDroneFocuser(droneManager);
            }
        }

        private IPvPCruiserHelper CreateAIHelper(IPvPUIManager uiManager, IPvPCameraFocuser cameraFocuser)
        {
            if (_applicationModel.IsTutorial)
            {
                return new PvPTutorialAICruiserHelper(uiManager, cameraFocuser);
            }
            else
            {
                return new PvPAICruiserHelper(uiManager, cameraFocuser);
            }
        }

        private IPvPCruiserHelper CreatePlayerHelper(IPvPUIManager uiManager, IPvPCameraFocuser cameraFocuser)
        {
            if (_applicationModel.IsTutorial)
            {
                return new PvPTutorialPlayerCruiserHelper(uiManager, cameraFocuser);
            }
            else
            {
                return new PvPPlayerCruiserHelper(uiManager, cameraFocuser);
            }
        }
    }
}