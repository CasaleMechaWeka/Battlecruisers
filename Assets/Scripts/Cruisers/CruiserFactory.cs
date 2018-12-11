using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Buildings;
using BattleCruisers.Buildables.BuildProgress;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Fog;
using BattleCruisers.Cruisers.Helpers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.Targets.TargetTrackers;
using BattleCruisers.Targets.TargetTrackers.UserChosen;
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

namespace BattleCruisers.Cruisers
{
    public class CruiserFactory : ICruiserFactory
	{
        private readonly IPrefabFactory _prefabFactory;
		private readonly IDeferrer _deferrer;
        private readonly IVariableDelayDeferrer _variableDelayDeferrer;
        private readonly ISpriteProvider _spriteProvider;
        private readonly Cruiser _playerCruiser, _aiCruiser;
        private readonly ICamera _soleCamera;
        private readonly IAudioSource _audioSource;

        public CruiserFactory(
            IPrefabFactory prefabFactory, 
            IDeferrer deferrer, 
            IVariableDelayDeferrer variableDelayDeferrer, 
            ISpriteProvider spriteProvider,
            Cruiser playerCruiser,
            Cruiser aiCruiser,
            ICamera soleCamera,
            IAudioSource audioSource)
        {
            Helper.AssertIsNotNull(prefabFactory, deferrer, variableDelayDeferrer, spriteProvider, playerCruiser, aiCruiser, soleCamera, audioSource);
            
            _prefabFactory = prefabFactory;
            _deferrer = deferrer;
            _variableDelayDeferrer = variableDelayDeferrer;
            _spriteProvider = spriteProvider;
            _playerCruiser = playerCruiser;
            _aiCruiser = aiCruiser;
            _soleCamera = soleCamera;
            _audioSource = audioSource;
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