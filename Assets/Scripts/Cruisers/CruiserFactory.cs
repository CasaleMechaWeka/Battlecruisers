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
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras;
using BattleCruisers.UI.Common.Click;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Factories;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Cruisers
{
    public class CruiserFactory : ICruiserFactory
	{
        private readonly IPrefabFactory _prefabFactory;
		private readonly IDeferrer _deferrer;
        private readonly IVariableDelayDeferrer _variableDelayDeferrer;
        private readonly ISpriteProvider _spriteProvider;

        public CruiserFactory(IPrefabFactory prefabFactory, IDeferrer deferrer, IVariableDelayDeferrer variableDelayDeferrer, ISpriteProvider spriteProvider)
        {
            Helper.AssertIsNotNull(prefabFactory, deferrer, variableDelayDeferrer, spriteProvider);
            
            _prefabFactory = prefabFactory;
            _deferrer = deferrer;
            _variableDelayDeferrer = variableDelayDeferrer;
            _spriteProvider = spriteProvider;
        }

        // FELIX  Create separate Player/AI cruiser methods, then don't need stupid ifs on isPlayerCruiser :P
        public void InitialiseCruiser(
            Cruiser cruiser, 
            ICruiser enemyCruiser,
            IUIManager uiManager,
            ICruiserHelper helper,
            Faction faction, 
            Direction facingDirection,
            ISlotFilter highlightableFilter,
            IBuildProgressCalculator buildProgressCalculator,
            IRankedTargetTracker userChosenTargetTracker,
            IUserChosenTargetHelper userChosenTargetHelper)
        {
            Helper.AssertIsNotNull(
                cruiser, 
                enemyCruiser, 
                uiManager, 
                helper, 
                highlightableFilter, 
                buildProgressCalculator, 
                userChosenTargetTracker, 
                userChosenTargetHelper);

            IFactoryProvider factoryProvider = new FactoryProvider(_prefabFactory, cruiser, enemyCruiser, _spriteProvider, _variableDelayDeferrer, userChosenTargetTracker);
            IDroneManager droneManager = new DroneManager();
            IDroneConsumerProvider droneConsumerProvider = new DroneConsumerProvider(droneManager);
            bool isPlayerCruiser = facingDirection == Direction.Right;
            IDroneNumFeedbackFactory feedbackFactory = CreateFeedbackFactory(isPlayerCruiser);
            RepairManager repairManager = new RepairManager(_deferrer, feedbackFactory);
            new FogOfWarManager(cruiser.Fog, cruiser, enemyCruiser);
            bool shouldShowFog = !isPlayerCruiser;
            IDoubleClickHandler<IBuilding> buildingDoubleClickHandler = CreateBuildingDoubleClickHandler(isPlayerCruiser, droneManager, userChosenTargetHelper);
            IDoubleClickHandler<ICruiser> cruiserDoubleClickHandler = CreateCruiserDoubleClickHandler(isPlayerCruiser, userChosenTargetHelper);

            ICruiserArgs cruiserArgs
                = new CruiserArgs(
                    faction,
                    enemyCruiser,
                    uiManager,
                    droneManager,
                    droneConsumerProvider,
                    factoryProvider,
                    facingDirection,
                    repairManager,
                    shouldShowFog,
                    helper,
                    highlightableFilter,
                    buildProgressCalculator,
                    buildingDoubleClickHandler,
                    cruiserDoubleClickHandler);

            cruiser.Initialise(cruiserArgs);
        }

        private IDroneNumFeedbackFactory CreateFeedbackFactory(bool isPlayerCruiser)
        {
            if (isPlayerCruiser)
            {
                return new DroneNumFeedbackFactory();
            }
            else
            {
                // TEMP  Want to see repair drone numbers on AI cruiser, helps me debug :)
                // For end game use Dummy factory :)
                return new DroneNumFeedbackFactory();
                //return new DummyDroneNumFeedbackFactory();
            }
        }

        private IDoubleClickHandler<IBuilding> CreateBuildingDoubleClickHandler(bool isPlayerCruiser, IDroneManager droneManager, IUserChosenTargetHelper userChosenTargetHelper)
        {
            if (isPlayerCruiser)
            {
                return new PlayerBuildingDoubleClickHandler(droneManager);
            }
            else
            {
                return new AIBuildingDoubleClickHandler(userChosenTargetHelper);
            }
        }

        private IDoubleClickHandler<ICruiser> CreateCruiserDoubleClickHandler(bool isPlayerCruiser, IUserChosenTargetHelper userChosenTargetHelper)
        {
            if (isPlayerCruiser)
            {
                return new PlayerCruiserDoubleClickHandler();
            }
            else
            {
                return new AICruiserDoubleClickHandler(userChosenTargetHelper);
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