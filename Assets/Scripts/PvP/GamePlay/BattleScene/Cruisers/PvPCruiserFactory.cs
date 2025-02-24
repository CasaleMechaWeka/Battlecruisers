using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Repairables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Fog;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Helpers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Targets.TargetTrackers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Navigation;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.Click;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.Utils.Properties;
using UnityEngine;
using System.Threading.Tasks;
using BattleCruisers.Buildables;

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

        public async Task<PvPCruiser> CreatePlayerACruiser(Team team)
        {
            PvPCruiser playerACruiserPrefab = _factoryProvider.PrefabFactory.GetCruiserPrefab(_helper.PlayerACruiser);
            PvPCruiser playerACruiser = _factoryProvider.PrefabFactory.CreateCruiser(playerACruiserPrefab, SynchedServerData.Instance.playerAClientNetworkId.Value, -CRUISER_OFFSET_IN_M);

            //   PvPCruiser playerACruiser = _factoryProvider.PrefabFactory.CreateCruiser(_helper.PlayerACruiser.PrefabName, SynchedServerData.Instance.playerAClientNetworkId.Value, -CRUISER_OFFSET_IN_M);
            playerACruiser.Position = new Vector3(-CRUISER_OFFSET_IN_M, playerACruiser.YAdjustmentInM, 0);

            return playerACruiser;
        }

        public async Task<PvPCruiser> CreatePlayerBCruiser(Team team)
        {
            PvPCruiser playerBCruiserPrefab = _factoryProvider.PrefabFactory.GetCruiserPrefab(_helper.PlayerBCruiser);
            PvPCruiser playerBCruiser = _factoryProvider.PrefabFactory.CreateCruiser(playerBCruiserPrefab, SynchedServerData.Instance.playerBClientNetworkId.Value, CRUISER_OFFSET_IN_M);
            // PvPCruiser playerBCruiser = _factoryProvider.PrefabFactory.CreateCruiser(_helper.PlayerBCruiser.PrefabName, SynchedServerData.Instance.playerBClientNetworkId.Value, CRUISER_OFFSET_IN_M);
            playerBCruiser.Position = new Vector3(CRUISER_OFFSET_IN_M, playerBCruiser.YAdjustmentInM, 0);
            Quaternion rotation = playerBCruiser.Rotation;
            rotation.eulerAngles = new Vector3(0, 180, 0);
            playerBCruiser.Rotation = rotation;
            return playerBCruiser;
        }

        public async Task<PvPCruiser> CreateAIBotCruiser(Team team)
        {
            PvPCruiser aiBotCruiserPrefab = _factoryProvider.PrefabFactory.GetCruiserPrefab(_helper.PlayerBCruiser);
            PvPCruiser aiBotCruiser = _factoryProvider.PrefabFactory.CreateAIBotCruiser(aiBotCruiserPrefab, CRUISER_OFFSET_IN_M);
            aiBotCruiser.Position = new Vector3(CRUISER_OFFSET_IN_M, aiBotCruiser.YAdjustmentInM, 0);
            Quaternion rotation = aiBotCruiser.Rotation;
            rotation.eulerAngles = new Vector3(0, 180, 0);
            aiBotCruiser.Rotation = rotation;
            return aiBotCruiser;
        }

        public void InitialisePlayerACruiser(
            PvPCruiser playerACruiser,
            PvPCruiser playerBCruiser,
            // IPvPCameraFocuser cameraFocuser,
            IPvPRankedTargetTracker userChosenTargetTracker
            // IPvPUserChosenTargetHelper userChosenTargetHelper
            )
        {
            PvPHelper.AssertIsNotNull(playerACruiser, playerBCruiser,/* cameraFocuser,*/ userChosenTargetTracker);

            IPvPCruiserHelper helper = CreatePlayerHelper(/*_uiManager , cameraFocuser*/);
            Faction faction = Faction.Blues;
            Direction facingDirection = Direction.Right;
            PvPFogStrength fogStrength = PvPFogStrength.Weak;
            IPvPDoubleClickHandler<IPvPBuilding> buildingDoubleClickHandler = new PvPPlayerBuildingDoubleClickHandler();
            IPvPDoubleClickHandler<IPvPCruiser> cruiserDoubleClickHandler = new PvPPlayerCruiserDoubleClickHandler();

            InitialiseCruiser(
                playerACruiser,
                playerBCruiser,
                // _uiManager,
                helper,
                faction,
                facingDirection,
                fogStrength,
                _highlightableSlotFilter,
                _helper.CreatePlayerACruiserBuildProgressCalculator(),
                userChosenTargetTracker,
                buildingDoubleClickHandler,
                cruiserDoubleClickHandler,
                _factoryProvider.DroneMonitor.PlayerACruiserHasActiveDrones,
                isPlayerCruiser: true);
        }

        public void InitialisePlayerBCruiser(
            PvPCruiser playerBCruiser,
            PvPCruiser playerACruiser,
        //    IPvPCameraFocuser cameraFocuser,
            IPvPRankedTargetTracker userChosenTargetTracker
            /* IPvPUserChosenTargetHelper userChosenTargetHelper */)
        {
            PvPHelper.AssertIsNotNull(playerBCruiser, playerACruiser, userChosenTargetTracker /*, userChosenTargetHelper*/);

            IPvPCruiserHelper helper = CreatePlayerBHelper(/*_uiManager , cameraFocuser*/);
            Faction faction = Faction.Reds;
            Direction facingDirection = Direction.Left;
            PvPFogStrength fogStrength = PvPFogStrength.Strong;

            IPvPDoubleClickHandler<IPvPBuilding> buildingDoubleClickHandler = new PvPPlayerBuildingDoubleClickHandler();
            IPvPDoubleClickHandler<IPvPCruiser> cruiserDoubleClickHandler = new PvPPlayerCruiserDoubleClickHandler();

            InitialiseCruiser(
                playerBCruiser,
                playerACruiser,
                // _uiManager,
                helper,
                faction,
                facingDirection,
                fogStrength,
                _highlightableSlotFilter,
                _helper.CreatePlayerBCruiserBuildProgressCalculator(),
                userChosenTargetTracker,
                buildingDoubleClickHandler,
                cruiserDoubleClickHandler,
                _factoryProvider.DroneMonitor.PlayerBCruiserHasActiveDrones,
                isPlayerCruiser: true);
        }

        private void InitialiseCruiser(
            PvPCruiser cruiser,
            IPvPCruiser enemyCruiser,
            // IPvPUIManager uiManager,
            IPvPCruiserHelper helper,
            Faction faction,
            Direction facingDirection,
            PvPFogStrength fogStrength,
            IPvPSlotFilter highlightableFilter,
            IPvPBuildProgressCalculator buildProgressCalculator,
            IPvPRankedTargetTracker userChosenTargetTracker,
            IPvPDoubleClickHandler<IPvPBuilding> buildingDoubleClickHandler,
            IPvPDoubleClickHandler<IPvPCruiser> cruiserDoubleClickHandler,
            IBroadcastingProperty<bool> parentCruiserHasActiveDrones,
            bool isPlayerCruiser)
        {

            IPvPCruiserSpecificFactories cruiserSpecificFactories
                = new PvPCruiserSpecificFactories(
                    _factoryProvider,
                    cruiser,
                    enemyCruiser,
                    userChosenTargetTracker,
                    _factoryProvider.UpdaterProvider,
                    faction);

            IDroneManager droneManager = new PvPDroneManager();
            IPvPDroneFocuser droneFocuser = CreateDroneFocuser(isPlayerCruiser, droneManager /*, _factoryProvider.Sound.PrioritisedSoundPlayer*/);
            IDroneConsumerProvider droneConsumerProvider = new PvPDroneConsumerProvider(droneManager);
            PvPFogOfWarManager fogOfWarManager = new PvPFogOfWarManager(cruiser.Fog, _fogVisibilityDecider, cruiser.BuildingMonitor, enemyCruiser.BuildingMonitor, enemyCruiser.UnitMonitor);

            PvPRepairManager repairManager = new PvPRepairManager(cruiserSpecificFactories.DroneFeedbackFactory, droneConsumerProvider, cruiser);
            if (!isPlayerCruiser)
            {
                repairManager.RemoveCruiser();
            }

            IPvPCruiserArgs cruiserArgs
                = new PvPCruiserArgs(
                    faction,
                    enemyCruiser,
                    // uiManager,
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

        private IPvPDroneFocuser CreateDroneFocuser(bool isPlayerCruiser, IDroneManager droneManager /*, IPrioritisedSoundPlayer soundPlayer */)
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

        private IPvPCruiserHelper CreatePlayerBHelper(IPvPUIManager uiManager, IPvPCameraFocuser cameraFocuser)
        {
            // if (_applicationModel.IsTutorial)
            // {
            //     return new PvPTutorialAICruiserHelper(uiManager, cameraFocuser);
            // }
            // else
            // {
            return new PvPPlayerBCruiserHelper(uiManager, cameraFocuser);
            // }
        }

        private IPvPCruiserHelper CreatePlayerBHelper(/*IPvPUIManager uiManager  , IPvPCameraFocuser cameraFocuser*/)
        {
            // if (_applicationModel.IsTutorial)
            // {
            //     return new PvPTutorialAICruiserHelper(uiManager, cameraFocuser);
            // }
            // else
            // {
            return new PvPPlayerBCruiserHelper(/*uiManager , cameraFocuser*/);
            // }
        }

        private IPvPCruiserHelper CreatePlayerHelper(IPvPUIManager uiManager, IPvPCameraFocuser cameraFocuser)
        {
            // if (_applicationModel.IsTutorial)
            // {
            //     return new PvPTutorialPlayerCruiserHelper(uiManager, cameraFocuser);
            // }
            // else
            // {
            return new PvPPlayerCruiserHelper(uiManager, cameraFocuser);
            // }
        }


        private IPvPCruiserHelper CreatePlayerHelper(/*IPvPUIManager uiManager , IPvPCameraFocuser cameraFocuser*/)
        {
            // if (_applicationModel.IsTutorial)
            // {
            //     return new PvPTutorialPlayerCruiserHelper(uiManager, cameraFocuser);
            // }
            // else
            // {
            return new PvPPlayerCruiserHelper(/*uiManager , cameraFocuser*/);
            // }
        }
    }
}