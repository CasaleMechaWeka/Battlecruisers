using BattleCruisers.Buildables;
using BattleCruisers.Buildables.Repairables;
using BattleCruisers.Buildables.Units;
using BattleCruisers.Cruisers.Drones;
using BattleCruisers.Cruisers.Fog;
using BattleCruisers.Cruisers.Helpers;
using BattleCruisers.Cruisers.Slots;
using BattleCruisers.UI.BattleScene.Manager;
using BattleCruisers.UI.Cameras;
using BattleCruisers.Utils;
using BattleCruisers.Utils.Fetchers;
using BattleCruisers.Utils.Threading;

namespace BattleCruisers.Cruisers
{
	public class CruiserFactory : ICruiserFactory
	{
        private readonly IPrefabFactory _prefabFactory;
		private readonly IDeferrer _deferrer;
        private readonly ISpriteProvider _spriteProvider;

        public CruiserFactory(IPrefabFactory prefabFactory, IDeferrer deferrer, ISpriteProvider spriteProvider)
        {
            Helper.AssertIsNotNull(prefabFactory, deferrer, spriteProvider);
            
            _prefabFactory = prefabFactory;
            _deferrer = deferrer;
            _spriteProvider = spriteProvider;
        }

        public void InitialiseCruiser(
            Cruiser cruiser, 
            ICruiser enemyCruiser,
            IUIManager uiManager,
            ICruiserHelper helper,
            Faction faction, 
            Direction facingDirection,
            ISlotFilter highlightableFilter,
            ISlotFilter clickableFilter)
        {
            Helper.AssertIsNotNull(cruiser, enemyCruiser, uiManager, helper, highlightableFilter, clickableFilter);

            IFactoryProvider factoryProvider = new FactoryProvider(_prefabFactory, cruiser, enemyCruiser, _spriteProvider);
            IDroneManager droneManager = new DroneManager();
            IDroneConsumerProvider droneConsumerProvider = new DroneConsumerProvider(droneManager);
            IDroneNumFeedbackFactory feedbackFactory = new DroneNumFeedbackFactory();
            RepairManager repairManager = new RepairManager(_deferrer, feedbackFactory);
            new FogOfWarManager(cruiser.Fog, cruiser, enemyCruiser);
            bool shouldShowFog = facingDirection == Direction.Left;

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
                    clickableFilter);

            cruiser.Initialise(cruiserArgs);
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