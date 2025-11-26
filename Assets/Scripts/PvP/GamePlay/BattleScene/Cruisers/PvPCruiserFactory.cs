using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Data;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Buildings;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.BuildProgress;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Buildables.Repairables;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Drones;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Fog;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Helpers;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers.Slots;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Scenes.BattleScene;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.UI.Common.Click;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Factories;
using BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Utils.Fetchers;
using BattleCruisers.Network.Multiplay.Matchplay.Shared;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.UI.Common.Click;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Properties;
using UnityEngine;

namespace BattleCruisers.Network.Multiplay.Matchplay.MultiplayBattleScene.Cruisers
{
    public class PvPCruiserFactory
    {
        private readonly PvPBattleSceneHelper _helper;
        private readonly IFilter<PvPSlot> _highlightableSlotFilter;

        private const int CRUISER_OFFSET_IN_M = 35;

        public PvPCruiserFactory(PvPBattleSceneHelper helper)
        {
            PvPHelper.AssertIsNotNull(helper);

            _helper = helper;
            _highlightableSlotFilter = helper.CreateHighlightableSlotFilter();
        }

        public PvPCruiser CreatePlayerACruiser()
        {
            PvPCruiser playerACruiserPrefab = PvPPrefabFactory.GetCruiserPrefab(_helper.PlayerACruiser);
            PvPCruiser playerACruiser = PvPPrefabFactory.CreateCruiser(playerACruiserPrefab, SynchedServerData.Instance.playerAClientNetworkId.Value, -CRUISER_OFFSET_IN_M);

            playerACruiser.Position = new Vector3(-CRUISER_OFFSET_IN_M, playerACruiser.YAdjustmentInM, 0);

            return playerACruiser;
        }

        public PvPCruiser CreatePlayerBCruiser()
        {
            PvPCruiser playerBCruiserPrefab = PvPPrefabFactory.GetCruiserPrefab(_helper.PlayerBCruiser);
            PvPCruiser playerBCruiser = PvPPrefabFactory.CreateCruiser(playerBCruiserPrefab, SynchedServerData.Instance.playerBClientNetworkId.Value, CRUISER_OFFSET_IN_M);
            playerBCruiser.Position = new Vector3(CRUISER_OFFSET_IN_M, playerBCruiser.YAdjustmentInM, 0);
            Quaternion rotation = playerBCruiser.Rotation;
            rotation.eulerAngles = new Vector3(0, 180, 0);
            playerBCruiser.Rotation = rotation;
            return playerBCruiser;
        }

        public void InitialisePlayerACruiser(
            PvPCruiser playerACruiser,
            PvPCruiser playerBCruiser,
            IRankedTargetTracker userChosenTargetTracker
            )
        {
            PvPHelper.AssertIsNotNull(playerACruiser, playerBCruiser, userChosenTargetTracker);

            PvPCruiserHelper helper = new PvPPlayerCruiserHelper();
            Faction faction = Faction.Blues;
            Direction facingDirection = Direction.Right;
            IDoubleClickHandler<IPvPBuilding> buildingDoubleClickHandler = new PvPPlayerBuildingDoubleClickHandler();
            IDoubleClickHandler<IPvPCruiser> cruiserDoubleClickHandler = new PvPPlayerCruiserDoubleClickHandler();

            InitialiseCruiser(
                playerACruiser,
                playerBCruiser,
                // _uiManager,
                helper,
                faction,
                facingDirection,
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
            IRankedTargetTracker userChosenTargetTracker)
        {
            PvPHelper.AssertIsNotNull(playerBCruiser, playerACruiser, userChosenTargetTracker);

            PvPCruiserHelper helper = new PvPPlayerBCruiserHelper();
            Faction faction = Faction.Reds;
            Direction facingDirection = Direction.Left;

            IDoubleClickHandler<IPvPBuilding> buildingDoubleClickHandler = new PvPPlayerBuildingDoubleClickHandler();
            IDoubleClickHandler<IPvPCruiser> cruiserDoubleClickHandler = new PvPPlayerCruiserDoubleClickHandler();

            InitialiseCruiser(
                playerBCruiser,
                playerACruiser,
                helper,
                faction,
                facingDirection,
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
            PvPCruiserHelper helper,
            Faction faction,
            Direction facingDirection,
            IFilter<PvPSlot> highlightableFilter,
            IPvPBuildProgressCalculator buildProgressCalculator,
            IRankedTargetTracker userChosenTargetTracker,
            IDoubleClickHandler<IPvPBuilding> buildingDoubleClickHandler,
            IDoubleClickHandler<IPvPCruiser> cruiserDoubleClickHandler,
            IBroadcastingProperty<bool> parentCruiserHasActiveDrones,
            bool isPlayerCruiser)
        {

            PvPCruiserSpecificFactories cruiserSpecificFactories
                = new PvPCruiserSpecificFactories(
                    cruiser,
                    enemyCruiser,
                    userChosenTargetTracker,
                    PvPFactoryProvider.UpdaterProvider,
                    faction,
                    ApplicationModel.IsTutorial);

            DroneManager droneManager = new DroneManager();
            IPvPDroneFocuser droneFocuser = CreateDroneFocuser(isPlayerCruiser, droneManager);
            IDroneConsumerProvider droneConsumerProvider = new DroneConsumerProvider(droneManager);
            PvPFogOfWarManager fogOfWarManager = new PvPFogOfWarManager(cruiser.Fog, cruiser.BuildingMonitor, enemyCruiser.BuildingMonitor, enemyCruiser.UnitMonitor);

            PvPRepairManager repairManager = new PvPRepairManager(cruiserSpecificFactories.DroneFeedbackFactory, droneConsumerProvider, cruiser);
            if (!isPlayerCruiser)
            {
                repairManager.RemoveCruiser();
            }

            PvPCruiserArgs cruiserArgs
                = new PvPCruiserArgs(
                    faction,
                    enemyCruiser,
                    droneManager,
                    droneFocuser,
                    droneConsumerProvider,
                    cruiserSpecificFactories,
                    facingDirection,
                    repairManager,
                    helper,
                    highlightableFilter,
                    buildProgressCalculator,
                    buildingDoubleClickHandler,
                    cruiserDoubleClickHandler,
                    fogOfWarManager,
                    parentCruiserHasActiveDrones);

            cruiser.Initialise(cruiserArgs);
        }

        private IPvPDroneFocuser CreateDroneFocuser(bool isPlayerCruiser, DroneManager droneManager)
        {
            if (isPlayerCruiser)
            {
                return new PvPPlayerCruiserDroneFocuser(droneManager, new DroneFocusSoundPicker());
            }
            else
            {
                return new PvPAICruiserDroneFocuser(droneManager);
            }
        }
    }
}