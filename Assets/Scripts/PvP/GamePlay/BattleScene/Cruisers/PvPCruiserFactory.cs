using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Fog;
using BattleCruisers.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Repairables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Fog;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Helpers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.BattleScene.Manager;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.Click;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Common.Click;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    public class PvPCruiserFactory : IPvPCruiserFactory
    {
        private readonly IPvPBattleSceneHelper _helper;
        private readonly IFilter<IPvPSlot> _highlightableSlotFilter;
        private readonly IPvPUIManager _uiManager;
        private readonly IFogVisibilityDecider _fogVisibilityDecider;

        private const int CRUISER_OFFSET_IN_M = 35;

        public PvPCruiserFactory(IPvPBattleSceneHelper helper)
        {
            PvPHelper.AssertIsNotNull(helper/*, uiManager*/);

            _helper = helper;
            _highlightableSlotFilter = helper.CreateHighlightableSlotFilter();
            // _uiManager = uiManager;
            _fogVisibilityDecider = new FogVisibilityDecider();
        }

        public PvPCruiser CreatePlayerACruiser(Team team)
        {
            PvPCruiser playerACruiserPrefab = PvPPrefabFactory.GetCruiserPrefab(_helper.PlayerACruiser);
            PvPCruiser playerACruiser = PvPPrefabFactory.CreateCruiser(playerACruiserPrefab, SynchedServerData.Instance.playerAClientNetworkId.Value, -CRUISER_OFFSET_IN_M);

            //   PvPCruiser playerACruiser = _factoryProvider.PrefabFactory.CreateCruiser(_helper.PlayerACruiser.PrefabName, SynchedServerData.Instance.playerAClientNetworkId.Value, -CRUISER_OFFSET_IN_M);
            playerACruiser.Position = new Vector3(-CRUISER_OFFSET_IN_M, playerACruiser.YAdjustmentInM, 0);

            return playerACruiser;
        }

        public PvPCruiser CreatePlayerBCruiser(Team team)
        {
            PvPCruiser playerBCruiserPrefab = PvPPrefabFactory.GetCruiserPrefab(_helper.PlayerBCruiser);
            PvPCruiser playerBCruiser = PvPPrefabFactory.CreateCruiser(playerBCruiserPrefab, SynchedServerData.Instance.playerBClientNetworkId.Value, CRUISER_OFFSET_IN_M);
            // PvPCruiser playerBCruiser = _factoryProvider.PrefabFactory.CreateCruiser(_helper.PlayerBCruiser.PrefabName, SynchedServerData.Instance.playerBClientNetworkId.Value, CRUISER_OFFSET_IN_M);
            playerBCruiser.Position = new Vector3(CRUISER_OFFSET_IN_M, playerBCruiser.YAdjustmentInM, 0);
            Quaternion rotation = playerBCruiser.Rotation;
            rotation.eulerAngles = new Vector3(0, 180, 0);
            playerBCruiser.Rotation = rotation;
            return playerBCruiser;
        }

        public PvPCruiser CreateAIBotCruiser(Team team)
        {
            PvPCruiser aiBotCruiserPrefab = PvPPrefabFactory.GetCruiserPrefab(_helper.PlayerBCruiser);
            PvPCruiser aiBotCruiser = PvPPrefabFactory.CreateAIBotCruiser(aiBotCruiserPrefab, CRUISER_OFFSET_IN_M);
            aiBotCruiser.Position = new Vector3(CRUISER_OFFSET_IN_M, aiBotCruiser.YAdjustmentInM, 0);
            Quaternion rotation = aiBotCruiser.Rotation;
            rotation.eulerAngles = new Vector3(0, 180, 0);
            aiBotCruiser.Rotation = rotation;
            return aiBotCruiser;
        }

        public void InitialisePlayerACruiser(
            PvPCruiser playerACruiser,
            PvPCruiser playerBCruiser,
            // ICameraFocuser cameraFocuser,
            IRankedTargetTracker userChosenTargetTracker
            // IUserChosenTargetHelper userChosenTargetHelper
            )
        {
            PvPHelper.AssertIsNotNull(playerACruiser, playerBCruiser,/* cameraFocuser,*/ userChosenTargetTracker);

            IPvPCruiserHelper helper = CreatePlayerHelper(/*_uiManager , cameraFocuser*/);
            Faction faction = Faction.Blues;
            Direction facingDirection = Direction.Right;
            PvPFogStrength fogStrength = PvPFogStrength.Weak;
            IDoubleClickHandler<IPvPBuilding> buildingDoubleClickHandler = new PvPPlayerBuildingDoubleClickHandler();
            IDoubleClickHandler<IPvPCruiser> cruiserDoubleClickHandler = new PvPPlayerCruiserDoubleClickHandler();

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
                PvPFactoryProvider.DroneMonitor.LeftCruiserHasActiveDrones,
                isPlayerCruiser: true);
        }

        public void InitialisePlayerBCruiser(
            PvPCruiser playerBCruiser,
            PvPCruiser playerACruiser,
        //    ICameraFocuser cameraFocuser,
            IRankedTargetTracker userChosenTargetTracker
            /* IUserChosenTargetHelper userChosenTargetHelper */)
        {
            PvPHelper.AssertIsNotNull(playerBCruiser, playerACruiser, userChosenTargetTracker /*, userChosenTargetHelper*/);

            IPvPCruiserHelper helper = CreatePlayerBHelper(/*_uiManager , cameraFocuser*/);
            Faction faction = Faction.Reds;
            Direction facingDirection = Direction.Left;
            PvPFogStrength fogStrength = PvPFogStrength.Strong;

            IDoubleClickHandler<IPvPBuilding> buildingDoubleClickHandler = new PvPPlayerBuildingDoubleClickHandler();
            IDoubleClickHandler<IPvPCruiser> cruiserDoubleClickHandler = new PvPPlayerCruiserDoubleClickHandler();

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
                PvPFactoryProvider.DroneMonitor.RightCruiserHasActiveDrones,
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
            IFilter<IPvPSlot> highlightableFilter,
            IPvPBuildProgressCalculator buildProgressCalculator,
            IRankedTargetTracker userChosenTargetTracker,
            IDoubleClickHandler<IPvPBuilding> buildingDoubleClickHandler,
            IDoubleClickHandler<IPvPCruiser> cruiserDoubleClickHandler,
            IBroadcastingProperty<bool> parentCruiserHasActiveDrones,
            bool isPlayerCruiser)
        {

            IPvPCruiserSpecificFactories cruiserSpecificFactories
                = new PvPCruiserSpecificFactories(
                    cruiser,
                    enemyCruiser,
                    userChosenTargetTracker,
                    PvPFactoryProvider.UpdaterProvider,
                    faction,
                    ApplicationModel.IsTutorial);

            DroneManager droneManager = new DroneManager();
            IPvPDroneFocuser droneFocuser = CreateDroneFocuser(isPlayerCruiser, droneManager /*, _factoryProvider.Sound.PrioritisedSoundPlayer*/);
            IDroneConsumerProvider droneConsumerProvider = new DroneConsumerProvider(droneManager);
            PvPFogOfWarManager fogOfWarManager = new PvPFogOfWarManager(cruiser.Fog, _fogVisibilityDecider, cruiser.BuildingMonitor, enemyCruiser.BuildingMonitor, enemyCruiser.UnitMonitor);

            PvPRepairManager repairManager = new PvPRepairManager(cruiserSpecificFactories.DroneFeedbackFactory, droneConsumerProvider, cruiser);
            if (!isPlayerCruiser)
            {
                repairManager.RemoveCruiser();
            }

            PvPCruiserArgs cruiserArgs
                = new PvPCruiserArgs(
                    faction,
                    enemyCruiser,
                    // uiManager,
                    droneManager,
                    droneFocuser,
                    droneConsumerProvider,
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

        private IPvPDroneFocuser CreateDroneFocuser(bool isPlayerCruiser, DroneManager droneManager /*, IPrioritisedSoundPlayer soundPlayer */)
        {
            if (isPlayerCruiser)
            {
                return new PvPPlayerCruiserDroneFocuser(droneManager, new DroneFocusSoundPicker() /*, soundPlayer*/);
            }
            else
            {
                return new PvPAICruiserDroneFocuser(droneManager);
            }
        }

        private IPvPCruiserHelper CreatePlayerBHelper(IPvPUIManager uiManager, ICameraFocuser cameraFocuser)
        {
            // if (ApplicationModel.IsTutorial)
            // {
            //     return new PvPTutorialAICruiserHelper(uiManager, cameraFocuser);
            // }
            // else
            // {
            return new PvPPlayerBCruiserHelper(uiManager, cameraFocuser);
            // }
        }

        private IPvPCruiserHelper CreatePlayerBHelper(/*IPvPUIManager uiManager  , ICameraFocuser cameraFocuser*/)
        {
            // if (ApplicationModel.IsTutorial)
            // {
            //     return new PvPTutorialAICruiserHelper(uiManager, cameraFocuser);
            // }
            // else
            // {
            return new PvPPlayerBCruiserHelper(/*uiManager , cameraFocuser*/);
            // }
        }

        private IPvPCruiserHelper CreatePlayerHelper(IPvPUIManager uiManager, ICameraFocuser cameraFocuser)
        {
            // if (ApplicationModel.IsTutorial)
            // {
            //     return new PvPTutorialPlayerCruiserHelper(uiManager, cameraFocuser);
            // }
            // else
            // {
            return new PvPPlayerCruiserHelper(uiManager, cameraFocuser);
            // }
        }


        private IPvPCruiserHelper CreatePlayerHelper(/*IPvPUIManager uiManager , ICameraFocuser cameraFocuser*/)
        {
            // if (ApplicationModel.IsTutorial)
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