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
using BattleCruisers.Data.Models.PrefabKeys;
using BattleCruisers.Scenes.BattleScene;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.BattleScene.Navigation;
using BattleCruisers.UI.Common.Click;
using BattleCruisers.UI.Sound.Players;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Properties;
using UnityEngine;
using UnityEngine.Assertions;

namespace BattleCruisers.Cruisers
{
    public class CruiserFactory
    {
        private readonly IBattleSceneHelper _helper;
        private readonly IFilter<Slot> _highlightableSlotFilter;
        private readonly UIManager _uiManager;

        private const int CRUISER_OFFSET_IN_M = 35;

        public CruiserFactory(
            IBattleSceneHelper helper,
            UIManager uiManager)
        {
            Helper.AssertIsNotNull(helper, uiManager);

            _helper = helper;
            _highlightableSlotFilter = helper.CreateHighlightableSlotFilter();
            _uiManager = uiManager;
        }

        public Cruiser CreatePlayerCruiser()
        {
            Cruiser playerCruiserPrefab = PrefabFactory.GetCruiserPrefab(_helper.PlayerCruiser);
            Cruiser playerCruiser = PrefabFactory.CreateCruiser(playerCruiserPrefab);
            playerCruiser.Position = new Vector3(-CRUISER_OFFSET_IN_M, playerCruiser.YAdjustmentInM, 0);

            return playerCruiser;
        }

        public Cruiser CreateAICruiser(IPrefabKey aiCruiserKey)
        {
            Assert.IsNotNull(aiCruiserKey);

            Cruiser aiCruiserPrefab = PrefabFactory.GetCruiserPrefab(aiCruiserKey);
            Cruiser aiCruiser = PrefabFactory.CreateCruiser(aiCruiserPrefab);

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
                _helper.CreatePlayerCruiserBuildProgressCalculator(),
                userChosenTargetTracker,
                buildingDoubleClickHandler,
                cruiserDoubleClickHandler,
                FactoryProvider.DroneMonitor.LeftCruiserHasActiveDrones,
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
                _helper.CreateAICruiserBuildProgressCalculator(),
                userChosenTargetTracker,
                buildingDoubleClickHandler,
                cruiserDoubleClickHandler,
                FactoryProvider.DroneMonitor.RightCruiserHasActiveDrones,
                isPlayerCruiser: false);
        }

        private void InitialiseCruiser(
            Cruiser cruiser,
            ICruiser enemyCruiser,
            UIManager uiManager,
            ICruiserHelper helper,
            Faction faction,
            Direction facingDirection,
            FogStrength fogStrength,
            IFilter<Slot> highlightableFilter,
            IBuildProgressCalculator buildProgressCalculator,
            IRankedTargetTracker userChosenTargetTracker,
            IDoubleClickHandler<IBuilding> buildingDoubleClickHandler,
            IDoubleClickHandler<ICruiser> cruiserDoubleClickHandler,
            IBroadcastingProperty<bool> parentCruiserHasActiveDrones,
            bool isPlayerCruiser)
        {
            CruiserSpecificFactories cruiserSpecificFactories
                = new CruiserSpecificFactories(
                    cruiser,
                    enemyCruiser,
                    userChosenTargetTracker,
                    FactoryProvider.UpdaterProvider,
                    faction,
                    ApplicationModel.IsTutorial);

            DroneManager droneManager = new DroneManager();
            IDroneFocuser droneFocuser = CreateDroneFocuser(isPlayerCruiser, droneManager, FactoryProvider.Sound.IPrioritisedSoundPlayer);
            IDroneConsumerProvider droneConsumerProvider = new DroneConsumerProvider(droneManager);
            FogOfWarManager fogOfWarManager = new FogOfWarManager(cruiser.Fog, cruiser.BuildingMonitor, enemyCruiser.BuildingMonitor, enemyCruiser.UnitMonitor);

            RepairManager repairManager = new RepairManager(cruiserSpecificFactories.DroneFeedbackFactory, droneConsumerProvider, cruiser);
            if (!isPlayerCruiser)
            {
                repairManager.RemoveCruiser();
            }

            CruiserArgs cruiserArgs
                = new CruiserArgs(
                    faction,
                    enemyCruiser,
                    uiManager,
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

        private IDroneFocuser CreateDroneFocuser(bool isPlayerCruiser, DroneManager droneManager, IPrioritisedSoundPlayer soundPlayer)
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

        private ICruiserHelper CreateAIHelper(UIManager uiManager, ICameraFocuser cameraFocuser)
        {
            if (ApplicationModel.IsTutorial)
            {
                return new TutorialAICruiserHelper(uiManager, cameraFocuser);
            }
            else
            {
                return new AICruiserHelper(uiManager, cameraFocuser);
            }
        }

        private ICruiserHelper CreatePlayerHelper(UIManager uiManager, ICameraFocuser cameraFocuser)
        {
            if (ApplicationModel.IsTutorial)
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